using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaEAD.Models
{
    public class Aluno
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O nome � obrigat�rio")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        
        [EmailAddress(ErrorMessage = "E-mail inv�lido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        
        [Phone(ErrorMessage = "Telefone inv�lido")]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }
        
        // Navega��o para matr�culas
        public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}