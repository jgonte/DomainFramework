using DomainFramework.Core;

namespace DomainFramework.Tests
{
    class SaveExecutiveEmployeePersonCommandAggregate : CommandAggregate<ExecutiveEntity>
    {
        public PersonEntity Person { get; private set; }

        public EmployeeEntity Employee { get; private set; }

        public SaveExecutiveEmployeePersonCommandAggregate(DataAccess.RepositoryContext context, 
            string firstName, decimal salary, decimal bonus) : base(context)
        {
            Person = new PersonEntity
            {
                FirstName = firstName
            };

            var savePerson = new SaveEntityCommandOperation<PersonEntity>(Person);

            Enqueue(savePerson);

            Employee = new EmployeeEntity
            {
                Salary = salary
            };

            var saveEmployee = new SaveEntityCommandOperation<EmployeeEntity>(Employee, 
                new EntityDependency[] 
                {
                    new EntityDependency
                    {
                        Entity = Person
                    }
                });

            Enqueue(saveEmployee);

            RootEntity = new ExecutiveEntity
            {
                Bonus = bonus
            };

            var saveExecutive = new SaveEntityCommandOperation<ExecutiveEntity>(RootEntity,
                new EntityDependency[]
                {
                    new EntityDependency
                    {
                        Entity = Employee
                    }
                });

            Enqueue(saveExecutive);
        }
    }
}