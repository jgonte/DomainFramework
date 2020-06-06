using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace RegisterUser.UserBoundedContext
{
    public class RegisterUserInputDto : IInputDataTransferObject
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public virtual void Validate(ValidationResult result)
        {
            Username.ValidateNotEmpty(result, nameof(Username));

            Username.ValidateMaxLength(result, nameof(Username), 50);

            Email.ValidateEmail(result, nameof(Email));

            Email.ValidateNotEmpty(result, nameof(Email));

            Email.ValidateMaxLength(result, nameof(Email), 256);

            Password.ValidateNotEmpty(result, nameof(Password));

            Password.ValidateMaxLength(result, nameof(Password), 32);

            ConfirmPassword.ValidateNotEmpty(result, nameof(ConfirmPassword));

            ConfirmPassword.ValidateMaxLength(result, nameof(ConfirmPassword), 32);

            ConfirmPassword.ValidateIsEqual(result, nameof(ConfirmPassword), Password, nameof(Password), "Passwords must match!");
        }

    }
}