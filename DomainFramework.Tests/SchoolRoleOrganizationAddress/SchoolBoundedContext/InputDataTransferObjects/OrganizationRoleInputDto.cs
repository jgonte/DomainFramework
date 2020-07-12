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

        public virtual void Validate(ValidationResult result)
        {
            OrganizationId.ValidateRequired(result, nameof(OrganizationId));

            RoleId.ValidateRequired(result, nameof(RoleId));
        }

    }
}