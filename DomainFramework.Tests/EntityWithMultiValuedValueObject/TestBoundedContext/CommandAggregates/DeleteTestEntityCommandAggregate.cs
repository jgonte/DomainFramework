using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class DeleteTestEntityCommandAggregate : CommandAggregate<TestEntity>
    {
        public DeleteTestEntityCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName()))
        {
        }

        public DeleteTestEntityCommandAggregate(DeleteTestEntityInputDto entity, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName()))
        {
            Initialize(entity, dependencies);
        }

        public override void Initialize(IInputDataTransferObject entity, EntityDependency[] dependencies)
        {
            Initialize((DeleteTestEntityInputDto)entity, dependencies);
        }

        private void Initialize(DeleteTestEntityInputDto entity, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<TestEntity>(() => new TestEntityCommandRepository());

            RootEntity = new TestEntity
            {
                Id = entity.TestEntityId
            };

            Enqueue(new DeleteEntityCommandOperation<TestEntity>(RootEntity, dependencies));
        }

    }
}