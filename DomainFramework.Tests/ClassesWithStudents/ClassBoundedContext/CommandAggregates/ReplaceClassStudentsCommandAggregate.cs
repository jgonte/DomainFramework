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

        public ReplaceClassStudentsCommandAggregate(ReplaceClassStudentsInputDto enrollment, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
            Initialize(enrollment, dependencies);
        }

        public override void Initialize(IInputDataTransferObject enrollment, EntityDependency[] dependencies)
        {
            Initialize((ReplaceClassStudentsInputDto)enrollment, dependencies);
        }

        private void Initialize(ReplaceClassStudentsInputDto enrollment, EntityDependency[] dependencies)
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
                foreach (var student in enrollment.Students)
                {
                    var studentEntity = new Student
                    {
                        FirstName = student.FirstName
                    };

                    Enqueue(new InsertEntityCommandOperation<Student>(studentEntity));

                    var classEnrollmentEntity = new ClassEnrollment
                    {
                        Id = new ClassEnrollmentId
                        {
                            ClassId = enrollment.ClassId
                        },
                        StartedDateTime = student.StartedDateTime
                    };

                    Enqueue(new InsertEntityCommandOperation<ClassEnrollment>(classEnrollmentEntity, new EntityDependency[]
                    {
                        new EntityDependency
                        {
                            Entity = studentEntity
                        }
                    }, selector: "Classes"));
                }
            }
        }

    }
}