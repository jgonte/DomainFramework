using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class CustomerQueryRepository : EntityQueryRepository<Customer, int>
    {
        public override (int, IEnumerable<Customer>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Customer>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pCustomer_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .Execute();

            return (result.Count, result.Records);
        }

        public async override Task<(int, IEnumerable<Customer>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Customer>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pCustomer_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Count())
                .ExecuteAsync();

            return (result.Count, result.Records);
        }

        public override IEnumerable<Customer> GetAll()
        {
            var result = Query<Customer>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pCustomer_GetAll]")
                .Execute();

            return result.Records;
        }

        public async override Task<IEnumerable<Customer>> GetAllAsync()
        {
            var result = await Query<Customer>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pCustomer_GetAll]")
                .ExecuteAsync();

            return result.Records;
        }

        public override Customer GetById(int customerId)
        {
            var result = Query<Customer>
                .Single()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pCustomer_GetById]")
                .Parameters(
                    p => p.Name("customerId").Value(customerId)
                )
                .Execute();

            return result.Record;
        }

        public async override Task<Customer> GetByIdAsync(int customerId)
        {
            var result = await Query<Customer>
                .Single()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pCustomer_GetById]")
                .Parameters(
                    p => p.Name("customerId").Value(customerId)
                )
                .ExecuteAsync();

            return result.Record;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Customer>(new CustomerQueryRepository());
        }

    }
}