using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class GetByIdOrganizationQueryAggregate : GetByIdQueryAggregate<Organization, int?, OrganizationOutputDto>
    {
        public GetSingleLinkedEntityQueryOperation<Address> GetAddressQueryOperation { get; }

        public Address Address => GetAddressQueryOperation.LinkedEntity;

        public GetByIdOrganizationQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(TestConnectionClass.GetConnectionName());

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

        public PhoneOutputDto GetPhoneDto() =>
            new PhoneOutputDto
            {
                Number = RootEntity.Phone.Number
            };

        public override void PopulateDto(Organization entity)
        {
            OutputDto.Id = entity.Id.Value;

            OutputDto.Name = entity.Name;

            OutputDto.AddressId = entity.AddressId;

            OutputDto.Phone = GetPhoneDto();

            OutputDto.Address = GetAddressDto();
        }

    }
}
