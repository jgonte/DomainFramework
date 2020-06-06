using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class SaveAddressCommandAggregate : CommandAggregate<Address>
    {
        public SaveAddressCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName()))
        {
        }

        public SaveAddressCommandAggregate(AddressInputDto address, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName()))
        {
            Initialize(address, dependencies);
        }

        public override void Initialize(IInputDataTransferObject address, EntityDependency[] dependencies)
        {
            Initialize((AddressInputDto)address, dependencies);
        }

        private void Initialize(AddressInputDto address, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Address>(() => new AddressCommandRepository());

            RootEntity = new Address
            {
                Id = address.AddressId,
                Street = address.Street
            };

            Enqueue(new SaveEntityCommandOperation<Address>(RootEntity, dependencies));
        }

    }
}