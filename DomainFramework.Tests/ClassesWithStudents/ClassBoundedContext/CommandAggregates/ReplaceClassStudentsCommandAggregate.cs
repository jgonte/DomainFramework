using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class ReplaceClassStudentsCommandAggregate : CommandAggregate<ClassEnrollment>
    {
        public ReplaceClassStudentsCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
        }

        public ReplaceClassStudentsCommandAggregate(ReplaceClassStudentsInputDto enrollment) : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
            Initialize(enrollment);
        }

        public override void Initialize(IInputDataTransferObject enrollment)
        {
            Initialize((ReplaceClassStudentsInputDto)enrollment);
        }

        private void Initialize(ReplaceClassStudentsInputDto enrollment)
        {
            RegisterCommandRepositoryFactory<ClassEnrollment>(() => new ClassEnrollmentCommandRepository());

            RegisterCommandRepositoryFactory<Student>(() => new StudentCommandRepository());

            RootEntity = new ClassEnrollment
            {
                Id = new ClassEnrollmentId
                {
                    ClassId = enrollment.ClassId,
                    StudentId = enrollment.StudentId
                }
            };

            Enqueue(new DeleteEntityCollectionCommandOperation<ClassEnrollment>(RootEntity, "Students"));

            if (enrollment.Students?.Any() == true)
            {
                foreach (var dto in enrollment.Students)
                {
                    Enqueue(new AddLinkedEntityCommandOperation<ClassEnrollment, Student>(RootEntity, () => new Student
                    {
                        FirstName = dto.FirstName
                    }, "Students"));
                }
            }
        }

    }
}