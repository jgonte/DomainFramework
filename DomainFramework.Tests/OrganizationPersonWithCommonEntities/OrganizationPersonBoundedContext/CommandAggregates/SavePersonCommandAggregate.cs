using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class SavePersonCommandAggregate : CommandAggregate<Person>
    {
        public SavePersonCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName()))
        {
        }

        public SavePersonCommandAggregate(SavePersonInputDto organization, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName()))
        {
            Initialize(organization, dependencies);
        }

        public override void Initialize(IInputDataTransferObject organization, EntityDependency[] dependencies)
        {
            Initialize((SavePersonInputDto)organization, dependencies);
        }

        private void Initialize(SavePersonInputDto organization, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Person>(() => new PersonCommandRepository());

            RootEntity = new Person
            {
                Id = organization.PersonId,
                Name = organization.Name
            };

            Enqueue(new SaveEntityCommandOperation<Person>(RootEntity, dependencies));

            Enqueue(new DeleteLinksCommandOperation<Person>(RootEntity, "UnlinkPhonesFromPerson"));

            if (organization.Phones?.Any() == true)
            {
                foreach (var dto in organization.Phones)
                {
                    ILinkedAggregateCommandOperation operation;

                    if (dto is SavePhoneInputDto)
                    {
                        operation = new AddLinkedAggregateCommandOperation<Person, SavePhoneCommandAggregate, SavePhoneInputDto>(
                            RootEntity,
                            (SavePhoneInputDto)dto,
                            new EntityDependency[]
                            {
                                new EntityDependency
                                {
                                    Entity = RootEntity,
                                    Selector = "Phones"
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

            Enqueue(new DeleteLinksCommandOperation<Person>(RootEntity, "UnlinkAddressFromPerson"));

            if (organization.Address != null)
            {
                ILinkedAggregateCommandOperation operation;

                var address = organization.Address;

                if (address is SaveAddressInputDto)
                {
                    operation = new AddLinkedAggregateCommandOperation<Person, SaveAddressCommandAggregate, SaveAddressInputDto>(
                        RootEntity,
                        (SaveAddressInputDto)address,
                        new EntityDependency[]
                        {
                            new EntityDependency
                            {
                                Entity = RootEntity,
                                Selector = "Address"
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