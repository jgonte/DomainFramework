using DataAccess;
using DomainFramework.Core;

namespace DomainFramework.DataAccess
{
    public abstract class CommandRepository<E, K, T> : Core.CommandRepository<E, K, T>
        where E : Entity<K, T>
    {
        public override void Insert(E entity, IUnitOfWork unitOfWork = null)
        {
            var command = CreateInsertCommand(entity);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);
            }
            else
            {
                HandleInsert(command, entity);
            }
        }

        public override bool Update(E entity, IUnitOfWork unitOfWork = null)
        {
            var command = CreateUpdateCommand(entity);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return HandleUpdate(command, entity);
            }
        }


        public override bool Delete(E entity, IUnitOfWork unitOfWork = null)
        {
            var command = CreateDeleteCommand(entity);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return HandleDelete(command, entity);
            }
        }

        protected abstract Command CreateInsertCommand(E entity);

        protected abstract void HandleInsert(Command command, E entity);

        protected abstract Command CreateUpdateCommand(E entity);

        protected abstract bool HandleUpdate(Command command, E entity);

        protected abstract Command CreateDeleteCommand(E entity);

        protected abstract bool HandleDelete(Command command, E entity);

    }
}
