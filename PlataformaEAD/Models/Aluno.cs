using System.ComponentModel.DataAnnotations;

namespace PlataformaEAD.Models
{
    public class Aluno
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(255, ErrorMessage = "Email deve ter no máximo 255 caracteres")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Telefone inválido")]
        [StringLength(20, ErrorMessage = "Telefone deve ter no máximo 20 caracteres")]
        public string? Telefone { get; set; }

        // Relacionamento 1:N com Matrícula
        public virtual ICollection<Matricula> Matriculas { get; set; } = new List<Matricula>();
    }
}