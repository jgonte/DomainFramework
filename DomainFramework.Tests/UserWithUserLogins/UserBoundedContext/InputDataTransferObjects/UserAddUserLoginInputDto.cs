using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Validation;

namespace UserWithUserLogins.UserBoundedContext
{
    public class UserAddUserLoginInputDto : IInputDataTransferObject
    {
        public int UserId { get; set; }

        public List<UserLoginInputDto> UserLogins { get; set; } = new List<UserLoginInputDto>();

        public virtual void Validate(ValidationResult result)
        {
            UserId.ValidateNotZero(result, nameof(UserId));

            var userLoginsCount = (uint)UserLogins.Where(item => item != null).Count();

            userLoginsCount.ValidateCountIsEqualOrGreaterThan(result, 1, nameof(UserLogins));

            foreach (var userLogin in UserLogins)
            {
                userLogin.Validate(result);
            }
        }

    }
}