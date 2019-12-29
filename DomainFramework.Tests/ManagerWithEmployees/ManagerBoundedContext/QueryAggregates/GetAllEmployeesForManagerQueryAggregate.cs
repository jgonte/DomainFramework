//using DomainFramework.Core;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace ManagerWithEmployees.ManagerBoundedContext
//{
//    public class GetAllEmployeesForManagerQueryAggregate : QueryAggregate<Employee, EmployeeOutputDto>
//    {
//        public GetAllEmployeesForManagerQueryAggregate()
//        {
//            var context = new DomainFramework.DataAccess.RepositoryContext(ManagerWithEmployeesConnectionClass.GetConnectionName());

//            EmployeeQueryRepository.Register(context);

//            RepositoryContext = context;
//        }

//        public EmployeeOutputDto Get(int? managerId, IAuthenticatedUser user)
//        {
//            var repository = (EmployeeQueryRepository)RepositoryContext.GetQueryRepository(typeof(Employee));

//            RootEntity = repository.GetAllEmployeesForManager(managerId, user);

//            if (RootEntity == null)
//            {
//                return null;
//            }

//            LoadLinks(null);

//            PopulateDto(RootEntity);

//            return OutputDto;
//        }

//        public async Task<EmployeeOutputDto> GetAsync(int? managerId, IAuthenticatedUser user)
//        {
//            var repository = (EmployeeQueryRepository)RepositoryContext.GetQueryRepository(typeof(Employee));

//            RootEntity = await repository.GetAllEmployeesForManagerAsync(managerId, user);

//            if (RootEntity == null)
//            {
//                return null;
//            }

//            await LoadLinksAsync(null);

//            PopulateDto(RootEntity);

//            return OutputDto;
//        }

//        public override void PopulateDto(Employee entity)
//        {
//            if (entity is Manager)
//            {
//                var manager = (Manager)entity;

//                var managerDto = new ManagerOutputDto();

//                managerDto.Id = manager.Id.Value;

//                managerDto.Department = manager.Department;

//                managerDto.Name = manager.Name;

//                managerDto.SupervisorId = manager.SupervisorId;

//                OutputDto = managerDto;
//            }
//            else
//            {
//                var employeeDto = new EmployeeOutputDto();

//                employeeDto.Id = entity.Id.Value;

//                employeeDto.Name = entity.Name;

//                employeeDto.SupervisorId = entity.SupervisorId;

//                OutputDto = employeeDto;
//            }
//        }

//    }
//}