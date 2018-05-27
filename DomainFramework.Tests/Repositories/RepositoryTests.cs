using DomainFramework.Tests.Entitites;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DomainFramework.Tests.Repositories
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void Person_Repository_Test()
        {
            //var commandRepository = new InMemoryCommandRepository<PersonEntity, Person>(null);

            //var entity = new PersonEntity(new Person { Name = "Yana" });

            //commandRepository.Save(entity);

            //Assert.AreEqual(1, entity.Id);

            //var queryRepository = new InMemoryQueryRepository<PersonEntity, Person>(null);

            //var e = queryRepository.GetById(1);

            //Assert.AreEqual(entity, e);

            //Assert.AreEqual("Yana", e.Data.Name);

            //e.Data.Name = "Sarah";

            //commandRepository.Update(e);

            //e = queryRepository.GetById(1);

            //Assert.AreEqual("Sarah", e.Data.Name);

            //e = queryRepository.GetById(2);

            //Assert.IsNull(e);
        }

        [TestMethod]
        public void Student_Repository_Test()
        {
            //var personCommandRepository = new InMemoryCommandRepository<PersonEntity, Person>(null);

            //var personEntity = new PersonEntity(new Person { Name = "Sarah" });

            //personCommandRepository.Save(personEntity);

            //Assert.AreEqual(1, personEntity.Id);

            //var personQueryRepository = new InMemoryQueryRepository<PersonEntity, Person>(null);

            //var e = personQueryRepository.GetById(1);

            //var studentRoleCommandRepository = new InMemoryCommandRepository<StudentRoleEntity, StudentRole>(null);

            //var studentRoleEntity = new StudentRoleEntity(new StudentRole { PersonId = personEntity.Id, StudentNumber = "12345" });

            //studentRoleCommandRepository.Save(studentRoleEntity);

            //Assert.AreEqual(1, studentRoleEntity.Id);

            //var studentRoleQueryRepository = new InMemoryQueryRepository<StudentRoleEntity, StudentRole>(null);

            //var s = studentRoleQueryRepository.GetById(1);

            //Assert.AreEqual(studentRoleEntity, s);

            //Assert.AreEqual("12345", s.Data.StudentNumber);

            //s.Data.StudentNumber = "45678";

            //studentRoleCommandRepository.Update(s);

            //s = studentRoleQueryRepository.GetById(1);

            //Assert.AreEqual("45678", s.Data.StudentNumber);

            //s = studentRoleQueryRepository.GetById(2);

            //Assert.IsNull(s);
        }
    }
}
