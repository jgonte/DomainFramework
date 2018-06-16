using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DomainFramework.Core;

namespace DomainFramework.Tests.EntityWithValueObjectCollection
{
    class PhoneCommandRepository : DataAccess.CommandValueObjectRepository<Phone>
    {
        protected override Command CreateInsertCommand(IValueObject valueObject, IAuthenticatedUser user)
        {
            var command = Query<PageEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Phone_Create")
                .AutoGenerateParameters( // Generate the parameters from the data
                    qbeObject: valueObject
                );

            command.OnBeforeCommandExecuted(() =>
            {
                var entity = (PersonEntity4)TransferEntities().Single();

                command.Parameters( // Map the extra parameters for the foreign key(s)
                    p => p.Name("personId").Value(entity.Id)
                );
            });

            return command;
        }

        protected override Command CreateDeleteAllCommand(IValueObject valueObject, IAuthenticatedUser user)
        {
            throw new System.NotImplementedException();
        }

        protected override bool HandleDeleteAll(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<bool> HandleDeleteAllAsync(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override void HandleInsert(Command command)
        {
            throw new System.NotImplementedException();
        }

        protected override Task HandleInsertAsync(Command command)
        {
            throw new System.NotImplementedException();
        }
    }
}