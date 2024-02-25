using QuickActions.Common.Specifications;
using Refit;

namespace QuickActions.Common.Interfaces
{
    public interface ICrud<TEntity> where TEntity : class
    {
        [Post("/")]
        public Task Create([Body] TEntity entity);
        [Post("/Many")]
        public Task Create([Body] IEnumerable<TEntity> entities);
        [Post("/Read")]
        public Task<TEntity> Read([Body] Specification<TEntity> specification);
        [Post("/Read/Many")]
        public Task<List<TEntity>> Read([Body] Specification<TEntity> specification, [Query] int start, [Query] int skip);
        [Patch("/")]
        public Task Update([Body] TEntity entity);
        [Delete("/")]
        public Task Delete([Body] Specification<TEntity> specification);
    }
}