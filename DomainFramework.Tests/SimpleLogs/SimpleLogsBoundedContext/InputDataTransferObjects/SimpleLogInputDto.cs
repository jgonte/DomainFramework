using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace SimpleLogs.SimpleLogsBoundedContext
{
    public class SimpleLogInputDto : IInputDataTransferObject
    {
        public string MessageType { get; set; }

        public string Message { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            MessageType.ValidateNotEmpty(result, nameof(MessageType));

            MessageType.ValidateMaxLength(result, nameof(MessageType), 1);

            Message.ValidateNotEmpty(result, nameof(Message));

            Message.ValidateMaxLength(result, nameof(Message), 50);
        }

    }
}