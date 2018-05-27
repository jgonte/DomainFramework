using System;

namespace DomainFramework.Core
{
    public interface IRepositoryContext
    {
        /// <summary>
        /// The name of the connection
        /// </summary>
        string ConnectionName { get; set; }

        void RegisterCommandRepository<E>(ICommandRepository repository);

        ICommandRepository GetCommandRepository(Type type);

        void RegisterQueryRepository<E>(IQueryRepository repository);

        IQueryRepository GetQueryRepository(Type type);

        /// <summary>
        /// Factory to create the unit of work
        /// </summary>
        /// <returns></returns>
        IUnitOfWork CreateUnitOfWork();
    }
}