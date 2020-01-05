using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class GetAllEmployeesForManagerQueryAggregate : GetAllLinkedQueryAggregateCollection<int?, Employee, EmployeeOutputDto, GetEmployeeByIdAggregate>
    {
        public GetAllEmployeesForManagerQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ManagerWithEmployeesConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            EmployeeQueryRepository.Register(context);

            GetAllLinkedEntities = (repository, id, user) => ((EmployeeQueryRepository)repository).GetAllEmployeesForManager(id).ToList();

            GetAllLinkedEntitiesAsync = async (repository, id, user) =>
            {
                var entities = await ((EmployeeQueryRepository)repository).GetAllEmployeesForManagerAsync(id);

                return entities.ToList();
            };
        }

    }
}