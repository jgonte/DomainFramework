using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace UserWithUserLogins.UserBoundedContext
{
    public class UserOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginOutputDto> UserLogins { get; set; }

    }
}