using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class Organization : Entity<int?>
    {
        /// <summary>
        /// The name of the organization
        /// </summary>
        public string Name { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedWhen { get; set; }

        public int? LastUpdatedBy { get; set; }

        public DateTime? LastUpdatedWhen { get; set; }

        public int AddressId { get; set; }

        public Phone Phone { get; set; }

    }
}