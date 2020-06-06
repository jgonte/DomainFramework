using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class SaveAddressCommandAggregate : CommandAggregate<Address>
    {
        public SaveAddressCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName()))
        {
        }

        public SaveAddressCommandAggregate(SaveAddressInputDto address, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName()))
        {
            Initialize(address, dependencies);
        }

        public override void Initialize(IInputDataTransferObject address, EntityDependency[] dependencies)
        {
            Initialize((SaveAddressInputDto)address, dependencies);
        }

        private void Initialize(SaveAddressInputDto address, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Address>(() => new AddressCommandRepository());

            var addressDependency = dependencies?.SingleOrDefault(d => d.Selector == "Address")?.Entity;

            var organizationDependency = (addressDependency is Organization) ? (Organization)addressDependency : null;

            var personDependency = (addressDependency is Person) ? (Person)addressDependency : null;

            RootEntity = new Address
            {
                Id = address.AddressId,
                Street = address.Street,
                OrganizationId = (organizationDependency != null) ? organizationDependency.Id : address.OrganizationId,
                PersonId = (personDependency != null) ? personDependency.Id : address.PersonId
            };

            Enqueue(new SaveEntityCommandOperation<Address>(RootEntity, dependencies));
        }

    }
}