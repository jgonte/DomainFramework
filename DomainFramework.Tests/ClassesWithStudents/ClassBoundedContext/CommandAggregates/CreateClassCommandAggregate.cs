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

        public CreateClassCommandAggregate(ClassInputDto @class, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
            Initialize(@class, dependencies);
        }

        public override void Initialize(IInputDataTransferObject @class, EntityDependency[] dependencies)
        {
            Initialize((ClassInputDto)@class, dependencies);
        }

        private void Initialize(ClassInputDto @class, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Class>(() => new ClassCommandRepository());

            RootEntity = new Class
            {
                Name = @class.Name
            };

            Enqueue(new InsertEntityCommandOperation<Class>(RootEntity, dependencies));
        }

    }
}