using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Validation;

namespace UserWithUserLogins.UserBoundedContext
{
    public class UserAddUserLoginInputDto : IInputDataTransferObject
    {
        public int Id { get; set; }

        public List<AddUserLoginInputDto> UserLogins { get; set; } = new List<AddUserLoginInputDto>();

        public void Validate(ValidationResult result)
        {
            Id.ValidateNotZero(result, nameof(Id));

            var userLoginsCount = (uint)UserLogins.Where(item => item != null).Count();

            userLoginsCount.ValidateCountIsEqualOrGreaterThan(result, 1, nameof(UserLogins));

            foreach (var userLogin in UserLogins)
            {
                userLogin.Validate(result);
            }
        }

    }
}