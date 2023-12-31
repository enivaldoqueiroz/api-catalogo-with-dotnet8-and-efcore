using api_catalogo.Context;
using api_catalogo.Models;
using api_catalogo.Pagination;
using api_catalogo.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api_catalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public async Task<PagedList<Categoria>> GetCategoriasParameter(CategoriasParameters categoriaParameters)
        {
            return await PagedList<Categoria>.ToPagedList(Get().OrderBy(on => on.Nome),
                categoriaParameters.PageNumber,
                categoriaParameters.PageSize);
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return await Get().Include(x => x.Produtos).ToListAsync();
        }

    }
}
