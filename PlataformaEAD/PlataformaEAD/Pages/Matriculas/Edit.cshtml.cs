using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Data;
using PlataformaEAD.Models;
using System.Threading.Tasks;

namespace PlataformaEAD.Pages.Matriculas
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Matricula Matricula { get; set; } = default!;
        
        public Aluno Aluno { get; set; } = default!;
        public Curso Curso { get; set; } = default!;

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
            Aluno = matricula.Aluno;
            Curso = matricula.Curso;
            
            // Converter UTC para hora local para exibição
            Matricula.Data = Matricula.Data.ToLocalTime();
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validações específicas
            if (Matricula.Status == StatusMatricula.Concluido && Matricula.Progresso < 100)
            {
                ModelState.AddModelError("Matricula.Status", "Para concluir o curso, o progresso deve ser 100%.");
                
                // Recarregar as entidades relacionadas
                var matricula = await _context.Matriculas
                    .Include(m => m.Aluno)
                    .Include(m => m.Curso)
                    .FirstOrDefaultAsync(m => m.AlunoId == Matricula.AlunoId && m.CursoId == Matricula.CursoId);
                
                Aluno = matricula.Aluno;
                Curso = matricula.Curso;
                
                return Page();
            }
            
            if (!ModelState.IsValid)
            {
                // Recarregar as entidades relacionadas
                var matricula = await _context.Matriculas
                    .Include(m => m.Aluno)
                    .Include(m => m.Curso)
                    .FirstOrDefaultAsync(m => m.AlunoId == Matricula.AlunoId && m.CursoId == Matricula.CursoId);
                
                Aluno = matricula.Aluno;
                Curso = matricula.Curso;
                
                return Page();
            }

            // Obter a entidade original para não perder a data de matrícula original
            var matriculaOriginal = await _context.Matriculas
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.AlunoId == Matricula.AlunoId && m.CursoId == Matricula.CursoId);
            
            // Manter a data original
            Matricula.Data = matriculaOriginal.Data;
            
            _context.Attach(Matricula).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = "Matrícula atualizada com sucesso!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatriculaExists(Matricula.AlunoId, Matricula.CursoId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MatriculaExists(int alunoId, int cursoId)
        {
            return _context.Matriculas.Any(m => m.AlunoId == alunoId && m.CursoId == cursoId);
        }
    }
}