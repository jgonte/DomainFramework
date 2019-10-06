using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    public class PersonQueryAggregate : GetByIdQueryAggregate<Person, int?, PersonOutputDto>
    {
        private EmployeeQueryAggregate _employeeQueryAggregate;

        public GetSingleLinkedEntityQueryOperation<Person> GetSpouseQueryOperation { get; }

        public Person Spouse => GetSpouseQueryOperation.LinkedEntity;

        public PersonQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(EmployeeWithSpouseConnectionClass.GetConnectionName());

            PersonQueryRepository.Register(context);

            RepositoryContext = context;

            GetSpouseQueryOperation = new GetSingleLinkedEntityQueryOperation<Person>
            {
                GetLinkedEntity = (repository, entity, user) => ((PersonQueryRepository)repository).GetSpouseForPerson(RootEntity.Id, user),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((PersonQueryRepository)repository).GetSpouseForPersonAsync(RootEntity.Id, user)
            };

            QueryOperations.Enqueue(GetSpouseQueryOperation);
        }

        public PersonOutputDto GetSpouseDto()
        {
            if (Spouse != null)
            {
                if (Spouse is Employee)
                {
                    var employee = (Employee)Spouse;

                    var dto = new EmployeeOutputDto
                    {
                        Id = employee.Id.Value,
                        HireDate = employee.HireDate,
                        Name = employee.Name,
                        MarriedToPersonId = employee.MarriedToPersonId,
                        CellPhone = (employee.CellPhone.IsEmpty()) ? null : new PhoneNumberOutputDto
                        {
                            AreaCode = employee.CellPhone.AreaCode,
                            Exchange = employee.CellPhone.Exchange,
                            Number = employee.CellPhone.Number
                        }
                    };

                    if (_employeeQueryAggregate == null)
                    {
                        _employeeQueryAggregate = new EmployeeQueryAggregate();
                    }

                    if (_employeeQueryAggregate.RootEntity == null)
                    {
                        _employeeQueryAggregate.RootEntity = employee;

                        _employeeQueryAggregate.LoadLinks();

                        dto.Spouse = _employeeQueryAggregate.GetSpouseDto();

                        _employeeQueryAggregate.RootEntity = null;
                    }

                    return dto;
                }
                else
                {
                    var dto = new PersonOutputDto
                    {
                        Id = Spouse.Id.Value,
                        Name = Spouse.Name,
                        MarriedToPersonId = Spouse.MarriedToPersonId,
                        CellPhone = (Spouse.CellPhone.IsEmpty()) ? null : new PhoneNumberOutputDto
                        {
                            AreaCode = Spouse.CellPhone.AreaCode,
                            Exchange = Spouse.CellPhone.Exchange,
                            Number = Spouse.CellPhone.Number
                        }
                    };

                    return dto;
                }
            }

            return null;
        }

        public PhoneNumberOutputDto GetCellPhoneDto(Person person) => 
            (person.CellPhone.IsEmpty()) ? null : new PhoneNumberOutputDto
            {
                AreaCode = person.CellPhone.AreaCode,
                Exchange = person.CellPhone.Exchange,
                Number = person.CellPhone.Number
            };

        public override void PopulateDto(Person entity)
        {
            if (entity is Employee)
            {
                var employee = (Employee)entity;

                var employeeDto = new EmployeeOutputDto();

                employeeDto.Id = employee.Id.Value;

                employeeDto.HireDate = employee.HireDate;

                employeeDto.Name = employee.Name;

                employeeDto.MarriedToPersonId = employee.MarriedToPersonId;

                employeeDto.CellPhone = GetCellPhoneDto(employee);

                OutputDto = employeeDto;
            }
            else
            {
                var personDto = new PersonOutputDto();

                personDto.Id = entity.Id.Value;

                personDto.Name = entity.Name;

                personDto.MarriedToPersonId = entity.MarriedToPersonId;

                personDto.CellPhone = GetCellPhoneDto(entity);

                OutputDto = personDto;
            }

            OutputDto.Spouse = GetSpouseDto();
        }

    }
}