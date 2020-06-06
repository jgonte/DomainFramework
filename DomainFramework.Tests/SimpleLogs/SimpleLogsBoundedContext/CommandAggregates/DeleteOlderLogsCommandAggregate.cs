using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleLogs.SimpleLogsBoundedContext
{
    public class DeleteOlderLogsCommandAggregate : CommandAggregate<SimpleLog>
    {
        public DeleteOlderLogsCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(SimpleLogsConnectionClass.GetConnectionName()))
        {
        }

        public DeleteOlderLogsCommandAggregate(DeleteOlderLogsInputDto log, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(SimpleLogsConnectionClass.GetConnectionName()))
        {
            Initialize(log, dependencies);
        }

        public override void Initialize(IInputDataTransferObject log, EntityDependency[] dependencies)
        {
            Initialize((DeleteOlderLogsInputDto)log, dependencies);
        }

        private void Initialize(DeleteOlderLogsInputDto log, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<SimpleLog>(() => new SimpleLogCommandRepository());

            RootEntity = new SimpleLog
            {
                When = log.When
            };

            Enqueue(new DeleteEntityCommandOperation<SimpleLog>(RootEntity, null, "DeleteOlderLogs"));
        }

    }
}