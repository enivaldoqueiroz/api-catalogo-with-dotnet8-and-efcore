using api_catalogo.Context;
using api_catalogo.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace api_catalogo.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AppDbContext _appDbContext;

        public Repository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IQueryable<T> Get() //Lista de entidades
        {
            return _appDbContext.Set<T>().AsNoTracking();
        }

        public T GetById(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            return _appDbContext.Set<T>().AsNoTracking().SingleOrDefault(predicate);
        }

        //O método Set<T> do contexto retorna uma instância DbSet<T>
        //para o acesso a entidades de determinado tipo no contexto
        public void Add(T entity)
        {
            _appDbContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _appDbContext.Entry(entity).State = EntityState.Modified;
            _appDbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _appDbContext.Entry(entity).State = EntityState.Modified;
            _appDbContext.Remove(entity);
        }
    }
}
