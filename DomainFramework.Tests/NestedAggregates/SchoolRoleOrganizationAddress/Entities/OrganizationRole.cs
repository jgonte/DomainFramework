﻿using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class OrganizationRoleId
    {
        public int? OrganizationId { get; set; }

        public int? RoleId { get; set; }

    }

    public class OrganizationRole : Entity<OrganizationRoleId>
    {
        public int CreatedBy { get; set; }

        public DateTime CreatedWhen { get; set; }

        public int? LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedWhen { get; set; }

    }
}
