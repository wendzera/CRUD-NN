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

            // Configuração da entidade de junção Matricula (N:N com carga)
            modelBuilder.Entity<Matricula>(entity =>
            {
                // Define a chave primária composta
                entity.HasKey(m => new { m.AlunoId, m.CursoId });

                // Configuração da relação Aluno -> Matricula (1:N)
                entity.HasOne(m => m.Aluno)
                      .WithMany(a => a.Matriculas)
                      .HasForeignKey(m => m.AlunoId)
                      .OnDelete(DeleteBehavior.Restrict); // Não permitir exclusão em cascata

                // Configuração da relação Curso -> Matricula (1:N)
                entity.HasOne(m => m.Curso)
                      .WithMany(c => c.Matriculas)
                      .HasForeignKey(m => m.CursoId)
                      .OnDelete(DeleteBehavior.Restrict); // Não permitir exclusão em cascata
                
                // Configuração para campos decimais
                entity.Property(m => m.PrecoPago)
                      .HasPrecision(18, 2);
                
                entity.Property(m => m.NotaFinal)
                      .HasPrecision(10, 2);
                
                // Configuração para data (armazenamos em UTC)
                entity.Property(m => m.Data)
                      .HasColumnType("timestamp with time zone"); // PostgreSQL timestamp with timezone
            });

            // Configuração adicional para o preço base do curso
            modelBuilder.Entity<Curso>()
                .Property(c => c.PrecoBase)
                .HasPrecision(18, 2);
        }
    }
}