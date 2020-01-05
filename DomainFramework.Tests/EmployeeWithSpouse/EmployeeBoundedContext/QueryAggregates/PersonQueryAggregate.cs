using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithSpouse.EmployeeBoundedContext
{
    public class PersonQueryAggregate : GetByIdQueryAggregate<Person, int?, PersonOutputDto>
    {
        public PersonQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(EmployeeWithSpouseConnectionClass.GetConnectionName());

            PersonQueryRepository.Register(context);

            RepositoryContext = context;
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
        }

    }
}