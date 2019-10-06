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

            RegisterCommandRepositoryFactory<Address>(() => new AddressCommandRepository());

            var address = organization.Address;

            var entityForAddress = new Address
            {
                Street = address.Street
            };

            Enqueue(new SaveEntityCommandOperation<Address>(entityForAddress));

            RootEntity = new Organization
            {
                Id = organization.Id,
                Name = organization.Name,
                AddressId = organization.AddressId,
                Phone = new Phone
                {
                    Number = organization.Phone.Number
                }
            };

            Enqueue(new SaveEntityCommandOperation<Organization>(RootEntity, new EntityDependency[]
            {
                new EntityDependency
                {
                    Entity = entityForAddress
                }
            }));
        }

    }
}