using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class CollectionValueObjectLinkTransactedOperation<TEntity, TLinkedValueObject, TRepositoryKey> : ITransactedOperation
        where TEntity : IEntity
        where TLinkedValueObject : IValueObject
    {
        private TEntity _rootEntity;

        public Func<TEntity, IEnumerable<TLinkedValueObject>> GetLinkedValueObjects { get; set; }

        public bool RequiresUnitOfWork => GetLinkedValueObjects(_rootEntity).Count() > 1; // One save per value object

        public CollectionValueObjectLinkTransactedOperation(TEntity rootEntity)
        {
            if (rootEntity == null)
            {
                throw new ArgumentNullException(nameof(rootEntity));
            }

            _rootEntity = rootEntity;
        }

        public void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            // Remove all the value objects attached to the entity
            var repository = (IValueObjectCommandRepository)repositoryContext.CreateCommandRepository(typeof(TRepositoryKey));

            repository.TransferEntities = () => new IEntity[] { _rootEntity };

            repository.DeleteAll(user, unitOfWork);

            foreach (var linkedValueObject in GetLinkedValueObjects(_rootEntity))
            {
                repository = (IValueObjectCommandRepository)repositoryContext.CreateCommandRepository(typeof(TRepositoryKey));

                repository.TransferEntities = () => new IEntity[] { _rootEntity };

                repository.Insert(linkedValueObject, user, unitOfWork);
            }
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var tasks = new Queue<Task>();

            // Remove all the value objects attached to the entity
            var repository = (IValueObjectCommandRepository)repositoryContext.CreateCommandRepository(typeof(TRepositoryKey));

            repository.TransferEntities = () => new IEntity[] { _rootEntity };

            tasks.Enqueue(
                repository.DeleteAllAsync(user, unitOfWork)
            );

            foreach (var linkedValueObject in GetLinkedValueObjects(_rootEntity))
            {
                repository = (IValueObjectCommandRepository)repositoryContext.CreateCommandRepository(typeof(TRepositoryKey));

                repository.TransferEntities = () => new IEntity[] { _rootEntity };

                tasks.Enqueue(
                    repository.InsertAsync(linkedValueObject, user, unitOfWork)
                );
            }

            await Task.WhenAll(tasks);
        }
    }
}
