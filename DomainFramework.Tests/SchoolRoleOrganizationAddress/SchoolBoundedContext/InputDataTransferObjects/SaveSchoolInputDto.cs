using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class SaveSchoolInputDto : IInputDataTransferObject
    {
        public int SchoolId { get; set; }

        public bool IsCharter { get; set; }

        public OrganizationInputDto Organization { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Organization?.Validate(result);
        }

    }
}