using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace DomainFramework.Tests
{
    class ClassEnrollmentCommandAggregate : CommandAggregate<ClassEntity>
    {
        public CommandCollectionBinaryEntityLink<ClassEntity, StudentEntity, ClassEnrollmentEntity> StudentsToEnrollLink { get; set; } = new CommandCollectionBinaryEntityLink<ClassEntity, StudentEntity, ClassEnrollmentEntity>();

        public IEnumerable<StudentEntity> StudentsToEnroll => StudentsToEnrollLink.LinkedEntities;

        public ClassEnrollmentCommandAggregate(RepositoryContext context, ClassEntity entity) : base(context, entity)
        {
            // Create the links to the collection of entity links
            CollectionEntityLinks = new List<ICommandCollectionEntityLink<ClassEntity>>();

            // Register the link to the pages collection
            CollectionEntityLinks.Add(StudentsToEnrollLink);
        }

        // Suppose the class and the student do not exist by the time we enroll it so we need to create the class and student records in the database
        public void EnrollStudent(StudentEntity student, DateTime enrollmentDateTime)
        {
            StudentsToEnrollLink.AddEntity(student);

            StudentsToEnrollLink.AddJoinEntity(new ClassEnrollmentEntity { StartedDateTime = enrollmentDateTime });
        }
    }
}
