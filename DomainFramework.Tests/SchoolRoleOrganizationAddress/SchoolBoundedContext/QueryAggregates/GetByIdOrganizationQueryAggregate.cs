using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class GetByIdOrganizationQueryAggregate : GetByIdQueryAggregate<Organization, int?, OrganizationOutputDto>
    {
        public GetSingleLinkedEntityQueryOperation<Address> GetAddressQueryOperation { get; }

        public Address Address => GetAddressQueryOperation.LinkedEntity;

        public GetByIdOrganizationQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName());

            OrganizationQueryRepository.Register(context);

            AddressQueryRepository.Register(context);

            RepositoryContext = context;

            GetAddressQueryOperation = new GetSingleLinkedEntityQueryOperation<Address>
            {
                GetLinkedEntity = (repository, entity, user) => ((AddressQueryRepository)repository).GetAddressForOrganization(RootEntity.Id, user),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((AddressQueryRepository)repository).GetAddressForOrganizationAsync(RootEntity.Id, user)
            };

            QueryOperations.Enqueue(GetAddressQueryOperation);
        }

        public AddressOutputDto GetAddressDto()
        {
            if (Address != null)
            {
                var dto = new AddressOutputDto
                {
                    Id = Address.Id.Value,
                    Street = Address.Street
                };

                return dto;
            }

            return null;
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

            OutputDto.Address = GetAddressDto();
        }

    }
}