using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api_catalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopularProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Insert na tabela Categoria no padrão do PostgreSQL
            migrationBuilder.Sql("INSERT INTO public.\"Produtos\"(\"Nome\", \"Descricao\", \"Preco\", \"ImagemUrl\", \"Estoque\", \"DataCadastro\", \"CategoriaId\") " +
                                 "VALUES ('Coca-Cola Diet', 'Refrigerante de Cola 350 ml', 5.45, 'cocacola.jpg', 50, now(), 1)");

            migrationBuilder.Sql("INSERT INTO public.\"Produtos\"(\"Nome\", \"Descricao\", \"Preco\", \"ImagemUrl\", \"Estoque\", \"DataCadastro\", \"CategoriaId\") " +
                                 "VALUES ('Lanche de Atum', 'Lanche de atum com maionese', 8.50, 'atum.jpg', 10, now(), 2)");

            migrationBuilder.Sql("INSERT INTO public.\"Produtos\"(\"Nome\", \"Descricao\", \"Preco\", \"ImagemUrl\", \"Estoque\", \"DataCadastro\", \"CategoriaId\") " +
                                 "VALUES ('Pudim 100g', 'Pudim de leite condensado 100g', 6.75, 'pudim.jpg', 20, now(), 3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM public.\"Produtos\"");
        }
    }
}
