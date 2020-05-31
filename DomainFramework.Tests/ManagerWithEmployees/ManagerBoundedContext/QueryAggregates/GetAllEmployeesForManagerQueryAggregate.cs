using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class GetAllEmployeesForManagerQueryAggregate : GetAllQueryAggregateCollection<Employee, EmployeeOutputDto, GetEmployeeByIdAggregate>
    {
        public GetAllEmployeesForManagerQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ManagerWithEmployeesConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            EmployeeQueryRepository.Register(context);
        }

    }
}