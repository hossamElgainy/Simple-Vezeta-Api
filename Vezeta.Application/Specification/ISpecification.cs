using System.Linq.Expressions;

namespace Vezeta.Application.Specification
{

    public interface ISpecification<T> where T : class
    {
        //Criteria => value of the where condition
        public Expression<Func<T, bool>> Criteria { get; set; }

        //Includes signature [entity may have one navigational prop or more]
        public List<Expression<Func<T, object>>> Includes { get; set; }
        public List<string> IncludeStrings { get; }

        //orderBy props signature
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }

        //pagination props signature
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }
    }
}
