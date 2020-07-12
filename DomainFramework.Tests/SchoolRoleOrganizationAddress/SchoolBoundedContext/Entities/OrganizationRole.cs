using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class OrganizationRoleId
    {
        public int OrganizationId { get; set; }

        public int RoleId { get; set; }

    }

    public class OrganizationRole : Entity<OrganizationRoleId>
    {
        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

    }
}