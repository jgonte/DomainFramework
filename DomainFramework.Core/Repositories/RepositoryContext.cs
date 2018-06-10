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
        private Dictionary<Type, Func<ICommandRepository>> _commandRepositoryFactoryMap { get; set; } = new Dictionary<Type, Func<ICommandRepository>>();

        /// <summary>
        /// Maps the entity to the repository to retrieve the repository for the entity
        /// One entity can be mapped to one repository
        /// </summary>
        private Dictionary<Type, IQueryRepository> _queryRepositoryMap { get; set; } = new Dictionary<Type, IQueryRepository>();

        public void RegisterCommandRepositoryFactory<EntityType>(Func<ICommandRepository> factory)
        {
            _commandRepositoryFactoryMap.Add(typeof(EntityType), factory);
        }

        public ICommandRepository CreateCommandRepository(Type entityType)
        {
            if (!_commandRepositoryFactoryMap.ContainsKey(entityType))
            {
                throw new InvalidOperationException($"Repository factory not found for entity type: {entityType}");
            }

            var repository = _commandRepositoryFactoryMap[entityType]();

            repository.ConnectionName = ConnectionName;

            return repository;
        }

        public void RegisterQueryRepository<EntityType>(IQueryRepository repository)
        {
            if (string.IsNullOrWhiteSpace(repository.ConnectionName))
            {
                repository.ConnectionName = ConnectionName;
            }

            _queryRepositoryMap.Add(typeof(EntityType), repository);
        }

        public IQueryRepository GetQueryRepository(Type type)
        {
            return _queryRepositoryMap[type];
        }

        public abstract IUnitOfWork CreateUnitOfWork();
    }
}