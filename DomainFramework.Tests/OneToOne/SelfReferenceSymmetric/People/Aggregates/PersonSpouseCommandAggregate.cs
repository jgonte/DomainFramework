using DomainFramework.Core;
using Utilities;

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

            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<PersonEntity2>(RootEntity, CommandOperationTypes.Save)
            );

            Spouse = new PersonEntity2
            {
                FirstName = personSpouse.SpouseName
            };

            TransactedOperations.Enqueue(
                new SingleSymetricEntityLinkTransactedOperation<PersonEntity2>(RootEntity, Spouse)
            );
        }
    }
}