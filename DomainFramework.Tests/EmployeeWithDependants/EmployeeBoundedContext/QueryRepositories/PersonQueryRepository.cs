using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class PersonQueryRepository : EntityQueryRepository<Person, int?>
    {
        public override (int, IEnumerable<Person>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Person>
                .Collection()
                .Connection(EmployeeWithDependantsConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pPerson_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .MapTypes(
                    7,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Person)).Index(2)
                )
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Person>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Person>
                .Collection()
                .Connection(EmployeeWithDependantsConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pPerson_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .MapTypes(
                    7,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Person)).Index(2)
                )
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public IEnumerable<Person> GetAll()
        {
            var result = Query<Person>
                .Collection()
                .Connection(EmployeeWithDependantsConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pPerson_GetAll]")
                .MapTypes(
                    7,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Person)).Index(2)
                )
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            var result = await Query<Person>
                .Collection()
                .Connection(EmployeeWithDependantsConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pPerson_GetAll]")
                .MapTypes(
                    7,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Person)).Index(2)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override Person GetById(int? personId)
        {
            var result = Query<Person>
                .Single()
                .Connection(EmployeeWithDependantsConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pPerson_GetById]")
                .Parameters(
                    p => p.Name("personId").Value(personId.Value)
                )
                .MapTypes(
                    7,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Person)).Index(2)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Person> GetByIdAsync(int? personId)
        {
            var result = await Query<Person>
                .Single()
                .Connection(EmployeeWithDependantsConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pPerson_GetById]")
                .Parameters(
                    p => p.Name("personId").Value(personId.Value)
                )
                .MapTypes(
                    7,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Person)).Index(2)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Person>(new PersonQueryRepository());
        }

    }
}