using Core.Infrastructures;
using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer("server=.;initial catalog=InqueryInsurance;user id=sa;password=1234;TrustServerCertificate=True", b => b.MigrationsAssembly("InqueryInsurance")));
builder.Services.AddIdentity<User, IdentityRole<long>>(x =>
{
    x.Password.RequireUppercase = false;
    x.Password.RequireLowercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequiredLength = 4;
}).AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<InsurancePatternService>();
builder.Services.AddScoped<RequestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
