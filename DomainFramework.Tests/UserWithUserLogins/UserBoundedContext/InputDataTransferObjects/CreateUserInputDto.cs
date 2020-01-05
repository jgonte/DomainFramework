using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Validation;

namespace UserWithUserLogins.UserBoundedContext
{
    public class CreateUserInputDto : IInputDataTransferObject
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public List<CreateUserLoginInputDto> UserLogins { get; set; } = new List<CreateUserLoginInputDto>();

        public void Validate(ValidationResult result)
        {
            UserName.ValidateNotEmpty(result, nameof(UserName));

            UserName.ValidateMaxLength(result, nameof(UserName), 256);

            Email.ValidateEmail(result, nameof(Email));

            Email.ValidateNotEmpty(result, nameof(Email));

            Email.ValidateMaxLength(result, nameof(Email), 256);

            var userLoginsCount = (uint)UserLogins.Where(item => item != null).Count();

            userLoginsCount.ValidateCountIsEqualOrGreaterThan(result, 1, nameof(UserLogins));

            foreach (var userLogin in UserLogins)
            {
                userLogin.Validate(result);
            }
        }

    }
}