using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class GetByIdPersonQueryAggregate : GetByIdQueryAggregate<Person, int?, PersonOutputDto>
    {
        public GetByIdPersonQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(EmployeeWithDependantsConnectionClass.GetConnectionName());

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

                employeeDto.ProviderEmployeeId = employee.ProviderEmployeeId;

                employeeDto.CellPhone = GetCellPhoneDto(employee);

                OutputDto = employeeDto;
            }
            else
            {
                var personDto = new PersonOutputDto();

                personDto.Id = entity.Id.Value;

                personDto.Name = entity.Name;

                personDto.ProviderEmployeeId = entity.ProviderEmployeeId;

                personDto.CellPhone = GetCellPhoneDto(entity);

                OutputDto = personDto;
            }
        }

    }
}