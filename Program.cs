using EventManagementApp.Data;
using EventManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddDbContext<EventContext>(options =>
    options.UseSqlite("Data Source=events.db"));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=events.db"));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
});


builder.Services.AddDbContext<EventContext>(options =>
    options.UseSqlite("Data Source=events.db"));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var eventContext = services.GetRequiredService<EventManagementApp.Data.EventContext>();
        eventContext.Database.Migrate();

        var identityContext = services.GetRequiredService<EventManagementApp.Data.AppDbContext>();
        identityContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Optional: log or handle error
        Console.WriteLine($"Migration error: {ex.Message}");
    }
}

app.UseStaticFiles();
app.UseRouting();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
   pattern: "{controller=Event}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();


app.Run();
