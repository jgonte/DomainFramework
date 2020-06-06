using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonWithSpouseAndBestFriend.PersonBoundedContext
{
    public class SavePersonCommandAggregate : CommandAggregate<Person>
    {
        public SavePersonCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName()))
        {
        }

        public SavePersonCommandAggregate(SavePersonInputDto person, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName()))
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

            var spouseDependency = (Person)dependencies?.SingleOrDefault(d => d.Selector == "Spouse")?.Entity;

            var bestFriendDependency = (Person)dependencies?.SingleOrDefault(d => d.Selector == "BestFriend")?.Entity;

            RootEntity = new Person
            {
                Id = person.PersonId,
                Name = person.Name,
                Gender = person.Gender,
                SpouseId = (spouseDependency != null) ? spouseDependency.Id : person.SpouseId,
                BestFriendId = (bestFriendDependency != null) ? bestFriendDependency.Id : person.BestFriendId
            };

            Enqueue(new SaveEntityCommandOperation<Person>(RootEntity, dependencies));

            Enqueue(new DeleteLinksCommandOperation<Person>(RootEntity, "UnlinkMarriedToFromPerson"));

            if (person.MarriedTo != null)
            {
                ILinkedAggregateCommandOperation operation;

                var marriedTo = person.MarriedTo;

                if (marriedTo is SavePersonInputDto)
                {
                    operation = new AddLinkedAggregateCommandOperation<Person, SavePersonCommandAggregate, SavePersonInputDto>(
                        RootEntity,
                        (SavePersonInputDto)marriedTo,
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

            Enqueue(new DeleteLinksCommandOperation<Person>(RootEntity, "UnlinkBestFriendOfFromPerson"));

            if (person.BestFriendOf != null)
            {
                ILinkedAggregateCommandOperation operation;

                var bestFriendOf = person.BestFriendOf;

                if (bestFriendOf is SavePersonInputDto)
                {
                    operation = new AddLinkedAggregateCommandOperation<Person, SavePersonCommandAggregate, SavePersonInputDto>(
                        RootEntity,
                        (SavePersonInputDto)bestFriendOf,
                        new EntityDependency[]
                        {
                            new EntityDependency
                            {
                                Entity = RootEntity,
                                Selector = "BestFriendOf"
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
                        Selector = "BestFriendOf"
                    }
                }));
            }
        }

    }
}