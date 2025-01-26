using System.Xml.Linq;
using ASP_projekt;
using ASP_projekt.Interfaces;
using ASP_projekt.Models;
using ASP_projekt.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;





var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
//builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ISuperheroService, SuperheroService>();
builder.Services.AddScoped<ICustomAuthService, CustomAuthService>();



builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("AppDbConnection"));
});




var app = builder.Build();

//Console.WriteLine("Connection String: " + builder.Configuration.GetConnectionString("DefaultConnection"));


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.Use(async (context, next) =>
{
    if (!context.Request.Path.StartsWithSegments("/Login") &&
        !context.Request.Cookies.ContainsKey("IsAuthenticated"))
    {
        context.Response.Redirect("/Login/OnLogin");
    }
    else
    {
        await next();
    }
});


app.UseAuthentication();
app.UseAuthorization();

//app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Superhero}/{action=Index}/{id?}");

app.Run();

