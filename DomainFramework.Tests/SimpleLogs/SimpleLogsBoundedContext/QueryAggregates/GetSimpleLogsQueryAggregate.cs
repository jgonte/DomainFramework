using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleLogs.SimpleLogsBoundedContext
{
    public class GetSimpleLogsQueryAggregate : GetQueryAggregateCollection<SimpleLog, SimpleLogOutputDto, GetSimpleLogByIdQueryAggregate>
    {
        public GetSimpleLogsQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(SimpleLogsConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            SimpleLogQueryRepository.Register(context);
        }

    }
}