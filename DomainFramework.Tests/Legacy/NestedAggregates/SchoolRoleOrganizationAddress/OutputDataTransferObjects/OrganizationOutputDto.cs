using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class OrganizationOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int AddressId { get; set; }

        public AddressOutputDto Address { get; set; }

        public PhoneOutputDto Phone { get; set; }

    }
}