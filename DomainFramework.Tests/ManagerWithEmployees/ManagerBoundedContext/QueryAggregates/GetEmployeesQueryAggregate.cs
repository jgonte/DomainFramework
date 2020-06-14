using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class GetEmployeesQueryAggregate : QueryAggregateCollection<Employee, EmployeeOutputDto, GetEmployeeByIdAggregate>
    {
        public GetEmployeesQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ManagerWithEmployeesConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            EmployeeQueryRepository.Register(context);
        }

        public (int, IEnumerable<EmployeeOutputDto>) Get(CollectionQueryParameters queryParameters)
        {
            var repository = (EmployeeQueryRepository)RepositoryContext.GetQueryRepository(typeof(Employee));

            var (count, entities) = repository.Get(queryParameters);

            var data = new Tuple<int, IEnumerable<IEntity>>(count, entities);

            return Get(data);
        }

        public async Task<(int, IEnumerable<EmployeeOutputDto>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var repository = (EmployeeQueryRepository)RepositoryContext.GetQueryRepository(typeof(Employee));

            var (count, entities) = await repository.GetAsync(queryParameters);

            var data = new Tuple<int, IEnumerable<IEntity>>(count, entities);

            return await GetAsync(data);
        }

    }
}