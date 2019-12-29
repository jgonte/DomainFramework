using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class GetClassByIdQueryAggregate : GetByIdQueryAggregate<Class, int?, ClassOutputDto>
    {
        public GetCollectionLinkedEntityQueryOperation<Student> GetStudentsQueryOperation { get; }

        public IEnumerable<Student> Students => GetStudentsQueryOperation.LinkedEntities;

        public GetClassByIdQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName());

            ClassQueryRepository.Register(context);

            StudentQueryRepository.Register(context);

            RepositoryContext = context;

            GetStudentsQueryOperation = new GetCollectionLinkedEntityQueryOperation<Student>
            {
                GetLinkedEntities = (repository, entity, user) => ((StudentQueryRepository)repository).GetAllStudentsForClass(RootEntity.Id).ToList(),
                GetLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((StudentQueryRepository)repository).GetAllStudentsForClassAsync(RootEntity.Id);

                    return entities.ToList();
                }
            };

            QueryOperations.Enqueue(GetStudentsQueryOperation);
        }

        public List<StudentOutputDto> GetStudentsDtos()
        {
            return Students
                .Select(e => new StudentOutputDto
                {
                    Id = e.Id.Value,
                    FirstName = e.FirstName
                })
                .ToList();
        }

        public override void PopulateDto(Class entity)
        {
            OutputDto.Id = entity.Id.Value;

            OutputDto.Name = entity.Name;

            OutputDto.Students = GetStudentsDtos();
        }

    }
}