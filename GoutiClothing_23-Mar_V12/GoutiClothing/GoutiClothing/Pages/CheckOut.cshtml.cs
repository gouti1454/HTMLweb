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
    public class CheckOutModel : PageModel
    {
        private readonly AppDataContext _dbContext;
        public string CartUser;
        public CheckOutModel(AppDataContext dbContext)
        {
            _dbContext = dbContext;
            CartUser = (User != null && User.Identity != null && !String.IsNullOrEmpty(User.Identity.Name)) ? User.Identity.Name : "test";
        }

        [BindProperty]
        public Entities.Order Order { get; set; }

        public Entities.Cart Cart { get; set; } = new Entities.Cart();

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await GetCartByUserName(CartUser);
            //_cart = await GetCartByUserName(CartUser);
            ViewData["ItemsCount"] = Cart.Items.Count;
            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            Cart = await GetCartByUserName(CartUser);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.UserName = CartUser;
            Order.TotalPrice = Cart.TotalPrice;

            await _dbContext.Orders.AddAsync(Order);
            await _dbContext.SaveChangesAsync();
            var cart = await GetCartByUserName(CartUser);

            cart.Items.Clear();

            _dbContext.Entry(cart).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return RedirectToPage("Confirmation", "OrderSubmitted");
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