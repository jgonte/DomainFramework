using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class CreateClassEnrollmentCommandAggregate : CommandAggregate<ClassEnrollment>
    {
        public CreateClassEnrollmentCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
        }

        public CreateClassEnrollmentCommandAggregate(ClassEnrollmentInputDto enrollment) : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
            Initialize(enrollment);
        }

        public override void Initialize(IInputDataTransferObject enrollment)
        {
            Initialize((ClassEnrollmentInputDto)enrollment);
        }

        private void Initialize(ClassEnrollmentInputDto enrollment)
        {
            RegisterCommandRepositoryFactory<ClassEnrollment>(() => new ClassEnrollmentCommandRepository());

            RootEntity = new ClassEnrollment
            {
                Id = new ClassEnrollmentId
                {
                    ClassId = enrollment.ClassId,
                    StudentId = enrollment.StudentId
                },
                Name = enrollment.Name,
                StartedDateTime = enrollment.StartedDateTime
            };

            Enqueue(new InsertEntityCommandOperation<ClassEnrollment>(RootEntity));
        }

    }
}