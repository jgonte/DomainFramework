using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace UserWithUserLogins.UserBoundedContext
{
    public class UserLoginOutputDto : IOutputDataTransferObject
    {
        public string Provider { get; set; }

        public string UserKey { get; set; }

    }
}