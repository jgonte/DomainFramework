using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class GetEmployeeByIdAggregate : GetByIdQueryAggregate<Employee, int?, EmployeeOutputDto>
    {
        public GetEmployeeByIdAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ManagerWithEmployeesConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            EmployeeQueryRepository.Register(context);
        }

        public override void PopulateDto(Employee entity)
        {
            if (entity is Manager)
            {
                var manager = (Manager)entity;

                var managerDto = new ManagerOutputDto();

                managerDto.Id = manager.Id.Value;

                managerDto.Department = manager.Department;

                managerDto.Name = manager.Name;

                managerDto.SupervisorId = manager.SupervisorId;

                OutputDto = managerDto;
            }
            else
            {
                var employeeDto = new EmployeeOutputDto();

                employeeDto.Id = entity.Id.Value;

                employeeDto.Name = entity.Name;

                employeeDto.SupervisorId = entity.SupervisorId;

                OutputDto = employeeDto;
            }
        }

    }
}