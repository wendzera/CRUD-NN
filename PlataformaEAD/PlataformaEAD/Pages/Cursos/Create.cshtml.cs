using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PlataformaEAD.Data;
using PlataformaEAD.Models;
using System.Threading.Tasks;

namespace PlataformaEAD.Pages.Cursos
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Curso Curso { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Cursos.Add(Curso);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Curso criado com sucesso!";
            return RedirectToPage("./Index");
        }
    }
}