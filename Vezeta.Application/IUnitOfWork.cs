

using Vezeta.Application.Interfaces;

namespace Vezeta.Application
{
    public interface IUnitOfWork: IAsyncDisposable
    {
        IGenericRepo<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> Complete();
        public void Rollback();

    }
}
