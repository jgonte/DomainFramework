using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassesWithStudents.ClassBoundedContext
{
    public class ClassEnrollmentQueryRepository : EntityQueryRepository<ClassEnrollment, ClassEnrollmentId>
    {
        public override ClassEnrollment GetById(ClassEnrollmentId classEnrollmentId, IAuthenticatedUser user)
        {
            var result = Query<ClassEnrollment>
                .Single()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClassEnrollment_GetById]")
                .Parameters(
                    p => p.Name("classId").Value(classEnrollmentId.ClassId.Value),
                    p => p.Name("studentId").Value(classEnrollmentId.StudentId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<ClassEnrollment> GetByIdAsync(ClassEnrollmentId classEnrollmentId, IAuthenticatedUser user)
        {
            var result = await Query<ClassEnrollment>
                .Single()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClassEnrollment_GetById]")
                .Parameters(
                    p => p.Name("classId").Value(classEnrollmentId.ClassId.Value),
                    p => p.Name("studentId").Value(classEnrollmentId.StudentId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<ClassEnrollment> Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<ClassEnrollment>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClassEnrollment_Get]")
                .QueryParameters(queryParameters)
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<ClassEnrollment>> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<ClassEnrollment>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClassEnrollment_Get]")
                .QueryParameters(queryParameters)
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<ClassEnrollment>(new ClassEnrollmentQueryRepository());
        }

    }
}