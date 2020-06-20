using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class GetNotEnrolledStudentsQueryAggregate : QueryAggregateCollection<Student, StudentOutputDto, GetStudentByIdQueryAggregate>
    {
        public GetNotEnrolledStudentsQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            StudentQueryRepository.Register(context);
        }

        public (int, IEnumerable<StudentOutputDto>) Get(CollectionQueryParameters queryParameters)
        {
            var repository = (StudentQueryRepository)RepositoryContext.GetQueryRepository(typeof(Student));

            var (count, entities) = repository.GetNotEnrolled(queryParameters);

            var data = new Tuple<int, IEnumerable<IEntity>>(count, entities);

            return Get(data);
        }

        public async Task<(int, IEnumerable<StudentOutputDto>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var repository = (StudentQueryRepository)RepositoryContext.GetQueryRepository(typeof(Student));

            var (count, entities) = await repository.GetNotEnrolledAsync(queryParameters);

            var data = new Tuple<int, IEnumerable<IEntity>>(count, entities);

            return await GetAsync(data);
        }

    }
}