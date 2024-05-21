


using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Vezeta.Application.Interfaces;
using Vezeta.Application.Specification;
using Vezeta.EF.Data;
using VezetaEF;

namespace Vezeta.EF.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        public GenericRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        private readonly ApplicationDbContext _context;


        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _context.Set<T>().ToListAsync();

        public async Task<T> GetBYIdAsync(string id)
            => await _context.Set<T>().FindAsync(id);


        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
          => await ApplySpecification(spec).ToListAsync();


        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
          => await ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }


        public async Task Add(T entity)
             => await _context.Set<T>().AddAsync(entity);

        public void Update(T entity)
             => _context.Set<T>().Update(entity);

        public void Delete(T entity)
             => _context.Set<T>().Remove(entity);


        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
                 => SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);

        public async Task<T> GetByAnyColumn(Expression<Func<T, bool>> match)
            =>  _context.Set<T>().SingleOrDefault(match);

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().CountAsync(predicate);
        }
             
    }

}