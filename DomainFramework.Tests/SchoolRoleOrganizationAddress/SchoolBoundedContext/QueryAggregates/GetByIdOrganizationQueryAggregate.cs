using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class GetByIdOrganizationQueryAggregate : GetByIdQueryAggregate<Organization, int?, OrganizationOutputDto>
    {
        public GetLinkedAggregateQuerySingleItemOperation<int?, Address, AddressOutputDto> GetAddressLinkedAggregateQueryOperation { get; set; }

        public GetByIdOrganizationQueryAggregate() : this(null)
        {
        }

        public GetByIdOrganizationQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            OrganizationQueryRepository.Register(context);

            AddressQueryRepository.Register(context);

            GetAddressLinkedAggregateQueryOperation = new GetLinkedAggregateQuerySingleItemOperation<int?, Address, AddressOutputDto>
            {
                OnBeforeExecute = entity =>
                {
                    if (ProcessedEntities.Contains(("Address", entity)))
                    {
                        return false;
                    }

                    ProcessedEntities.Add(("Address", entity));

                    return true;
                },
                GetLinkedEntity = (repository, entity, user) => ((AddressQueryRepository)repository).GetAddressForOrganization(RootEntity.Id),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((AddressQueryRepository)repository).GetAddressForOrganizationAsync(RootEntity.Id),
                CreateLinkedQueryAggregate = entity =>
                {
                    if (entity is Address)
                    {
                        return new GetAddressByIdQueryAggregate();
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetAddressLinkedAggregateQueryOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.OrganizationId = RootEntity.Id.Value;

            OutputDto.Name = RootEntity.Name;

            OutputDto.AddressId = RootEntity.AddressId;

            OutputDto.Phone = GetPhoneDto();

            OutputDto.Address = GetAddressLinkedAggregateQueryOperation.OutputDto;
        }

        public PhoneOutputDto GetPhoneDto() => 
            new PhoneOutputDto
            {
                Number = RootEntity.Phone.Number
            };

    }
}