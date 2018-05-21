namespace DomainFramework.Core
{
    public interface ICommandAggregate<TEntity> : IAggregate<TEntity>
    {
        void Save(IUnitOfWork unitOfWork = null);

        void Delete(IUnitOfWork unitOfWork = null);
    }
}