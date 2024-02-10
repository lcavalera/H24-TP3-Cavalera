using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ModernRecrut.Documents.ApplicationCore.Interfaces;
using ModernRecrut.Documents.ApplicationCore.Services;
using ModernRecrut.Postulations.ApplicationCore.Interfaces;
using ModernRecrut.Postulations.ApplicationCore.Services;
using ModernRecrut.Postulations.Infrastructure;
using ModernRecrut.Postulations.Infrastructure.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Postulation",
        Version = "v1",
        Description = "API pour la gestion des postulation pour MODERN RECRUT",
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

builder.Services.AddDbContext<GestPostulationContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));

builder.Services.AddScoped<IPostulationService, PostulationService>();
builder.Services.AddScoped<IGenererEvaluation, GenererEvaluation>();

var app = builder.Build();

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
