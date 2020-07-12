using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonWithSpouseAndBestFriend.PersonBoundedContext
{
    public class PersonQueryAggregate : GetByIdQueryAggregate<Person, int, PersonOutputDto>
    {
        public GetLinkedAggregateQuerySingleItemOperation<int, Person, PersonOutputDto> GetMarriedToLinkedAggregateQueryOperation { get; set; }

        public GetLinkedAggregateQuerySingleItemOperation<int, Person, PersonOutputDto> GetBestFriendOfLinkedAggregateQueryOperation { get; set; }

        public PersonQueryAggregate() : this(null)
        {
        }

        public PersonQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(PersonWithSpouseAndBestFriendConnectionClass.GetConnectionName()), processedEntities)
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
                        return new PersonQueryAggregate(new HashSet<(string, IEntity)>
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

            GetBestFriendOfLinkedAggregateQueryOperation = new GetLinkedAggregateQuerySingleItemOperation<int, Person, PersonOutputDto>
            {
                OnBeforeExecute = entity =>
                {
                    if (ProcessedEntities.Contains(("BestFriendOf", entity)))
                    {
                        return false;
                    }

                    ProcessedEntities.Add(("BestFriendOf", entity));

                    return true;
                },
                GetLinkedEntity = (repository, entity, user) => ((PersonQueryRepository)repository).GetBestFriendOfForPerson(RootEntity.Id),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((PersonQueryRepository)repository).GetBestFriendOfForPersonAsync(RootEntity.Id),
                CreateLinkedQueryAggregate = entity =>
                {
                    if (entity is Person)
                    {
                        return new PersonQueryAggregate(new HashSet<(string, IEntity)>
                        {
                            ("BestFriendOf", entity)
                        });
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetBestFriendOfLinkedAggregateQueryOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.PersonId = RootEntity.Id;

            OutputDto.Name = RootEntity.Name;

            OutputDto.Gender = RootEntity.Gender;

            OutputDto.SpouseId = RootEntity.SpouseId;

            OutputDto.BestFriendId = RootEntity.BestFriendId;

            OutputDto.MarriedTo = GetMarriedToLinkedAggregateQueryOperation.OutputDto;

            OutputDto.BestFriendOf = GetBestFriendOfLinkedAggregateQueryOperation.OutputDto;
        }

    }
}