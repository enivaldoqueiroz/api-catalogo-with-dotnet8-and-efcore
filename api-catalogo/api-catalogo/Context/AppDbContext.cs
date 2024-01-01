using api_catalogo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api_catalogo.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Categoria>? Categorias { get; set; }
        public DbSet<Produto>? Produtos { get; set; }

        //Usando EntityFrameworkCore Fluent API para deixar os models de Categoria e Produtos sem as DataAnnotations
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //Implementando a Fluent API para a a model Categoria

        //    //Categoria
        //    modelBuilder.Entity<Categoria>().HasKey(c => c.CategoriaId);
        //    modelBuilder.Entity<Categoria>().Property(c => c.Nome).HasMaxLength(80).IsRequired();
        //    modelBuilder.Entity<Categoria>().Property(c => c.ImagemUrl).HasMaxLength(300).IsRequired();

        //    //Produto
        //    modelBuilder.Entity<Produto>().HasKey(c => c.ProdutoId);
        //    modelBuilder.Entity<Produto>().Property(c => c.Nome).HasMaxLength(80).IsRequired();
        //    modelBuilder.Entity<Produto>().Property(c => c.Descricao).HasMaxLength(300).IsRequired();
        //    modelBuilder.Entity<Produto>().Property(c => c.Preco).HasPrecision(14,2).IsRequired();
        //    modelBuilder.Entity<Produto>().Property(c => c.ImagemUrl).HasMaxLength(300).IsRequired();

        //    //Relacionamento
        //    modelBuilder.Entity<Produto>()
        //        .HasOne<Categoria>(c => c.Categoria) //Uma Categoria
        //            .WithMany(p => p.Produtos) //Para muitos Produtos
        //                .HasForeignKey(c => c.CategoriaId); //Chave estrangeira de Categoria
        //}
    }
}
