using Microsoft.EntityFrameworkCore;
using QuickActions.Common.Specifications;
using System.Linq.Expressions;

namespace QuickActions.Api
{
    public class CrudRepository<TEntity> where TEntity : class
    {
        private readonly DbContext dbContext;
        private readonly DbSet<TEntity> dbSet;

        public CrudRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<TEntity>();
        }

        public virtual Task Create(TEntity entity)
        {
            return Create(new[] { entity });
        }

        public virtual async Task Create(IEnumerable<TEntity> entities)
        {
            await dbSet.AddRangeAsync(entities);
            await dbContext.SaveChangesAsync();
        }

        public virtual async Task<TEntity> Read(ISpecification<TEntity> specification)
        {
            return (await Read(specification, 0, 1)).FirstOrDefault(); ;
        }

        public virtual Task<List<TEntity>> Read(ISpecification<TEntity> specification, int start, int skip)
        {
            var entry = IncludeProperties(specification.GetIncludes());
            return entry.Where(specification.GetExpression()).Skip(start).Take(skip).AsNoTrackingWithIdentityResolution().ToListAsync();
        }

        public virtual async Task Update(TEntity entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(ISpecification<TEntity> specification)
        {
            var entitiesToRemove = await Read(specification, 0, int.MaxValue);
            dbSet.RemoveRange(entitiesToRemove);
            await dbContext.SaveChangesAsync();
        }

        protected IQueryable<TEntity> IncludeProperties(IEnumerable<Expression<Func<TEntity, object>>> includeProperties)
        {
            if (!includeProperties.Any()) return dbSet;
            return includeProperties.Aggregate(dbSet.AsNoTracking(), (query, includeProperty) => query.Include(includeProperty));
        }
    }
}