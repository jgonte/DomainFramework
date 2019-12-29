using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class GetByIdOrganizationQueryAggregate : GetByIdQueryAggregate<Organization, int?, OrganizationOutputDto>
    {
        public GetByIdOrganizationQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName());

            OrganizationQueryRepository.Register(context);

            AddressQueryRepository.Register(context);

            RepositoryContext = context;
        }

        public PhoneOutputDto GetPhoneDto(Organization organization) => 
            new PhoneOutputDto
            {
                Number = organization.Phone.Number
            };

        public override void PopulateDto(Organization entity)
        {
            OutputDto.Id = entity.Id.Value;

            OutputDto.Name = entity.Name;

            OutputDto.AddressId = entity.AddressId;

            OutputDto.Phone = GetPhoneDto(entity);

            //OutputDto.Address = GetAddressDto();
        }

    }
}