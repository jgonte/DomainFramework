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
    public class StudentQueryRepository : EntityQueryRepository<Student, int>
    {
        public override (int, IEnumerable<Student>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Student>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pStudent_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Student>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Student>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pStudent_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Student> GetAll()
        {
            var result = Query<Student>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pStudent_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Student>> GetAllAsync()
        {
            var result = await Query<Student>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pStudent_GetAll]")
                .ExecuteAsync();

            return result.Records;
        }

        public override Student GetById(int studentId)
        {
            var result = Query<Student>
                .Single()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pStudent_GetById]")
                .Parameters(
                    p => p.Name("studentId").Value(studentId)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<Student> GetByIdAsync(int studentId)
        {
            var result = await Query<Student>
                .Single()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pStudent_GetById]")
                .Parameters(
                    p => p.Name("studentId").Value(studentId)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public IEnumerable<Student> GetAllStudentsForClass(int classId)
        {
            var result = Query<Student>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_GetAllStudents]")
                .Parameters(
                    p => p.Name("classId").Value(classId)
                )
                .Execute();

            return result.Records;
        }

        public async Task<IEnumerable<Student>> GetAllStudentsForClassAsync(int classId)
        {
            var result = await Query<Student>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_GetAllStudents]")
                .Parameters(
                    p => p.Name("classId").Value(classId)
                )
                .ExecuteAsync();

            return result.Records;
        }

        public (int, IEnumerable<Student>) GetEnrolled(int classId, CollectionQueryParameters queryParameters)
        {
            var result = Query<Student>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[Student_GetEnrolled]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Parameters(
                    p => p.Name("classId").Value(classId)
                )
                .Execute();

            return (result.Count, result.Records);
        }

        public async Task<(int, IEnumerable<Student>)> GetEnrolledAsync(int classId, CollectionQueryParameters queryParameters)
        {
            var result = await Query<Student>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[Student_GetEnrolled]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Parameters(
                    p => p.Name("classId").Value(classId)
                )
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public (int, IEnumerable<Student>) GetNotEnrolled(CollectionQueryParameters queryParameters)
        {
            var result = Query<Student>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[Student_GetNotEnrolled]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async Task<(int, IEnumerable<Student>)> GetNotEnrolledAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Student>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[Student_GetNotEnrolled]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Student>(new StudentQueryRepository());
        }

    }
}