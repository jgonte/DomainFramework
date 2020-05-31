using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class Organization : Entity<int?>
    {
        /// <summary>
        /// The name of the organization
        /// </summary>
        public string Name { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public int? AddressId { get; set; }

        public Phone Phone { get; set; }

    }
}