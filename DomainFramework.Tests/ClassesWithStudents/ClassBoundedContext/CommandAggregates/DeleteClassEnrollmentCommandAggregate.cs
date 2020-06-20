using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class DeleteClassEnrollmentCommandAggregate : CommandAggregate<ClassEnrollment>
    {
        public DeleteClassEnrollmentCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
        }

        public DeleteClassEnrollmentCommandAggregate(ClassEnrollmentInputDto enrollment, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
            Initialize(enrollment, dependencies);
        }

        public override void Initialize(IInputDataTransferObject enrollment, EntityDependency[] dependencies)
        {
            Initialize((ClassEnrollmentInputDto)enrollment, dependencies);
        }

        private void Initialize(ClassEnrollmentInputDto enrollment, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<ClassEnrollment>(() => new ClassEnrollmentCommandRepository());

            RootEntity = new ClassEnrollment
            {
                Id = new ClassEnrollmentId
                {
                    ClassId = enrollment.ClassId,
                    StudentId = enrollment.StudentId
                },
                StartedDateTime = enrollment.StartedDateTime
            };

            Enqueue(new DeleteEntityCommandOperation<ClassEnrollment>(RootEntity, dependencies));
        }

    }
}