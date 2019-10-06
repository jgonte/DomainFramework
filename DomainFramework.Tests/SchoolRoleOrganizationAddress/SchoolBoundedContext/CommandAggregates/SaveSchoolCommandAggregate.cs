using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class SaveSchoolCommandAggregate : CommandAggregate<School>
    {
        public SaveSchoolCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName()))
        {
        }

        public SaveSchoolCommandAggregate(SaveSchoolInputDto school, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName()))
        {
            Initialize(school, dependencies);
        }

        public override void Initialize(IInputDataTransferObject school, EntityDependency[] dependencies)
        {
            Initialize((SaveSchoolInputDto)school, dependencies);
        }

        private void Initialize(SaveSchoolInputDto school, EntityDependency[] dependencies)
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