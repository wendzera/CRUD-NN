using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Data;
using PlataformaEAD.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaEAD.Pages.Matriculas
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Matricula Matricula { get; set; } = new Matricula();

        [BindProperty, Required(ErrorMessage = "Selecione pelo menos um curso")]
        public int[] CursosSelecionados { get; set; } = Array.Empty<int>();

        public List<Curso> CursosDisponiveis { get; set; } = new List<Curso>();

        public async Task<IActionResult> OnGetAsync(int? alunoId, int? cursoId)
        {
            // Preencher a lista de alunos para o select
            ViewData["Alunos"] = new SelectList(await _context.Alunos.OrderBy(a => a.Nome).ToListAsync(), "Id", "Nome");
            
            // Pré-selecionar o aluno se foi fornecido na URL
            if (alunoId.HasValue)
            {
                Matricula.AlunoId = alunoId.Value;
                await CarregarCursosDisponiveisAsync(alunoId.Value);
                
                // Pré-selecionar o curso se foi fornecido na URL
                if (cursoId.HasValue)
                {
                    CursosSelecionados = new int[] { cursoId.Value };
                }
            }
            else
            {
                // Se não há aluno selecionado, carrega todos os cursos
                CursosDisponiveis = await _context.Cursos.OrderBy(c => c.Titulo).ToListAsync();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validações
            if (!ModelState.IsValid)
            {
                ViewData["Alunos"] = new SelectList(await _context.Alunos.OrderBy(a => a.Nome).ToListAsync(), "Id", "Nome");
                await CarregarCursosDisponiveisAsync(Matricula.AlunoId);
                return Page();
            }

            if (CursosSelecionados.Length == 0)
            {
                ModelState.AddModelError("CursosSelecionados", "Selecione pelo menos um curso");
                ViewData["Alunos"] = new SelectList(await _context.Alunos.OrderBy(a => a.Nome).ToListAsync(), "Id", "Nome");
                await CarregarCursosDisponiveisAsync(Matricula.AlunoId);
                return Page();
            }

            // Criar uma matrícula para cada curso selecionado
            var dataMatricula = DateTime.UtcNow;
            var matriculasParaAdicionar = new List<Matricula>();
            
            foreach (var cursoId in CursosSelecionados)
            {
                // Obter o preço pago informado pelo usuário
                var precoString = Request.Form[$"PrecosPagos_{cursoId}"];
                decimal precoPago = 0;
                
                if (decimal.TryParse(precoString, out decimal valor))
                {
                    precoPago = valor;
                }
                else
                {
                    // Se não conseguir obter o preço, usa o preço base do curso
                    var curso = await _context.Cursos.FindAsync(cursoId);
                    precoPago = curso?.PrecoBase ?? 0;
                }
                
                var matricula = new Matricula
                {
                    AlunoId = Matricula.AlunoId,
                    CursoId = cursoId,
                    Data = dataMatricula,
                    PrecoPago = precoPago,
                    Status = StatusMatricula.Ativo,
                    Progresso = 0
                };
                
                matriculasParaAdicionar.Add(matricula);
            }
            
            _context.Matriculas.AddRange(matriculasParaAdicionar);
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Matrícula(s) realizada(s) com sucesso!";
            return RedirectToPage("./Index");
        }
        
        private async Task CarregarCursosDisponiveisAsync(int alunoId)
        {
            // Obter todos os cursos
            var todosCursos = await _context.Cursos.OrderBy(c => c.Titulo).ToListAsync();
            
            // Obter os IDs de cursos em que o aluno já está matriculado
            var cursosMatriculados = await _context.Matriculas
                .Where(m => m.AlunoId == alunoId)
                .Select(m => m.CursoId)
                .ToListAsync();
            
            // Filtrar os cursos disponíveis (aqueles em que o aluno ainda não está matriculado)
            CursosDisponiveis = todosCursos.Where(c => !cursosMatriculados.Contains(c.Id)).ToList();
        }
    }
}