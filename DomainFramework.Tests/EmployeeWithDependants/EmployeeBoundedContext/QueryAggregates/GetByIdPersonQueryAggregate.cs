using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeWithDependants.EmployeeBoundedContext
{
    public class GetByIdPersonQueryAggregate : GetByIdQueryAggregate<Person, int?, PersonOutputDto>
    {
        public GetByIdPersonQueryAggregate() : this(null)
        {
        }

        public GetByIdPersonQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(EmployeeWithDependantsConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            PersonQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.Name = RootEntity.Name;

            OutputDto.ProviderEmployeeId = RootEntity.ProviderEmployeeId;

            OutputDto.CellPhone = GetCellPhoneDto();
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