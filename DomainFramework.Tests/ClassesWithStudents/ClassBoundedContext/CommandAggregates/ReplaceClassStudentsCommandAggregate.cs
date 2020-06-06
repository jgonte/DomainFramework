using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class ReplaceClassStudentsCommandAggregate : CommandAggregate<Class>
    {
        public ReplaceClassStudentsCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
        }

        public ReplaceClassStudentsCommandAggregate(ReplaceClassStudentsInputDto enrollment, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
            Initialize(enrollment, dependencies);
        }

        public override void Initialize(IInputDataTransferObject enrollment, EntityDependency[] dependencies)
        {
            Initialize((ReplaceClassStudentsInputDto)enrollment, dependencies);
        }

        private void Initialize(ReplaceClassStudentsInputDto enrollment, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<Class>(() => new ClassCommandRepository());

            RootEntity = new Class
            {
                Id = enrollment.ClassId
            };

            Enqueue(new DeleteLinksCommandOperation<Class>(RootEntity, "UnlinkStudentsFromClass"));

            if (enrollment.Students?.Any() == true)
            {
                foreach (var dto in enrollment.Students)
                {
                    ILinkedAggregateCommandOperation operation;

                    if (dto is StudentInputDto)
                    {
                        operation = new AddLinkedAggregateCommandOperation<Class, CreateStudentCommandAggregate, StudentInputDto>(
                            RootEntity,
                            (StudentInputDto)dto
                        );

                        Enqueue(operation);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }

                    Enqueue(new AddLinkedAggregateCommandOperation<Class, CreateClassEnrollmentCommandAggregate, ClassEnrollmentInputDto>(
                        RootEntity,
                        dto.Enrollment,
                        new EntityDependency[]
                        {
                            new EntityDependency
                            {
                                Entity = RootEntity,
                                Selector = "Classes"
                            },
                            new EntityDependency
                            {
                                Entity = operation.CommandAggregate.RootEntity,
                                Selector = "Students"
                            }
                        }
                    ));
                }
            }
        }

    }
}