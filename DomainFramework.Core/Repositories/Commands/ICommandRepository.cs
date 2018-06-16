using System;
using System.Collections.Generic;

namespace DomainFramework.Core
{
    /// <summary>
    /// Common interface for Entity and ValueObject repositories
    /// </summary>
    public interface ICommandRepository : IRepository
    {
        /// <summary>
        /// The entities whose ids are required by the linked and join entities at the time of persistance
        /// </summary>
        Func<IEnumerable<IEntity>> TransferEntities { get; set; }
    }
}