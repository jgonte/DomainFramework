using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class GetByIdEmployeeQueryAggregate : GetByIdQueryAggregate<Employee, int?, EmployeeOutputDto>
    {
        private GetByIdEmployeeQueryAggregate _getByIdEmployeeQueryAggregate;

        public GetCollectionLinkedEntityQueryOperation<Person> GetDependantsQueryOperation { get; }

        public IEnumerable<Person> Dependants => GetDependantsQueryOperation.LinkedEntities;

        public GetByIdEmployeeQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(EmployeeWithDependantsConnectionClass.GetConnectionName());

            EmployeeQueryRepository.Register(context);

            PersonQueryRepository.Register(context);

            RepositoryContext = context;

            GetDependantsQueryOperation = new GetCollectionLinkedEntityQueryOperation<Person>
            {
                GetLinkedEntities = (repository, entity, user) => ((PersonQueryRepository)repository).GetDependantsForEmployee(RootEntity.Id, user).ToList(),
                GetLinkedEntitiesAsync = async (repository, entity, user) =>
                {
                    var entities = await ((PersonQueryRepository)repository).GetDependantsForEmployeeAsync(RootEntity.Id, user);

                    return entities.ToList();
                }
            };

            QueryOperations.Enqueue(GetDependantsQueryOperation);
        }

        public List<PersonOutputDto> GetDependantsDtos()
        {
            if (Dependants?.Any() == true)
            {
                var dependants = new List<PersonOutputDto>();

                foreach (var person in Dependants)
                {
                    if (person is Employee)
                    {
                        var employee = (Employee)person;

                        var dto = new EmployeeOutputDto
                        {
                            Id = employee.Id.Value,
                            HireDate = employee.HireDate,
                            Name = employee.Name,
                            ProviderEmployeeId = employee.ProviderEmployeeId,
                            CellPhone = new PhoneNumberOutputDto
                            {
                                AreaCode = employee.CellPhone.AreaCode,
                                Exchange = employee.CellPhone.Exchange,
                                Number = employee.CellPhone.Number
                            }
                        };

                        if (_getByIdEmployeeQueryAggregate == null)
                        {
                            _getByIdEmployeeQueryAggregate = new GetByIdEmployeeQueryAggregate();
                        }

                        if (_getByIdEmployeeQueryAggregate.RootEntity == null)
                        {
                            _getByIdEmployeeQueryAggregate.RootEntity = employee;

                            _getByIdEmployeeQueryAggregate.LoadLinks();

                            dto.Dependants = _getByIdEmployeeQueryAggregate.GetDependantsDtos();

                            _getByIdEmployeeQueryAggregate.RootEntity = null;
                        }

                        dependants.Add(dto);
                    }
                    else
                    {
                        var dto = new PersonOutputDto
                        {
                            Id = person.Id.Value,
                            Name = person.Name,
                            ProviderEmployeeId = person.ProviderEmployeeId,
                            CellPhone = new PhoneNumberOutputDto
                            {
                                AreaCode = person.CellPhone.AreaCode,
                                Exchange = person.CellPhone.Exchange,
                                Number = person.CellPhone.Number
                            }
                        };

                        dependants.Add(dto);
                    }
                }

                return dependants;
            }

            return null;
        }

        public PhoneNumberOutputDto GetCellPhoneDto(Person person) => 
            new PhoneNumberOutputDto
            {
                AreaCode = person.CellPhone.AreaCode,
                Exchange = person.CellPhone.Exchange,
                Number = person.CellPhone.Number
            };

        public override void PopulateDto(Employee entity)
        {
            OutputDto.Id = entity.Id.Value;

            OutputDto.HireDate = entity.HireDate;

            OutputDto.Name = entity.Name;

            OutputDto.ProviderEmployeeId = entity.ProviderEmployeeId;

            OutputDto.CellPhone = GetCellPhoneDto(entity);

            OutputDto.Dependants = GetDependantsDtos();
        }

    }
}