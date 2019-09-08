using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.SchoolRoleOrganizationAddress
{
    public class GetByIdSchoolQueryAggregate : GetByIdQueryAggregate<School, int?, SchoolOutputDto>
    {
        public GetByIdSchoolQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(TestConnectionClass.GetConnectionName());

            SchoolQueryRepository.Register(context);

            OrganizationQueryRepository.Register(context);

            RepositoryContext = context;

            var linkedGetByIdOrganizationQueryAggregateOperation = new AddLinkedAggregateQueryOperation<Organization, GetByIdOrganizationQueryAggregate, OrganizationOutputDto>
            {
                Query = (aggregate, user) =>
                {
                    var repository = RepositoryContext.GetQueryRepository(typeof(Organization));

                    var entity = ((OrganizationQueryRepository)repository).GetOrganizationForOrganizationRole(RootEntity.Id, user);

                    aggregate.RootEntity = entity;

                    aggregate.LoadLinks(user);

                    aggregate.PopulateDto(entity);

                    OutputDto.Organization = aggregate.OutputDto;
                },
                QueryAsync = async (aggregate, user) =>
                {
                    var repository = RepositoryContext.GetQueryRepository(typeof(Organization));

                    var entity = await ((OrganizationQueryRepository)repository).GetOrganizationForOrganizationRoleAsync(RootEntity.Id, user);

                    aggregate.RootEntity = entity;

                    await aggregate.LoadLinksAsync(user);

                    aggregate.PopulateDto(entity);

                    OutputDto.Organization = aggregate.OutputDto;
                }
            };

            QueryOperations.Enqueue(linkedGetByIdOrganizationQueryAggregateOperation);
        }

        public override void PopulateDto(School entity)
        {
            OutputDto.Id = entity.Id.Value;

            OutputDto.IsCharter = entity.IsCharter;
        }

    }
}
