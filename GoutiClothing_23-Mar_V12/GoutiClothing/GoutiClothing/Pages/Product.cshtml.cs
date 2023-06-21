using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoutiClothing.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GoutiClothing
{
    public class ProductModel : PageModel
    {
        private readonly AppDataContext _dbContext;
        public string CartUser;
        public ProductModel(AppDataContext dbContext)
        {
            _dbContext = dbContext;
            CartUser = (User != null && User.Identity != null && !String.IsNullOrEmpty(User.Identity.Name)) ? User.Identity.Name : "test";
        }

        public IEnumerable<Category> CategoryList { get; set; } = new List<Category>();
        public IEnumerable<Product> ProductList { get; set; } = new List<Product>();
        public Entities.Cart _cart = new Entities.Cart();

        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(int? categoryId)
        {
            CategoryList = await _dbContext.Categories.ToListAsync();

            if (categoryId.HasValue)
            {
                ProductList = await _dbContext.Products.Where(x => x.CategoryId == categoryId).ToListAsync();
                SelectedCategory = CategoryList.FirstOrDefault(c => c.Id == categoryId.Value)?.Name;
            }
            else
            {
                ProductList = await _dbContext.Products.ToListAsync();
            }
            _cart = await GetCartByUserName(CartUser);
            ViewData["ItemsCount"] = _cart.Items.Count;
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int productId)
        {
            

            _cart = await GetCartByUserName(CartUser);
            _cart.Items.Add(
                   new CartItem
                   {
                       ProductId = productId,
                       Color = "",
                       Price = _dbContext.Products.FirstOrDefault(p => p.Id == productId).Price,
                       Quantity = 1
                   }
               );

            _dbContext.Entry(_cart).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            _cart = await GetCartByUserName(CartUser);
            ViewData["ItemsCount"] = _cart.Items.Count;
            ProductList = await _dbContext.Products.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostSearchProductAsync()
        {
            var searchText = Request.Form["ProductSearch"].ToString();
            if (searchText !=null && searchText.ToString().Length > 0)
            {
                ProductList = await _dbContext.Products.Where(x=>x.Name.Contains(searchText)).ToListAsync();
            }
            else
            {
                ProductList = await _dbContext.Products.ToListAsync();
            }
            _cart = await GetCartByUserName(CartUser);
            ViewData["ItemsCount"] = _cart.Items.Count;

            return Page();
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