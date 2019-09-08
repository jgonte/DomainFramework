using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class PhoneOutputDto : IOutputDataTransferObject
    {
        public string Number { get; set; }

    }
}