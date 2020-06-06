using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class SavePhoneCommandAggregate : CommandAggregate<Phone>
    {
        public SavePhoneCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName()))
        {
        }

        public SavePhoneCommandAggregate(SavePhoneInputDto phone, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName()))
        {
            Initialize(phone, dependencies);
        }

        public override void Initialize(IInputDataTransferObject phone, EntityDependency[] dependencies)
        {
            Initialize((SavePhoneInputDto)phone, dependencies);
        }

        private void Initialize(SavePhoneInputDto phone, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Phone>(() => new PhoneCommandRepository());

            var phonesDependency = dependencies?.SingleOrDefault(d => d.Selector == "Phones")?.Entity;

            var organizationDependency = (phonesDependency is Organization) ? (Organization)phonesDependency : null;

            var personDependency = (phonesDependency is Person) ? (Person)phonesDependency : null;

            RootEntity = new Phone
            {
                Id = phone.PhoneId,
                Number = phone.Number,
                OrganizationId = (organizationDependency != null) ? organizationDependency.Id : phone.OrganizationId,
                PersonId = (personDependency != null) ? personDependency.Id : phone.PersonId
            };

            Enqueue(new SaveEntityCommandOperation<Phone>(RootEntity, dependencies));
        }

    }
}