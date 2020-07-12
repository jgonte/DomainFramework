using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerWithEmployees.ManagerBoundedContext
{
    public class ManagerQueryAggregate : GetByIdQueryAggregate<Manager, int, ManagerOutputDto>
    {
        public GetAllLinkedAggregateQueryCollectionOperation<int, Employee, EmployeeOutputDto> GetAllEmployeesLinkedAggregateQueryOperation { get; set; }

        public ManagerQueryAggregate() : this(null)
        {
        }

        public ManagerQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(ManagerWithEmployeesConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            ManagerQueryRepository.Register(context);

            EmployeeQueryRepository.Register(context);

            GetAllEmployeesLinkedAggregateQueryOperation = new GetAllLinkedAggregateQueryCollectionOperation<int, Employee, EmployeeOutputDto>
            {
                GetAllLinkedEntities = (repository, entity, user) => ((EmployeeQueryRepository)repository).GetAllEmployeesForManager(RootEntity.Id).ToList(),
                GetAllLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((EmployeeQueryRepository)repository).GetAllEmployeesForManagerAsync(RootEntity.Id);

                    return entities.ToList();
                },
                CreateLinkedQueryAggregate = entity =>
                {
                    if (entity is Manager)
                    {
                        return new ManagerQueryAggregate(new HashSet<(string, IEntity)>
                        {
                            ("Employees", entity)
                        });
                    }
                    else if (entity is Employee)
                    {
                        return new GetEmployeeByIdAggregate();
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetAllEmployeesLinkedAggregateQueryOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.EmployeeId = RootEntity.Id;

            OutputDto.Department = RootEntity.Department;

            OutputDto.Name = RootEntity.Name;

            OutputDto.SupervisorId = RootEntity.SupervisorId;

            OutputDto.Employees = GetAllEmployeesLinkedAggregateQueryOperation.OutputDtos;
        }

    }
}