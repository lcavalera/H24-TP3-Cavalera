using Microsoft.OpenApi.Models;
using ModernRecrut.Favoris.ApplicationCore.Interfaces;
using ModernRecrut.Favoris.ApplicationCore.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddResponseCaching();
builder.Services.AddMemoryCache(options =>
{
    options.SizeLimit = 5000000;
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Favoris",
        Version = "v1",
        Description = "API pour la gestion des favoris pour MODERN RECRUT",
        License = new OpenApiLicense
        {
            Name = "Apache 2.0",
            Url = new Uri("http://www.apache.org")
        },
        Contact = new OpenApiContact
        {
            Name = "Luca Cavalera, Frederic Laverdiere",
            Email = "helloWorld@gmail.com",
            Url = new Uri("https://google.com/")
        }
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<IFavorisService, FavorisService>();
var app = builder.Build();
app.UseResponseCaching();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
