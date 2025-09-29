using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Data;
using PlataformaEAD.Models;
using System.Threading.Tasks;

namespace PlataformaEAD.Pages.Alunos
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Aluno Aluno { get; set; } = default!;
        public bool TemMatriculas { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var aluno = await _context.Alunos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (aluno == null)
            {
                return NotFound();
            }

            Aluno = aluno;
            TemMatriculas = await _context.Matriculas.AnyAsync(m => m.AlunoId == id);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);

            if (aluno == null)
            {
                return NotFound();
            }

            try
            {
                _context.Alunos.Remove(aluno);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Aluno excluído com sucesso!";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException ex)
            {
                // Captura erro de restrição de chave estrangeira
                TempData["Error"] = "Não foi possível excluir o aluno pois ele possui matrículas associadas.";
                return RedirectToPage("./Index");
            }
        }
    }
}