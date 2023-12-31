using api_catalogo.Models;
using api_catalogo.Pagination;

namespace api_catalogo.Repository.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<PagedList<Categoria>> GetCategoriasParameter(CategoriasParameters categoriasParameters);

        Task<IEnumerable<Categoria>> GetCategoriasProdutos();
    }
}
