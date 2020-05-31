using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace UserWithUserLogins.UserBoundedContext
{
    public class User : Entity<int?>
    {
        /// <summary>
        /// The user name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The normalized user name
        /// </summary>
        public string NormalizedUserName { get; set; }

        /// <summary>
        /// The user's email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The user's normalized email
        /// </summary>
        public string NormalizedEmail { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

    }
}