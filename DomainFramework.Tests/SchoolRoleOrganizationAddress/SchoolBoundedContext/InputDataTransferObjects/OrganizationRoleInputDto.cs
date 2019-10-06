using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class OrganizationRoleInputDto : IInputDataTransferObject
    {
        public int OrganizationId { get; set; }

        public int RoleId { get; set; }

        public void Validate(ValidationResult result)
        {
            OrganizationId.ValidateNotZero(result, nameof(OrganizationId));

            RoleId.ValidateNotZero(result, nameof(RoleId));
        }

    }
}