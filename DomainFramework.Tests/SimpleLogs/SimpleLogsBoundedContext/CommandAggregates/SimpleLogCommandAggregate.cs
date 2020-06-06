using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleLogs.SimpleLogsBoundedContext
{
    public class SimpleLogCommandAggregate : CommandAggregate<SimpleLog>
    {
        public SimpleLogCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(SimpleLogsConnectionClass.GetConnectionName()))
        {
        }

        public SimpleLogCommandAggregate(SimpleLogInputDto log, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(SimpleLogsConnectionClass.GetConnectionName()))
        {
            Initialize(log, dependencies);
        }

        public override void Initialize(IInputDataTransferObject log, EntityDependency[] dependencies)
        {
            Initialize((SimpleLogInputDto)log, dependencies);
        }

        private void Initialize(SimpleLogInputDto log, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<SimpleLog>(() => new SimpleLogCommandRepository());

            RootEntity = new SimpleLog
            {
                MessageType = log.MessageType,
                Message = log.Message
            };

            Enqueue(new InsertEntityCommandOperation<SimpleLog>(RootEntity, dependencies));
        }

    }
}