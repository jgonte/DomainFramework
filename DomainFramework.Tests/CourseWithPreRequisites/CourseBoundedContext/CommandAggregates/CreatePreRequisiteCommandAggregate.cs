using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseWithPreRequisites.CourseBoundedContext
{
    public class CreatePreRequisiteCommandAggregate : CommandAggregate<PreRequisite>
    {
        public CreatePreRequisiteCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(CourseWithPreRequisitesConnectionClass.GetConnectionName()))
        {
        }

        public CreatePreRequisiteCommandAggregate(PreRequisiteInputDto preRequisite, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(CourseWithPreRequisitesConnectionClass.GetConnectionName()))
        {
            Initialize(preRequisite, dependencies);
        }

        public override void Initialize(IInputDataTransferObject preRequisite, EntityDependency[] dependencies)
        {
            Initialize((PreRequisiteInputDto)preRequisite, dependencies);
        }

        private void Initialize(PreRequisiteInputDto preRequisite, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<PreRequisite>(() => new PreRequisiteCommandRepository());

            RootEntity = new PreRequisite
            {
                Id = new PreRequisiteId
                {
                    RequiredCourseId = preRequisite.RequiredCourseId,
                    CourseId = preRequisite.CourseId
                }
            };

            Enqueue(new InsertEntityCommandOperation<PreRequisite>(RootEntity, dependencies));
        }

    }
}