using System;

namespace DomainFramework.Core
{
    public interface IRepositoryContext
    {
        /// <summary>
        /// The name of the connection
        /// </summary>
        string ConnectionName { get; set; }

        void RegisterCommandRepositoryFactory<EntityType>(Func<ICommandRepository> repository);

        ICommandRepository CreateCommandRepository(Type type);

        void RegisterQueryRepository<EntityType>(IEntityQueryRepository repository);

        IEntityQueryRepository GetQueryRepository(Type type);

        /// <summary>
        /// Factory to create the unit of work
        /// </summary>
        /// <returns></returns>
        IUnitOfWork CreateUnitOfWork();
    }
}