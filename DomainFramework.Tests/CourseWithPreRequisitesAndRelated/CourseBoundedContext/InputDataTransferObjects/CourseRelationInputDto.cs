using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace CourseWithPreRequisitesAndRelated.CourseBoundedContext
{
    public class CourseRelationInputDto : IInputDataTransferObject
    {
        public int Relates_CourseId { get; set; }

        public int IsRelatedTo_CourseId { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Relates_CourseId.ValidateRequired(result, nameof(Relates_CourseId));

            IsRelatedTo_CourseId.ValidateRequired(result, nameof(IsRelatedTo_CourseId));
        }

    }
}