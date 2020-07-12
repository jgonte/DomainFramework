using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class GetAddressByIdQueryAggregate : GetByIdQueryAggregate<Address, int, AddressOutputDto>
    {
        public GetAddressByIdQueryAggregate() : this(null)
        {
        }

        public GetAddressByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            AddressQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.AddressId = RootEntity.Id;

            OutputDto.Street = RootEntity.Street;
        }

    }
}