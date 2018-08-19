using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class PersonSpouseQueryAggregate : QueryAggregate<PersonEntity2, int?>
    {
        public GetSingleLinkedEntityLoadOperation<PersonEntity2> GetSpouseLoadOperation { get; }

        public PersonEntity2 Spouse => GetSpouseLoadOperation.LinkedEntity;

        public PersonSpouseQueryAggregate(DataAccess.RepositoryContext context) : base(context)
        {
            GetSpouseLoadOperation = new GetSingleLinkedEntityLoadOperation<PersonEntity2>
            {
                GetLinkedEntity = (repository, entity, user) =>
                    ((PersonQueryRepository2)repository).GetForPerson(RootEntity.Id, user: null),

                GetLinkedEntityAsync = async (repository, entity, user) =>
                    await ((PersonQueryRepository2)repository).GetForPersonAsync(RootEntity.Id, user: null)
            };

            LoadOperations.Enqueue(
                GetSpouseLoadOperation
            );
        }

    }
}