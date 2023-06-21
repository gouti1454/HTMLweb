#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GoutiClothing;
using GoutiClothing.Entities;

namespace GoutiClothing.Pages.Admin1
{
    public class IndexModel : PageModel
    {
        private readonly GoutiClothing.AppDataContext _dbcontext;

        public IndexModel(GoutiClothing.AppDataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IList<Product> Product { get;set; }

        public async Task OnGetAsync()
        {
            Product = await _dbcontext.Products
                .Include(p => p.Category).ToListAsync();
        }
    }
}
