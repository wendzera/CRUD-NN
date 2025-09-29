using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Data;
using PlataformaEAD.Models;
using PlataformaEAD.ViewModels;

namespace PlataformaEAD.Controllers
{
    public class MatriculasController : Controller
    {
        private readonly AppDbContext _context;

        public MatriculasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Matriculas
        public async Task<IActionResult> Index()
        {
            var matriculas = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .OrderBy(m => m.Aluno.Nome)
                .ThenBy(m => m.Curso.Titulo)
                .ToListAsync();

            return View(matriculas);
        }

        // GET: Matriculas/Details/5/3
        public async Task<IActionResult> Details(int? alunoId, int? cursoId)
        {
            if (alunoId == null || cursoId == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);

            if (matricula == null)
            {
                return NotFound();
            }

            return View(matricula);
        }

        // GET: Matriculas/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new MatriculaViewModel
            {
                Alunos = await _context.Alunos.OrderBy(a => a.Nome).ToListAsync(),
                Cursos = await _context.Cursos.OrderBy(c => c.Titulo).ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Matriculas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatriculaViewModel viewModel)
        {
            // Validação customizada: deve ter pelo menos um curso selecionado
            if (viewModel.CursoIds == null || !viewModel.CursoIds.Any())
            {
                ModelState.AddModelError("CursoIds", "Selecione pelo menos um curso.");
            }

            // Validação: Status Concluído requer progresso 100%
            if (viewModel.Status == StatusMatricula.Concluido && viewModel.Progresso < 100)
            {
                ModelState.AddModelError("Progresso", "Para alterar status para Concluído, o progresso deve ser 100%.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var matriculas = new List<Matricula>();
                    
                    foreach (var cursoId in viewModel.CursoIds)
                    {
                        // Verificar se a matrícula já existe
                        var matriculaExistente = await _context.Matriculas
                            .FirstOrDefaultAsync(m => m.AlunoId == viewModel.AlunoId && m.CursoId == cursoId);
                        
                        if (matriculaExistente != null)
                        {
                            var curso = await _context.Cursos.FindAsync(cursoId);
                            ModelState.AddModelError("", $"O aluno já está matriculado no curso '{curso?.Titulo}'.");
                            break;
                        }

                        var matricula = new Matricula
                        {
                            AlunoId = viewModel.AlunoId,
                            CursoId = cursoId,
                            Data = viewModel.Data.ToUniversalTime(),
                            PrecoPago = viewModel.PrecoPago,
                            Status = viewModel.Status,
                            Progresso = viewModel.Progresso,
                            NotaFinal = viewModel.NotaFinal
                        };
                        
                        matriculas.Add(matricula);
                    }

                    if (ModelState.IsValid)
                    {
                        _context.Matriculas.AddRange(matriculas);
                        await _context.SaveChangesAsync();
                        TempData["Sucesso"] = $"Matrícula(s) criada(s) com sucesso! ({matriculas.Count} curso(s))";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Erro ao salvar matrícula: " + ex.Message);
                }
            }

            // Recarregar listas para o formulário
            viewModel.Alunos = await _context.Alunos.OrderBy(a => a.Nome).ToListAsync();
            viewModel.Cursos = await _context.Cursos.OrderBy(c => c.Titulo).ToListAsync();
            return View(viewModel);
        }

        // GET: Matriculas/Edit/5/3
        public async Task<IActionResult> Edit(int? alunoId, int? cursoId)
        {
            if (alunoId == null || cursoId == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);

            if (matricula == null)
            {
                return NotFound();
            }

            var viewModel = new EditarMatriculaViewModel
            {
                AlunoId = matricula.AlunoId,
                CursoId = matricula.CursoId,
                Data = matricula.Data,
                PrecoPago = matricula.PrecoPago,
                Status = matricula.Status,
                Progresso = matricula.Progresso,
                NotaFinal = matricula.NotaFinal,
                AlunoNome = matricula.Aluno.Nome,
                CursoTitulo = matricula.Curso.Titulo
            };

            return View(viewModel);
        }

        // POST: Matriculas/Edit/5/3
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditarMatriculaViewModel viewModel)
        {
            // Validação: Status Concluído requer progresso 100%
            if (viewModel.Status == StatusMatricula.Concluido && viewModel.Progresso < 100)
            {
                ModelState.AddModelError("Progresso", "Para alterar status para Concluído, o progresso deve ser 100%.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var matricula = await _context.Matriculas
                        .FirstOrDefaultAsync(m => m.AlunoId == viewModel.AlunoId && m.CursoId == viewModel.CursoId);

                    if (matricula == null)
                    {
                        return NotFound();
                    }

                    matricula.Data = viewModel.Data.ToUniversalTime();
                    matricula.PrecoPago = viewModel.PrecoPago;
                    matricula.Status = viewModel.Status;
                    matricula.Progresso = viewModel.Progresso;
                    matricula.NotaFinal = viewModel.NotaFinal;

                    _context.Update(matricula);
                    await _context.SaveChangesAsync();
                    TempData["Sucesso"] = "Matrícula atualizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "A matrícula foi modificada por outro usuário.");
                }
            }

            // Recarregar dados para exibição
            var matriculaInfo = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == viewModel.AlunoId && m.CursoId == viewModel.CursoId);

            if (matriculaInfo != null)
            {
                viewModel.AlunoNome = matriculaInfo.Aluno.Nome;
                viewModel.CursoTitulo = matriculaInfo.Curso.Titulo;
            }

            return View(viewModel);
        }

        // GET: Matriculas/Delete/5/3
        public async Task<IActionResult> Delete(int? alunoId, int? cursoId)
        {
            if (alunoId == null || cursoId == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);

            if (matricula == null)
            {
                return NotFound();
            }

            return View(matricula);
        }

        // POST: Matriculas/Delete/5/3
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int alunoId, int cursoId)
        {
            var matricula = await _context.Matriculas
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);

            if (matricula != null)
            {
                _context.Matriculas.Remove(matricula);
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = "Matrícula excluída com sucesso!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}