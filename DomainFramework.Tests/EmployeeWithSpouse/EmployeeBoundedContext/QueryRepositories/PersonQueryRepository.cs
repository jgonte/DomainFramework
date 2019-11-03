using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    public class PersonQueryRepository : EntityQueryRepository<Person, int?>
    {
        public override Person GetById(int? personId, IAuthenticatedUser user)
        {
            var result = Query<Person>
                .Single()
                .Connection(EmployeeWithSpouseConnectionClass.GetConnectionName())
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

        public async override Task<Person> GetByIdAsync(int? personId, IAuthenticatedUser user)
        {
            var result = await Query<Person>
                .Single()
                .Connection(EmployeeWithSpouseConnectionClass.GetConnectionName())
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

        public override (int, IEnumerable<Person>) Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Person>
                .Collection()
                .Connection(EmployeeWithSpouseConnectionClass.GetConnectionName())
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

        public async override Task<(int, IEnumerable<Person>)> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Person>
                .Collection()
                .Connection(EmployeeWithSpouseConnectionClass.GetConnectionName())
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

        public Person GetSpouseForPerson(int? marriedToPersonId, IAuthenticatedUser user)
        {
            var result = Query<Person>
                .Single()
                .Connection(EmployeeWithSpouseConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pPerson_GetSpouse_ForPerson]")
                .Parameters(
                    p => p.Name("marriedToPersonId").Value(marriedToPersonId.Value)
                )
                .MapTypes(
                    7,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Person)).Index(2)
                )
                .Execute();

            return result.Data;
        }

        public async Task<Person> GetSpouseForPersonAsync(int? marriedToPersonId, IAuthenticatedUser user)
        {
            var result = await Query<Person>
                .Single()
                .Connection(EmployeeWithSpouseConnectionClass.GetConnectionName())
                .StoredProcedure("[EmployeeBoundedContext].[pPerson_GetSpouse_ForPerson]")
                .Parameters(
                    p => p.Name("marriedToPersonId").Value(marriedToPersonId.Value)
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