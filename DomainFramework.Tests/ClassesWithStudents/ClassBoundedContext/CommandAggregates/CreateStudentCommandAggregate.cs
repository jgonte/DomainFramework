using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class CreateStudentCommandAggregate : CommandAggregate<Student>
    {
        public CreateStudentCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
        }

        public CreateStudentCommandAggregate(StudentInputDto student) : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
            Initialize(student);
        }

        public override void Initialize(IInputDataTransferObject student)
        {
            Initialize((StudentInputDto)student);
        }

        private void Initialize(StudentInputDto student)
        {
            RegisterCommandRepositoryFactory<Student>(() => new StudentCommandRepository());

            RootEntity = new Student
            {
                FirstName = student.FirstName
            };

            Enqueue(new InsertEntityCommandOperation<Student>(RootEntity));
        }

    }
}