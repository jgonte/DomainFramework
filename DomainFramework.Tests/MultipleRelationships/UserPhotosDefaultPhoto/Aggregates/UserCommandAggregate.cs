using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class UserCommandAggregate : CommandAggregate<UserEntity>
    {
        private CollectionEntityLinkTransactedOperation<UserEntity, PhotoEntity> _userPhotosLinks { get; set; }

        public IEnumerable<PhotoEntity> Photos => _userPhotosLinks.LinkedEntities;

        public UserCommandAggregate(DataAccess.RepositoryContext context, UserEntity entity) : base(context, entity)
        {
            TransactedSaveOperations.Enqueue(
                new SaveEntityTransactedOperation<UserEntity>(entity)
            );

            _userPhotosLinks = new CollectionEntityLinkTransactedOperation<UserEntity, PhotoEntity>(entity);

            TransactedSaveOperations.Enqueue(_userPhotosLinks);
        }

        public void AddPhoto(PhotoEntity photo)
        {
            _userPhotosLinks.AddLinkedEntity(photo);
        }

        public void SetDefaultPhoto(PhotoEntity defaultPhoto)
        {
            TransactedSaveOperations.Enqueue(
                new SingleEntityLinkTransactedOperation<UserEntity, PhotoEntity>(RootEntity, defaultPhoto)
            );
        }
    }
}