using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class PersonSpouseQueryAggregate : GetByIdQueryAggregate<PersonEntity2, int?, PersonOutputDto>
    {
        public GetSingleLinkedEntityQueryOperation<PersonEntity2> GetSpouseLoadOperation { get; }

        public PersonEntity2 Spouse => GetSpouseLoadOperation.LinkedEntity;

        public PersonSpouseQueryAggregate(DataAccess.RepositoryContext context) : base(context)
        {
            GetSpouseLoadOperation = new GetSingleLinkedEntityQueryOperation<PersonEntity2>
            {
                GetLinkedEntity = (repository, entity, user) =>
                    ((PersonQueryRepository2)repository).GetForPerson(RootEntity.Id),

                GetLinkedEntityAsync = async (repository, entity, user) =>
                    await ((PersonQueryRepository2)repository).GetForPersonAsync(RootEntity.Id)
            };

            QueryOperations.Enqueue(
                GetSpouseLoadOperation
            );
        }

        public override void PopulateDto(PersonEntity2 entity)
        {
        }
    }
}