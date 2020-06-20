using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class OrganizationQueryAggregate : GetByIdQueryAggregate<Organization, int?, OrganizationOutputDto>
    {
        public GetAllLinkedAggregateQueryCollectionOperation<int?, Phone, PhoneOutputDto> GetAllPhonesLinkedAggregateQueryOperation { get; set; }

        public GetLinkedAggregateQuerySingleItemOperation<int?, Address, AddressOutputDto> GetAddressLinkedAggregateQueryOperation { get; set; }

        public OrganizationQueryAggregate() : this(null)
        {
        }

        public OrganizationQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            OrganizationQueryRepository.Register(context);

            PhoneQueryRepository.Register(context);

            AddressQueryRepository.Register(context);

            GetAllPhonesLinkedAggregateQueryOperation = new GetAllLinkedAggregateQueryCollectionOperation<int?, Phone, PhoneOutputDto>
            {
                GetAllLinkedEntities = (repository, entity, user) => ((PhoneQueryRepository)repository).GetAllPhonesForOrganization(RootEntity.Id).ToList(),
                GetAllLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PhoneQueryRepository)repository).GetAllPhonesForOrganizationAsync(RootEntity.Id);

                    return entities.ToList();
                },
                CreateLinkedQueryAggregate = entity =>
                {
                    if (entity is Phone)
                    {
                        return new GetPhoneQueryAggregate();
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetAllPhonesLinkedAggregateQueryOperation);

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
                        return new GetAddressQueryAggregate();
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

            OutputDto.Phones = GetAllPhonesLinkedAggregateQueryOperation.OutputDtos;

            OutputDto.Address = GetAddressLinkedAggregateQueryOperation.OutputDto;
        }

    }
}