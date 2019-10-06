using DataAccess;
using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class CustomerQueryRepository : EntityQueryRepository<Customer, int?>
    {
        public override Customer GetById(int? customerId, IAuthenticatedUser user)
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

        public async override Task<Customer> GetByIdAsync(int? customerId, IAuthenticatedUser user)
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

        public override IEnumerable<Customer> Get(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = Query<Customer>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pCustomer_Get]")
                .Execute();

            return result.Data;
        }

        public async override Task<IEnumerable<Customer>> GetAsync(QueryParameters queryParameters, IAuthenticatedUser user)
        {
            var result = await Query<Customer>
                .Collection()
                .Connection(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName())
                .StoredProcedure("[ExecutiveBoundedContext].[pCustomer_Get]")
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<Customer>(new CustomerQueryRepository());
        }

    }
}