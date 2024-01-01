using api_catalogo.Context;
using api_catalogo.DTOs.Mappings;
using api_catalogo.Extensions;
using api_catalogo.Filters;
using api_catalogo.Logging;
using api_catalogo.Repository;
using api_catalogo.Repository.Interfaces;
using api_catalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Método responsável por habiltar os serviços dos controladores 
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions
    .ReferenceHandler = ReferenceHandler.IgnoreCycles);//Ignora uma referencia cicla


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ConnectionStrings
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Configuração do banco
builder.Services.AddDbContext<AppDbContext>
    (opts =>
    {
        opts.UseNpgsql(connectionString);
    });

//Add Serviços do JWT
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddTransient<IMeuServico, MeuServico>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Registro do serviço de Mapeamento do AutoMapper
var mappingConfig = new MapperConfiguration(mc => 
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Não esta funcionando pois esta pois esta no padrão do .Net6
ILoggerFactory loggerFactory = LoggerFactory.Create(builder => {
    builder.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
    {
        LogLevel = LogLevel.Information
    }));
});

app.ConfigureExceptionHandler();//Middle de tramento de erros personalizado

app.UseHttpsRedirection();

//Middleware de roteamento
app.UseRouting();
//Middleware que habilita a autenticacao
app.UseAuthentication();
//Middleware que habilita a autorizacao
app.UseAuthorization();

app.MapControllers();//Método responsável por mapear os Endpoint dos controladores

app.Run();
