namespace DomainFramework.Core
{
    /// <summary>
    /// Defines a write only repository
    /// </summary>
    public interface ICommandRepository : IRepository
    {
        void Save(IEntity entity, IUnitOfWork unitOfWork = null);

        void Insert(IEntity entity, IUnitOfWork unitOfWork = null);

        bool Update(IEntity entity, IUnitOfWork unitOfWork = null);

        bool Delete(IEntity entity, IUnitOfWork unitOfWork = null);
    }
}