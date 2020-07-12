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

        public List<UserLoginInputDto> UserLogins { get; set; } = new List<UserLoginInputDto>();

        public virtual void Validate(ValidationResult result)
        {
            UserName.ValidateRequired(result, nameof(UserName));

            UserName.ValidateMaxLength(result, nameof(UserName), 256);

            Email.ValidateEmail(result, nameof(Email));

            Email.ValidateRequired(result, nameof(Email));

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