using System.Linq.Expressions;

namespace QuickActions.Common.Specifications
{
    public interface ISpecification<T> where T : class
    {
        bool IsSatisfiedBy(T entity);
        Expression<Func<T, bool>> GetExpression();
        List<Expression<Func<T, object>>> GetIncludes();
    }
}
