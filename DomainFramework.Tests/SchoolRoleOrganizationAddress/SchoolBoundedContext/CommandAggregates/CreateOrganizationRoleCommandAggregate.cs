using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class CreateOrganizationRoleCommandAggregate : CommandAggregate<OrganizationRole>
    {
        public CreateOrganizationRoleCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName()))
        {
        }

        public CreateOrganizationRoleCommandAggregate(OrganizationRoleInputDto organizationRole, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName()))
        {
            Initialize(organizationRole, dependencies);
        }

        public override void Initialize(IInputDataTransferObject organizationRole, EntityDependency[] dependencies)
        {
            Initialize((OrganizationRoleInputDto)organizationRole, dependencies);
        }

        private void Initialize(OrganizationRoleInputDto organizationRole, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<OrganizationRole>(() => new OrganizationRoleCommandRepository());

            RootEntity = new OrganizationRole
            {
                Id = new OrganizationRoleId
                {
                    OrganizationId = organizationRole.OrganizationId,
                    RoleId = organizationRole.RoleId
                }
            };

            Enqueue(new InsertEntityCommandOperation<OrganizationRole>(RootEntity, dependencies));
        }

    }
}