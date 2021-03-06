using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class ClassQueryRepository : EntityQueryRepository<Class, int>
    {
        public override (int, IEnumerable<Class>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Class>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            var count = (int)result.GetParameter("count").Value;

            return (count, result.Records);
        }

        public async override Task<(int, IEnumerable<Class>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Class>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            var count = (int)result.GetParameter("count").Value;

            return (count, result.Records);
        }

        public override IEnumerable<Class> GetAll()
        {
            var result = Query<Class>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Class>> GetAllAsync()
        {
            var result = await Query<Class>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_GetAll]")
                .ExecuteAsync();

            return result.Records;
        }

        public override Class GetById(int classId)
        {
            var result = Query<Class>
                .Single()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_GetById]")
                .Parameters(
                    p => p.Name("classId").Value(classId)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<Class> GetByIdAsync(int classId)
        {
            var result = await Query<Class>
                .Single()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_GetById]")
                .Parameters(
                    p => p.Name("classId").Value(classId)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public IEnumerable<Class> GetAllClassesForStudent(int studentId)
        {
            var result = Query<Class>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pStudent_GetAllClasses]")
                .Parameters(
                    p => p.Name("studentId").Value(studentId)
                )
                .Execute();

            return result.Records;
        }

        public async Task<IEnumerable<Class>> GetAllClassesForStudentAsync(int studentId)
        {
            var result = await Query<Class>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pStudent_GetAllClasses]")
                .Parameters(
                    p => p.Name("studentId").Value(studentId)
                )
                .ExecuteAsync();

            return result.Records;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Class>(new ClassQueryRepository());
        }

    }
}