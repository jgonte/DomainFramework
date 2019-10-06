using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class PhoneOutputDto : IOutputDataTransferObject
    {
        public string Number { get; set; }

    }
}