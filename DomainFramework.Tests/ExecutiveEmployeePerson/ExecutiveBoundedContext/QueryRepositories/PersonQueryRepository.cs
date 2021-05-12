using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace ExecutiveEmployeePerson.ExecutiveBoundedContext
{
    public class PersonQueryRepository : EntityQueryRepository<Person, int>
    {
        public override (int, IEnumerable<Person>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Person>
                .Collection()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pPerson_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .MapTypes(
                    4,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Executive)).Index(2),
                    tm => tm.Type(typeof(Person)).Index(3)
                )
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Person>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Person>
                .Collection()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pPerson_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .MapTypes(
                    4,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Executive)).Index(2),
                    tm => tm.Type(typeof(Person)).Index(3)
                )
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Person> GetAll()
        {
            var result = Query<Person>
                .Collection()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pPerson_GetAll]")
                .MapTypes(
                    4,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Executive)).Index(2),
                    tm => tm.Type(typeof(Person)).Index(3)
                )
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Person>> GetAllAsync()
        {
            var result = await Query<Person>
                .Collection()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pPerson_GetAll]")
                .MapTypes(
                    4,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Executive)).Index(2),
                    tm => tm.Type(typeof(Person)).Index(3)
                )
                .ExecuteAsync();

            return result.Records;
        }

        public override Person GetById(int personId)
        {
            var result = Query<Person>
                .Single()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pPerson_GetById]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .MapTypes(
                    4,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Executive)).Index(2),
                    tm => tm.Type(typeof(Person)).Index(3)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<Person> GetByIdAsync(int personId)
        {
            var result = await Query<Person>
                .Single()
                .Connection(ExecutiveEmployeePersonConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pPerson_GetById]")
                .Parameters(
                    p => p.Name("personId").Value(personId)
                )
                .MapTypes(
                    4,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Executive)).Index(2),
                    tm => tm.Type(typeof(Person)).Index(3)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Person>(new PersonQueryRepository());
        }

    }
}