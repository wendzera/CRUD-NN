using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Models;

namespace PlataformaEAD.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade Aluno
            modelBuilder.Entity<Aluno>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Nome).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Email).HasMaxLength(255);
                entity.Property(a => a.Telefone).HasMaxLength(20);
                entity.HasIndex(a => a.Email).IsUnique();
            });

            // Configuração da entidade Curso
            modelBuilder.Entity<Curso>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Descricao).HasMaxLength(1000);
                entity.Property(c => c.PrecoBase).HasPrecision(18, 2);
            });

            // Configuração da entidade Matrícula (N:N com carga)
            modelBuilder.Entity<Matricula>(entity =>
            {
                // Chave composta
                entity.HasKey(m => new { m.AlunoId, m.CursoId });

                // Propriedades
                entity.Property(m => m.Data)
                    .IsRequired()
                    .HasColumnType("timestamp with time zone"); // UTC timestamp

                entity.Property(m => m.PrecoPago)
                    .IsRequired()
                    .HasPrecision(18, 2);

                entity.Property(m => m.NotaFinal)
                    .HasPrecision(4, 2);

                entity.Property(m => m.Status)
                    .IsRequired()
                    .HasConversion<int>();

                // Relacionamentos
                entity.HasOne(m => m.Aluno)
                    .WithMany(a => a.Matriculas)
                    .HasForeignKey(m => m.AlunoId)
                    .OnDelete(DeleteBehavior.Restrict); // Não apagar registros históricos

                entity.HasOne(m => m.Curso)
                    .WithMany(c => c.Matriculas)
                    .HasForeignKey(m => m.CursoId)
                    .OnDelete(DeleteBehavior.Restrict); // Não apagar registros históricos
            });
        }
    }
}