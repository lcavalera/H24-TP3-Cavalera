using Azure.Storage.Blobs;
using Microsoft.OpenApi.Models;
using ModernRecrut.Documents.ApplicationCore.Interfaces;
using ModernRecrut.Documents.ApplicationCore.Services;
using System.Reflection;
using Microsoft.Extensions.Azure;

const string chemin = "wwwroot\\Documents";

if (!Directory.Exists(chemin))
{
    Directory.CreateDirectory(chemin);
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Documents",
        Version = "v1",
        Description = "API pour la gestion des documents pour MODERN RECRUT",
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

builder.Services.AddAzureClients
    (configure =>
    {
        configure.AddBlobServiceClient(builder.Configuration.GetConnectionString("StorageConnectionString"));
    });

builder.Services.AddScoped<IStorageServiceHelper, StorageServiceHelper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
