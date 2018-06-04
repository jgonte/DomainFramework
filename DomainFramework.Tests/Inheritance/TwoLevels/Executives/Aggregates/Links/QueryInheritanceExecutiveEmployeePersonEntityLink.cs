using System.Threading.Tasks;
using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class QueryInheritanceExecutiveEmployeePersonEntityLink : QueryInheritanceEntityLink<int?, EmployeeEntity>
    {
        public override void PopulateEntity(IRepositoryContext repositoryContext, int? derivedEntityId)
        {
            var repository = (EmployeeQueryRepository)repositoryContext.GetQueryRepository(typeof(EmployeeEntity));

            LinkedEntity = (repository).GetById(derivedEntityId);
        }

        public override async Task PopulateEntityAsync(IRepositoryContext repositoryContext, int? derivedEntityId)
        {
            var repository = (EmployeeQueryRepository)repositoryContext.GetQueryRepository(typeof(EmployeeEntity));

            LinkedEntity = await (repository).GetByIdAsync(derivedEntityId);
        }
    }
}