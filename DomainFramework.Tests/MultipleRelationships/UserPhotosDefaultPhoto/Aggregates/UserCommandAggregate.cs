using DomainFramework.Core;
using System.Collections.Generic;
using Utilities;

namespace DomainFramework.Tests
{
    class UserCommandAggregate : CommandAggregate<UserEntity>
    {
        private CollectionEntityLinkTransactedOperation<UserEntity, PhotoEntity> _userPhotosLinks { get; set; }

        public IEnumerable<PhotoEntity> Photos => _userPhotosLinks.LinkedEntities;

        public UserCommandAggregate(DataAccess.RepositoryContext context, UserEntity entity) : base(context, entity)
        {
            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<UserEntity>(entity, CommandOperationTypes.Save)
            );

            _userPhotosLinks = new CollectionEntityLinkTransactedOperation<UserEntity, PhotoEntity>(entity);

            TransactedOperations.Enqueue(_userPhotosLinks);
        }

        public void AddPhoto(PhotoEntity photo)
        {
            _userPhotosLinks.AddLinkedEntity(photo);
        }

        public void SetDefaultPhoto(PhotoEntity defaultPhoto)
        {
            TransactedOperations.Enqueue(
                new SingleEntityLinkTransactedOperation<UserEntity, PhotoEntity>(RootEntity, defaultPhoto)
            );
        }
    }
}