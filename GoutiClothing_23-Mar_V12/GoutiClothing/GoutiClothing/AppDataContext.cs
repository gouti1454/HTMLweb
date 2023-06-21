using GoutiClothing.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoutiClothing
{
    //create the AppDbcontext to intract with database and data layer connecting with the database
    // type Dbcontext then press ctrl+dot then select "using entity framework"
    public class AppDataContext: IdentityDbContext
    {
        //retrive the option from connection string and pass to the dbcontext
        //below statement needed for the connection to happen.
        public AppDataContext(DbContextOptions<AppDataContext> options): base(options)
        {

        }
        //to create the table in DB
        //Product is the actuall db name
        public DbSet<Product> Products { get; set; }

        //after updating the program.cs with connection string and adding above retrive options.
        //now run the migration to create the DB 
        //add-migration AddProductsToDb - "AddProductsToDb" givenig a meaningful name as reference.
        //update-database

        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Contact> Contacts { get; set; }


        public DbSet<Appuser> Appuser { get; set; }
        //after creating the model Appuser.cs 
        //the dbset is pushed to databsetable
        //add-migration extendIdentityUser
        //update-database

    }
}
