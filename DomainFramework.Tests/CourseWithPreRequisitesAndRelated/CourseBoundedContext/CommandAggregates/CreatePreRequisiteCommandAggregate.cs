using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CourseWithPreRequisitesAndRelated.CourseBoundedContext
{
    public class CreatePreRequisiteCommandAggregate : CommandAggregate<PreRequisite>
    {
        public CreatePreRequisiteCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName()))
        {
        }

        public CreatePreRequisiteCommandAggregate(PreRequisiteInputDto preRequisite, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(CourseWithPreRequisitesAndRelatedConnectionClass.GetConnectionName()))
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
                    Requires_CourseId = preRequisite.Requires_CourseId,
                    IsRequiredBy_CourseId = preRequisite.IsRequiredBy_CourseId
                }
            };

            Enqueue(new InsertEntityCommandOperation<PreRequisite>(RootEntity, dependencies));
        }

    }
}