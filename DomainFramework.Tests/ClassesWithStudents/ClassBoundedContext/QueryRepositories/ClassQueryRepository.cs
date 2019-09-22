using DataAccess;
using DomainFramework.Core;
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

        public override IEnumerable<Class> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Class>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_Get]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Class>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Class>
                .Collection()
                .Connection(ClassesWithStudentsConnectionClass.GetConnectionName())
                .StoredProcedure("[ClassBoundedContext].[pClass_Get]")
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Class>(new ClassQueryRepository());
        }

    }
}