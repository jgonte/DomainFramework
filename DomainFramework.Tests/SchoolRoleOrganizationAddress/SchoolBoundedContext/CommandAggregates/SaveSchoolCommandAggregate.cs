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
            RegisterCommandRepositoryFactory<OrganizationRole>(() => new OrganizationRoleCommandRepository());

            RegisterCommandRepositoryFactory<School>(() => new SchoolCommandRepository());

            RegisterCommandRepositoryFactory<Role>(() => new RoleCommandRepository());

            RootEntity = new School
            {
                Id = school.SchoolId,
                IsCharter = school.IsCharter
            };

            Enqueue(new SaveEntityCommandOperation<School>(RootEntity, dependencies));

            Enqueue(new DeleteLinksCommandOperation<School>(RootEntity, "UnlinkOrganizationFromRole"));

            if (school.Organization != null)
            {
                ILinkedAggregateCommandOperation operation;

                if (school.Organization is OrganizationInputDto)
                {
                    operation = new AddLinkedAggregateCommandOperation<School, SaveOrganizationCommandAggregate, OrganizationInputDto>(
                        RootEntity,
                        (OrganizationInputDto)school.Organization
                    );

                    Enqueue(operation);
                }
                else
                {
                    throw new NotImplementedException();
                }

                Enqueue(new AddLinkedAggregateCommandOperation<School, CreateOrganizationRoleCommandAggregate, OrganizationRoleInputDto>(
                    RootEntity,
                    school.Organization.OrganizationRole,
                    new EntityDependency[]
                    {
                        new EntityDependency
                        {
                            Entity = RootEntity,
                            Selector = "Role"
                        },
                        new EntityDependency
                        {
                            Entity = operation.CommandAggregate.RootEntity,
                            Selector = "Organization"
                        }
                    }
                ));
            }
        }

    }
}