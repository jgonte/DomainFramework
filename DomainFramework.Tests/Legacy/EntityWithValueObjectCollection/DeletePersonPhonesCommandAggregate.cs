using DomainFramework.Core;

namespace DomainFramework.Tests.EntityWithValueObjectCollection
{
    class DeletePersonPhonesCommandAggregate : CommandAggregate<PersonEntity4>
    {
        public DeletePersonPhonesCommandAggregate(DataAccess.RepositoryContext context, int personId) : base(context)
        {
            RootEntity = new PersonEntity4
            {
                Id = personId
            };

            Enqueue(
                new DeleteEntityCommandOperation<PersonEntity4>(RootEntity)
            );
        }

        public override void Initialize(IInputDataTransferObject inputDto)
        {
            throw new System.NotImplementedException();
        }
    }
}