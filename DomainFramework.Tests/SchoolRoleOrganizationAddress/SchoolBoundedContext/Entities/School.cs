using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class School : Role
    {
        /// <summary>
        /// Whether the school is a charter one
        /// </summary>
        public bool IsCharter { get; set; }

    }
}