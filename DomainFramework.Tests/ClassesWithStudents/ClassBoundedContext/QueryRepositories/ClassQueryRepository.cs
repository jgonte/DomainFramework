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
        public override Class GetById(int? classId, IAuthenticatedUser user)
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

        public async override Task<Class> GetByIdAsync(int? classId, IAuthenticatedUser user)
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

        public override (int, IEnumerable<Class>) Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
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

        public async override Task<(int, IEnumerable<Class>)> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
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

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Class>(new ClassQueryRepository());
        }

    }
}