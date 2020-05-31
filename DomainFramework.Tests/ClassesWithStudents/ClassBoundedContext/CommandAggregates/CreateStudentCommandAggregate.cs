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

        public CreateStudentCommandAggregate(StudentInputDto student, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
            Initialize(student, dependencies);
        }

        public override void Initialize(IInputDataTransferObject student, EntityDependency[] dependencies)
        {
            Initialize((StudentInputDto)student, dependencies);
        }

        private void Initialize(StudentInputDto student, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Student>(() => new StudentCommandRepository());

            RootEntity = new Student
            {
                FirstName = student.FirstName
            };

            Enqueue(new InsertEntityCommandOperation<Student>(RootEntity, dependencies));
        }

    }
}