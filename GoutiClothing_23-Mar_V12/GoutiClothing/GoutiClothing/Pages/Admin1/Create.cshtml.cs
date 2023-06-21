#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using GoutiClothing;
using GoutiClothing.Entities;

namespace GoutiClothing.Pages.Admin1
{
    public class CreateModel : PageModel
    {
        private readonly GoutiClothing.AppDataContext _dbcontext;

        public CreateModel(GoutiClothing.AppDataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public IActionResult OnGet()
        {
            ViewData["CategoryId"] = new SelectList(_dbcontext.Categories, "Id", "Name");

            return Page();
        }

        [BindProperty]
        public Product Product { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _dbcontext.Products.Add(Product);
            await _dbcontext.SaveChangesAsync();
            //TempData function store the reslut for only one request and resets after ever refersh.
            //TempData is used to desplay notification only once.
            TempData["success"] = "Completed";

            return RedirectToPage("/Admin1/Index");
        }
    }
}
