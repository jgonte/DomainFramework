using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonWithSpouse.PersonBoundedContext
{
    public class SavePersonCommandAggregate : CommandAggregate<Person>
    {
        public SavePersonCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(PersonWithSpouseConnectionClass.GetConnectionName()))
        {
        }

        public SavePersonCommandAggregate(PersonInputDto person, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(PersonWithSpouseConnectionClass.GetConnectionName()))
        {
            Initialize(person, dependencies);
        }

        public override void Initialize(IInputDataTransferObject person, EntityDependency[] dependencies)
        {
            Initialize((PersonInputDto)person, dependencies);
        }

        private void Initialize(PersonInputDto person, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Person>(() => new PersonCommandRepository());

            var spouseDependency = (Person)dependencies?.SingleOrDefault()?.Entity;

            RootEntity = new Person
            {
                Id = person.PersonId,
                Name = person.Name,
                Gender = person.Gender,
                SpouseId = (spouseDependency != null) ? spouseDependency.Id : person.SpouseId
            };

            Enqueue(new SaveEntityCommandOperation<Person>(RootEntity, dependencies));

            Enqueue(new DeleteLinksCommandOperation<Person>(RootEntity, "UnlinkMarriedToFromPerson"));

            if (person.MarriedTo != null)
            {
                ILinkedAggregateCommandOperation operation;

                var marriedTo = person.MarriedTo;

                if (marriedTo is PersonInputDto)
                {
                    operation = new AddLinkedAggregateCommandOperation<Person, SavePersonCommandAggregate, PersonInputDto>(
                        RootEntity,
                        (PersonInputDto)marriedTo,
                        new EntityDependency[]
                        {
                            new EntityDependency
                            {
                                Entity = RootEntity,
                                Selector = "MarriedTo"
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
                        Selector = "MarriedTo"
                    }
                }));
            }
        }

    }
}