using api_catalogo.Context;
using api_catalogo.DTOs.Mappings;
using api_catalogo.Extensions;
using api_catalogo.Filters;
using api_catalogo.Logging;
using api_catalogo.Repository;
using api_catalogo.Repository.Interfaces;
using api_catalogo.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Asp.Versioning;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//M�todo respons�vel por habiltar os servi�os dos controladores 
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions
    .ReferenceHandler = ReferenceHandler.IgnoreCycles);//Ignora uma referencia cicla

builder.Services.AddEndpointsApiExplorer();

//Configurando o Swagger para usar o token JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "APICatalogo", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Header de autoriza��o JWT usando o esquema Bearer.\r\n\r\nInforme 'Bearer'[e�spa�o] e o seu token.\r\n\r\nExamplo: \'Bearer 12345abcdef\'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    //Inserindo comentario comentario XML nos EndPoints
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

//ConnectionStrings
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//Configura��o do banco
builder.Services.AddDbContext<AppDbContext>
    (opts =>
    {
        opts.UseNpgsql(connectionString);
    });

//Add Servi�os do Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//Add Servi�o do Token JWT
/*
 Adiciona o manipulador de autendicacao e define o
 esquema de autenticacao usado : Bearer
 valida o emissor, a audiencia e a chave
 usando a chave secreta valida a assinatura
 */
builder.Services.AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidAudience = builder.Configuration["TokenConfiguration:Audience"],
                ValidIssuer = builder.Configuration["TokenConfiguration:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:key"]))
            });

builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddTransient<IMeuServico, MeuServico>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//Registro do servi�o de Mapeamento do AutoMapper
var mappingConfig = new MapperConfiguration(mc => 
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

//Habilitando CORS para usar via parametro
//builder.Services.AddCors(optons =>
//{
//    optons.AddPolicy("PermitirApiRequest",
//        builder =>
//        builder.WithOrigins("http://www.apirequest.io")
//                .WithMethods("GET"));
//});

//Servi�o de versionamento na API
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1,0);
    options.ReportApiVersions = true;
});

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

//Middleware de roteamento
app.UseRouting();
//Middleware que habilita a autenticacao
app.UseAuthentication();
//Middleware que habilita a autorizacao
app.UseAuthorization();

//Determina que a api catalogo possa rebecer requisi��es do apirequest.oi
//AllowAnyMethod() permite todos os m�todos HTTP
//With.Methods("GET") restringe a requisi��o para apenas o verbo GET
//app.UseCors(ops => ops
//    .WithOrigins("https://www.apirequest.oi/")
//    .WithMethods("GET"));

app.MapControllers();//M�todo respons�vel por mapear os Endpoint dos controladores

app.Run();
