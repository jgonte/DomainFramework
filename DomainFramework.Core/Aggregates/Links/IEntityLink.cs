using System;

namespace DomainFramework.Core
{
    public interface IEntityLink
    {
        /// <summary>
        /// The type of the linked entity to retrieve its repository
        /// </summary>
        Type LinkedEntityType { get; }
    }
}