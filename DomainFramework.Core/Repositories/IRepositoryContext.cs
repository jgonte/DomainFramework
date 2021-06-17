using System;
using System.Collections.Generic;

namespace DomainFramework.Core
{
    /// <summary>
    /// The information shared across all the command operations of the command aggregate
    /// </summary>
    public interface IRepositoryContext
    {
        /// <summary>
        /// The name of the connection
        /// </summary>
        string ConnectionName { get; set; }

        /// <summary>
        /// The dependencies to entities either passed to the aggregate or being created using the command operations
        /// </summary>
        List<EntityDependency> Dependencies { get; set; }

        void RegisterCommandRepositoryFactory<EntityType>(Func<ICommandRepository> repository);

        ICommandRepository CreateCommandRepository(Type type);

        void RegisterQueryRepository<EntityType>(IQueryRepository repository);

        IQueryRepository GetQueryRepository(Type type);

        /// <summary>
        /// Factory to create the unit of work
        /// </summary>
        /// <returns></returns>
        IUnitOfWork CreateUnitOfWork();
    }
}