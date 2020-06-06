using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class ReplaceClassStudentsInputDto : IInputDataTransferObject
    {
        public int ClassId { get; set; }

        public List<StudentInputDto> Students { get; set; } = new List<StudentInputDto>();

        public virtual void Validate(ValidationResult result)
        {
            ClassId.ValidateNotZero(result, nameof(ClassId));

            foreach (var student in Students)
            {
                student.Validate(result);
            }
        }

    }
}