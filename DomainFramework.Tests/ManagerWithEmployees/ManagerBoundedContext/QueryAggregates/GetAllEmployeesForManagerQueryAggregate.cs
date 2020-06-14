using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class GetAllEmployeesForManagerQueryAggregate : QueryAggregateCollection<Employee, EmployeeOutputDto, GetEmployeeByIdAggregate>
    {
        public GetAllEmployeesForManagerQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ManagerWithEmployeesConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            EmployeeQueryRepository.Register(context);
        }

        public (int, IEnumerable<EmployeeOutputDto>) Get(int? managerId)
        {
            var repository = (EmployeeQueryRepository)RepositoryContext.GetQueryRepository(typeof(Employee));

            var entities = repository.GetAllEmployeesForManager(managerId);

            var data = new Tuple<int, IEnumerable<IEntity>>(entities.Count(), entities);

            return Get(data);
        }

        public async Task<(int, IEnumerable<EmployeeOutputDto>)> GetAsync(int? managerId)
        {
            var repository = (EmployeeQueryRepository)RepositoryContext.GetQueryRepository(typeof(Employee));

            var entities = repository.GetAllEmployeesForManager(managerId);

            var data = new Tuple<int, IEnumerable<IEntity>>(entities.Count(), entities);

            return await GetAsync(data);
        }

    }
}