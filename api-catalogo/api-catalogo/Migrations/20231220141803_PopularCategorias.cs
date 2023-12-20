using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_catalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopularCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Insert na tabela Categoria no padrão do PostgreSQL
            migrationBuilder.Sql("INSERT INTO public.\"Categorias\"(\"Nome\", \"ImagemUrl\") VALUES ('Bebidas', 'bebidas.jpg')");
            migrationBuilder.Sql("INSERT INTO public.\"Categorias\"(\"Nome\", \"ImagemUrl\") VALUES ('Lanches', 'lanches.jpg')");
            migrationBuilder.Sql("INSERT INTO public.\"Categorias\"(\"Nome\", \"ImagemUrl\") VALUES ('Sobremesas', 'sobremesas.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM public.\"Categorias\"");
        }
    }
}
