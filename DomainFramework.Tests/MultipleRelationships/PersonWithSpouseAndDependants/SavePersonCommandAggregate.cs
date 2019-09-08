using DomainFramework.Core;
using System.Linq;

namespace DomainFramework.Tests.PersonWithSpouseAndDependants
{
    public class SavePersonCommandAggregate : CommandAggregate<Person>
    {
        public SavePersonCommandAggregate(SavePersonInputDto person) : base(new DataAccess.RepositoryContext(TestConnectionClass.GetConnectionName()))
        {
            RegisterCommandRepositoryFactory<Person>(() => new PersonCommandRepository());

            RootEntity = new Person
            {
                Id = person.Id,
                Name = person.Name,
                MarriedPersonId = person.MarriedPersonId,
                ProviderPersonId = person.ProviderPersonId
            };

            Enqueue(new SaveEntityCommandOperation<Person>(RootEntity));

            Enqueue(new DeleteEntityCollectionCommandOperation<Person>(RootEntity, "Spouse"));

            if (person.Spouse != null)
            {
                var spouse = person.Spouse;

                var spouseEntity = new Person
                {
                    Id = spouse.Id,
                    Name = spouse.Name,
                    MarriedPersonId = spouse.MarriedPersonId,
                    ProviderPersonId = spouse.ProviderPersonId
                };

                Enqueue(new AddLinkedEntityCommandOperation<Person, Person>(RootEntity, 
                    () => spouseEntity, 
                    "Spouse"));

                // Link the spouse back to the root entity
                Enqueue(new UpdateEntityCommandOperation<Person>(RootEntity,
                    new EntityDependency[]
                    {
                        new EntityDependency
                        {
                            Entity = spouseEntity,
                            Selector = "Spouse"
                        }
                    })
                );
            }

            Enqueue(new DeleteEntityCollectionCommandOperation<Person>(RootEntity, "Dependants"));

            if (person.Dependants?.Any() == true)
            {
                foreach (var dependant in person.Dependants)
                {
                    Enqueue(new AddLinkedEntityCommandOperation<Person, Person>(RootEntity, () => new Person
                    {
                        Id = dependant.Id,
                        Name = dependant.Name,
                        MarriedPersonId = dependant.MarriedPersonId,
                        ProviderPersonId = dependant.ProviderPersonId
                    },
                    "Dependants"));
                }
            }
        }

        public override void Initialize(IInputDataTransferObject inputDto)
        {
            throw new System.NotImplementedException();
        }
    }
}