using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class ManagerQueryAggregate : GetByIdQueryAggregate<Manager, int?, ManagerOutputDto>
    {
        private ManagerQueryAggregate _managerQueryAggregate;

        public GetCollectionLinkedEntityQueryOperation<Employee> GetEmployeesQueryOperation { get; }

        public IEnumerable<Employee> Employees => GetEmployeesQueryOperation.LinkedEntities;

        public ManagerQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(ManagerWithEmployeesConnectionClass.GetConnectionName());

            ManagerQueryRepository.Register(context);

            EmployeeQueryRepository.Register(context);

            RepositoryContext = context;

            GetEmployeesQueryOperation = new GetCollectionLinkedEntityQueryOperation<Employee>
            {
                GetLinkedEntities = (repository, entity, user) => ((EmployeeQueryRepository)repository).GetEmployeesForManager(RootEntity.Id, user).ToList(),
                GetLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((EmployeeQueryRepository)repository).GetEmployeesForManagerAsync(RootEntity.Id, user);

                    return entities.ToList();
                }
            };

            QueryOperations.Enqueue(GetEmployeesQueryOperation);
        }

        public List<EmployeeOutputDto> GetEmployeesDtos()
        {
            if (Employees?.Any() == true)
            {
                var employees = new List<EmployeeOutputDto>();

                foreach (var employee in Employees)
                {
                    if (employee is Manager)
                    {
                        var manager = (Manager)employee;

                        var dto = new ManagerOutputDto
                        {
                            Id = manager.Id.Value,
                            Department = manager.Department,
                            Name = manager.Name,
                            SupervisorId = manager.SupervisorId
                        };

                        if (_managerQueryAggregate == null)
                        {
                            _managerQueryAggregate = new ManagerQueryAggregate();
                        }

                        if (_managerQueryAggregate.RootEntity == null)
                        {
                            _managerQueryAggregate.RootEntity = manager;

                            _managerQueryAggregate.LoadLinks();

                            dto.Employees = _managerQueryAggregate.GetEmployeesDtos();

                            _managerQueryAggregate.RootEntity = null;
                        }

                        employees.Add(dto);
                    }
                    else
                    {
                        var dto = new EmployeeOutputDto
                        {
                            Id = employee.Id.Value,
                            Name = employee.Name,
                            SupervisorId = employee.SupervisorId
                        };

                        employees.Add(dto);
                    }
                }

                return employees;
            }

            return null;
        }

        public override void PopulateDto(Manager entity)
        {
            OutputDto.Id = entity.Id.Value;

            OutputDto.Department = entity.Department;

            OutputDto.Name = entity.Name;

            OutputDto.SupervisorId = entity.SupervisorId;

            OutputDto.Employees = GetEmployeesDtos();
        }

    }
}