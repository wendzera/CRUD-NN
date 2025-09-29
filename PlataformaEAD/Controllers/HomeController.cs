using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Data;
using PlataformaEAD.Models;

namespace PlataformaEAD.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(AppDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var totalAlunos = await _context.Alunos.CountAsync();
            var totalCursos = await _context.Cursos.CountAsync();
            var totalMatriculas = await _context.Matriculas.CountAsync();
            var matriculasAtivas = await _context.Matriculas
                .CountAsync(m => m.Status == StatusMatricula.Ativo);

            ViewData["TotalAlunos"] = totalAlunos;
            ViewData["TotalCursos"] = totalCursos;
            ViewData["TotalMatriculas"] = totalMatriculas;
            ViewData["MatriculasAtivas"] = matriculasAtivas;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}