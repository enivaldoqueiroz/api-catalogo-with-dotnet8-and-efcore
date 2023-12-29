using System.Linq.Expressions;

namespace api_catalogo.Repository.Interfaces
{
    public interface IRepository<T> //Interface Generica
    {
        IQueryable<T> Get();
        T GetById(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
