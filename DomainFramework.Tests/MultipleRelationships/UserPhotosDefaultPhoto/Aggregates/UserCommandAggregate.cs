using DomainFramework.Core;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class UserCommandAggregate : CommandAggregate<UserEntity>
    {
        public UserCommandAggregate(IRepositoryContext context, UserInputDto userInputDto) : base(context)
        {
            RootEntity = new UserEntity
            {
                Name = userInputDto.Name
            };

            Enqueue(
                new SaveEntityCommandOperation<UserEntity>(RootEntity)
            );

            foreach (var photoDto in userInputDto.Photos)
            {
                var photoEntity = new PhotoEntity
                {
                    Description = photoDto.Description
                };

                var addPhoto = new AddLinkedEntityCommandOperation<UserEntity, PhotoEntity>(
                    RootEntity,
                    getLinkedEntity: () => photoEntity
                );

                Enqueue(addPhoto);

                if (photoDto.IsDefault)
                {
                    var updateUserWithDefaultPhoto = new UpdateEntityCommandOperation<UserEntity>(
                        RootEntity,
                        new IEntity[] { RootEntity, photoEntity}
                    );

                    Enqueue(updateUserWithDefaultPhoto);
                }
            }
        }
    }
}