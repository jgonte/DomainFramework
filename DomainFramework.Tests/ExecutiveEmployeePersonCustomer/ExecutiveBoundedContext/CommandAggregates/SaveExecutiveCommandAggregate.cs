using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class SaveExecutiveCommandAggregate : CommandAggregate<Executive>
    {
        public SaveExecutiveCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName()))
        {
        }

        public SaveExecutiveCommandAggregate(SaveExecutiveInputDto executive, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName()))
        {
            Initialize(executive, dependencies);
        }

        public override void Initialize(IInputDataTransferObject executive, EntityDependency[] dependencies)
        {
            Initialize((SaveExecutiveInputDto)executive, dependencies);
        }

        private void Initialize(SaveExecutiveInputDto executive, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Executive>(() => new ExecutiveCommandRepository());

            RegisterCommandRepositoryFactory<Employee>(() => new EmployeeCommandRepository());

            RegisterCommandRepositoryFactory<Person>(() => new PersonCommandRepository());

            RootEntity = new Executive
            {
                Id = executive.ExecutiveId,
                Bonus = executive.Bonus,
                HireDate = executive.HireDate,
                Name = executive.Name,
                Asset = new Asset
                {
                    Number = executive.Asset.Number
                }
            };

            Enqueue(new SaveEntityCommandOperation<Executive>(RootEntity, dependencies));
        }

    }
}