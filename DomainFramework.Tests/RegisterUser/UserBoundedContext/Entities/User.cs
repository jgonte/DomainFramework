using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace RegisterUser.UserBoundedContext
{
    public class User : Entity<int?>
    {
        /// <summary>
        /// The system identifier of the user
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// The user name
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password salt
        /// </summary>
        public string PasswordSalt { get; set; }

        /// <summary>
        /// The password hash
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// The email of the user
        /// </summary>
        public string Email { get; set; }

    }
}