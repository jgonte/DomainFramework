using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System.Linq;

namespace DomainFramework.Tests.EntityWithValueObjectCollection
{
    class PhoneCommandRepository : LinkedValueObjectCommandRepository<Phone>
    {
        protected override Command CreateInsertCommand(Phone phone, IAuthenticatedUser user)
        {
            return Query<Phone>
                .Single()
                .Connection(ConnectionName)
                .StoredProcedure("p_Phone_Create")
                .AutoGenerateParameters( // Generate the parameters from the data
                    qbeObject: phone
                )
                .OnBeforeCommandExecuted(cmd =>
                {
                    var entity = (PersonEntity4)Dependencies().Single();

                    cmd.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("personId").Value(entity.Id)
                    );
                });
        }

        protected override Command CreateDeleteCollectionCommand(IAuthenticatedUser user)
        {
            return Command
                .NonQuery()
                .Connection(ConnectionName)
                .StoredProcedure("p_Delete_Phones_For_Person")
                .OnBeforeCommandExecuted(cmd =>
                {
                    var entity = (PersonEntity4)Dependencies().Single();

                    cmd.Parameters( // Map the extra parameters for the foreign key(s)
                        p => p.Name("personId").Value(entity.Id)
                    );
                });
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<Phone>)command).Execute();
        }

        public class RepositoryKey
        {
        }
    }
}