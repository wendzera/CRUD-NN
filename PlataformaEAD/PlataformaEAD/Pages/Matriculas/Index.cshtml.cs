using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Data;
using PlataformaEAD.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaEAD.Pages.Matriculas
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Matricula> Matriculas { get; set; } = default!;
        
        public string AlunoSort { get; set; }
        public string CursoSort { get; set; }
        public string DataSort { get; set; }
        public string ProgressoSort { get; set; }
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            AlunoSort = sortOrder == "aluno" ? "aluno_desc" : "aluno";
            CursoSort = sortOrder == "curso" ? "curso_desc" : "curso";
            DataSort = sortOrder == "data" ? "data_desc" : "data";
            ProgressoSort = sortOrder == "progresso" ? "progresso_desc" : "progresso";
            
            CurrentFilter = searchString;

            IQueryable<Matricula> matriculasQuery = _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso);

            if (!string.IsNullOrEmpty(searchString))
            {
                matriculasQuery = matriculasQuery.Where(m => 
                    m.Aluno.Nome.Contains(searchString) || 
                    m.Curso.Titulo.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "aluno_desc":
                    matriculasQuery = matriculasQuery.OrderByDescending(m => m.Aluno.Nome);
                    break;
                case "aluno":
                    matriculasQuery = matriculasQuery.OrderBy(m => m.Aluno.Nome);
                    break;
                case "curso_desc":
                    matriculasQuery = matriculasQuery.OrderByDescending(m => m.Curso.Titulo);
                    break;
                case "curso":
                    matriculasQuery = matriculasQuery.OrderBy(m => m.Curso.Titulo);
                    break;
                case "data_desc":
                    matriculasQuery = matriculasQuery.OrderByDescending(m => m.Data);
                    break;
                case "data":
                    matriculasQuery = matriculasQuery.OrderBy(m => m.Data);
                    break;
                case "progresso_desc":
                    matriculasQuery = matriculasQuery.OrderByDescending(m => m.Progresso);
                    break;
                case "progresso":
                    matriculasQuery = matriculasQuery.OrderBy(m => m.Progresso);
                    break;
                default:
                    matriculasQuery = matriculasQuery.OrderBy(m => m.Aluno.Nome).ThenBy(m => m.Curso.Titulo);
                    break;
            }

            Matriculas = await matriculasQuery.ToListAsync();
        }
    }
}