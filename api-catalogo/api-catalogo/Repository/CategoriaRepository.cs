using api_catalogo.Context;
using api_catalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace api_catalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(x => x.Produtos);
        }
    }
}
