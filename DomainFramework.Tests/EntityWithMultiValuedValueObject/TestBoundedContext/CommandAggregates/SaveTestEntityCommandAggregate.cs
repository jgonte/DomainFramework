using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class SaveTestEntityCommandAggregate : CommandAggregate<TestEntity>
    {
        public SaveTestEntityCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName()))
        {
        }

        public SaveTestEntityCommandAggregate(SaveTestEntityInputDto entity, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName()))
        {
            Initialize(entity, dependencies);
        }

        public override void Initialize(IInputDataTransferObject entity, EntityDependency[] dependencies)
        {
            Initialize((SaveTestEntityInputDto)entity, dependencies);
        }

        private void Initialize(SaveTestEntityInputDto entity, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<TestEntity>(() => new TestEntityCommandRepository());

            RegisterCommandRepositoryFactory<TestEntity_TypeValues1_CommandRepository.RepositoryKey>(() => new TestEntity_TypeValues1_CommandRepository());

            RootEntity = new TestEntity
            {
                Id = entity.TestEntityId,
                Text = entity.Text
            };

            Enqueue(new SaveEntityCommandOperation<TestEntity>(RootEntity, dependencies));

            Enqueue(new DeleteLinkedValueObjectCommandOperation<TestEntity, TypeValue, TestEntity_TypeValues1_CommandRepository.RepositoryKey>(RootEntity));

            foreach (var dto in entity.TypeValues1)
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