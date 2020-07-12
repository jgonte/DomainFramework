using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonWithSpouse.PersonBoundedContext
{
    public class GetPersonByIdQueryAggregate : GetByIdQueryAggregate<Person, int, PersonOutputDto>
    {
        public GetLinkedAggregateQuerySingleItemOperation<int, Person, PersonOutputDto> GetMarriedToLinkedAggregateQueryOperation { get; set; }

        public GetPersonByIdQueryAggregate() : this(null)
        {
        }

        public GetPersonByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(PersonWithSpouseConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            PersonQueryRepository.Register(context);

            GetMarriedToLinkedAggregateQueryOperation = new GetLinkedAggregateQuerySingleItemOperation<int, Person, PersonOutputDto>
            {
                OnBeforeExecute = entity =>
                {
                    if (ProcessedEntities.Contains(("MarriedTo", entity)))
                    {
                        return false;
                    }

                    ProcessedEntities.Add(("MarriedTo", entity));

                    return true;
                },
                GetLinkedEntity = (repository, entity, user) => ((PersonQueryRepository)repository).GetMarriedToForPerson(RootEntity.Id),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((PersonQueryRepository)repository).GetMarriedToForPersonAsync(RootEntity.Id),
                CreateLinkedQueryAggregate = entity =>
                {
                    if (entity is Person)
                    {
                        return new GetPersonByIdQueryAggregate(new HashSet<(string, IEntity)>
                        {
                            ("MarriedTo", entity)
                        });
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetMarriedToLinkedAggregateQueryOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.PersonId = RootEntity.Id;

            OutputDto.Name = RootEntity.Name;

            OutputDto.Gender = RootEntity.Gender;

            OutputDto.SpouseId = RootEntity.SpouseId;

            OutputDto.MarriedTo = GetMarriedToLinkedAggregateQueryOperation.OutputDto;
        }

    }
}