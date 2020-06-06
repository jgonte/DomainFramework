using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonWithDisciplesAndServants.PersonBoundedContext
{
    public class GetPersonByIdQueryAggregate : GetByIdQueryAggregate<Person, int?, PersonOutputDto>
    {
        public GetAllLinkedAggregateQueryCollectionOperation<int?, Person, PersonOutputDto> GetAllDisciplesLinkedAggregateQueryOperation { get; set; }

        public GetAllLinkedAggregateQueryCollectionOperation<int?, Person, PersonOutputDto> GetAllServantsLinkedAggregateQueryOperation { get; set; }

        public GetPersonByIdQueryAggregate() : this(null)
        {
        }

        public GetPersonByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(PersonWithDisciplesAndServantsConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            PersonQueryRepository.Register(context);

            GetAllDisciplesLinkedAggregateQueryOperation = new GetAllLinkedAggregateQueryCollectionOperation<int?, Person, PersonOutputDto>
            {
                GetAllLinkedEntities = (repository, entity, user) => ((PersonQueryRepository)repository).GetAllDisciplesForPerson(RootEntity.Id).ToList(),
                GetAllLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PersonQueryRepository)repository).GetAllDisciplesForPersonAsync(RootEntity.Id);

                    return entities.ToList();
                },
                CreateLinkedQueryAggregate = entity => 
                {
                    if (entity is Person)
                    {
                        return new GetPersonByIdQueryAggregate(new HashSet<(string, IEntity)>
                        {
                            ("Disciples", entity)
                        });
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetAllDisciplesLinkedAggregateQueryOperation);

            GetAllServantsLinkedAggregateQueryOperation = new GetAllLinkedAggregateQueryCollectionOperation<int?, Person, PersonOutputDto>
            {
                GetAllLinkedEntities = (repository, entity, user) => ((PersonQueryRepository)repository).GetAllServantsForPerson(RootEntity.Id).ToList(),
                GetAllLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PersonQueryRepository)repository).GetAllServantsForPersonAsync(RootEntity.Id);

                    return entities.ToList();
                },
                CreateLinkedQueryAggregate = entity => 
                {
                    if (entity is Person)
                    {
                        return new GetPersonByIdQueryAggregate(new HashSet<(string, IEntity)>
                        {
                            ("Servants", entity)
                        });
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetAllServantsLinkedAggregateQueryOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.Name = RootEntity.Name;

            OutputDto.Gender = RootEntity.Gender;

            OutputDto.LeaderId = RootEntity.LeaderId;

            OutputDto.MasterId = RootEntity.MasterId;

            OutputDto.Disciples = GetAllDisciplesLinkedAggregateQueryOperation.OutputDtos;

            OutputDto.Servants = GetAllServantsLinkedAggregateQueryOperation.OutputDtos;
        }

    }
}