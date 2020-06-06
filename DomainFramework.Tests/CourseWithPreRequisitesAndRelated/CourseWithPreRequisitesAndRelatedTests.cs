using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CourseWithPreRequisitesAndRelated.CourseBoundedContext
{
    [TestClass]
    public class CourseWithPreRequisitesAndRelated
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\CourseWithPreRequisitesAndRelated\Sql\CreateTestDatabase.sql");

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

        [TestMethod]
        public void Course_With_PreRequisites_And_Related_Tests()
        {
            var courseCommandAggregate = new SaveCourseCommandAggregate(new CourseInputDto
            {
                Description = "Some Course",
                Requires = new List<CourseInputDto>
                {
                    new CourseInputDto
                    {
                        Description = "Pre-requisite 1",
                        PreRequisite = new PreRequisiteInputDto(),
                        CourseRelation = new CourseRelationInputDto()
                    },
                    new CourseInputDto
                    {
                        Description = "Pre-requisite 2",
                        PreRequisite = new PreRequisiteInputDto(),
                        CourseRelation = new CourseRelationInputDto()
                    }
                },
                Relates = new List<CourseInputDto>
                {
                    new CourseInputDto
                    {
                        Description = "Related course 1",
                        PreRequisite = new PreRequisiteInputDto(),
                        CourseRelation = new CourseRelationInputDto()
                    },
                    new CourseInputDto
                    {
                        Description = "Related course 2",
                        PreRequisite = new PreRequisiteInputDto(),
                        CourseRelation = new CourseRelationInputDto()
                    }
                },
                PreRequisite = new PreRequisiteInputDto(),
                CourseRelation = new CourseRelationInputDto()
            });

            courseCommandAggregate.Save();

            var courseId = courseCommandAggregate.RootEntity.Id;

            // Retrieve the course with the students
            var getCourseByIdQueryAggregate = new GetCourseByIdQueryAggregate();

            var classDto = getCourseByIdQueryAggregate.Get(courseId);

            Assert.AreEqual(courseId, classDto.Id);

            Assert.AreEqual("Some Course", classDto.Description);

            Assert.AreEqual(2, classDto.Requires.Count());

            Assert.AreEqual("Pre-requisite 1", classDto.Requires.ElementAt(0).Description);

            Assert.AreEqual("Pre-requisite 2", classDto.Requires.ElementAt(1).Description);

            Assert.AreEqual(2, classDto.Relates.Count());

            Assert.AreEqual("Related course 1", classDto.Relates.ElementAt(0).Description);

            Assert.AreEqual("Related course 2", classDto.Relates.ElementAt(1).Description);

            //// Replace the students
            //var replaceEnrollmentCommandAggregate = new ReplaceClassStudentsCommandAggregate(new ReplaceClassStudentsInputDto
            //{
            //    ClassId = courseId.Value,
            //    Students = new List<StudentInputDto>
            //    {
            //        new StudentInputDto
            //        {
            //            FirstName = "Jorge",
            //            Enrollment = new ClassEnrollmentInputDto
            //            {
            //                StartedDateTime = new DateTime(2010, 3, 11, 9, 16, 37),
            //                ClassId = courseId.Value
            //            }

            //        },
            //        new StudentInputDto
            //        {
            //            FirstName = "Moshe",
            //            Enrollment = new ClassEnrollmentInputDto
            //            {
            //                StartedDateTime = new DateTime(2011, 4, 12, 10, 16, 37),
            //                ClassId = courseId.Value
            //            }
            //        },
            //        new StudentInputDto
            //        {
            //            FirstName = "Daphni",
            //            Enrollment = new ClassEnrollmentInputDto
            //            {
            //                StartedDateTime =new DateTime(2012, 5, 13, 11, 16, 37),
            //                ClassId = courseId.Value
            //            }
            //        }
            //    }

            //});

            //replaceEnrollmentCommandAggregate.Save();

            //getCourseByIdQueryAggregate = new GetClassByIdQueryAggregate();

            //classDto = getCourseByIdQueryAggregate.Get(courseId);

            //Assert.AreEqual(courseId, classDto.Id);

            //Assert.AreEqual("Programming", classDto.Name);

            //Assert.AreEqual(3, classDto.Students.Count());

            //student = classDto.Students.ElementAt(0);

            //Assert.IsNotNull(student.Id);

            //Assert.AreEqual("Jorge", student.FirstName);

            //student = classDto.Students.ElementAt(1);

            //Assert.IsNotNull(student.Id);

            //Assert.AreEqual("Moshe", student.FirstName);

            //student = classDto.Students.ElementAt(2);

            //Assert.IsNotNull(student.Id);

            //Assert.AreEqual("Daphni", student.FirstName);
        }

        //[TestMethod]
        //public async Task Class_With_Students_Async_Tests()
        //{
        //    var context = new RepositoryContext(connectionName);

        //    context.RegisterCommandRepositoryFactory<ClassEntity>(() => new ClassCommandRepository());

        //    context.RegisterCommandRepositoryFactory<StudentEntity>(() => new StudentCommandRepository());

        //    context.RegisterCommandRepositoryFactory<ClassEnrollmentEntity>(() => new ClassEnrollmentCommandRepository());

        //    // Suppose the course and the student do not exist by the time we enroll it so we need to create the course and student records in the database
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

        //    var classDto = commandAggregate.RootEntity;

        //    Assert.AreEqual(3, commandAggregate.StudentsToEnroll.Count());

        //    Assert.AreEqual(3, commandAggregate.ClassEnrollments.Count());

        //    // Read

        //    context.RegisterQueryRepository<ClassEntity>(new ClassQueryRepository());

        //    context.RegisterQueryRepository<StudentEntity>(new StudentQueryRepository());

        //    var id = classDto.Id; // Keep the generated id

        //    classDto = new ClassEntity(); // New entity

        //    var classDto = new ClassEnrollmentQueryAggregate(context);

        //    await classDto.GetAsync(id, null);

        //    classDto = classDto.RootEntity;

        //    Assert.AreEqual(id, classDto.Id);

        //    Assert.AreEqual("Programming", classDto.Name);

        //    Assert.AreEqual(3, classDto.Students.Count());

        //    var student = classDto.Students.ElementAt(0);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Sarah", student.FirstName);

        //    student = classDto.Students.ElementAt(1);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Yana", student.FirstName);

        //    student = classDto.Students.ElementAt(2);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Mark", student.FirstName);

        //    // Create a new student and add it to an existing course
        //    var createStudentAggregate = new CreateStudentCommandAggregate(context, new StudentInputDto
        //    {
        //        FirstName = "Jorge"
        //    });

        //    createStudentAggregate.Save();

        //    student = createStudentAggregate.RootEntity;

        //    var addStudentAggregate = new AddStudentCommandAggregate(context, classDto.Id.Value, student.Id.Value);

        //    addStudentAggregate.Save();

        //    await classDto.GetAsync(id, null);

        //    classDto = classDto.RootEntity;

        //    Assert.AreEqual(id, classDto.Id);

        //    Assert.AreEqual("Programming", classDto.Name);

        //    Assert.AreEqual(4, classDto.Students.Count());

        //    student = classDto.Students.ElementAt(0);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Sarah", student.FirstName);

        //    student = classDto.Students.ElementAt(1);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Yana", student.FirstName);

        //    student = classDto.Students.ElementAt(2);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Mark", student.FirstName);

        //    student = classDto.Students.ElementAt(3);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Jorge", student.FirstName);

        //    // Create some students and replace the student of the course with those
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

        //    var replaceStudentsAggregate = new ReplaceStudentsAggregate(context, classDto.Id.Value, studentsId);

        //    replaceStudentsAggregate.Save();

        //    // Verify the students were replaced

        //    await classDto.GetAsync(id, null);

        //    classDto = classDto.RootEntity;

        //    Assert.AreEqual(id, classDto.Id);

        //    Assert.AreEqual("Programming", classDto.Name);

        //    Assert.AreEqual(2, classDto.Students.Count());

        //    student = classDto.Students.ElementAt(0);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Vassili", student.FirstName);

        //    student = classDto.Students.ElementAt(1);

        //    Assert.IsNotNull(student.Id);

        //    Assert.AreEqual("Tatiana", student.FirstName);
        //}

    }
}
