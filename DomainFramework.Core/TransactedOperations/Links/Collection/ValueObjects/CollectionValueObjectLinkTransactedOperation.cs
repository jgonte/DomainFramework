﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class CollectionValueObjectLinkTransactedOperation<TEntity, TLinkedValueObject> : ITransactedOperation
        where TEntity : IEntity
        where TLinkedValueObject : IValueObject
    {
        private IEntity _rootEntity;

        private List<TLinkedValueObject> _linkedValueObjects = new List<TLinkedValueObject>();

        public IEnumerable<TLinkedValueObject> LinkedValueObjects => _linkedValueObjects;

        public bool RequiresUnitOfWork => _linkedValueObjects.Count > 1; // One save per value object

        public CollectionValueObjectLinkTransactedOperation(TEntity rootEntity)
        {
            if (rootEntity == null)
            {
                throw new ArgumentNullException(nameof(rootEntity));
            }

            _rootEntity = rootEntity;
        }

        public void AddLinkedValueObject(TLinkedValueObject linkedValueObject)
        {
            _linkedValueObjects.Add(linkedValueObject);
        }

        public void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            // Remove all the value objects attached to the entity
            var repository = (ICommandValueObjectRepository)repositoryContext.CreateCommandRepository(typeof(TLinkedValueObject));

            repository.TransferEntities = () => new IEntity[] { _rootEntity };

            repository.DeleteAll(user, unitOfWork);

            foreach (var linkedValueObject in _linkedValueObjects)
            {
                repository = (ICommandValueObjectRepository)repositoryContext.CreateCommandRepository(typeof(TLinkedValueObject));

                repository.TransferEntities = () => new IEntity[] { _rootEntity };

                repository.Insert(linkedValueObject, user, unitOfWork);
            }
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var tasks = new Queue<Task>();

            // Remove all the value objects attached to the entity
            var repository = (ICommandValueObjectRepository)repositoryContext.CreateCommandRepository(typeof(TLinkedValueObject));

            repository.TransferEntities = () => new IEntity[] { _rootEntity };

            tasks.Enqueue(
                repository.DeleteAllAsync(user, unitOfWork)
            );

            foreach (var linkedValueObject in _linkedValueObjects)
            {
                repository = (ICommandValueObjectRepository)repositoryContext.CreateCommandRepository(typeof(TLinkedValueObject));

                repository.TransferEntities = () => new IEntity[] { _rootEntity };

                tasks.Enqueue(
                    repository.InsertAsync(linkedValueObject, user, unitOfWork)
                );
            }

            await Task.WhenAll(tasks);
        }
    }
}
