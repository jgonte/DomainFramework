using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class SchoolOutputDto : RoleOutputDto
    {
        public bool IsCharter { get; set; }

        public OrganizationOutputDto Organization { get; set; }

    }
}