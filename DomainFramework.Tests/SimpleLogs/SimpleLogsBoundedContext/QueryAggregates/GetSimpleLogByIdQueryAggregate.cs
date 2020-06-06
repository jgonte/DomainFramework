using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleLogs.SimpleLogsBoundedContext
{
    public class GetSimpleLogByIdQueryAggregate : GetByIdQueryAggregate<SimpleLog, int?, SimpleLogOutputDto>
    {
        public GetSimpleLogByIdQueryAggregate() : this(null)
        {
        }

        public GetSimpleLogByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(SimpleLogsConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            SimpleLogQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.MessageType = RootEntity.MessageType;

            OutputDto.Message = RootEntity.Message;

            OutputDto.When = RootEntity.When;
        }

    }
}