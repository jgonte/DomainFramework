using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class StudentQueryRepository : EntityQueryRepository<Student, int?>
    {
        public override Student GetById(int? studentId, IAuthenticatedUser user)
        {
            var result = Query<Student>
                .Single()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pStudent_GetById]")
                .Parameters(
                    p => p.Name("studentId").Value(studentId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Student> GetByIdAsync(int? studentId, IAuthenticatedUser user)
        {
            var result = await Query<Student>
                .Single()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pStudent_GetById]")
                .Parameters(
                    p => p.Name("studentId").Value(studentId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<Student> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Student>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pStudent_Get]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Student>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Student>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pStudent_Get]")
                .ExecuteAsync();

            return result.Data;
        }

        public IEnumerable<Student> GetStudentsForClassEnrollment(int? classId, IAuthenticatedUser user)
        {
            var result = Query<Student>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pStudent_GetStudents_ForClassEnrollment]")
                .Parameters(
                    p => p.Name("classId").Value(classId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Student>> GetStudentsForClassEnrollmentAsync(int? classId, IAuthenticatedUser user)
        {
            var result = await Query<Student>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pStudent_GetStudents_ForClassEnrollment]")
                .Parameters(
                    p => p.Name("classId").Value(classId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Student>(new StudentQueryRepository());
        }

    }
}