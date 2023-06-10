using DAL.AppDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DAL.Repositories.Interfaces;
using DAL.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;
using Utilities;
using System.Text.Json.Serialization;
using DAL.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DAL.AppDbContext.DbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")    
));

builder.Services.AddIdentity<AppUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<DAL.AppDbContext.DbContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

var app = builder.Build();

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

app.UseAuthentication();

app.Use(async (context, next) =>
{
    if (!context.User.Identity.IsAuthenticated &&
        !context.Request.Path.Value.StartsWith("/Identity/Account/Login"))
    {
        if (!context.Request.Path.Value.StartsWith("/Identity/Account/Login"))
        {
            context.Response.Redirect("/Identity/Account/Login");
            return;
        }
    }

    await next.Invoke();
});

app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Professor}/{controller=Home}/{action=Index}/{id?}");

app.Run();
