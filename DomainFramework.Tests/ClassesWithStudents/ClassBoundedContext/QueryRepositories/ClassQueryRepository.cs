using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class ClassQueryRepository : EntityQueryRepository<Class, int?>
    {
        public override (int, IEnumerable<Class>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Class>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Class>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Class>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public IEnumerable<Class> GetAll()
        {
            var result = Query<Class>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_GetAll]")
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Class>> GetAllAsync()
        {
            var result = await Query<Class>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override Class GetById(int? classId)
        {
            var result = Query<Class>
                .Single()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_GetById]")
                .Parameters(
                    p => p.Name("classId").Value(classId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Class> GetByIdAsync(int? classId)
        {
            var result = await Query<Class>
                .Single()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_GetById]")
                .Parameters(
                    p => p.Name("classId").Value(classId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public IEnumerable<Class> GetAllClassesForStudent(int? studentId)
        {
            var result = Query<Class>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pGetAll_Classes_For_Student]")
                .Parameters(
                    p => p.Name("studentId").Value(studentId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Class>> GetAllClassesForStudentAsync(int? studentId)
        {
            var result = await Query<Class>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pGetAll_Classes_For_Student]")
                .Parameters(
                    p => p.Name("studentId").Value(studentId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Class>(new ClassQueryRepository());
        }

    }
}