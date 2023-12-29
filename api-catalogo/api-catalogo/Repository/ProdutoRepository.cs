using api_catalogo.Context;
using api_catalogo.Models;
using api_catalogo.Pagination;

namespace api_catalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public PagedList<Produto> GetProdutosParameter(ProdutosParameters produtosParameters)
        {
            return PagedList<Produto>.ToPagedList(Get().OrderBy(on => on.ProdutoId), 
                produtosParameters.PageNumber, produtosParameters.PageSize);
        }

        // O método GetProdutos retorna uma lista paginada de produtos com base nos parâmetros fornecidos.

        // Parâmetros:
        // - produtosParameters: Objeto contendo informações sobre a paginação, como número da página e tamanho da página.
        //public IEnumerable<Produto> GetProdutosParameter(ProdutosParameters produtosParameters)
        //{
        //    // Chama o método Get para obter todos os produtos.
        //    return Get()
        //        // Ordena os produtos pelo nome em ordem ascendente.
        //        .OrderBy(on => on.Nome)
        //        // Pula os registros necessários com base no número da página e tamanho da página.
        //        .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
        //        // Seleciona a quantidade de registros especificada pelo tamanho da página.
        //        .Take(produtosParameters.PageSize)
        //        // Converte os resultados para uma lista.
        //        .ToList();
        //}

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy(c => c.Preco).ToList();
        }
    }
}
