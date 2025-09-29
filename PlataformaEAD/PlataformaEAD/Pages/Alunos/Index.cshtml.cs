using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PlataformaEAD.Data;
using PlataformaEAD.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaEAD.Pages.Alunos
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Aluno> Alunos { get; set; } = default!;
        
        public string NameSort { get; set; }
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            CurrentFilter = searchString;

            IQueryable<Aluno> alunosQuery = _context.Alunos;

            if (!string.IsNullOrEmpty(searchString))
            {
                alunosQuery = alunosQuery.Where(a => 
                    a.Nome.Contains(searchString) || 
                    (a.Email != null && a.Email.Contains(searchString)));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    alunosQuery = alunosQuery.OrderByDescending(a => a.Nome);
                    break;
                default:
                    alunosQuery = alunosQuery.OrderBy(a => a.Nome);
                    break;
            }

            Alunos = await alunosQuery.ToListAsync();
        }
    }
}