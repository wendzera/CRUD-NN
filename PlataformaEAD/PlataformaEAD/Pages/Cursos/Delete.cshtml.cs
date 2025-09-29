using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Data;
using PlataformaEAD.Models;
using System.Threading.Tasks;

namespace PlataformaEAD.Pages.Cursos
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Curso Curso { get; set; } = default!;
        public bool TemMatriculas { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var curso = await _context.Cursos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (curso == null)
            {
                return NotFound();
            }

            Curso = curso;
            TemMatriculas = await _context.Matriculas.AnyAsync(m => m.CursoId == id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);

            if (curso == null)
            {
                return NotFound();
            }

            try
            {
                _context.Cursos.Remove(curso);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Curso excluído com sucesso!";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex)
            {
                // Captura erro de restrição de chave estrangeira
                TempData["Error"] = "Não foi possível excluir o curso pois ele possui matrículas associadas.";
                return RedirectToPage("./Index");
            }
        }
    }
}