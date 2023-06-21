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
    public class DetailsModel : PageModel
    {
        private readonly GoutiClothing.AppDataContext _dbcontext;

        public DetailsModel(GoutiClothing.AppDataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _dbcontext.Products
                .Include(p => p.Category).FirstOrDefaultAsync(m => m.Id == id);

            if (Product == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
