using api_catalogo.Context;
using api_catalogo.Extensions;
using api_catalogo.Filters;
using api_catalogo.Logging;
using api_catalogo.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//M�todo respons�vel por habiltar os servi�os dos controladores 
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions
    .ReferenceHandler = ReferenceHandler.IgnoreCycles);//Ignora uma referencia cicla

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ApiLoggingFilter>();

//ConnectionStrings
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Configura��o do banco
builder.Services.AddDbContext<AppDbContext>
    (opts =>
    {
        opts.UseNpgsql(connectionString);
    });

builder.Services.AddTransient<IMeuServico, MeuServico>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//N�o esta funcionando pois esta pois esta no padr�o do .Net6
ILoggerFactory loggerFactory = LoggerFactory.Create(builder => {
    builder.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
    {
        LogLevel = LogLevel.Information
    }));
});

app.ConfigureExceptionHandler();//Middle de tramento de erros personalizado

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();//M�todo respons�vel por mapear os Endpoint dos controladores

app.Run();
