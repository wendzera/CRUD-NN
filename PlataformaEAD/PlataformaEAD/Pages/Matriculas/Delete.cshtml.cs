using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Data;
using PlataformaEAD.Models;
using System.Threading.Tasks;

namespace PlataformaEAD.Pages.Matriculas
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Matricula Matricula { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int alunoId, int cursoId)
        {
            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);

            if (matricula == null)
            {
                return NotFound();
            }

            Matricula = matricula;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int alunoId, int cursoId)
        {
            var matricula = await _context.Matriculas.FindAsync(alunoId, cursoId);

            if (matricula == null)
            {
                return NotFound();
            }

            _context.Matriculas.Remove(matricula);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Matrícula excluída com sucesso!";
            return RedirectToPage("./Index");
        }
    }
}