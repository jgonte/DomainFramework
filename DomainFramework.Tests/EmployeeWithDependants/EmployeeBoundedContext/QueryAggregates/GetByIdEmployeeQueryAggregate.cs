using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class GetByIdEmployeeQueryAggregate : GetByIdQueryAggregate<Employee, int?, EmployeeOutputDto>
    {
        public GetAllLinkedAggregateQueryCollectionOperation<int?, Person, PersonOutputDto> GetAllDependantsLinkedAggregateQueryOperation { get; set; }

        public GetByIdEmployeeQueryAggregate() : this(null)
        {
        }

        public GetByIdEmployeeQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(EmployeeWithDependantsConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            EmployeeQueryRepository.Register(context);

            PersonQueryRepository.Register(context);

            GetAllDependantsLinkedAggregateQueryOperation = new GetAllLinkedAggregateQueryCollectionOperation<int?, Person, PersonOutputDto>
            {
                GetAllLinkedEntities = (repository, entity, user) => ((PersonQueryRepository)repository).GetAllDependantsForEmployee(RootEntity.Id).ToList(),
                GetAllLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PersonQueryRepository)repository).GetAllDependantsForEmployeeAsync(RootEntity.Id);

                    return entities.ToList();
                },
                CreateLinkedQueryAggregate = entity => 
                {
                    if (entity is Employee)
                    {
                        return new GetByIdEmployeeQueryAggregate(new HashSet<(string, IEntity)>
                        {
                            ("Dependants", entity)
                        });
                    }
                    else if (entity is Person)
                    {
                        return new GetByIdPersonQueryAggregate();
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

            OutputDto.HireDate = RootEntity.HireDate;

            OutputDto.Name = RootEntity.Name;

            OutputDto.ProviderEmployeeId = RootEntity.ProviderEmployeeId;

            OutputDto.CellPhone = GetCellPhoneDto();

            OutputDto.Dependants = GetAllDependantsLinkedAggregateQueryOperation.OutputDtos;
        }

        public PhoneNumberOutputDto GetCellPhoneDto() => 
            new PhoneNumberOutputDto
            {
                AreaCode = RootEntity.CellPhone.AreaCode,
                Exchange = RootEntity.CellPhone.Exchange,
                Number = RootEntity.CellPhone.Number
            };

    }
}