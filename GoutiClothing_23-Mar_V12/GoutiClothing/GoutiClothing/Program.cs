
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GoutiClothing;
using Microsoft.AspNetCore.Identity.UI.Services;
using GoutiClothing.Utility;

var builder = WebApplication.CreateBuilder(args);




builder.Services.AddDbContext<AppDataContext>(c =>
c.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));



builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDataContext>().AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddDbContext<AppDataContext>(options =>
    options.UseSqlServer("DatabaseConnection"));

//Cutomcookies are used since the default identidy user is replaced.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});


// add repository dependecy



// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
