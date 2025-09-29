using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PlataformaEAD.Models
{
    public enum StatusMatricula
    {
        [Display(Name = "Ativo")]
        Ativo,
        
        [Display(Name = "Concluído")]
        Concluido,
        
        [Display(Name = "Cancelado")]
        Cancelado
    }

    public class Matricula
    {
        public int AlunoId { get; set; }
        public int CursoId { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Data de Matrícula")]
        public DateTime Data { get; set; } = DateTime.UtcNow; // Armazenamos em UTC
        
        [Display(Name = "Preço Pago")]
        [Precision(18, 2)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "O preço pago deve ser um valor positivo ou zero")]
        public decimal PrecoPago { get; set; }
        
        [Display(Name = "Status")]
        public StatusMatricula Status { get; set; } = StatusMatricula.Ativo;
        
        [Display(Name = "Progresso")]
        [Range(0, 100, ErrorMessage = "O progresso deve estar entre 0 e 100")]
        public int Progresso { get; set; } = 0;
        
        [Display(Name = "Nota Final")]
        [Range(0, 10, ErrorMessage = "A nota final deve estar entre 0 e 10")]
        public decimal? NotaFinal { get; set; }
        
        // Propriedades de navegação
        public Aluno Aluno { get; set; }
        public Curso Curso { get; set; }
    }
}