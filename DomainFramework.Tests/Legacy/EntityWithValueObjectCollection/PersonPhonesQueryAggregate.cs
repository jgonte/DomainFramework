using DomainFramework.Core;
using System.Linq;

namespace DomainFramework.Tests
{
    class PersonPhonesQueryAggregate : GetByIdQueryAggregate<PersonEntity4, int?, PersonOutputDto>
    {
        public SetCollectionLinkedValueObjectQueryOperation<PersonEntity4, Person_PhonesQueryRepository.RepositoryKey> SetPhonesLoadOperation { get; }

        public PersonPhonesQueryAggregate() : this(null)
        {
        }

        public PersonPhonesQueryAggregate(DataAccess.RepositoryContext context) : base(context)
        {
            SetPhonesLoadOperation = new SetCollectionLinkedValueObjectQueryOperation<PersonEntity4, Person_PhonesQueryRepository.RepositoryKey>
            {
                SetLinkedValueObjects = (repository, entity, user) =>
                    entity.Phones = ((Person_PhonesQueryRepository)repository).GetAll(RootEntity.Id).ToList(),

                //SetLinkedValueObjectsAsync = async (repository, entity, user) =>
                //{
                //    var entities = await ((PersonQueryRepository5)repository).GetPhonesAsync(RootEntity.Id, user: null);

                //    entity.Phones = entities.ToList();
                //}
            };

            QueryOperations.Enqueue(
                SetPhonesLoadOperation
            );
        }

        public override void PopulateDto(PersonEntity4 entity)
        {
        }
    }
}