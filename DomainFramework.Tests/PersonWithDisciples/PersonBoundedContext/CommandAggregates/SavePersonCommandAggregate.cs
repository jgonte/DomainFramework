using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonWithDisciples.PersonBoundedContext
{
    public class SavePersonCommandAggregate : CommandAggregate<Person>
    {
        public SavePersonCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(PersonWithDisciplesConnectionClass.GetConnectionName()))
        {
        }

        public SavePersonCommandAggregate(PersonInputDto person, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(PersonWithDisciplesConnectionClass.GetConnectionName()))
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

            var leaderDependency = (Person)dependencies?.SingleOrDefault()?.Entity;

            RootEntity = new Person
            {
                Id = person.PersonId,
                Name = person.Name,
                Gender = person.Gender,
                LeaderId = (leaderDependency != null) ? leaderDependency.Id : person.LeaderId
            };

            Enqueue(new SaveEntityCommandOperation<Person>(RootEntity, dependencies));

            Enqueue(new DeleteLinksCommandOperation<Person>(RootEntity, "UnlinkDisciplesFromPerson"));

            if (person.Disciples?.Any() == true)
            {
                foreach (var dto in person.Disciples)
                {
                    ILinkedAggregateCommandOperation operation;

                    if (dto is PersonInputDto)
                    {
                        operation = new AddLinkedAggregateCommandOperation<Person, SavePersonCommandAggregate, PersonInputDto>(
                            RootEntity,
                            (PersonInputDto)dto,
                            new EntityDependency[]
                            {
                                new EntityDependency
                                {
                                    Entity = RootEntity,
                                    Selector = "Disciples"
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