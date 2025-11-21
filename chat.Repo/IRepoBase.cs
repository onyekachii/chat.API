using chat.Domain.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace chat.Repo
{
    public interface IRepoBase<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> exp);
        Task<EntityEntry<T>> CreateAsync(T entity);
        void Update(T entity);
        void Delete(IBaseEntity entity);
        void HardDelete(T entity);
    }
}
