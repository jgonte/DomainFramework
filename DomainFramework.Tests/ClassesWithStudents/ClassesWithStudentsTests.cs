using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ClassesWithStudents.ClassBoundedContext
{
    [TestClass]
    public class ClassesWithStudents
    {
        #region Additional test attributes

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            // Create the test database
            var script = File.ReadAllText(
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\ClassesWithStudents\Sql\CreateTestDatabase.sql");

            ScriptRunner.Run(ConnectionManager.GetConnection("Master").ConnectionString, script);
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        //[TestMethod]
        //public void Class_With_Students_Tests()
        //{
        //    // Suppose the class and the student do not exist by the time we enroll it so we need to create the class and student records in the database
        //    var commandAggregate = new CreateClassEnrollmentCommandAggregate(
        //        new ClassEnrollmentInputDto
        //        {
        //            Name = "Programming",
        //            StudentsToEnroll = new List<StudentToEnrollDto>
        //            {
        //                new StudentToEnrollDto
        //                {
        //                    FirstName = "Sarah",
        //                    StartedDateTime = new DateTime(2017, 3, 12, 9, 16, 37)
        //                },
        //                new StudentToEnrollDto
        //                {
        //                    FirstName = "Yana",
        //                    StartedDateTime = new DateTime(2017, 4, 12, 10, 26, 47)
        //                },
        //                new StudentToEnrollDto
        //                {
        //                    FirstName = "Mark",
        //                    StartedDateTime = new DateTime(2017, 5, 14, 11, 24, 57)
        //                }
        //            }
        //        });

        //    commandAggregate.Save();

        //    var classEntity = commandAggregate.RootEntity;

        //    // Read
        //    context.RegisterQueryRepository<ClassEntity>(new ClassQueryRepository());

        //    context.RegisterQueryRepository<StudentEntity>(new StudentQueryRepository());

        //    var id = classEntity.Id; // Keep the generated id

        //    classEntity = new ClassEntity(); // New entity

        //    var queryAggregate = new ClassEnrollmentQueryAggregate(context);

        //    queryAggregate.Get(id, null);

        //    classEntity = queryAggregate.RootEntity;

        //    Assert.AreEqual(id, classEntity.Id);

        //    Assert.AreEqual("Programming", classEntity.Name);

        //    Assert.AreEqual(3, queryAggregate.Students.Count());

        //    var student = queryAggregate.Students.ElementAt(0);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Sarah", student.FirstName);

        //    student = queryAggregate.Students.ElementAt(1);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Yana", student.FirstName);

        //    student = queryAggregate.Students.ElementAt(2);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Mark", student.FirstName);
        //}

        //[TestMethod]
        //public async Task Class_With_Students_Async_Tests()
        //{
        //    var context = new RepositoryContext(connectionName);

        //    context.RegisterCommandRepositoryFactory<ClassEntity>(() => new ClassCommandRepository());

        //    context.RegisterCommandRepositoryFactory<StudentEntity>(() => new StudentCommandRepository());

        //    context.RegisterCommandRepositoryFactory<ClassEnrollmentEntity>(() => new ClassEnrollmentCommandRepository());

        //    // Suppose the class and the student do not exist by the time we enroll it so we need to create the class and student records in the database
        //    var commandAggregate = new ClassEnrollmentCommandAggregate(context,
        //        new ClassEnrollmentDto
        //        {
        //            Name = "Programming",
        //            StudentsToEnroll = new List<StudentToEnrollDto>
        //            {
        //                new StudentToEnrollDto
        //                {
        //                    FirstName = "Sarah",
        //                    StartedDateTime = new DateTime(2017, 3, 12, 9, 16, 37)
        //                },
        //                new StudentToEnrollDto
        //                {
        //                    FirstName = "Yana",
        //                    StartedDateTime = new DateTime(2017, 4, 12, 10, 26, 47)
        //                },
        //                new StudentToEnrollDto
        //                {
        //                    FirstName = "Mark",
        //                    StartedDateTime = new DateTime(2017, 5, 14, 11, 24, 57)
        //                }
        //            }
        //        });

        //    await commandAggregate.SaveAsync();

        //    var classEntity = commandAggregate.RootEntity;

        //    Assert.AreEqual(3, commandAggregate.StudentsToEnroll.Count());

        //    Assert.AreEqual(3, commandAggregate.ClassEnrollments.Count());

        //    // Read

        //    context.RegisterQueryRepository<ClassEntity>(new ClassQueryRepository());

        //    context.RegisterQueryRepository<StudentEntity>(new StudentQueryRepository());

        //    var id = classEntity.Id; // Keep the generated id

        //    classEntity = new ClassEntity(); // New entity

        //    var queryAggregate = new ClassEnrollmentQueryAggregate(context);

        //    await queryAggregate.GetAsync(id, null);

        //    classEntity = queryAggregate.RootEntity;

        //    Assert.AreEqual(id, classEntity.Id);

        //    Assert.AreEqual("Programming", classEntity.Name);

        //    Assert.AreEqual(3, queryAggregate.Students.Count());

        //    var student = queryAggregate.Students.ElementAt(0);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Sarah", student.FirstName);

        //    student = queryAggregate.Students.ElementAt(1);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Yana", student.FirstName);

        //    student = queryAggregate.Students.ElementAt(2);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Mark", student.FirstName);

        //    // Create a new student and add it to an existing class
        //    var createStudentAggregate = new CreateStudentCommandAggregate(context, new StudentInputDto
        //    {
        //        FirstName = "Jorge"
        //    });

        //    createStudentAggregate.Save();

        //    student = createStudentAggregate.RootEntity;

        //    var addStudentAggregate = new AddStudentCommandAggregate(context, classEntity.Id.Value, student.Id.Value);

        //    addStudentAggregate.Save();

        //    await queryAggregate.GetAsync(id, null);

        //    classEntity = queryAggregate.RootEntity;

        //    Assert.AreEqual(id, classEntity.Id);

        //    Assert.AreEqual("Programming", classEntity.Name);

        //    Assert.AreEqual(4, queryAggregate.Students.Count());

        //    student = queryAggregate.Students.ElementAt(0);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Sarah", student.FirstName);

        //    student = queryAggregate.Students.ElementAt(1);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Yana", student.FirstName);

        //    student = queryAggregate.Students.ElementAt(2);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Mark", student.FirstName);

        //    student = queryAggregate.Students.ElementAt(3);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Jorge", student.FirstName);

        //    // Create some students and replace the student of the class with those
        //    Guid[] studentsId = new Guid[2];

        //    createStudentAggregate = new CreateStudentCommandAggregate(context, new StudentInputDto
        //    {
        //        FirstName = "Vassili"
        //    });

        //    createStudentAggregate.Save();

        //    studentsId[0] = createStudentAggregate.RootEntity.Id.Value;

        //    createStudentAggregate = new CreateStudentCommandAggregate(context, new StudentInputDto
        //    {
        //        FirstName = "Tatiana"
        //    });

        //    createStudentAggregate.Save();

        //    studentsId[1] = createStudentAggregate.RootEntity.Id.Value;

        //    var replaceStudentsAggregate = new ReplaceStudentsAggregate(context, classEntity.Id.Value, studentsId);

        //    replaceStudentsAggregate.Save();

        //    // Verify the students were replaced

        //    await queryAggregate.GetAsync(id, null);

        //    classEntity = queryAggregate.RootEntity;

        //    Assert.AreEqual(id, classEntity.Id);

        //    Assert.AreEqual("Programming", classEntity.Name);

        //    Assert.AreEqual(2, queryAggregate.Students.Count());

        //    student = queryAggregate.Students.ElementAt(0);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Vassili", student.FirstName);

        //    student = queryAggregate.Students.ElementAt(1);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Tatiana", student.FirstName);
        //}

    }
}
