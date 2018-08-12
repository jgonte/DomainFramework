using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DomainFramework.Core;

namespace DomainFramework.Tests.EntityWithValueObjectCollection
{
    class PhoneCommandRepository : DataAccess.CommandValueObjectRepository<Phone>
    {
        protected override Command CreateInsertCommand(Phone phone, IAuthenticatedUser user)
        {
            return Query<PageEntity>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Phone_Create")
                .AutoGenerateParameters( // Generate the parameters from the data
                    qbeObject: phone
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var entity = (PersonEntity4)TransferEntities().Single();

                    cmd.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("personId").Value(entity.Id)
                    );
                });
        }

        protected override Command CreateDeleteAllCommand(IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Delete_Phones_For_Person")
                .OnBeforeCommandExecuted(cmd =>
                {
                    var entity = (PersonEntity4)TransferEntities().Single();

                    cmd.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("personId").Value(entity.Id)
                    );
                });
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