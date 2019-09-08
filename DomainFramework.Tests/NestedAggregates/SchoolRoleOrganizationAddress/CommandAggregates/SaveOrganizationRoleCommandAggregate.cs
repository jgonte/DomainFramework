using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class SaveOrganizationRoleCommandAggregate : CommandAggregate<OrganizationRole>
    {
        public SaveOrganizationRoleCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(TestConnectionClass.GetConnectionName()))
        {
        }

        public SaveOrganizationRoleCommandAggregate(OrganizationRoleInputDto organizationRole) : base(new DomainFramework.DataAccess.RepositoryContext(TestConnectionClass.GetConnectionName()))
        {
            Initialize(organizationRole);
        }

        public override void Initialize(IInputDataTransferObject organizationRole)
        {
            Initialize((OrganizationRoleInputDto)organizationRole);
        }

        private void Initialize(OrganizationRoleInputDto organizationRole)
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

            Enqueue(new SaveEntityCommandOperation<OrganizationRole>(RootEntity));
        }

    }
}
