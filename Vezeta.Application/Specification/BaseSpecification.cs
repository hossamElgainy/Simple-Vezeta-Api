
using System.Linq.Expressions;

namespace Vezeta.Application.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : class, new()
    {
        public Expression<Func<T, bool>> Criteria { get; set; } //where
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        //public List<Func<IQueryable<T>, IIncludableQueryable<T, object>>> Includes { get; set; } = new();
        public List<string> IncludeStrings { get; } = new List<string>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        //pagination
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

        public BaseSpecification()
        {

        }
        public BaseSpecification(Expression<Func<T, bool>> criteriaEx)
        {
            Criteria = criteriaEx;

        }


        public void AddOrderBy(Expression<Func<T, object>> orderByEx)
        {
            OrderBy = orderByEx;
        }


        public void AddOrderByDsc(Expression<Func<T, object>> orderByDscEx)
        {
            OrderByDesc = orderByDscEx;
        }

        public void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }
        public void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

    }
}
