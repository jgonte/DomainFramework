using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class GetStudentByIdQueryAggregate : GetByIdQueryAggregate<Student, int?, StudentOutputDto>
    {
        public GetStudentByIdQueryAggregate() : this(null)
        {
        }

        public GetStudentByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(ClassesWithStudentsConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            StudentQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.StudentId = RootEntity.Id.Value;

            OutputDto.FirstName = RootEntity.FirstName;
        }

    }
}