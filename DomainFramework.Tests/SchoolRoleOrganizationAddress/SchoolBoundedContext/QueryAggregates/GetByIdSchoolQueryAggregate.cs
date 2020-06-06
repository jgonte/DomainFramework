using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class GetByIdSchoolQueryAggregate : GetByIdQueryAggregate<School, int?, SchoolOutputDto>
    {
        public GetLinkedAggregateQuerySingleItemOperation<int?, Organization, OrganizationOutputDto> GetOrganizationLinkedAggregateQueryOperation { get; set; }

        public GetByIdSchoolQueryAggregate() : this(null)
        {
        }

        public GetByIdSchoolQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            SchoolQueryRepository.Register(context);

            OrganizationQueryRepository.Register(context);

            GetOrganizationLinkedAggregateQueryOperation = new GetLinkedAggregateQuerySingleItemOperation<int?, Organization, OrganizationOutputDto>
            {
                OnBeforeExecute = entity =>
                {
                    if (ProcessedEntities.Contains(("Organization", entity)))
                    {
                        return false;
                    }

                    ProcessedEntities.Add(("Organization", entity));

                    return true;
                },
                GetLinkedEntity = (repository, entity, user) => ((OrganizationQueryRepository)repository).GetOrganizationForRole(RootEntity.Id),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((OrganizationQueryRepository)repository).GetOrganizationForRoleAsync(RootEntity.Id),
                CreateLinkedQueryAggregate = entity => 
                {
                    if (entity is School)
                    {
                        return new GetByIdSchoolQueryAggregate(new HashSet<(string, IEntity)>
                        {
                            ("Organization", entity)
                        });
                    }
                    else if (entity is Organization)
                    {
                        return new GetByIdOrganizationQueryAggregate();
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetOrganizationLinkedAggregateQueryOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.IsCharter = RootEntity.IsCharter;

            OutputDto.Organization = GetOrganizationLinkedAggregateQueryOperation.OutputDto;
        }

    }
}