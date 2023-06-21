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
    public class DeleteModel : PageModel
    {
        private readonly GoutiClothing.AppDataContext _dbcontext;

        public DeleteModel(GoutiClothing.AppDataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product = await _dbcontext.Products.FindAsync(id);

            if (Product != null)
            {
                _dbcontext.Products.Remove(Product);
                await _dbcontext.SaveChangesAsync();
            }
            //TempData function store the reslut for only one request and resets after ever refersh.
            //TempData is used to desplay notification only once.
            TempData["success"] = "Completed";

            return RedirectToPage("/Admin1/Index");
        }
    }
}
