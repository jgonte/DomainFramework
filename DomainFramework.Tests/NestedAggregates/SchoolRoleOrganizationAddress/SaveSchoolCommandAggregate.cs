using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class SaveSchoolCommandAggregate : CommandAggregate<School>
    {
        public SaveSchoolCommandAggregate(SaveSchoolInputDto school) : base(new DomainFramework.DataAccess.RepositoryContext(TestConnectionClass.GetConnectionName()))
        {
            RegisterCommandRepositoryFactory<School>(() => new SchoolCommandRepository());

            RegisterCommandRepositoryFactory<Organization>(() => new OrganizationCommandRepository());

            RootEntity = new School
            {
                Id = school.Id,
                IsCharter = school.IsCharter
            };

            Enqueue(new SaveEntityCommandOperation<School>(RootEntity));

            //Enqueue(new DeleteEntityCollectionCommandOperation<School>(RootEntity, "Organization"));

            if (school.Organization != null)
            {
                var organization = school.Organization;

                var entityForOrganization = new Organization
                {
                    Id = organization.Id,
                    Name = organization.Name,
                    AddressId = organization.AddressId,
                    Phone = new Phone
                    {
                        Number = organization.Phone.Number
                    }
                };

                Enqueue(new AddLinkedEntityCommandOperation<School, Organization>(RootEntity, () => entityForOrganization, "Organization"));
            }

            //Enqueue(new AddLinkedAggregateCommandOperation<Organization>(RootEntity, () => entityForOrganization, "Organization"));
        }

    }
}
