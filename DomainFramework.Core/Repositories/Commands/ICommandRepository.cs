<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DomainFramework.Core
=======
﻿namespace DomainFramework.Core
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
{
    /// <summary>
    /// Defines a write only repository
    /// </summary>
    public interface ICommandRepository : IRepository
    {
        void Save(IEntity entity, IUnitOfWork unitOfWork = null);

        void Insert(IEntity entity, IUnitOfWork unitOfWork = null);

        bool Update(IEntity entity, IUnitOfWork unitOfWork = null);

        bool Delete(IEntity entity, IUnitOfWork unitOfWork = null);
<<<<<<< HEAD

        Task SaveAsync(IEntity entity, IUnitOfWork unitOfWork = null);

        Task InsertAsync(IEntity entity, IUnitOfWork unitOfWork = null);

        Task<bool> UpdateAsync(IEntity entity, IUnitOfWork unitOfWork = null);

        Task<bool> DeleteAsync(IEntity entity, IUnitOfWork unitOfWork = null);

        /// <summary>
        /// The entities whise ids are required by the linked and join entities at the time of persistance
        /// </summary>
        Func<IEnumerable<IEntity>> TransferEntities { get; set; }
=======
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
    }
}