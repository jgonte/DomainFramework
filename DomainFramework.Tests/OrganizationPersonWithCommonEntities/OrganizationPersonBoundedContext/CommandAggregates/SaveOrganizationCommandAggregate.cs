using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class SaveOrganizationCommandAggregate : CommandAggregate<Organization>
    {
        public SaveOrganizationCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName()))
        {
        }

        public SaveOrganizationCommandAggregate(SaveOrganizationInputDto organization, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName()))
        {
            Initialize(organization, dependencies);
        }

        public override void Initialize(IInputDataTransferObject organization, EntityDependency[] dependencies)
        {
            Initialize((SaveOrganizationInputDto)organization, dependencies);
        }

        private void Initialize(SaveOrganizationInputDto organization, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Organization>(() => new OrganizationCommandRepository());

            RootEntity = new Organization
            {
                Id = organization.OrganizationId,
                Name = organization.Name
            };

            Enqueue(new SaveEntityCommandOperation<Organization>(RootEntity, dependencies));

            Enqueue(new DeleteLinksCommandOperation<Organization>(RootEntity, "UnlinkPhonesFromOrganization"));

            if (organization.Phones?.Any() == true)
            {
                foreach (var dto in organization.Phones)
                {
                    ILinkedAggregateCommandOperation operation;

                    if (dto is SavePhoneInputDto)
                    {
                        operation = new AddLinkedAggregateCommandOperation<Organization, SavePhoneCommandAggregate, SavePhoneInputDto>(
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

            Enqueue(new DeleteLinksCommandOperation<Organization>(RootEntity, "UnlinkAddressFromOrganization"));

            if (organization.Address != null)
            {
                ILinkedAggregateCommandOperation operation;

                var address = organization.Address;

                if (address is SaveAddressInputDto)
                {
                    operation = new AddLinkedAggregateCommandOperation<Organization, SaveAddressCommandAggregate, SaveAddressInputDto>(
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