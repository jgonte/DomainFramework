using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace RegisterUser.UserBoundedContext
{
    public class UserOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public Guid SubjectId { get; set; }

        public string Username { get; set; }

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }

        public string Email { get; set; }

    }
}