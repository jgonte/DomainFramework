using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExecutiveEmployeePersonCustomer.ExecutiveBoundedContext
{
    public class PersonQueryAggregate : GetByIdQueryAggregate<Person, int?, PersonOutputDto>
    {
        public PersonQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(ExecutiveEmployeePersonCustomerConnectionClass.GetConnectionName());

            PersonQueryRepository.Register(context);

            RepositoryContext = context;
        }

        public AssetOutputDto GetAssetDto(Executive executive) => 
            (executive.Asset.IsEmpty()) ? null : new AssetOutputDto
            {
                Number = executive.Asset.Number
            };

        public override void PopulateDto(Person entity)
        {
            if (entity is Customer)
            {
                var customer = (Customer)entity;

                var customerDto = new CustomerOutputDto();

                customerDto.Id = customer.Id.Value;

                customerDto.Rating = customer.Rating;

                customerDto.Name = customer.Name;

                OutputDto = customerDto;
            }
            else if (entity is Executive)
            {
                var executive = (Executive)entity;

                var executiveDto = new ExecutiveOutputDto();

                executiveDto.Id = executive.Id.Value;

                executiveDto.Bonus = executive.Bonus;

                executiveDto.HireDate = executive.HireDate;

                executiveDto.Name = executive.Name;

                executiveDto.Asset = GetAssetDto(executive);

                OutputDto = executiveDto;
            }
            else if (entity is Employee)
            {
                var employee = (Employee)entity;

                var employeeDto = new EmployeeOutputDto();

                employeeDto.Id = employee.Id.Value;

                employeeDto.HireDate = employee.HireDate;

                employeeDto.Name = employee.Name;

                OutputDto = employeeDto;
            }
            else
            {
                var personDto = new PersonOutputDto();

                personDto.Id = entity.Id.Value;

                personDto.Name = entity.Name;

                OutputDto = personDto;
            }
        }

    }
}