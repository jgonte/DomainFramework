using DomainFramework.Core;
using System;

namespace DomainFramework.Tests
{
    internal class AddStudentCommandAggregate : CommandAggregate<ClassEnrollmentEntity>
    {
        public AddStudentCommandAggregate(RepositoryContext context, Guid classId, Guid studentId) : base(context)
        {
            RootEntity = new ClassEnrollmentEntity
            {
                Id = new ClassEnrollmentEntityId
                {
                    ClassId = classId,
                    StudentId = studentId
                },
                StartedDateTime = DateTime.Now
            };

            Enqueue(
                new InsertEntityCommandOperation<ClassEnrollmentEntity>(RootEntity)
            );
        }
    }
}