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

        public override void Initialize(IInputDataTransferObject inputDto)
        {
            throw new NotImplementedException();
        }
    }
}