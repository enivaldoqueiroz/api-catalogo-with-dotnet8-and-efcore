using api_catalogo.Models;
using api_catalogo.Pagination;

namespace api_catalogo.Repository
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        PagedList<Produto> GetProdutosParameter(ProdutosParameters produtosParameters);

        IEnumerable<Produto> GetProdutosPorPreco();
    }
}
