using DomainFramework.Core;
using System.Collections.Generic;
using Utilities;

namespace DomainFramework.Tests
{
    class ManagerEmployeesCommandAggregate : CommandAggregate<PersonEntity3>
    {
        public List<PersonEntity3> Employees { get; set; } = new List<PersonEntity3>();

        public ManagerEmployeesCommandAggregate(IRepositoryContext context, ManagerDto managerDto) : base(context)
        {
            RootEntity = new PersonEntity3
            {
                FirstName = managerDto.Name
            };

            Enqueue(
                new SaveEntityCommandOperation<PersonEntity3>(RootEntity)
            );

            foreach (var employeeDto in managerDto.Employees)
            {
                var employeeEntity = new PersonEntity3
                {
                    Id = employeeDto.Id,
                    FirstName = employeeDto.FirstName
                };

                Employees.Add(employeeEntity);

                if (employeeEntity.Id != null)
                {
                    Enqueue(
                        new UpdateEntityCommandOperation<PersonEntity3>(
                            employeeEntity,
                            dependencies: new EntityDependency[]
                            {
                                new EntityDependency
                                {
                                    Entity = RootEntity
                                }
                            })
                    );
                }
                else
                {
                    Enqueue(
                        new AddLinkedEntityCommandOperation<PersonEntity3, PersonEntity3>(
                            RootEntity,
                            getLinkedEntity: () => employeeEntity
                        )
                    );
                }
            }
        }

        public override void Initialize(IInputDataTransferObject inputDto)
        {
            throw new System.NotImplementedException();
        }
    }
}