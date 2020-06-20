using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonWithDisciples.PersonBoundedContext
{
    public class GetPersonByIdQueryAggregate : GetByIdQueryAggregate<Person, int?, PersonOutputDto>
    {
        public GetAllLinkedAggregateQueryCollectionOperation<int?, Person, PersonOutputDto> GetAllDisciplesLinkedAggregateQueryOperation { get; set; }

        public GetPersonByIdQueryAggregate() : this(null)
        {
        }

        public GetPersonByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(PersonWithDisciplesConnectionClass.GetConnectionName()), processedEntities)
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
        }

        public override void PopulateDto()
        {
            OutputDto.PersonId = RootEntity.Id.Value;

            OutputDto.Name = RootEntity.Name;

            OutputDto.Gender = RootEntity.Gender;

            OutputDto.LeaderId = RootEntity.LeaderId;

            OutputDto.Disciples = GetAllDisciplesLinkedAggregateQueryOperation.OutputDtos;
        }

    }
}