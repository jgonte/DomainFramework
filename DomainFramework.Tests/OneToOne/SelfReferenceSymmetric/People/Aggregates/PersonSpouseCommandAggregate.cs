using DomainFramework.Core;

namespace DomainFramework.Tests.OneToOne.SelfReference
{
    class PersonSpouseCommandAggregate : CommandAggregate<PersonEntity2>
    {
        public PersonEntity2 Spouse { get; private set; }

        public PersonSpouseCommandAggregate(DataAccess.RepositoryContext context, PersonSpouseDto personSpouse) : base(context, null)
        {
            RootEntity = new PersonEntity2
            {
                FirstName = personSpouse.FirstName
            };

            TransactedSaveOperations.Enqueue(
                new SaveEntityTransactedOperation<PersonEntity2>(RootEntity)
            );

            Spouse = new PersonEntity2
            {
                FirstName = personSpouse.SpouseName
            };

            TransactedSaveOperations.Enqueue(
                new SingleSymetricEntityLinkTransactedOperation<PersonEntity2>(RootEntity, Spouse)
            );
        }
    }
}