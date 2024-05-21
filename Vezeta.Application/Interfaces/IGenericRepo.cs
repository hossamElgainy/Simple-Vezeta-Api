using System.Linq.Expressions;
using Vezeta.Application.Specification;

namespace Vezeta.Application.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {

        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetBYIdAsync(string id);
        Task<T> GetByAnyColumn(Expression<Func<T, bool>> match);



        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<T> GetEntityWithSpecAsync(ISpecification<T> spec);
        Task<int> GetCountWithSpecAsync(ISpecification<T> spec);

        Task Add(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
      
    }
}
