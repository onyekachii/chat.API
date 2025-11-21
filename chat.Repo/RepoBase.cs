using chat.Domain;
using chat.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace chat.Repo
{
    internal abstract class RepoBase<T> : IRepoBase<T> where T : class
    {
        protected ChatContext Context { get; set; }
        protected RepoBase(ChatContext context)
        {
            Context = context;
        }

        public async Task<EntityEntry<T>> CreateAsync(T entity) => await Context.Set<T>().AddAsync(entity);

        public void HardDelete(T entity) => Context.Set<T>().Remove(entity);

        public void Delete(IBaseEntity entity) => entity.SoftDeleted = true;
                
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> exp) => Context.Set<T>().Where(exp).AsNoTracking();

        public IQueryable<T> GetAll() => Context.Set<T>().AsNoTracking();

        public void Update(T entity) => Context.Set<T>().Update(entity);
    }
}
