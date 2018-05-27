using System;
using System.Collections.Generic;

namespace DomainFramework.Core
{
    public abstract class RepositoryContext : IRepositoryContext
    {
        public string ConnectionName { get; set; }

        /// <summary>
        /// Maps the entity to the repository to retrieve the repository for the entity
        /// One entity can be mapped to one repository
        /// </summary>
        private Dictionary<Type, ICommandRepository> _commandRepositoryMap { get; set; } = new Dictionary<Type, ICommandRepository>();

        /// <summary>
        /// Maps the entity to the repository to retrieve the repository for the entity
        /// One entity can be mapped to one repository
        /// </summary>
        private Dictionary<Type, IQueryRepository> _queryRepositoryMap { get; set; } = new Dictionary<Type, IQueryRepository>();

        public void RegisterCommandRepository<E>(ICommandRepository repository)
        {
            if (string.IsNullOrWhiteSpace(repository.ConnectionName))
            {
                repository.ConnectionName = ConnectionName;
            }

            _commandRepositoryMap.Add(typeof(E), repository);
        }

        public ICommandRepository GetCommandRepository(Type type)
        {
            return _commandRepositoryMap[type];
        }

        public void RegisterQueryRepository<E>(IQueryRepository repository)
        {
            if (string.IsNullOrWhiteSpace(repository.ConnectionName))
            {
                repository.ConnectionName = ConnectionName;
            }

            _queryRepositoryMap.Add(typeof(E), repository);
        }

        public IQueryRepository GetQueryRepository(Type type)
        {
            return _queryRepositoryMap[type];
        }

        public abstract IUnitOfWork CreateUnitOfWork();
    }
}