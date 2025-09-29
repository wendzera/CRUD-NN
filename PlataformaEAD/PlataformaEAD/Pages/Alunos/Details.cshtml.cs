using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Data;
using PlataformaEAD.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlataformaEAD.Pages.Alunos
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Aluno Aluno { get; set; } = default!;
        public IList<Matricula> Matriculas { get; set; } = new List<Matricula>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var aluno = await _context.Alunos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (aluno == null)
            {
                return NotFound();
            }

            Aluno = aluno;
            
            // Carrega matrículas do aluno com os respectivos cursos
            Matriculas = await _context.Matriculas
                .Include(m => m.Curso)
                .Where(m => m.AlunoId == id)
                .OrderBy(m => m.Curso.Titulo)
                .ToListAsync();

            return Page();
        }
    }
}