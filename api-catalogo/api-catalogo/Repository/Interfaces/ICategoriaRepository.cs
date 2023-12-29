using api_catalogo.Models;
using api_catalogo.Pagination;

namespace api_catalogo.Repository.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategoriasParameter(CategoriasParameters categoriasParameters);

        IEnumerable<Categoria> GetCategoriasProdutos();
    }
}
