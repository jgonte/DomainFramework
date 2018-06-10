using System;
using System.Threading.Tasks;

namespace DomainFramework.Core
{
    public class SaveBinaryEntityTransactedOperation<TBinaryEntity, TLinkedEntity1, TLinkedEntity2> : ITransactedOperation
        where TBinaryEntity : IEntity
        where TLinkedEntity1 : IEntity
        where TLinkedEntity2 : IEntity
    {
        private TBinaryEntity _binaryEntity;

        private TLinkedEntity1 _linkedEntity1;

        private TLinkedEntity2 _linkedEntity2;

        public bool RequiresUnitOfWork => false; // By itself does not require a transaction (one save)

        public SaveBinaryEntityTransactedOperation(TBinaryEntity binaryEntity, TLinkedEntity1 linkedEntity1, TLinkedEntity2 linkedEntity2)
        {
            if (binaryEntity == null)
            {
                throw new ArgumentNullException(nameof(binaryEntity));
            }

            if (linkedEntity1 == null)
            {
                throw new ArgumentNullException(nameof(linkedEntity1));
            }

            if (linkedEntity2 == null)
            {
                throw new ArgumentNullException(nameof(linkedEntity2));
            }

            _binaryEntity = binaryEntity;

            _linkedEntity1 = linkedEntity1;

            _linkedEntity2 = linkedEntity2;
        }

        public void Execute(IRepositoryContext repositoryContext,  IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = repositoryContext.CreateCommandRepository(typeof(TBinaryEntity));

            repository.TransferEntities = () => new IEntity[] { _linkedEntity1, _linkedEntity2 };

            repository.Save(_binaryEntity, user, unitOfWork);
        }

        public async Task ExecuteAsync(IRepositoryContext repositoryContext, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var repository = repositoryContext.CreateCommandRepository(typeof(TBinaryEntity));

            repository.TransferEntities = () => new IEntity[] { _linkedEntity1, _linkedEntity2 };

            await repository.SaveAsync(_binaryEntity, user, unitOfWork);
        }
    }
}
