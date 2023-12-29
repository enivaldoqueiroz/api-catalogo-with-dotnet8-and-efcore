namespace api_catalogo.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IProdutoRepository ProdutoRepository { get; }

        ICategoriaRepository CategoriaRepository { get; }

        void Commit();

        void Rollback();
    }
}
