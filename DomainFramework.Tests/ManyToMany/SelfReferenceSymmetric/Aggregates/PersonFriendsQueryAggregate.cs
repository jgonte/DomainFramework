using DomainFramework.Core;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests
{
    class PersonFriendsQueryAggregate : GetByIdQueryAggregate<PersonEntity, int?, object>
    {
        public GetCollectionLinkedEntityLoadOperation<PersonEntity> GetFriendsLoadOperation { get; }

        public IEnumerable<PersonEntity> Friends => GetFriendsLoadOperation.LinkedEntities;

        public PersonFriendsQueryAggregate(RepositoryContext context) : base(context)
        {
            GetFriendsLoadOperation = new GetCollectionLinkedEntityLoadOperation<PersonEntity>
            {
                GetLinkedEntities = (repository, entity, user) =>
                    ((PersonQueryRepository3)repository).GetForPerson(RootEntity.Id, user).ToList(),

                GetLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PersonQueryRepository3)repository).GetForPersonAsync(RootEntity.Id, user: null);

                    return entities.ToList();
                }
            };

            LoadOperations.Enqueue(
                GetFriendsLoadOperation
            );
        }

        public override object GetDataTransferObject()
        {
            return null;
        }
    }
}