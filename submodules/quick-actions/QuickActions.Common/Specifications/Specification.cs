using Serialize.Linq.Serializers;
using System.Linq.Expressions;

namespace QuickActions.Common.Specifications
{
    public class Specification<T> : ISpecification<T> where T : class
    {
        public string ExpressionString { get; set; }
        public string[] IncludesString { get; set; }

        protected Expression<Func<T, bool>> Predicate
        {
            get => string.IsNullOrWhiteSpace(ExpressionString) ? null : (Expression<Func<T, bool>>)serializer.DeserializeText(ExpressionString);
            set => ExpressionString = serializer.SerializeText(value);
        }

        protected List<Expression<Func<T, object>>> Includes
        {
            get => IncludesString == null || !IncludesString.Any() ? new List<Expression<Func<T, object>>>() : IncludesString.Select(s => (Expression<Func<T, object>>)serializer.DeserializeText(s)).ToList();
            set => IncludesString = value.Select(ex => serializer.SerializeText(ex)).ToArray();
        }

        private Func<T, bool> _function;
        private Func<T, bool> Function => _function ??= Predicate.Compile();

        private readonly ExpressionSerializer serializer = new(new JsonSerializer());

        public Specification() { }

        public Specification(Expression<Func<T, bool>> predicate)
        {
            Predicate = predicate;
        }

        public bool IsSatisfiedBy(T entity)
        {
            return Function.Invoke(entity);
        }

        public Expression<Func<T, bool>> GetExpression()
        {
            return Predicate ?? (t => true);
        }

        public List<Expression<Func<T, object>>> GetIncludes()
        {
            return Includes;
        }

        public Specification<T> Include(Expression<Func<T, object>> expression)
        {
            Includes = new List<Expression<Func<T, object>>>(Includes) { expression };
            return this;
        }

        public static implicit operator Func<T, bool>(Specification<T> spec)
        {
            return spec.Function;
        }

        public static implicit operator Expression<Func<T, bool>>(Specification<T> spec)
        {
            return spec.Predicate;
        }

        public static bool operator true(Specification<T> spec)
        {
            return false;
        }

        public static bool operator false(Specification<T> spec)
        {
            return false;
        }

        public static Specification<T> operator !(Specification<T> spec)
        {
            return new Specification<T>(
                Expression.Lambda<Func<T, bool>>(
                    Expression.Not(spec.Predicate.Body),
                    spec.Predicate.Parameters));
        }

        public static Specification<T> operator &(Specification<T> left, Specification<T> right)
        {
            var leftExpr = left.Predicate;
            var rightExpr = right.Predicate;
            var leftParam = leftExpr.Parameters[0];
            var rightParam = rightExpr.Parameters[0];

            return new Specification<T>(
                Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(
                        leftExpr.Body,
                        new ParameterReplacer(rightParam, leftParam).Visit(rightExpr.Body)),
                    leftParam));
        }

        public static Specification<T> operator |(Specification<T> left, Specification<T> right)
        {
            var leftExpr = left.Predicate;
            var rightExpr = right.Predicate;
            var leftParam = leftExpr.Parameters[0];
            var rightParam = rightExpr.Parameters[0];

            return new Specification<T>(
                Expression.Lambda<Func<T, bool>>(
                    Expression.OrElse(
                        leftExpr.Body,
                        new ParameterReplacer(rightParam, leftParam).Visit(rightExpr.Body)),
                    leftParam));
        }
    }
}