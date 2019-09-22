using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class SaveSchoolInputDto : IInputDataTransferObject
    {
        public int? Id { get; set; }

        public bool IsCharter { get; set; }

        public OrganizationInputDto Organization { get; set; }

        public void Validate(ValidationResult result)
        {
            Organization?.Validate(result);
        }

    }
}