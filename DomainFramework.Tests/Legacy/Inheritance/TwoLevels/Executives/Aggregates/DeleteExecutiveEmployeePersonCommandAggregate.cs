﻿using DomainFramework.Core;
using Utilities;

namespace DomainFramework.Tests
{
    class DeleteExecutiveEmployeePersonCommandAggregate : CommandAggregate<ExecutiveEntity>
    {
        public DeleteExecutiveEmployeePersonCommandAggregate(DataAccess.RepositoryContext context, int? executiveId) : base(context)
        {
            RootEntity = new ExecutiveEntity
            {
                Id = executiveId
            };

            Enqueue(
                new DeleteEntityCommandOperation<ExecutiveEntity>(RootEntity)
            );
        }

        public override void Initialize(IInputDataTransferObject inputDto, EntityDependency[] dependencies)
        {
            throw new System.NotImplementedException();
        }
    }
}