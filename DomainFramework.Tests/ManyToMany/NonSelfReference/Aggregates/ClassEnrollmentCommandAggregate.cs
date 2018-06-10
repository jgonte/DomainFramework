using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class ClassEnrollmentCommandAggregate : CommandAggregate<ClassEntity>
    {
        private CollectionBinaryEntityLinkTransactedOperation<ClassEntity, StudentEntity, ClassEnrollmentEntity> _studentsToEnrollLink { get; set; }

        public IEnumerable<StudentEntity> StudentsToEnroll => _studentsToEnrollLink.LinkedEntities;

        public ClassEnrollmentCommandAggregate(RepositoryContext context, ClassEntity entity) : base(context, entity)
        {
            TransactedSaveOperations.Enqueue(
                new SaveEntityTransactedOperation<ClassEntity>(entity)
            );

            _studentsToEnrollLink = new CollectionBinaryEntityLinkTransactedOperation<ClassEntity, StudentEntity, ClassEnrollmentEntity>(entity);

            TransactedSaveOperations.Enqueue(_studentsToEnrollLink);
        }

        // Suppose the class and the student do not exist by the time we enroll it so we need to create the class and student records in the database
        public void EnrollStudent(StudentEntity student, DateTime enrollmentDateTime)
        {
            _studentsToEnrollLink.AddLinkedEntity(student);

            _studentsToEnrollLink.AddBinaryEntity(new ClassEnrollmentEntity { StartedDateTime = enrollmentDateTime });
        }
    }
}
