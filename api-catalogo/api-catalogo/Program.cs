using api_catalogo.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();//Método responsável por habiltar os serviços dos controladores//Método responsável por habiltar os serviços dos controladores 
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();//Método responsável por mapear os Endpoint dos controladores

app.Run();
