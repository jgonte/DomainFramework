using DataAccess;
using DomainFramework.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EntityWithMultiValuedValueObject.TestBoundedContext
{
    [TestClass]
    public class EntityWithMultiValuedValueObject
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
                @"C:\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\EntityWithMultiValuedValueObject\Sql\CreateTestDatabase.sql"
            );

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
        public void Entity_With_MultiValued_ValueObject_Test()
        {
            // Create
            var commandAggregate = new SaveTestEntityCommandAggregate(new SaveTestEntityInputDto
            {
                Text = "Some text",
                TypeValues1 = new List<TypeValueInputDto>
                {
                    new TypeValueInputDto
                    {
                        DataType = TypeValue.DataTypes.Integer,
                        Data = "123"
                    },
                    new TypeValueInputDto
                    {
                        DataType = TypeValue.DataTypes.String,
                        Data = "Some string"
                    }
                }
            });

            commandAggregate.Save();

            var entityId = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new TestEntityQueryAggregate();

            var dto = queryAggregate.Get(entityId);

            Assert.AreEqual(entityId, dto.TestEntityId);

            Assert.AreEqual("Some text", dto.Text);

            Assert.AreEqual(2, dto.TypeValues1.Count());

            var typeValue = dto.TypeValues1.ElementAt(0);

            Assert.AreEqual(TypeValue.DataTypes.Integer, typeValue.DataType);

            Assert.AreEqual("123", typeValue.Data);

            typeValue = dto.TypeValues1.ElementAt(1);

            Assert.AreEqual(TypeValue.DataTypes.String, typeValue.DataType);

            Assert.AreEqual("Some string", typeValue.Data);

            // Update
            commandAggregate = new SaveTestEntityCommandAggregate(new SaveTestEntityInputDto
            {
                TestEntityId = entityId,
                Text = "Some replaced text",
                TypeValues1 = new List<TypeValueInputDto>
                {
                    new TypeValueInputDto
                    {
                        DataType = TypeValue.DataTypes.String,
                        Data = "Some updated string"
                    },
                    new TypeValueInputDto
                    {
                        DataType = TypeValue.DataTypes.Integer,
                        Data = "456"
                    }
                }
            });

            commandAggregate.Save();

            // Read changes
            queryAggregate = new TestEntityQueryAggregate();

            dto = queryAggregate.Get(entityId);

            Assert.AreEqual(entityId, dto.TestEntityId);

            Assert.AreEqual("Some replaced text", dto.Text);

            Assert.AreEqual(2, dto.TypeValues1.Count());

            typeValue = dto.TypeValues1.ElementAt(0);

            Assert.AreEqual(TypeValue.DataTypes.String, typeValue.DataType);

            Assert.AreEqual("Some updated string", typeValue.Data);

            typeValue = dto.TypeValues1.ElementAt(1);

            Assert.AreEqual(TypeValue.DataTypes.Integer, typeValue.DataType);

            Assert.AreEqual("456", typeValue.Data);

            // Add an extra data types without deleting the existing ones
            var addTypeValueCommandAggregate = new AddTypeValue1CommandAggregate(new AddTypeValues1InputDto
            {
                TestEntityId = entityId,
                TypeValues1 = new List<TypeValueInputDto>
                {
                    new TypeValueInputDto
                    {
                        DataType = TypeValue.DataTypes.String,
                        Data = "Some string added"
                    },
                    new TypeValueInputDto
                    {
                        DataType = TypeValue.DataTypes.Integer,
                        Data = "789"
                    }
                }
            });

            addTypeValueCommandAggregate.Save();

            // Read changes
            queryAggregate = new TestEntityQueryAggregate();

            dto = queryAggregate.Get(entityId);

            Assert.AreEqual(entityId, dto.TestEntityId);

            Assert.AreEqual("Some replaced text", dto.Text);

            Assert.AreEqual(4, dto.TypeValues1.Count());

            typeValue = dto.TypeValues1.ElementAt(0);

            Assert.AreEqual(TypeValue.DataTypes.String, typeValue.DataType);

            Assert.AreEqual("Some updated string", typeValue.Data);

            typeValue = dto.TypeValues1.ElementAt(1);

            Assert.AreEqual(TypeValue.DataTypes.Integer, typeValue.DataType);

            Assert.AreEqual("456", typeValue.Data);

            typeValue = dto.TypeValues1.ElementAt(2);

            Assert.AreEqual(TypeValue.DataTypes.String, typeValue.DataType);

            Assert.AreEqual("Some string added", typeValue.Data);

            typeValue = dto.TypeValues1.ElementAt(3);

            Assert.AreEqual(TypeValue.DataTypes.Integer, typeValue.DataType);

            Assert.AreEqual("789", typeValue.Data);
        }
    }
}
