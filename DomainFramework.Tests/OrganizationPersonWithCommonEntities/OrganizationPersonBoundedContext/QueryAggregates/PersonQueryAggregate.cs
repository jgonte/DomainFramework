using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationPersonWithCommonEntities.OrganizationPersonBoundedContext
{
    public class PersonQueryAggregate : GetByIdQueryAggregate<Person, int?, PersonOutputDto>
    {
        public GetAllLinkedAggregateQueryCollectionOperation<int?, Phone, PhoneOutputDto> GetAllPhonesLinkedAggregateQueryOperation { get; set; }

        public GetLinkedAggregateQuerySingleItemOperation<int?, Address, AddressOutputDto> GetAddressLinkedAggregateQueryOperation { get; set; }

        public PersonQueryAggregate() : this(null)
        {
        }

        public PersonQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(OrganizationPersonWithCommonEntitiesConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            PersonQueryRepository.Register(context);

            PhoneQueryRepository.Register(context);

            AddressQueryRepository.Register(context);

            GetAllPhonesLinkedAggregateQueryOperation = new GetAllLinkedAggregateQueryCollectionOperation<int?, Phone, PhoneOutputDto>
            {
                GetAllLinkedEntities = (repository, entity, user) => ((PhoneQueryRepository)repository).GetAllPhonesForPerson(RootEntity.Id).ToList(),
                GetAllLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PhoneQueryRepository)repository).GetAllPhonesForPersonAsync(RootEntity.Id);

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
                GetLinkedEntity = (repository, entity, user) => ((AddressQueryRepository)repository).GetAddressForPerson(RootEntity.Id),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((AddressQueryRepository)repository).GetAddressForPersonAsync(RootEntity.Id),
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
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.Name = RootEntity.Name;

            OutputDto.Phones = GetAllPhonesLinkedAggregateQueryOperation.OutputDtos;

            OutputDto.Address = GetAddressLinkedAggregateQueryOperation.OutputDto;
        }

    }
}