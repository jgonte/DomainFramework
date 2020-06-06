using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseWithPreRequisites.CourseBoundedContext
{
    public class SaveCourseCommandAggregate : CommandAggregate<Course>
    {
        public SaveCourseCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(CourseWithPreRequisitesConnectionClass.GetConnectionName()))
        {
        }

        public SaveCourseCommandAggregate(CourseInputDto course, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(CourseWithPreRequisitesConnectionClass.GetConnectionName()))
        {
            Initialize(course, dependencies);
        }

        public override void Initialize(IInputDataTransferObject course, EntityDependency[] dependencies)
        {
            Initialize((CourseInputDto)course, dependencies);
        }

        private void Initialize(CourseInputDto course, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Course>(() => new CourseCommandRepository());

            RootEntity = new Course
            {
                Id = course.CourseId,
                Description = course.Description
            };

            Enqueue(new SaveEntityCommandOperation<Course>(RootEntity, dependencies));

            Enqueue(new DeleteLinksCommandOperation<Course>(RootEntity, "UnlinkRequiresFromCourse"));

            if (course.Requires?.Any() == true)
            {
                foreach (var dto in course.Requires)
                {
                    ILinkedAggregateCommandOperation operation;

                    if (dto is CourseInputDto)
                    {
                        operation = new AddLinkedAggregateCommandOperation<Course, SaveCourseCommandAggregate, CourseInputDto>(
                            RootEntity,
                            (CourseInputDto)dto
                        );

                        Enqueue(operation);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    Enqueue(new AddLinkedAggregateCommandOperation<Course, CreatePreRequisiteCommandAggregate, PreRequisiteInputDto>(
                        RootEntity,
                        dto.PreRequisite,
                        new EntityDependency[]
                        {
                            new EntityDependency
                            {
                                Entity = RootEntity,
                                Selector = "IsRequiredBy"
                            },
                            new EntityDependency
                            {
                                Entity = operation.CommandAggregate.RootEntity,
                                Selector = "Requires"
                            }
                        }
                    ));
                }
            }
        }

    }
}