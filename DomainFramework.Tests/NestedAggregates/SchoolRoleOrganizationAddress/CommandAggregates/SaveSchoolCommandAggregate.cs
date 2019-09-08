using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class SaveSchoolCommandAggregate : CommandAggregate<School>
    {
        public SaveSchoolCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(TestConnectionClass.GetConnectionName()))
        {
        }

        public SaveSchoolCommandAggregate(SaveSchoolInputDto school) : base(new DomainFramework.DataAccess.RepositoryContext(TestConnectionClass.GetConnectionName()))
        {
            Initialize(school);
        }

        public override void Initialize(IInputDataTransferObject school)
        {
            Initialize((SaveSchoolInputDto)school);
        }

        private void Initialize(SaveSchoolInputDto school)
        {
            RegisterCommandRepositoryFactory<Role>(() => new RoleCommandRepository());

            RegisterCommandRepositoryFactory<OrganizationRole>(() => new OrganizationRoleCommandRepository());

            RegisterCommandRepositoryFactory<School>(() => new SchoolCommandRepository());

            RegisterCommandRepositoryFactory<Organization>(() => new OrganizationCommandRepository());

            RootEntity = new School
            {
                Id = school.Id,
                IsCharter = school.IsCharter
            };

            Enqueue(new SaveEntityCommandOperation<School>(RootEntity));

            Enqueue(new DeleteEntityCollectionCommandOperation<School>(RootEntity, "Organization"));

            var linkedSaveOrganizationCommandAggregateOperation = new AddLinkedAggregateCommandOperation<School, SaveOrganizationCommandAggregate, OrganizationInputDto>(RootEntity, school.Organization);

            Enqueue(linkedSaveOrganizationCommandAggregateOperation);

            var organizationRole = new OrganizationRole();

            Enqueue(new InsertEntityCommandOperation<OrganizationRole>(organizationRole, new EntityDependency[]
            {
                new EntityDependency
                {
                    Entity = linkedSaveOrganizationCommandAggregateOperation.CommandAggregate.RootEntity
                },
                new EntityDependency
                {
                    Entity = RootEntity
                }
            }));
        }

    }
}
