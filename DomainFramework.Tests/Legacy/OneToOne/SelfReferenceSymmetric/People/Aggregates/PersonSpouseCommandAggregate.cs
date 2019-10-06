using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests.OneToOne.SelfReference
{
    class PersonSpouseCommandAggregate : CommandAggregate<PersonEntity2>
    {
        public PersonEntity2 Spouse { get; private set; }

        public PersonSpouseCommandAggregate(DataAccess.RepositoryContext context, PersonSpouseDto personSpouse) : base(context)
        {
            // Save the spouse first so we can link to it
            Spouse = new PersonEntity2
            {
                FirstName = personSpouse.SpouseName
            };

            Enqueue(
                new SaveEntityCommandOperation<PersonEntity2>(Spouse)
            );

            // Save the root entity
            RootEntity = new PersonEntity2
            {
                FirstName = personSpouse.FirstName
            };

            Enqueue(
                new SaveEntityCommandOperation<PersonEntity2>(
                    RootEntity,
                    new EntityDependency[]
                    {
                        new EntityDependency
                        {
                            Entity = Spouse
                        }
                    })
            );

            // Update the spouse to link to tue root entity
            Enqueue(
                new UpdateEntityCommandOperation<PersonEntity2>(
                    Spouse,
                    new EntityDependency[]
                    {
                        new EntityDependency
                        {
                            Entity = RootEntity
                        }
                    })
            );
        }

        public override void Initialize(IInputDataTransferObject inputDto, EntityDependency[] dependencies)
        {
            throw new System.NotImplementedException();
        }
    }
}