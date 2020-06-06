using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class GetPhoneQueryAggregate : GetByIdQueryAggregate<Phone, int?, PhoneOutputDto>
    {
        public GetPhoneQueryAggregate() : this(null)
        {
        }

        public GetPhoneQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            PhoneQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.Number = RootEntity.Number;

            OutputDto.OrganizationId = RootEntity.OrganizationId;

            OutputDto.PersonId = RootEntity.PersonId;
        }

    }
}