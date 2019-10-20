using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class PersonQueryRepository : EntityQueryRepository<Person, int?>
    {
        public override Person GetById(int? personId, IAuthenticatedUser user)
        {
            var result = Query<Person>
                .Single()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pPerson_GetById]")
                .Parameters(
                    p => p.Name("personId").Value(personId.Value)
                )
                .MapTypes(
                    6,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Executive)).Index(2),
                    tm => tm.Type(typeof(Customer)).Index(3),
                    tm => tm.Type(typeof(Person)).Index(4)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Person> GetByIdAsync(int? personId, IAuthenticatedUser user)
        {
            var result = await Query<Person>
                .Single()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pPerson_GetById]")
                .Parameters(
                    p => p.Name("personId").Value(personId.Value)
                )
                .MapTypes(
                    6,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Executive)).Index(2),
                    tm => tm.Type(typeof(Customer)).Index(3),
                    tm => tm.Type(typeof(Person)).Index(4)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public override IEnumerable<Person> Get(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Person>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pPerson_Get]")
                .QueryParameters(queryParameters)
                .MapTypes(
                    6,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Executive)).Index(2),
                    tm => tm.Type(typeof(Customer)).Index(3),
                    tm => tm.Type(typeof(Person)).Index(4)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Person>> GetAsync(CollectionQueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Person>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pPerson_Get]")
                .QueryParameters(queryParameters)
                .MapTypes(
                    6,
                    tm => tm.Type(typeof(Employee)).Index(1),
                    tm => tm.Type(typeof(Executive)).Index(2),
                    tm => tm.Type(typeof(Customer)).Index(3),
                    tm => tm.Type(typeof(Person)).Index(4)
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