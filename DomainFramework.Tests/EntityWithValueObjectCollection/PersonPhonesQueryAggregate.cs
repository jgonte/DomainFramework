using DomainFramework.Core;
using System.Linq;

namespace DomainFramework.Tests
{
    class PersonPhonesQueryAggregate : GetByIdQueryAggregate<PersonEntity4, int?, object>
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
                    entity.Phones = ((Person_PhonesQueryRepository)repository).Get(RootEntity.Id, user).ToList(),

                //SetLinkedValueObjectsAsync = async (repository, entity, user) =>
                //{
                //    var entities = await ((PersonQueryRepository5)repository).GetPhonesAsync(RootEntity.Id, user: null);

                //    entity.Phones = entities.ToList();
                //}
            };

            LoadOperations.Enqueue(
                SetPhonesLoadOperation
            );
        }

        public override object GetDataTransferObject()
        {
            return null;
        }
    }
}