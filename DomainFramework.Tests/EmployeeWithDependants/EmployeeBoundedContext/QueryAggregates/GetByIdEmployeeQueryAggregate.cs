using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class GetByIdEmployeeQueryAggregate : GetByIdQueryAggregate<Employee, int?, EmployeeOutputDto>
    {
        public GetByIdEmployeeQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(EmployeeWithDependantsConnectionClass.GetConnectionName());

            EmployeeQueryRepository.Register(context);

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

        public override void PopulateDto(Employee entity)
        {
            OutputDto.Id = entity.Id.Value;

            OutputDto.HireDate = entity.HireDate;

            OutputDto.Name = entity.Name;

            OutputDto.ProviderEmployeeId = entity.ProviderEmployeeId;

            OutputDto.CellPhone = GetCellPhoneDto(entity);

            //OutputDto.Dependants = GetDependantsDtos();
        }

    }
}