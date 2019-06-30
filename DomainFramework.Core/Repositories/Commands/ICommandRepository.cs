using System;
using System.Collections.Generic;

namespace DomainFramework.Core
{
    /// <summary>
    /// Common interface for Entity and LinkedValueObject command repositories
    /// </summary>
    public interface ICommandRepository : IRepository
    {
        /// <summary>
        /// The entities whose ids are not available until they are inserted
        /// </summary>
        Func<IEnumerable<IEntity>> Dependencies { get; set; }
    }
}