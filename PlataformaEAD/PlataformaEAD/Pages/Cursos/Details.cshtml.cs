using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Data;
using PlataformaEAD.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlataformaEAD.Pages.Cursos
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Curso Curso { get; set; } = default!;
        public IList<Matricula> Matriculas { get; set; } = new List<Matricula>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var curso = await _context.Cursos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (curso == null)
            {
                return NotFound();
            }

            Curso = curso;
            
            // Carrega as matrículas do curso com os respectivos alunos
            Matriculas = await _context.Matriculas
                .Include(m => m.Aluno)
                .Where(m => m.CursoId == id)
                .OrderBy(m => m.Aluno.Nome)
                .ToListAsync();

            return Page();
        }
    }
}