using DataAccess;
using DomainFramework.Core;
using System.Threading.Tasks;

namespace DomainFramework.DataAccess
{
    /// <summary>
    /// Implementation of a command repository to work using the DataAccess library
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    public abstract class CommandRepository<TEntity, TKey> : Core.CommandRepository<TEntity, TKey>
        where TEntity : Entity<TKey>
    {
        public override void Insert(TEntity entity, IUnitOfWork unitOfWork = null)
        {
            var command = CreateInsertCommand(entity);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);
            }
            else
            {
                HandleInsert(command);
            }
        }

        public override bool Update(TEntity entity, IUnitOfWork unitOfWork = null)
        {
            var command = CreateUpdateCommand(entity);

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

        public override bool Delete(TEntity entity, IUnitOfWork unitOfWork = null)
        {
            var command = CreateDeleteCommand(entity);

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

        public override async Task InsertAsync(TEntity entity, IUnitOfWork unitOfWork = null)
        {
            var command = CreateInsertCommand(entity);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);
            }
            else
            {
                await HandleInsertAsync(command);
            }
        }

        public override async Task<bool> UpdateAsync(TEntity entity, IUnitOfWork unitOfWork = null)
        {
            var command = CreateUpdateCommand(entity);

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


        public override async Task<bool> DeleteAsync(TEntity entity, IUnitOfWork unitOfWork = null)
        {
            var command = CreateDeleteCommand(entity);

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

        protected abstract Command CreateInsertCommand(TEntity entity);

        protected abstract Command CreateUpdateCommand(TEntity entity);

        protected abstract Command CreateDeleteCommand(TEntity entity);

        protected abstract void HandleInsert(Command command);

        protected abstract bool HandleUpdate(Command command);
        
        protected abstract bool HandleDelete(Command command);

        protected abstract Task HandleInsertAsync(Command command);

        protected abstract Task<bool> HandleUpdateAsync(Command command);

        protected abstract Task<bool> HandleDeleteAsync(Command command);
    }
}
