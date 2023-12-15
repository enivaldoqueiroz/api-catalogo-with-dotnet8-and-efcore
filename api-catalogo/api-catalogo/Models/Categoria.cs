using System.Collections.ObjectModel;

namespace api_catalogo.Models;

public class Categoria
{
    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }

    public int CategoriaId { get; set; }

    public string? Nome { get; set; }
    public string? ImagemUrl { get; set; }

    //Propriedade de navegação
    public ICollection<Produto> Produtos { get; set; }
}
