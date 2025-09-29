using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Models;

namespace PlataformaEAD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura��o da entidade de jun��o Matricula (N:N com carga)
            modelBuilder.Entity<Matricula>(entity =>
            {
                // Define a chave prim�ria composta
                entity.HasKey(m => new { m.AlunoId, m.CursoId });

                // Configura��o da rela��o Aluno -> Matricula (1:N)
                entity.HasOne(m => m.Aluno)
                      .WithMany(a => a.Matriculas)
                      .HasForeignKey(m => m.AlunoId)
                      .OnDelete(DeleteBehavior.Restrict); // N�o permitir exclus�o em cascata

                // Configura��o da rela��o Curso -> Matricula (1:N)
                entity.HasOne(m => m.Curso)
                      .WithMany(c => c.Matriculas)
                      .HasForeignKey(m => m.CursoId)
                      .OnDelete(DeleteBehavior.Restrict); // N�o permitir exclus�o em cascata
                
                // Configura��o para campos decimais
                entity.Property(m => m.PrecoPago)
                      .HasPrecision(18, 2);
                
                entity.Property(m => m.NotaFinal)
                      .HasPrecision(10, 2);
                
                // Configura��o para data (armazenamos em UTC)
                entity.Property(m => m.Data)
                      .HasColumnType("timestamp with time zone"); // PostgreSQL timestamp with timezone
            });

            // Configura��o adicional para o pre�o base do curso
            modelBuilder.Entity<Curso>()
                .Property(c => c.PrecoBase)
                .HasPrecision(18, 2);
        }
    }
}