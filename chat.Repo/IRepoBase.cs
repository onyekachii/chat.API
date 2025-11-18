using chat.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace chat.Repo
{
    public interface IRepoBase<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> exp);
        Task CreateAsync(T entity);
        void Update(T entity);
        void Delete(IBaseEntity entity);
        void HardDelete(T entity);
        Task DeleteAllAsync();
    }
}
