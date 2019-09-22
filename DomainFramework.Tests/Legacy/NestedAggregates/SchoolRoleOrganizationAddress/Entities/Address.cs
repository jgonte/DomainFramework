using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class Address : Entity<int?>
    {
        /// <summary>
        /// The street
        /// </summary>
        public string Street { get; set; }

    }
}