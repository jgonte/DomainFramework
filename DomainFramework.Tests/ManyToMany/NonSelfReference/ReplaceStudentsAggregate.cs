using DomainFramework.Core;
using System;

namespace DomainFramework.Tests
{
    internal class ReplaceStudentsAggregate : CommandAggregate<ClassEnrollmentEntity>
    {
        public ReplaceStudentsAggregate(RepositoryContext context, Guid classId, Guid[] studentsId) : base(context)
        {
            RootEntity = new ClassEnrollmentEntity
            {
                Id = new ClassEnrollmentEntityId
                {
                    ClassId = classId
                }
            };

            var removeBinaryEntities = new StudentDeleteClassEnrollmentsCommandOperation(RootEntity);

            Enqueue(removeBinaryEntities);

            foreach (var studentId in studentsId)
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

                var insertBinaryEntity = new InsertEntityCommandOperation<ClassEnrollmentEntity>(RootEntity);

                Enqueue(insertBinaryEntity);
            }
            
        }
    }
}