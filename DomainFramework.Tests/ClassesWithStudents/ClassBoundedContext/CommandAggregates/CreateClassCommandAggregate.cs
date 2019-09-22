using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class CreateClassCommandAggregate : CommandAggregate<Class>
    {
        public CreateClassCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
        }

        public CreateClassCommandAggregate(ClassInputDto @class) : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
            Initialize(@class);
        }

        public override void Initialize(IInputDataTransferObject @class)
        {
            Initialize((ClassInputDto)@class);
        }

        private void Initialize(ClassInputDto @class)
        {
            RegisterCommandRepositoryFactory<Class>(() => new ClassCommandRepository());

            RootEntity = new Class
            {
                Name = @class.Name
            };

            Enqueue(new InsertEntityCommandOperation<Class>(RootEntity));
        }

    }
}