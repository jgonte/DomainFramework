using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class Address : Entity<int>
    {
        /// <summary>
        /// The street
        /// </summary>
        public string Street { get; set; }

    }
}