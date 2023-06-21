#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoutiClothing;
using GoutiClothing.Entities;

namespace GoutiClothing.Pages.Admin1
{
    public class EditModel : PageModel
    {
        private readonly GoutiClothing.AppDataContext _dbcontext;

        public EditModel(GoutiClothing.AppDataContext dbcontext)
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
            ViewData["CategoryId"] = new SelectList(_dbcontext.Categories, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbcontext.Attach(Product).State = EntityState.Modified;

            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(Product.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            //TempData function store the reslut for only one request and resets after ever refersh.
            //TempData is used to desplay notification only once.
            TempData["success"] = "Completed";

            return RedirectToPage("/Admin1/Index");
        }

        private bool ProductExists(int id)
        {
            return _dbcontext.Products.Any(e => e.Id == id);
        }
    }
}
