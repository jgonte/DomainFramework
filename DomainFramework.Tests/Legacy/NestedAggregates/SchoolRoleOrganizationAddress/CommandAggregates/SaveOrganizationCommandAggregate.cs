using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class SaveOrganizationCommandAggregate : CommandAggregate<Organization>
    {
        public SaveOrganizationCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(TestConnectionClass.GetConnectionName()))
        {
        }

        public SaveOrganizationCommandAggregate(OrganizationInputDto organization) : base(new DomainFramework.DataAccess.RepositoryContext(TestConnectionClass.GetConnectionName()))
        {
            Initialize(organization);
        }

        public override void Initialize(IInputDataTransferObject inputDto)
        {
            Initialize((OrganizationInputDto)inputDto);
        }

        private void Initialize(OrganizationInputDto organization)
        {
            RegisterCommandRepositoryFactory<Organization>(() => new OrganizationCommandRepository());

            RegisterCommandRepositoryFactory<Address>(() => new AddressCommandRepository());

            var address = organization.Address;

            var entityForAddress = new Address
            {
                Id = address.Id,
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

            Enqueue(new SaveEntityCommandOperation<Organization>(RootEntity,
                new EntityDependency[]
                {
                    new EntityDependency
                    {
                        Entity = entityForAddress
                    }
                }));

            //Enqueue(new AddLinkedEntityCommandOperation<Organization, Address>(RootEntity, () => entityForAddress, "Address"));

            //Enqueue(new DeleteEntityCollectionCommandOperation<Organization>(RootEntity, "Address"));

            //if (organization.Address != null)
            //{
            //    var address = organization.Address;

            //    var entityForAddress = new Address
            //    {
            //        Id = address.Id,
            //        Street = address.Street
            //    };

            //    Enqueue(new AddLinkedEntityCommandOperation<Organization, Address>(RootEntity, () => entityForAddress, "Address"));
            //}
        }
    }
}
