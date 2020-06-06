using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonWithSpouseAndDependants.PersonBoundedContext
{
    public class SavePersonCommandAggregate : CommandAggregate<Person>
    {
        public SavePersonCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(PersonWithSpouseAndDependantsConnectionClass.GetConnectionName()))
        {
        }

        public SavePersonCommandAggregate(SavePersonInputDto person, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(PersonWithSpouseAndDependantsConnectionClass.GetConnectionName()))
        {
            Initialize(person, dependencies);
        }

        public override void Initialize(IInputDataTransferObject person, EntityDependency[] dependencies)
        {
            Initialize((SavePersonInputDto)person, dependencies);
        }

        private void Initialize(SavePersonInputDto person, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Person>(() => new PersonCommandRepository());

            var marriedToPersonDependency = (Person)dependencies?.SingleOrDefault(d => d.Selector == "MarriedToPerson")?.Entity;

            var dependsOnPersonDependency = (Person)dependencies?.SingleOrDefault(d => d.Selector == "DependsOnPerson")?.Entity;

            RootEntity = new Person
            {
                Id = person.PersonId,
                Name = person.Name,
                MarriedPersonId = (marriedToPersonDependency != null) ? marriedToPersonDependency.Id : person.MarriedPersonId,
                ProviderPersonId = (dependsOnPersonDependency != null) ? dependsOnPersonDependency.Id : person.ProviderPersonId
            };

            Enqueue(new SaveEntityCommandOperation<Person>(RootEntity, dependencies));

            Enqueue(new DeleteLinksCommandOperation<Person>(RootEntity, "UnlinkSpouseFromPerson"));

            if (person.Spouse != null)
            {
                ILinkedAggregateCommandOperation operation;

                var spouse = person.Spouse;

                if (spouse is SavePersonInputDto)
                {
                    operation = new AddLinkedAggregateCommandOperation<Person, SavePersonCommandAggregate, SavePersonInputDto>(
                        RootEntity,
                        (SavePersonInputDto)spouse,
                        new EntityDependency[]
                        {
                            new EntityDependency
                            {
                                Entity = RootEntity,
                                Selector = "Spouse"
                            }
                        }
                    );

                    Enqueue(operation);
                }
                else
                {
                    throw new NotImplementedException();
                }

                Enqueue(new UpdateEntityCommandOperation<Person>(RootEntity, new EntityDependency[]
                {
                    new EntityDependency
                    {
                        Entity = operation.CommandAggregate.RootEntity,
                        Selector = "Spouse"
                    }
                }));
            }

            Enqueue(new DeleteLinksCommandOperation<Person>(RootEntity, "UnlinkDependantsFromPerson"));

            if (person.Dependants?.Any() == true)
            {
                foreach (var dto in person.Dependants)
                {
                    ILinkedAggregateCommandOperation operation;

                    if (dto is SavePersonInputDto)
                    {
                        operation = new AddLinkedAggregateCommandOperation<Person, SavePersonCommandAggregate, SavePersonInputDto>(
                            RootEntity,
                            (SavePersonInputDto)dto,
                            new EntityDependency[]
                            {
                                new EntityDependency
                                {
                                    Entity = RootEntity,
                                    Selector = "Dependants"
                                }
                            }
                        );

                        Enqueue(operation);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

    }
}