namespace api_catalogo.Repository
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepository { get; }

        ICategoriaRepository CategoriaRepository { get; }

        void Commit();

        void Rollback();
    }
}
