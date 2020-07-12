using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class GetExecutiveByIdQueryAggregate : GetByIdQueryAggregate<Executive, int, ExecutiveOutputDto>
    {
        public GetExecutiveByIdQueryAggregate() : this(null)
        {
        }

        public GetExecutiveByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            ExecutiveQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.PersonId = RootEntity.Id;

            OutputDto.Bonus = RootEntity.Bonus;

            OutputDto.HireDate = RootEntity.HireDate;

            OutputDto.Name = RootEntity.Name;

            OutputDto.Asset = GetAssetDto();
        }

        public AssetOutputDto GetAssetDto() => 
            new AssetOutputDto
            {
                Number = RootEntity.Asset.Number
            };

    }
}