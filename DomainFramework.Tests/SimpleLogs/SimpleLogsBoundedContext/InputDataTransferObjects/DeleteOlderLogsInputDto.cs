using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace SimpleLogs.SimpleLogsBoundedContext
{
    public class DeleteOlderLogsInputDto : IInputDataTransferObject
    {
        public DateTime When { get; set; }

        public virtual void Validate(ValidationResult result)
        {
        }

    }
}