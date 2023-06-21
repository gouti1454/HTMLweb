using System;
using System.Threading.Tasks;
using GoutiClothing.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GoutiClothing
{
    //[Authorize]
    public class ProductDetailModel : PageModel
    {
        private readonly AppDataContext _dbContext;
        public string CartUser;
        public ProductDetailModel(AppDataContext dbContext)
        {
            _dbContext = dbContext;
            CartUser = (User != null && User.Identity != null && !String.IsNullOrEmpty(User.Identity.Name)) ? User.Identity.Name : "test";
        }

        public Product Product { get; set; }

        [BindProperty]
        public string Color { get; set; }

        [BindProperty]
        public int Quantity { get; set; }

        public async Task<IActionResult> OnGetAsync(int? productId)
        {
            if (productId == null)
            {
                return NotFound();
            }

            Product = await _dbContext.Products.Where(x=> x.Id==productId).FirstOrDefaultAsync();
            if (Product == null)
            {
                return NotFound();
            }
            var _cart = await GetCartByUserName(CartUser);
            ViewData["ItemsCount"] = _cart.Items.Count;
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            

           var _cart = await GetCartByUserName(CartUser);
            _cart.Items.Add(
                   new CartItem
                   {
                       ProductId = productId,
                       Color = "",
                       Price = _dbContext.Products.FirstOrDefault(p => p.Id == productId).Price,
                       Quantity = Quantity
                   }
               );

            _dbContext.Entry(_cart).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            _cart = await GetCartByUserName(CartUser);
            ViewData["ItemsCount"] = _cart.Items.Count;
            
            return RedirectToPage("Cart");
        }
        public async Task<Cart> GetCartByUserName(string userName)
        {
            var cart = _dbContext.Carts
                        .Include(c => c.Items)
                            .ThenInclude(i => i.Product)
                        .FirstOrDefault(c => c.UserName == userName);

            if (cart != null)
                return cart;

            // if it is first attempt create new
            var newCart = new Cart
            {
                UserName = userName
            };

            _dbContext.Carts.Add(newCart);
            await _dbContext.SaveChangesAsync();
            return newCart;
        }
    }
}