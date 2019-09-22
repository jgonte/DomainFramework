using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class School : Role
    {
        /// <summary>
        /// Whether the school is a charter one
        /// </summary>
        public bool IsCharter { get; set; }

    }
}