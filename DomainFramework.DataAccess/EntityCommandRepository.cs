using DataAccess;
using DomainFramework.Core;
using System.Threading.Tasks;

namespace DomainFramework.DataAccess
{
    /// <summary>
    /// Implementation of a command repository to work using the DataAccess library
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EntityCommandRepository<TEntity> : Core.EntityCommandRepository<TEntity>
        where TEntity : IEntity
    {
        public override void Insert(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateInsertCommand(entity, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);
            }
            else
            {
                HandleInsert(command);
            }
        }

        public override bool Update(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateUpdateCommand(entity, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return HandleUpdate(command);
            }
        }

        public override bool Delete(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateDeleteCommand(entity, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return HandleDelete(command);
            }
        }

        public override async Task InsertAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateInsertCommand(entity, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);
            }
            else
            {
                await HandleInsertAsync(command);
            }
        }

        public override async Task<bool> UpdateAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateUpdateCommand(entity, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return await HandleUpdateAsync(command);
            }
        }


        public override async Task<bool> DeleteAsync(TEntity entity, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateDeleteCommand(entity, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return await HandleDeleteAsync(command);
            }
        }

        protected abstract Command CreateInsertCommand(TEntity entity, IAuthenticatedUser user);

        protected abstract Command CreateUpdateCommand(TEntity entity, IAuthenticatedUser user);

        protected abstract Command CreateDeleteCommand(TEntity entity, IAuthenticatedUser user);

        protected abstract void HandleInsert(Command command);

        protected abstract bool HandleUpdate(Command command);
        
        protected abstract bool HandleDelete(Command command);

        protected abstract Task HandleInsertAsync(Command command);

        protected abstract Task<bool> HandleUpdateAsync(Command command);

        protected abstract Task<bool> HandleDeleteAsync(Command command);
    }
}
