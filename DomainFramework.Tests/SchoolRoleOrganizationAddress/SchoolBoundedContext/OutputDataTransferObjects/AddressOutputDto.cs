using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class AddressOutputDto : IOutputDataTransferObject
    {
        public int AddressId { get; set; }

        public string Street { get; set; }

    }
}