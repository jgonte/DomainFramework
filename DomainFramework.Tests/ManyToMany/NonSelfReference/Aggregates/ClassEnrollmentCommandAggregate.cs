using DomainFramework.Core;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace DomainFramework.Tests
{
    class ClassEnrollmentCommandAggregate : CommandAggregate<ClassEntity>
    {
        public List<StudentEntity> StudentsToEnroll { get; internal set; } = new List<StudentEntity>();

        public List<ClassEnrollmentEntity> ClassEnrollments { get; internal set; } = new List<ClassEnrollmentEntity>();

        public ClassEnrollmentCommandAggregate(RepositoryContext context, ClassEnrollmentDto classEnrollment) : base(context)
        {
            RootEntity = new ClassEntity
            {
                Name = classEnrollment.Name
            };

            Enqueue(
                new SaveEntityCommandOperation<ClassEntity>(RootEntity)
            );

            if (classEnrollment.StudentsToEnroll?.Any() == true)
            {
                foreach (var studentToEnroll in classEnrollment.StudentsToEnroll)
                {
                    var studentEntity = new StudentEntity
                    {
                        FirstName = studentToEnroll.FirstName
                    };

                    StudentsToEnroll.Add(studentEntity);

                    var createStudentOperation = new InsertEntityCommandOperation<StudentEntity>(studentEntity);

                    Enqueue(createStudentOperation);

                    var classEnrollmentEntity = new ClassEnrollmentEntity
                    {
                        StartedDateTime = studentToEnroll.StartedDateTime
                    };

                    ClassEnrollments.Add(classEnrollmentEntity);

                    var createEnrollmentOperation = new InsertEntityCommandOperation<ClassEnrollmentEntity>(
                        classEnrollmentEntity,
                        new EntityDependency[]
                        {
                            new EntityDependency
                            {
                                Entity = RootEntity
                            },
                            new EntityDependency
                            {
                                Entity = studentEntity
                            }
                        });

                    Enqueue(createEnrollmentOperation);
                }

            }
        }

    }
}
