//using DomainFramework.Core;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ManagerWithEmployees.ManagerBoundedContext
//{
//    public class GetAllEmployeesForManagerQueryAggregateCollection : QueryAggregate<GetAllEmployeesForManagerQueryAggregate, Employee, EmployeeOutputDto>
//    {
//        public GetAllEmployeesForManagerQueryAggregateCollection()
//        {
//            var context = new DomainFramework.DataAccess.RepositoryContext(ManagerWithEmployeesConnectionClass.GetConnectionName());

//            EmployeeQueryRepository.Register(context);

//            RepositoryContext = context;
//        }

//    }
//}