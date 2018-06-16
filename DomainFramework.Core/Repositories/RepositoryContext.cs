using System;
using System.Collections.Generic;

namespace DomainFramework.Core
{
    public abstract class RepositoryContext : IRepositoryContext
    {
        public string ConnectionName { get; set; }

        /// <summary>
        /// Maps the entity or value object to a factory method to create the repository
        /// Only one entity or value object can be mapped to one repository
        /// </summary>
        // A value object needs to be mapped to a repository when one entity contains several value objects
        private Dictionary<Type, Func<ICommandRepository>> _commandRepositoryFactoryMap { get; set; } = new Dictionary<Type, Func<ICommandRepository>>();

        /// <summary>
        /// Maps the entity to the repository to retrieve the repository for the entity
        /// One entity can be mapped to one repository
        /// </summary>
        private Dictionary<Type, IQueryRepository> _queryRepositoryMap { get; set; } = new Dictionary<Type, IQueryRepository>();

        public void RegisterCommandRepositoryFactory<EntityOrValueObjectType>(Func<ICommandRepository> factory)
        {
            _commandRepositoryFactoryMap.Add(typeof(EntityOrValueObjectType), factory);
        }

        public ICommandRepository CreateCommandRepository(Type entityOrValueObjectType)
        {
            if (!_commandRepositoryFactoryMap.ContainsKey(entityOrValueObjectType))
            {
                throw new InvalidOperationException($"Repository factory not found for entity type: {entityOrValueObjectType}");
            }

            var repository = _commandRepositoryFactoryMap[entityOrValueObjectType]();

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