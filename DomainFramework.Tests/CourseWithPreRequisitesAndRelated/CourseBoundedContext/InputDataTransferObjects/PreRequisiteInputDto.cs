using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace CourseWithPreRequisitesAndRelated.CourseBoundedContext
{
    public class PreRequisiteInputDto : IInputDataTransferObject
    {
        public int Requires_CourseId { get; set; }

        public int IsRequiredBy_CourseId { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Requires_CourseId.ValidateRequired(result, nameof(Requires_CourseId));

            IsRequiredBy_CourseId.ValidateRequired(result, nameof(IsRequiredBy_CourseId));
        }

    }
}