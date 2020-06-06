using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeePerson.EmployeeBoundedContext
{
    public class EmployeeQueryAggregate : GetByIdQueryAggregate<Employee, int?, EmployeeOutputDto>
    {
        public EmployeeQueryAggregate() : this(null)
        {
        }

        public EmployeeQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(EmployeePersonConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            EmployeeQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.HireDate = RootEntity.HireDate;

            OutputDto.Name = RootEntity.Name;

            OutputDto.Gender = RootEntity.Gender;

            OutputDto.CellPhone = GetCellPhoneDto();
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