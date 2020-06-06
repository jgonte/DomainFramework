using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class AddTypeValue1CommandAggregate : CommandAggregate<TestEntity>
    {
        public AddTypeValue1CommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName()))
        {
        }

        public AddTypeValue1CommandAggregate(AddTypeValues1InputDto typeValue, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName()))
        {
            Initialize(typeValue, dependencies);
        }

        public override void Initialize(IInputDataTransferObject typeValue, EntityDependency[] dependencies)
        {
            Initialize((AddTypeValues1InputDto)typeValue, dependencies);
        }

        private void Initialize(AddTypeValues1InputDto typeValue, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<TestEntity>(() => new TestEntityCommandRepository());

            RegisterCommandRepositoryFactory<TestEntity_TypeValues1_CommandRepository.RepositoryKey>(() => new TestEntity_TypeValues1_CommandRepository());

            RootEntity = new TestEntity
            {
                Id = typeValue.TestEntityId
            };

            foreach (var dto in typeValue.TypeValues1)
            {
                var typeValueValueObject = new TypeValue
                {
                    DataType = dto.DataType,
                    Data = dto.Data
                };

                Enqueue(new AddLinkedValueObjectCommandOperation<TestEntity, TypeValue, TestEntity_TypeValues1_CommandRepository.RepositoryKey>(RootEntity, typeValueValueObject));
            }
        }

    }
}