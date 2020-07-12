using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    public class EmployeeQueryAggregate : GetByIdQueryAggregate<Employee, int, EmployeeOutputDto>
    {
        public GetLinkedAggregateQuerySingleItemOperation<int, Person, PersonOutputDto> GetSpouseLinkedAggregateQueryOperation { get; set; }

        public EmployeeQueryAggregate() : this(null)
        {
        }

        public EmployeeQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(EmployeeWithSpouseConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            EmployeeQueryRepository.Register(context);

            PersonQueryRepository.Register(context);

            GetSpouseLinkedAggregateQueryOperation = new GetLinkedAggregateQuerySingleItemOperation<int, Person, PersonOutputDto>
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
                    if (entity is Employee)
                    {
                        return new EmployeeQueryAggregate(new HashSet<(string, IEntity)>
                        {
                            ("Spouse", entity)
                        });
                    }
                    else if (entity is Person)
                    {
                        return new PersonQueryAggregate();
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            };

            QueryOperations.Enqueue(GetSpouseLinkedAggregateQueryOperation);
        }

        public override void PopulateDto()
        {
            OutputDto.PersonId = RootEntity.Id;

            OutputDto.HireDate = RootEntity.HireDate;

            OutputDto.Name = RootEntity.Name;

            OutputDto.MarriedToPersonId = RootEntity.MarriedToPersonId;

            OutputDto.CellPhone = GetCellPhoneDto();

            OutputDto.Spouse = GetSpouseLinkedAggregateQueryOperation.OutputDto;
        }

        public PhoneNumberOutputDto GetCellPhoneDto() => 
            (RootEntity.CellPhone.IsEmpty()) ? null : new PhoneNumberOutputDto
            {
                AreaCode = RootEntity.CellPhone.AreaCode,
                Exchange = RootEntity.CellPhone.Exchange,
                Number = RootEntity.CellPhone.Number
            };

    }
}