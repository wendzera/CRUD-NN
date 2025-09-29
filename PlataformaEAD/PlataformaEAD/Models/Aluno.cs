using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlataformaEAD.Models
{
    public class Aluno
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "O nome é obrigatório")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }
        
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        
        [Phone(ErrorMessage = "Telefone inválido")]
        [Display(Name = "Telefone")]
        public string Telefone { get; set; }
        
        // Navegação para matrículas
        public ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}