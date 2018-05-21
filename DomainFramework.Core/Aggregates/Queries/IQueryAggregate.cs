namespace DomainFramework.Core
{
    public interface IQueryAggregate<TEntity> : IAggregate<TEntity>
    {
        object Load(IUnitOfWork unitOfWork = null);
    }
}
