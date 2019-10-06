using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class SaveEmployeePersonCommandAggregate : CommandAggregate<EmployeeEntity>
    {
        public PersonEntity Person { get; private set; }

        public SaveEmployeePersonCommandAggregate(DataAccess.RepositoryContext context, string firstName, int salary) : base(context)
        {
            Person = new PersonEntity
            {
                FirstName = firstName
            };

            var savePerson = new SaveEntityCommandOperation<PersonEntity>(Person);

            Enqueue(savePerson);

            RootEntity = new EmployeeEntity
            {
                Salary = salary
            };

            var saveEmployee = new SaveEntityCommandOperation<EmployeeEntity>(RootEntity,
                new EntityDependency[] 
                {
                    new EntityDependency
                    {
                        Entity =Person
                    }
                });

            Enqueue(saveEmployee);
        }

        public override void Initialize(IInputDataTransferObject inputDto, EntityDependency[] dependencies)
        {
            throw new System.NotImplementedException();
        }
    }
}