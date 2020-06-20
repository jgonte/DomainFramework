using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class GetEmployeeByIdAggregate : GetByIdQueryAggregate<Employee, int?, EmployeeOutputDto>
    {
        public GetEmployeeByIdAggregate() : this(null)
        {
        }

        public GetEmployeeByIdAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(ManagerWithEmployeesConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            EmployeeQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.EmployeeId = RootEntity.Id.Value;

            OutputDto.Name = RootEntity.Name;

            OutputDto.SupervisorId = RootEntity.SupervisorId;
        }

    }
}