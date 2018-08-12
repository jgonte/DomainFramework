using DataAccess;
using DomainFramework.Core;
using System.Threading.Tasks;

namespace DomainFramework.DataAccess
{
    /// <summary>
    /// Implementation of a command repository to work using the DataAccess library
    /// </summary>
    /// <typeparam name="TValueObject"></typeparam>
    public abstract class CommandValueObjectRepository<TValueObject> : Core.ValueObjectCommandRepository<TValueObject>
        where TValueObject : IValueObject
    {
        public override void Insert(TValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateInsertCommand(valueObject, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);
            }
            else
            {
                HandleInsert(command);
            }
        }

        public override bool DeleteAll(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateDeleteAllCommand(user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return HandleDeleteAll(command);
            }
        }

        public override async Task InsertAsync(TValueObject valueObject, IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateInsertCommand(valueObject, user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);
            }
            else
            {
                await HandleInsertAsync(command);
            }
        }

        public override async Task<bool> DeleteAllAsync(IAuthenticatedUser user, IUnitOfWork unitOfWork)
        {
            var command = CreateDeleteAllCommand(user);

            if (unitOfWork != null)
            {
                ((UnitOfWork)unitOfWork).Commands(command);

                return true;
            }
            else
            {
                return await HandleDeleteAllAsync(command);
            }
        }

        protected abstract Command CreateInsertCommand(TValueObject valueObject, IAuthenticatedUser user);

        protected abstract Command CreateDeleteAllCommand(IAuthenticatedUser user);

        protected abstract void HandleInsert(Command command);

        protected abstract bool HandleDeleteAll(Command command);

        protected abstract Task HandleInsertAsync(Command command);

        protected abstract Task<bool> HandleDeleteAllAsync(Command command);
    }
}
