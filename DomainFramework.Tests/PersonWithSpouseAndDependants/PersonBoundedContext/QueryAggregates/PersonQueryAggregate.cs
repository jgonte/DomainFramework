using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonWithSpouseAndDependants.PersonBoundedContext
{
    public class PersonQueryAggregate : GetByIdQueryAggregate<Person, int?, PersonOutputDto>
    {
        public GetLinkedAggregateQuerySingleItemOperation<int?, Person, PersonOutputDto> GetSpouseLinkedAggregateQueryOperation { get; set; }

        public GetAllLinkedAggregateQueryCollectionOperation<int?, Person, PersonOutputDto> GetAllDependantsLinkedAggregateQueryOperation { get; set; }

        public PersonQueryAggregate() : this(null)
        {
        }

        public PersonQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(PersonWithSpouseAndDependantsConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            PersonQueryRepository.Register(context);

            GetSpouseLinkedAggregateQueryOperation = new GetLinkedAggregateQuerySingleItemOperation<int?, Person, PersonOutputDto>
            {
                OnBeforeExecute = entity =>
                {
                    if (ProcessedEntities.Contains(("Spouse", entity)))
                    {
                        return false;
                    }

                    ProcessedEntities.Add(("Spouse", entity));

                    return true;
                },
                GetLinkedEntity = (repository, entity, user) => ((PersonQueryRepository)repository).GetSpouseForPerson(RootEntity.Id),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((PersonQueryRepository)repository).GetSpouseForPersonAsync(RootEntity.Id),
                CreateLinkedQueryAggregate = entity => 
                {
                    if (entity is Person)
                    {
                        return new PersonQueryAggregate(new HashSet<(string, IEntity)>
                        {
                            ("Spouse", entity)
                        });
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetSpouseLinkedAggregateQueryOperation);

            GetAllDependantsLinkedAggregateQueryOperation = new GetAllLinkedAggregateQueryCollectionOperation<int?, Person, PersonOutputDto>
            {
                GetAllLinkedEntities = (repository, entity, user) => ((PersonQueryRepository)repository).GetAllDependantsForPerson(RootEntity.Id).ToList(),
                GetAllLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PersonQueryRepository)repository).GetAllDependantsForPersonAsync(RootEntity.Id);

                    return entities.ToList();
                },
                CreateLinkedQueryAggregate = entity => 
                {
                    if (entity is Person)
                    {
                        return new PersonQueryAggregate(new HashSet<(string, IEntity)>
                        {
                            ("Dependants", entity)
                        });
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetAllDependantsLinkedAggregateQueryOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.Name = RootEntity.Name;

            OutputDto.MarriedPersonId = RootEntity.MarriedPersonId;

            OutputDto.ProviderPersonId = RootEntity.ProviderPersonId;

            OutputDto.Spouse = GetSpouseLinkedAggregateQueryOperation.OutputDto;

            OutputDto.Dependants = GetAllDependantsLinkedAggregateQueryOperation.OutputDtos;
        }

    }
}