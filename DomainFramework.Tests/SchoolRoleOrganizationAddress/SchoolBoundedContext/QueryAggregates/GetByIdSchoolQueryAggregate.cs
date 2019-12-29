using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class GetByIdSchoolQueryAggregate : GetByIdQueryAggregate<School, int?, SchoolOutputDto>
    {
        public GetByIdSchoolQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName());

            SchoolQueryRepository.Register(context);

            OrganizationQueryRepository.Register(context);

            RepositoryContext = context;
        }

        public override void PopulateDto(School entity)
        {
            OutputDto.Id = entity.Id.Value;

            OutputDto.IsCharter = entity.IsCharter;
        }

    }
}