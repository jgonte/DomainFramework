using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseWithPreRequisitesAndRelated.CourseBoundedContext
{
    public class CreateCourseRelationCommandAggregate : CommandAggregate<CourseRelation>
    {
        public CreateCourseRelationCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName()))
        {
        }

        public CreateCourseRelationCommandAggregate(CourseRelationInputDto courseRelation, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName()))
        {
            Initialize(courseRelation, dependencies);
        }

        public override void Initialize(IInputDataTransferObject courseRelation, EntityDependency[] dependencies)
        {
            Initialize((CourseRelationInputDto)courseRelation, dependencies);
        }

        private void Initialize(CourseRelationInputDto courseRelation, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<CourseRelation>(() => new CourseRelationCommandRepository());

            RootEntity = new CourseRelation
            {
                Id = new CourseRelationId
                {
                    Relates_CourseId = courseRelation.Relates_CourseId,
                    IsRelatedTo_CourseId = courseRelation.IsRelatedTo_CourseId
                }
            };

            Enqueue(new InsertEntityCommandOperation<CourseRelation>(RootEntity, dependencies));
        }

    }
}