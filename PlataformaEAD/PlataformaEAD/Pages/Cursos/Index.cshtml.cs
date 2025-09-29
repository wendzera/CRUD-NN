using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Data;
using PlataformaEAD.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaEAD.Pages.Cursos
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Curso> Cursos { get; set; } = default!;
        
        public string TitleSort { get; set; }
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            TitleSort = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            CurrentFilter = searchString;

            IQueryable<Curso> cursosQuery = _context.Cursos;

            if (!string.IsNullOrEmpty(searchString))
            {
                cursosQuery = cursosQuery.Where(c => 
                    c.Titulo.Contains(searchString) || 
                    (c.Descricao != null && c.Descricao.Contains(searchString)));
            }

            switch (sortOrder)
            {
                case "title_desc":
                    cursosQuery = cursosQuery.OrderByDescending(c => c.Titulo);
                    break;
                default:
                    cursosQuery = cursosQuery.OrderBy(c => c.Titulo);
                    break;
            }

            Cursos = await cursosQuery.ToListAsync();
        }
    }
}