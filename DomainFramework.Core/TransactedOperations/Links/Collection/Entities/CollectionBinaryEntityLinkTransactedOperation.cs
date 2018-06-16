using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class CollectionBinaryEntityLinkTransactedOperation<TEntity, TLinkedEntity, TBinaryEntity> : ITransactedOperation
        where TEntity : IEntity
        where TLinkedEntity : IEntity
        where TBinaryEntity : IEntity
    {
        private IEntity _rootEntity;

        private List<TLinkedEntity> _linkedEntities = new List<TLinkedEntity>();

        public IEnumerable<TLinkedEntity> LinkedEntities => _linkedEntities;

        private List<TBinaryEntity> _binaryEntities = new List<TBinaryEntity>();

        public IEnumerable<TBinaryEntity> BinaryEntities => _binaryEntities;

        public bool RequiresUnitOfWork => _linkedEntities.Any(); // Two saves for each linked entity

        public CollectionBinaryEntityLinkTransactedOperation(TEntity rootEntity)
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

        public void AddBinaryEntity(TBinaryEntity binaryEntity)
        {
            _binaryEntities.Add(binaryEntity);
        }

        public void Execute(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var i = 0;

            foreach (var linkedEntity in _linkedEntities)
            {
                var linkedEntityRepository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TLinkedEntity));

                linkedEntityRepository.TransferEntities = () => new IEntity[] { _rootEntity };

                linkedEntityRepository.Save(linkedEntity, user, unitOfWork);

                var joinEntityRepository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TBinaryEntity));

                joinEntityRepository.TransferEntities = () => new IEntity[] { _rootEntity, linkedEntity };

                joinEntityRepository.Save(_binaryEntities[i++], user, unitOfWork);
            }
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var tasks = new Queue<Task>();

            var i = 0;

            foreach (var linkedEntity in _linkedEntities)
            {
                var linkedEntityRepository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TLinkedEntity));

                linkedEntityRepository.TransferEntities = () => new IEntity[] { _rootEntity };

                tasks.Enqueue(
                    linkedEntityRepository.SaveAsync(linkedEntity, user, unitOfWork)
                );

                var joinEntityRepository = (ICommandEntityRepository)repositoryContext.CreateCommandRepository(typeof(TBinaryEntity));

                joinEntityRepository.TransferEntities = () => new IEntity[] { _rootEntity, linkedEntity };

                tasks.Enqueue(
                    joinEntityRepository.SaveAsync(_binaryEntities[i++], user, unitOfWork)
                );
            }

            await Task.WhenAll(tasks);
        }
    }
}
