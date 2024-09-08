using System.Linq.Expressions;

namespace Repositories.Contracts
{
    public interface IRepositoryBase<T>
    {
        //CRUD
        IQueryable<T> FindAll(bool tractChanges);
        IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression,bool tractChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);

    }
}
