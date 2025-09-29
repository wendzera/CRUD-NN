using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaEAD.Models
{
    public class Curso
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Título é obrigatório")]
        [StringLength(200, ErrorMessage = "Título deve ter no máximo 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Descrição deve ter no máximo 1000 caracteres")]
        public string? Descricao { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Preço base deve ser maior ou igual a 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoBase { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Carga horária deve ser maior que 0")]
        public int CargaHoraria { get; set; }

        // Relacionamento 1:N com Matrícula
        public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}