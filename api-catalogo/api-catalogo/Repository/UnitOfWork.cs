using api_catalogo.Context;
using api_catalogo.Repository.Interfaces;

namespace api_catalogo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ProdutoRepository _produtoRepository;
        private CategoriaRepository _categoriaRepository;
        public AppDbContext _appDbContext;
        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // Propriedade ProdutoRepository:
        // - Esta propriedade é uma instância do IProdutoRepository, que representa um repositório de produtos.
        public IProdutoRepository ProdutoRepository
        {
            get
            {
                // Se a instância do _produtoRepository ainda não foi inicializada, cria uma nova instância
                // do ProdutoRepository, utilizando o contexto do aplicativo (_appDbContext).
                return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_appDbContext);
            }
        }

        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepository ?? new CategoriaRepository(_appDbContext);
            }
        }

        public async Task Commit()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _appDbContext.Dispose();
        }

    }
}
