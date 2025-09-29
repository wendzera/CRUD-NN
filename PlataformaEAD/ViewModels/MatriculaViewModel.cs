using System.ComponentModel.DataAnnotations;
using PlataformaEAD.Models;

namespace PlataformaEAD.ViewModels
{
    public class MatriculaViewModel
    {
        [Required(ErrorMessage = "Selecione um aluno")]
        public int AlunoId { get; set; }

        [Required(ErrorMessage = "Selecione pelo menos um curso")]
        public List<int> CursoIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "Data é obrigatória")]
        public DateTime Data { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Preço pago é obrigatório")]
        [Range(0, double.MaxValue, ErrorMessage = "Preço pago deve ser maior ou igual a 0")]
        public decimal PrecoPago { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public StatusMatricula Status { get; set; } = StatusMatricula.Ativo;

        [Range(0, 100, ErrorMessage = "Progresso deve estar entre 0 e 100")]
        public int Progresso { get; set; } = 0;

        [Range(0, 10, ErrorMessage = "Nota final deve estar entre 0 e 10")]
        public decimal? NotaFinal { get; set; }

        // Propriedades para dropdowns
        public List<Aluno> Alunos { get; set; } = new List<Aluno>();
        public List<Curso> Cursos { get; set; } = new List<Curso>();
    }

    public class EditarMatriculaViewModel
    {
        public int AlunoId { get; set; }
        public int CursoId { get; set; }

        [Required(ErrorMessage = "Data é obrigatória")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Preço pago é obrigatório")]
        [Range(0, double.MaxValue, ErrorMessage = "Preço pago deve ser maior ou igual a 0")]
        public decimal PrecoPago { get; set; }

        [Required(ErrorMessage = "Status é obrigatório")]
        public StatusMatricula Status { get; set; }

        [Range(0, 100, ErrorMessage = "Progresso deve estar entre 0 e 100")]
        public int Progresso { get; set; }

        [Range(0, 10, ErrorMessage = "Nota final deve estar entre 0 e 10")]
        public decimal? NotaFinal { get; set; }

        // Para exibição
        public string? AlunoNome { get; set; }
        public string? CursoTitulo { get; set; }
    }
}