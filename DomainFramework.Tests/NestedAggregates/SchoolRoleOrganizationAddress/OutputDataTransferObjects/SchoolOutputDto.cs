using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class SchoolOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public bool IsCharter { get; set; }

        public OrganizationOutputDto Organization { get; set; }

    }
}
