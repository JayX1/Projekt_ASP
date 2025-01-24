using System.Xml.Linq;
using ASP_projekt.Interfaces;
using ASP_projekt.Models;
using ASP_projekt.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ISuperheroService, SuperheroService>();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration["AppDbContext:ConnectionString"]);
});



var app = builder.Build();

Console.WriteLine("Connection String: " + builder.Configuration.GetConnectionString("DefaultConnection"));


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Superhero}/{action=Index}/{id?}");

app.Run();

