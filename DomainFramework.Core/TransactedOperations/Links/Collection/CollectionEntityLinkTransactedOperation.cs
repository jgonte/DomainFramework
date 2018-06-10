using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class CollectionEntityLinkTransactedOperation<TEntity, TLinkedEntity> : ITransactedOperation
        where TEntity : IEntity
        where TLinkedEntity : IEntity
    {
        private IEntity _rootEntity;

        private List<TLinkedEntity> _linkedEntities = new List<TLinkedEntity>();

        public IEnumerable<TLinkedEntity> LinkedEntities => _linkedEntities;

        public bool RequiresUnitOfWork => _linkedEntities.Count > 1; // One save per linked entity

        public CollectionEntityLinkTransactedOperation(TEntity rootEntity)
        {
            if (rootEntity == null)
            {
                throw new ArgumentNullException(nameof(rootEntity));
            }

            _rootEntity = rootEntity;
        }

        public void AddLinkedEntity(TLinkedEntity linkedEntity)
        {
            _linkedEntities.Add(linkedEntity);
        }

        public void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            foreach (var linkedEntity in _linkedEntities)
            {
                var repository = repositoryContext.CreateCommandRepository(typeof(TLinkedEntity));

                repository.TransferEntities = () => new IEntity[] { _rootEntity };

                repository.Save(linkedEntity, user, unitOfWork);
            }
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var tasks = new Queue<Task>();

            foreach (var linkedEntity in _linkedEntities)
            {
                var repository = repositoryContext.CreateCommandRepository(typeof(TLinkedEntity));

                repository.TransferEntities = () => new IEntity[] { _rootEntity };

                tasks.Enqueue(
                    repository.SaveAsync(linkedEntity, user, unitOfWork)
                );
            }

            await Task.WhenAll(tasks);
        }
    }
}
