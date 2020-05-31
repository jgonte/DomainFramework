using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class SaveOrganizationCommandAggregate : CommandAggregate<Organization>
    {
        public SaveOrganizationCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName()))
        {
        }

        public SaveOrganizationCommandAggregate(OrganizationInputDto organization, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName()))
        {
            Initialize(organization, dependencies);
        }

        public override void Initialize(IInputDataTransferObject organization, EntityDependency[] dependencies)
        {
            Initialize((OrganizationInputDto)organization, dependencies);
        }

        private void Initialize(OrganizationInputDto organization, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Organization>(() => new OrganizationCommandRepository());

            var addressDependency = (Address)dependencies?.SingleOrDefault()?.Entity;

            RootEntity = new Organization
            {
                Id = organization.OrganizationId,
                Name = organization.Name,
                AddressId = (addressDependency != null) ? addressDependency.Id : organization.AddressId,
                Phone = new Phone
                {
                    Number = organization.Phone.Number
                }
            };

            var existanceDependencies = new List<EntityDependency>();

            Enqueue(new DeleteLinksCommandOperation<Organization>(RootEntity, "UnlinkAddressFromOrganization"));

            if (organization.Address != null)
            {
                var address = organization.Address;

                if (address is AddressInputDto)
                {
                    var operation = new AddLinkedAggregateCommandOperation<Organization, SaveAddressCommandAggregate, AddressInputDto>(
                        RootEntity,
                        (AddressInputDto)address,
                        dependencies
                    );

                    Enqueue(operation);

                    existanceDependencies.Add(new EntityDependency
                    {
                        Entity = operation.CommandAggregate.RootEntity,
                        Selector = "Address"
                    });
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            Enqueue(new SaveEntityCommandOperation<Organization>(RootEntity, existanceDependencies.ToArray()));
        }

    }
}