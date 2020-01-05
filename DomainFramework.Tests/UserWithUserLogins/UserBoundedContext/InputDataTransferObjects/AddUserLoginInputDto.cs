using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace UserWithUserLogins.UserBoundedContext
{
    public class AddUserLoginInputDto : IInputDataTransferObject
    {
        public string Provider { get; set; }

        public string UserKey { get; set; }

        public void Validate(ValidationResult result)
        {
            Provider.ValidateNotEmpty(result, nameof(Provider));

            Provider.ValidateMaxLength(result, nameof(Provider), 128);

            UserKey.ValidateNotEmpty(result, nameof(UserKey));

            UserKey.ValidateMaxLength(result, nameof(UserKey), 128);
        }

    }
}