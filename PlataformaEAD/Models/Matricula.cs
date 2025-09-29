using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaEAD.Models
{
    public enum StatusMatricula
    {
        Ativo = 1,
        Concluido = 2,
        Cancelado = 3
    }

    public class Matricula
    {
        // Chave composta: AlunoId + CursoId
        public int AlunoId { get; set; }
        public int CursoId { get; set; }

        [Required(ErrorMessage = "Data é obrigatória")]
        public DateTime Data { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Preço pago é obrigatório")]
        [Range(0, double.MaxValue, ErrorMessage = "Preço pago deve ser maior ou igual a 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoPago { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public StatusMatricula Status { get; set; } = StatusMatricula.Ativo;

        [Range(0, 100, ErrorMessage = "Progresso deve estar entre 0 e 100")]
        public int Progresso { get; set; } = 0;

        [Range(0, 10, ErrorMessage = "Nota final deve estar entre 0 e 10")]
        [Column(TypeName = "decimal(4,2)")]
        public decimal? NotaFinal { get; set; }

        // Relacionamentos
        public virtual Aluno Aluno { get; set; } = null!;
        public virtual Curso Curso { get; set; } = null!;
    }
}