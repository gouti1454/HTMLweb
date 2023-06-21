using System;
using System.Threading.Tasks;
using GoutiClothing.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GoutiClothing
{
    [Authorize]
    public class CartModel : PageModel
    {
        private readonly AppDataContext _dbContext;
        public string CartUser;
        public CartModel(AppDataContext dbContext)
        {

            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            CartUser = (User != null && User.Identity != null && !String.IsNullOrEmpty(User.Identity.Name)) ? User.Identity.Name : "test";
        }

        public Cart Cart { get; set; } = new Cart();        

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await GetCartByUserName(CartUser);
            ViewData["ItemsCount"] = Cart.Items.Count;
            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(int cartId, int cartItemId)
        {
            var cart = _dbContext.Carts
                       .Include(c => c.Items)
                       .FirstOrDefault(c => c.Id == cartId);

            if (cart != null)
            {
                var removedItem = cart.Items.FirstOrDefault(x => x.Id == cartItemId);
                cart.Items.Remove(removedItem);

                _dbContext.Entry(cart).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToPage();
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