using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests
{
    class ClassEnrollmentQueryAggregate : GetByIdQueryAggregate<ClassEntity, Guid?, object>
    {
        public GetCollectionLinkedEntityLoadOperation<StudentEntity> GetStudentsLoadOperation { get; }

        public IEnumerable<StudentEntity> Students => GetStudentsLoadOperation.LinkedEntities;

        public ClassEnrollmentQueryAggregate(RepositoryContext context) : base(context)
        {
            GetStudentsLoadOperation = new GetCollectionLinkedEntityLoadOperation<StudentEntity>
            {
                GetLinkedEntities = (repository, entity, user) =>
                    ((StudentQueryRepository)repository).GetForClass(RootEntity.Id, user).ToList(),

                GetLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((StudentQueryRepository)repository).GetForClassAsync(RootEntity.Id, user: null);

                    return entities.ToList();
                }
            };

            LoadOperations.Enqueue(
                GetStudentsLoadOperation
            );
        }

        public override object GetDataTransferObject()
        {
            return null;
        }
    }
}