using api_catalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace api_catalogo.Context;

public class EntityFrameworkCoreDbContext : DbContext
{
    protected EntityFrameworkCoreDbContext(DbContextOptions<EntityFrameworkCoreDbContext> options) : base(options) {  }

    public DbSet<Categoria>? Categorias { get; set; }
    public DbSet<Produto>? Produtos { get; set; }
}
