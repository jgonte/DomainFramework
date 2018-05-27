using DataAccess;
using DomainFramework.Core;
<<<<<<< HEAD
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
=======

namespace DomainFramework.DataAccess
{
    public abstract class CommandRepository<E, K, T> : Core.CommandRepository<E, K, T>
        where E : Entity<K, T>
    {
        public override void Insert(E entity, IUnitOfWork unitOfWork = null)
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
        {
            var command = CreateInsertCommand(entity);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);
            }
            else
            {
<<<<<<< HEAD
                HandleInsert(command);
            }
        }

        public override bool Update(TEntity entity, IUnitOfWork unitOfWork = null)
=======
                HandleInsert(command, entity);
            }
        }

        public override bool Update(E entity, IUnitOfWork unitOfWork = null)
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
        {
            var command = CreateUpdateCommand(entity);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
<<<<<<< HEAD
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
=======
                return HandleUpdate(command, entity);
            }
        }


        public override bool Delete(E entity, IUnitOfWork unitOfWork = null)
>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
        {
            var command = CreateDeleteCommand(entity);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
<<<<<<< HEAD
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
=======
                return HandleDelete(command, entity);
            }
        }

        protected abstract Command CreateInsertCommand(E entity);

        protected abstract void HandleInsert(Command command, E entity);

        protected abstract Command CreateUpdateCommand(E entity);

        protected abstract bool HandleUpdate(Command command, E entity);

        protected abstract Command CreateDeleteCommand(E entity);

        protected abstract bool HandleDelete(Command command, E entity);

>>>>>>> bd9dc060af59b6a4d3f8b8d2e65aaf2f692497d3
    }
}
