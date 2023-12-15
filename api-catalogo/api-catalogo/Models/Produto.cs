namespace api_catalogo.Models;

public class Produto
{
    public int ProdutoId { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }

    //Propriedade de Categoria
    public int CategoriaId { get; set; }

    //Propriedade de navegação
    public Categoria Categoria { get; set; }
}
