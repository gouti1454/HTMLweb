using GoutiClothing.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GoutiClothing.Pages
{
    //[Authorize]
    public class IndexModel : PageModel
    {
        
        private readonly AppDataContext _dbContext;
        private readonly IConfiguration _Configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public string CartUser;
        public IndexModel(AppDataContext dbContext, IConfiguration Configuration, UserManager<IdentityUser> userManager,
           RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _Configuration = Configuration;
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            CartUser = (User!=null && User.Identity!=null && !String.IsNullOrEmpty( User.Identity.Name)) ?  User.Identity.Name : "test" ;
        }

        public IEnumerable<Product> ProductList { get; set; } = new List<Product>();
        public Entities.Cart _cart = new Entities.Cart(); 

        public async Task<IActionResult> OnGetAsync()
        {
            await CreateRoles();
            if (!_dbContext.Categories.Any())
            {
                _dbContext.Categories.AddRange(GetPreconfiguredCategories());
                await _dbContext.SaveChangesAsync();
            }

            if (!_dbContext.Products.Any())
            {
                _dbContext.Products.AddRange(GetPreconfiguredProducts());
                await _dbContext.SaveChangesAsync();
            }
            _cart = await GetCartByUserName(CartUser);
            ViewData["ItemsCount"] = _cart.Items.Count;
            ProductList = await _dbContext.Products.ToListAsync();
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

        private static IEnumerable<Category> GetPreconfiguredCategories()
        {
            return new List<Category>()
            {
                new Category()
                {
                    Name = "Nike",
                    Description = "Nike Jackets are very good quality jackets",
                    ImageName = "one"
                },
                new Category()
                {
                    Name = "Superdry",
                    Description = "Very Nice jackets",
                    ImageName = "two"
                }
            };
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Name = "Liverpool F.C. Strike",
                    Summary = "Men's Nike Therma-FIT Football Jacket",
                    Description = "The Liverpool F.C. Strike Jacket is equipped with down fill and thermal fabric, so you can stay warm while you cheer for your team.",
                    ImageFile = "product1.jpg",
                    Price = 149,
                    CategoryId = 1
                },
                new Product()
                {
                    Name = "Los Angeles Lakers Courtside",
                    Summary = "Men's Nike NBA Jacket",
                    Description = "The season is on, and the Los Angeles Lakers Courtside Jacket gets you in on the action. Made from light, water-repellent material with lightweight insulation, this hooded, bomber-style jacket has a team-colour liner, striped ribbing and drawcord toggles. A large NBA 75th anniversary logo commemorates the league's big year.",
                    ImageFile = "product2.jpg",
                    Price = 124,
                    CategoryId = 1
                },
               
                new Product()
                {
                    Name = "Nike x sacai",
                    Summary = "Men's Jacket",
                    Description = "Chitose Abe takes the next step in her collaboration with Nike, highlighting the beauty of motion by remixing performance garments.The layered look of the Nike x sacai Jacket is the result of combining the sacai Bomber with the Nike Team USA Windrunner Jacket.",
                    ImageFile = "product4.jpg",
                    Price = 470,
                    CategoryId = 1
                },
                //categpry 2 product
                new Product()
                {
                    Name = "Longline Down Puffer Coat",
                    Summary = "Relaxed fit – the classic Superdry fit. Not too slim, not too loose, just right. Go for your normal size.",
                    Description = "For those days when the outdoors seem unforgiving, our Longline Down Puffer Coat will keep you protected. With an iconic silhouette and down padding, it's sure to keep you warm - like a personal duvet! On top of that, it's got a choice of two zip fastenings, depending on whether you want a looser or more snug fit. Your comfort, now in your control.",
                    ImageFile = "sup-product-5.jpg",
                    Price = 199,
                    CategoryId = 2
                },
                new Product()
                {
                    Name = "Original & Vintage",
                    Summary = "Classic Rookie Jacket",
                    Description = "Military style has long been an inspiration and, with the Classic Rookie jacket, it is no different. All those classic styling cues feature in this lightweight cotton jacket so that you can bring an element of military ruggedness to your everyday look.",
                    ImageFile = "sup-product-6.jpg",
                    Price = 105,
                    CategoryId = 2
                },
                new Product()
                {
                    Name = "Ultimate Microfibre SD Wind Jacket",
                    Summary = "Bungee cord hood, adjustable both around the hem and across the back",
                    Description = "Wrap up and protect yourself from the harsh outdoor elements with our Ultimate Microfibre SD Windcheater Jacket. Functional, practical and adjustable, it's been designed with your needs in mind. Its microfibre material is soft-touch and elevates the comfort of this jacket.",
                    ImageFile = "sup-product-7.jpg",
                    Price = 240,
                    CategoryId = 2
                }
            };
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
        public async Task CreateRoles()
        {

            string[] roleNames = { "Admin", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    //create the roles and seed them to the database
                    roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            //************ Currently the pushing new user is commented and
            //new admin user can be registered from web*****//

            ////Here you could create a super user who will maintain the web app
            //var defaultuser = new IdentityUser
            //{

            //    UserName = _Configuration["AdminUserName"],
            //    Email = _Configuration["AdminUserEmail"],
            //};
            ////Ensure you have these values in your appsettings.json file
            //string userPWD = _Configuration["AdminUserPassword"];
            //var _user = await _userManager.FindByEmailAsync(_Configuration["AdminUserEmail"]);

            //if (_user == null)
            //{
            //    var createAdminUser = await _userManager.CreateAsync(defaultuser, userPWD);
            //    if (createAdminUser.Succeeded)
            //    {
            //        //here we tie the new user to the role
            //        await _userManager.AddToRoleAsync(defaultuser, "Admin");

            //    }
            //}
        }
    }
}
