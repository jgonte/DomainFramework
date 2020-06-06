using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseWithPreRequisites.CourseBoundedContext
{
    public class GetCourseByIdQueryAggregate : GetByIdQueryAggregate<Course, int?, CourseOutputDto>
    {
        public GetAllLinkedAggregateQueryCollectionOperation<int?, Course, CourseOutputDto> GetAllRequiresLinkedAggregateQueryOperation { get; set; }

        public GetCourseByIdQueryAggregate() : this(null)
        {
        }

        public GetCourseByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(CourseWithPreRequisitesConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            CourseQueryRepository.Register(context);

            GetAllRequiresLinkedAggregateQueryOperation = new GetAllLinkedAggregateQueryCollectionOperation<int?, Course, CourseOutputDto>
            {
                GetAllLinkedEntities = (repository, entity, user) => ((CourseQueryRepository)repository).GetAllRequiresForCourse(RootEntity.Id).ToList(),
                GetAllLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((CourseQueryRepository)repository).GetAllRequiresForCourseAsync(RootEntity.Id);

                    return entities.ToList();
                },
                CreateLinkedQueryAggregate = entity => 
                {
                    if (entity is Course)
                    {
                        return new GetCourseByIdQueryAggregate(new HashSet<(string, IEntity)>
                        {
                            ("Requires", entity)
                        });
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetAllRequiresLinkedAggregateQueryOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.Description = RootEntity.Description;

            OutputDto.Requires = GetAllRequiresLinkedAggregateQueryOperation.OutputDtos;
        }

    }
}