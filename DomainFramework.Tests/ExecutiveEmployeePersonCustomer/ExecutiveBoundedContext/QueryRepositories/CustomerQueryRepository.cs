using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class CustomerQueryRepository : EntityQueryRepository<Customer, int?>
    {
        public override (int, IEnumerable<Customer>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<Customer>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pCustomer_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<Customer>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<Customer>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pCustomer_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public IEnumerable<Customer> GetAll()
        {
            var result = Query<Customer>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pCustomer_GetAll]")
                .Execute();

            return result.Data;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var result = await Query<Customer>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pCustomer_GetAll]")
                .ExecuteAsync();

            return result.Data;
        }

        public override Customer GetById(int? customerId)
        {
            var result = Query<Customer>
                .Single()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pCustomer_GetById]")
                .Parameters(
                    p => p.Name("customerId").Value(customerId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<Customer> GetByIdAsync(int? customerId)
        {
            var result = await Query<Customer>
                .Single()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pCustomer_GetById]")
                .Parameters(
                    p => p.Name("customerId").Value(customerId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Customer>(new CustomerQueryRepository());
        }

    }
}