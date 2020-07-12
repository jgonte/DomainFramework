using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    public class GetTestEntityByIdQueryAggregate : GetByIdQueryAggregate<TestEntity, int, TestEntityOutputDto>
    {
        public GetCollectionLinkedValueObjectQueryOperation<TestEntity, TypeValue, TestEntity_TypeValues1_QueryRepository.RepositoryKey> GetTypeValues1Operation { get; private set; }

        public GetTestEntityByIdQueryAggregate() : this(null)
        {
        }

        public GetTestEntityByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(EntityWithMultiValuedValueObjectConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            TestEntityQueryRepository.Register(context);

            TestEntity_TypeValues1_QueryRepository.Register(context);

            GetTypeValues1Operation = new GetCollectionLinkedValueObjectQueryOperation<TestEntity, TypeValue, TestEntity_TypeValues1_QueryRepository.RepositoryKey>
            {
                GetLinkedValueObjects = (repository, entity, user) => ((TestEntity_TypeValues1_QueryRepository)repository).GetAll(RootEntity.Id).ToList(),
                GetLinkedValueObjectsAsync = async (repository, entity, user) =>
                {
                    var items = await ((TestEntity_TypeValues1_QueryRepository)repository).GetAllAsync(RootEntity.Id);

                    return items.ToList();
                }
            };

            QueryOperations.Enqueue(GetTypeValues1Operation);
        }

        public override void PopulateDto()
        {
            OutputDto.TestEntityId = RootEntity.Id;

            OutputDto.Text = RootEntity.Text;

            OutputDto.TypeValues1 = GetTypeValues1Dtos();
        }

        public List<TypeValueOutputDto> GetTypeValues1Dtos()
        {
            return GetTypeValues1Operation
                .LinkedValueObjects
                .Select(vo => new TypeValueOutputDto
                {
                    DataType = vo.DataType,
                    Data = vo.Data
                })
                .ToList();
        }

    }
}