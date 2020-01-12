using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolRoleOrganizationAddress.SchoolBoundedContext
{
    public class GetByIdSchoolQueryAggregate : GetByIdQueryAggregate<School, int?, SchoolOutputDto>
    {
        public GetByIdSchoolQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(SchoolRoleOrganizationAddressConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            SchoolQueryRepository.Register(context);

            OrganizationQueryRepository.Register(context);

            var linkedGetByIdOrganizationQueryAggregateOperation = new AddLinkedAggregateQueryOperation<Organization, GetByIdOrganizationQueryAggregate, OrganizationOutputDto>
            {
                Query = (aggregate, user) =>
                {
                    var repository = RepositoryContext.GetQueryRepository(typeof(Organization));

                    var entity = ((OrganizationQueryRepository)repository).GetById(RootEntity.Id);

                    aggregate.RootEntity = entity;

                    aggregate.LoadLinks(user);

                    aggregate.PopulateDto(entity);

                    OutputDto.Organization = aggregate.OutputDto;
                },
                QueryAsync = async (aggregate, user) =>
                {
                    var repository = RepositoryContext.GetQueryRepository(typeof(Organization));

                    var entity = await ((OrganizationQueryRepository)repository).GetByIdAsync(RootEntity.Id);

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