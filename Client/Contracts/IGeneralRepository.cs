using API.Utilities.Handler;

namespace Client.Contracts;

public interface IGeneralRepository<TEntity>
    where TEntity : class
{
    Task<ResponseHandler<IEnumerable<TEntity>>> Get();
    Task<ResponseHandler<TEntity>> Get(Guid guid);
    Task<ResponseHandler<TEntity>> Post(TEntity entity);
    Task<ResponseHandler<TEntity>> Delete(Guid guid);
}
