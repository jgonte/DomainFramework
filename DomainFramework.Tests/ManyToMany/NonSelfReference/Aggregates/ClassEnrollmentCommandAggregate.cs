using DomainFramework.Core;
using System.Collections.Generic;
using System.Linq;
using Utilities;

namespace DomainFramework.Tests
{
    class ClassEnrollmentCommandAggregate : CommandAggregate<ClassEntity>
    {
        private CollectionBinaryEntityLinkTransactedOperation<ClassEntity, StudentEntity, ClassEnrollmentEntity> _studentsToEnrollLink { get; set; }

        public IEnumerable<StudentEntity> StudentsToEnroll => _studentsToEnrollLink.LinkedEntities;

        public IEnumerable<ClassEnrollmentEntity> ClassEnrollments => _studentsToEnrollLink.BinaryEntities;

        public ClassEnrollmentCommandAggregate(RepositoryContext context, ClassEnrollmentDto classEnrollment) : base(context, null)
        {
            RootEntity = new ClassEntity
            {
                Name = classEnrollment.Name
            };

            TransactedOperations.Enqueue(
                new EntityCommandTransactedOperation<ClassEntity>(RootEntity, CommandOperations.Save)
            );

            _studentsToEnrollLink = new CollectionBinaryEntityLinkTransactedOperation<ClassEntity, StudentEntity, ClassEnrollmentEntity>(RootEntity);

            if (classEnrollment.StudentsToEnroll?.Any() == true)
            {
                foreach (var studentToEnroll in classEnrollment.StudentsToEnroll)
                {
                    var student = new StudentEntity
                    {
                        FirstName = studentToEnroll.FirstName
                    };

                    _studentsToEnrollLink.AddLinkedEntity(student);

                    var classEnrollmentEntity = new ClassEnrollmentEntity
                    {
                        StartedDateTime = studentToEnroll.StartedDateTime
                    };

                    _studentsToEnrollLink.AddBinaryEntity(classEnrollmentEntity);
                }

                TransactedOperations.Enqueue(_studentsToEnrollLink);
            }
        }
    }
}
