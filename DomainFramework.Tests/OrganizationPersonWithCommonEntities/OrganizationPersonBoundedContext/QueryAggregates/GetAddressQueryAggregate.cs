using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class GetAddressQueryAggregate : GetByIdQueryAggregate<Address, int?, AddressOutputDto>
    {
        public GetAddressQueryAggregate() : this(null)
        {
        }

        public GetAddressQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            AddressQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.Street = RootEntity.Street;

            OutputDto.OrganizationId = RootEntity.OrganizationId;

            OutputDto.PersonId = RootEntity.PersonId;
        }

    }
}