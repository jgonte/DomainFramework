using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    public class EmployeeQueryAggregate : GetByIdQueryAggregate<Employee, int?, EmployeeOutputDto>
    {
        private EmployeeQueryAggregate _employeeQueryAggregate;

        public GetSingleLinkedEntityQueryOperation<Person> GetSpouseQueryOperation { get; }

        public Person Spouse => GetSpouseQueryOperation.LinkedEntity;

        public EmployeeQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(EmployeeWithSpouseConnectionClass.GetConnectionName());

            EmployeeQueryRepository.Register(context);

            PersonQueryRepository.Register(context);

            RepositoryContext = context;

            GetSpouseQueryOperation = new GetSingleLinkedEntityQueryOperation<Person>
            {
                GetLinkedEntity = (repository, entity, user) => ((PersonQueryRepository)repository).GetSpouseForPerson(RootEntity.Id, user),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((PersonQueryRepository)repository).GetSpouseForPersonAsync(RootEntity.Id, user)
            };

            QueryOperations.Enqueue(GetSpouseQueryOperation);
        }

        public PhoneNumberOutputDto GetCellPhoneDto(Person person) => 
            (person.CellPhone.IsEmpty()) ? null : new PhoneNumberOutputDto
            {
                AreaCode = person.CellPhone.AreaCode,
                Exchange = person.CellPhone.Exchange,
                Number = person.CellPhone.Number
            };

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

        public override void PopulateDto(Employee entity)
        {
            OutputDto.Id = entity.Id.Value;

            OutputDto.HireDate = entity.HireDate;

            OutputDto.Name = entity.Name;

            OutputDto.MarriedToPersonId = entity.MarriedToPersonId;

            OutputDto.CellPhone = GetCellPhoneDto(entity);

            OutputDto.Spouse = GetSpouseDto();
        }

    }
}