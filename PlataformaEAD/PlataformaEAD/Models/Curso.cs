using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PlataformaEAD.Models
{
    public class Curso
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O título é obrigatório")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }
        
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        
        [Display(Name = "Preço Base")]
        [Precision(18, 2)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "O preço deve ser um valor positivo")]
        public decimal PrecoBase { get; set; }
        
        [Display(Name = "Carga Horária")]
        [Range(1, int.MaxValue, ErrorMessage = "A carga horária deve ser maior que zero")]
        public int CargaHoraria { get; set; }
        
        // Navegação para matrículas
        public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}