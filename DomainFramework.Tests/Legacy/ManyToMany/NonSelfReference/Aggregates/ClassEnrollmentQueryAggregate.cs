using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DomainFramework.Tests
{
    class ClassEnrollmentOutputDto : IOutputDataTransferObject
    {

    }

    class ClassEnrollmentQueryAggregate : GetByIdQueryAggregate<ClassEntity, Guid?, ClassEnrollmentOutputDto>
    {
        public GetCollectionLinkedEntityQueryOperation<StudentEntity> GetStudentsLoadOperation { get; }

        public IEnumerable<StudentEntity> Students => GetStudentsLoadOperation.LinkedEntities;

        public ClassEnrollmentQueryAggregate(RepositoryContext context) : base(context)
        {
            GetStudentsLoadOperation = new GetCollectionLinkedEntityQueryOperation<StudentEntity>
            {
                GetLinkedEntities = (repository, entity, user) =>
                    ((StudentQueryRepository)repository).GetForClass(RootEntity.Id, user).ToList(),

                GetLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((StudentQueryRepository)repository).GetForClassAsync(RootEntity.Id, user: null);

                    return entities.ToList();
                }
            };

            QueryOperations.Enqueue(
                GetStudentsLoadOperation
            );
        }

        public override void PopulateDto(ClassEntity entity)
        {
        }
    }
}