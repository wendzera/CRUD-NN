using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PlataformaEAD.Models
{
    public class Curso
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O t�tulo � obrigat�rio")]
        [Display(Name = "T�tulo")]
        public string Titulo { get; set; }
        
        [Display(Name = "Descri��o")]
        public string Descricao { get; set; }
        
        [Display(Name = "Pre�o Base")]
        [Precision(18, 2)]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "O pre�o deve ser um valor positivo")]
        public decimal PrecoBase { get; set; }
        
        [Display(Name = "Carga Hor�ria")]
        [Range(1, int.MaxValue, ErrorMessage = "A carga hor�ria deve ser maior que zero")]
        public int CargaHoraria { get; set; }
        
        // Navega��o para matr�culas
        public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}