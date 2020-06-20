using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class GetClassByIdQueryAggregate : GetByIdQueryAggregate<Class, int?, ClassOutputDto>
    {
        public GetAllLinkedAggregateQueryCollectionOperation<int?, Student, StudentOutputDto> GetAllStudentsLinkedAggregateQueryOperation { get; set; }

        public GetClassByIdQueryAggregate() : this(null)
        {
        }

        public GetClassByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            ClassQueryRepository.Register(context);

            StudentQueryRepository.Register(context);

            GetAllStudentsLinkedAggregateQueryOperation = new GetAllLinkedAggregateQueryCollectionOperation<int?, Student, StudentOutputDto>
            {
                GetAllLinkedEntities = (repository, entity, user) => ((StudentQueryRepository)repository).GetAllStudentsForClass(RootEntity.Id).ToList(),
                GetAllLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((StudentQueryRepository)repository).GetAllStudentsForClassAsync(RootEntity.Id);

                    return entities.ToList();
                },
                CreateLinkedQueryAggregate = entity =>
                {
                    if (entity is Student)
                    {
                        return new GetStudentByIdQueryAggregate();
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetAllStudentsLinkedAggregateQueryOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.ClassId = RootEntity.Id.Value;

            OutputDto.Name = RootEntity.Name;

            OutputDto.Students = GetAllStudentsLinkedAggregateQueryOperation.OutputDtos;
        }

    }
}