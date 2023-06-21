using Microsoft.AspNetCore.Identity;

namespace GoutiClothing.Entities
{
    //<IdentityUser> is the default calss for the User Account. Same is used in Model -Appuser.cs
    //builder.Services.AddDefaultIdentity<IdentityUser>()
    //    .AddEntityFrameworkStores<AppDbContext>();
    // The <IdentityUser> is same declared in the Program.cs
    public class Appuser : IdentityUser
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
    }
}
