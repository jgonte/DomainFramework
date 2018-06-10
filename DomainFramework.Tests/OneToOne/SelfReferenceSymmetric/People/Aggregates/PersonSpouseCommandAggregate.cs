using DomainFramework.Core;

namespace DomainFramework.Tests.OneToOne.SelfReference
{
    class PersonSpouseCommandAggregate : CommandAggregate<PersonEntity2>
    {
        public PersonEntity2 Spouse { get; private set; }

        public PersonSpouseCommandAggregate(DataAccess.RepositoryContext context, string firstName, string spouseName) : base(context, null)
        {
            RootEntity = new PersonEntity2
            {
                FirstName = firstName
            };

            TransactedSaveOperations.Enqueue(
                new SaveEntityTransactedOperation<PersonEntity2>(RootEntity)
            );

            Spouse = new PersonEntity2
            {
                FirstName = spouseName
            };

            TransactedSaveOperations.Enqueue(
                new SingleSymetricEntityLinkTransactedOperation<PersonEntity2>(RootEntity, Spouse)
            );
        }
    }
}