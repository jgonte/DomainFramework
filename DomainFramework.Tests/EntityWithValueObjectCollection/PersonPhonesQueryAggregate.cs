using DomainFramework.Core;
using System.Linq;

namespace DomainFramework.Tests
{
    class PersonPhonesQueryAggregate : QueryAggregate<PersonEntity4, int?, object>
    {
        public SetCollectionLinkedValueObjectLoadOperation<PersonEntity4, Phone> SetPhonesLoadOperation { get; }

        public PersonPhonesQueryAggregate() : this(null)
        {
        }

        public PersonPhonesQueryAggregate(DataAccess.RepositoryContext context) : base(context)
        {
            SetPhonesLoadOperation = new SetCollectionLinkedValueObjectLoadOperation<PersonEntity4, Phone>
            {
                SetLinkedValueObjects = (repository, entity, user) =>
                    entity.Phones = ((PersonQueryRepository5)repository).GetPhones(RootEntity.Id, user).ToList(),

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
            throw new System.NotImplementedException();
        }
    }
}