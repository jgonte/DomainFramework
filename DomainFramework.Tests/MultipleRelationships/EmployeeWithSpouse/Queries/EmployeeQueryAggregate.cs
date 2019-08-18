using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DomainFramework.Tests.EmployeeWithSpouse
{
    public class EmployeeQueryAggregate : GetByIdQueryAggregate<Employee, int?, EmployeeOutputDto>
    {
        private EmployeeQueryAggregate _employeeQueryAggregate;

        public GetSingleLinkedEntityLoadOperation<Person> GetSpouseLoadOperation { get; }

        public Person Spouse => GetSpouseLoadOperation.LinkedEntity;

        public EmployeeQueryAggregate()
        {
            var context = new DomainFramework.DataAccess.RepositoryContext(TestConnectionClass.GetConnectionName());

            EmployeeQueryRepository.Register(context);

            PersonQueryRepository.Register(context);

            RepositoryContext = context;

            GetSpouseLoadOperation = new GetSingleLinkedEntityLoadOperation<Person>
            {
                GetLinkedEntity = (repository, entity, user) => ((PersonQueryRepository)repository).GetSpouseForPerson(RootEntity.Id, user),
                GetLinkedEntityAsync = async (repository, entity, user) => await ((PersonQueryRepository)repository).GetSpouseForPersonAsync(RootEntity.Id, user)
            };

            LoadOperations.Enqueue(GetSpouseLoadOperation);
        }

        public override EmployeeOutputDto GetDataTransferObject()
        {
            var dto = new EmployeeOutputDto
            {
                Id = RootEntity.Id.Value,
                HireDate = RootEntity.HireDate,
                Name = RootEntity.Name,
                MarriedToPersonId = RootEntity.MarriedToPersonId,
                CellPhone = GetCellPhoneDto()
            };

            dto.Spouse = GetSpouseDto();

            return dto;
        }

        public PhoneNumberOutputDto GetCellPhoneDto() =>
            (RootEntity.CellPhone.IsEmpty()) ? null : new PhoneNumberOutputDto
            {
                AreaCode = RootEntity.CellPhone.AreaCode,
                Exchange = RootEntity.CellPhone.Exchange,
                Number = RootEntity.CellPhone.Number
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

    }
}
