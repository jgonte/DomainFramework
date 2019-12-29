using DomainFramework.Core;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests
{
    class PersonOutputDto : IOutputDataTransferObject
    {

    }

    class PersonFriendsQueryAggregate : GetByIdQueryAggregate<PersonEntity, int?, PersonOutputDto>
    {
        public GetCollectionLinkedEntityQueryOperation<PersonEntity> GetFriendsLoadOperation { get; }

        public IEnumerable<PersonEntity> Friends => GetFriendsLoadOperation.LinkedEntities;

        public PersonFriendsQueryAggregate(RepositoryContext context) : base(context)
        {
            GetFriendsLoadOperation = new GetCollectionLinkedEntityQueryOperation<PersonEntity>
            {
                GetLinkedEntities = (repository, entity, user) =>
                    ((PersonQueryRepository3)repository).GetForPerson(RootEntity.Id).ToList(),

                GetLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PersonQueryRepository3)repository).GetForPersonAsync(RootEntity.Id);

                    return entities.ToList();
                }
            };

            QueryOperations.Enqueue(
                GetFriendsLoadOperation
            );
        }

        public override void PopulateDto(PersonEntity entity)
        {
        }
    }
}