using ModernRecrut.MVC.Interfaces;
using ModernRecrut.MVC.Services;
using Serilog;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModernRecrut.MVC.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("IdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'IdentityContextConnection' not found.");

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<Utilisateur>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();


builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventLog();
builder.Logging.AddSerilog();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IRolesService, RolesService>();

builder.Services.AddHttpClient<IOffreEmploiService, OffresEmploiServiceProxy>(client =>
client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlAPIOffreEmploi")));

builder.Services.AddHttpClient<IFavorisService, FavorisServiceProxy>(client
=> client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlAPIFavoris")));

builder.Services.AddHttpClient<IDocumentsService, DocumentsServiceProxy>(client
=> client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlAPIDocuments")));

builder.Services.AddHttpClient<IPostulationService, PostulationsServiceProxy>(client
=> client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("UrlAPIPostulations")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Home/CodeStatus?code={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
