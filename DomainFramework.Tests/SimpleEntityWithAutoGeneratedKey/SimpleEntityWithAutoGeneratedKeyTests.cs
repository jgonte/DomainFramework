﻿using DataAccess;
using DomainFramework.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SimpleEntityWithAutoGeneratedKey.SimpleEntityWithAutoGeneratedKeyBoundedContext
{
    [TestClass]
    public class SimpleEntityWithAutoGeneratedKeyTests
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
                @"C:\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\SimpleEntityWithAutoGeneratedKey\Sql\CreateTestDatabase.sql");

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
        public void Simple_Entity_With_AutoGenerated_Key_Test()
        {
            // Insert
            var commandAggregate = new SaveTestEntityCommandAggregate(new SaveTestEntityInputDto
            {
                Enumeration1 = TestEntity.EnumerationType1.Element3,
                Text = "Some text",
                TypeValue1 = new TypeValueInputDto
                {
                    DataType = TypeValue.DataTypes.Integer,
                    Data = "305"
                },
                Url = new UrlInputDto
                {
                    Value = "https://myurl.dev"
                },
                Distance = new SelectionCriteriaInputDto
                {
                    Selected = true,
                    YesNoNotSure = "?"
                },
                Time = new SelectionCriteriaInputDto
                {
                    Selected = false,
                    YesNoNotSure = "Y"
                },
                Traffic = new SelectionCriteriaInputDto
                {
                    Selected = true,
                    YesNoNotSure = "N"
                }
            });

            commandAggregate.Save();

            var id = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new TestEntityQueryAggregate();

            var dto = queryAggregate.Get(id);

            Assert.IsTrue(dto.IsEnumeration1Element3);

            Assert.AreEqual("Some text", dto.Text);

            Assert.AreEqual(TypeValue.DataTypes.Integer, dto.TypeValue1.DataType);

            Assert.AreEqual("305", dto.TypeValue1.Data);

            Assert.AreEqual("https://myurl.dev", dto.Url.Value);

            Assert.IsTrue(dto.Distance.Selected.Value);

            Assert.IsTrue(dto.Distance.NotSure);

            Assert.IsFalse(dto.Time.Selected.Value);

            Assert.IsTrue(dto.Time.Yes);

            Assert.IsTrue(dto.Traffic.Selected.Value);

            Assert.IsTrue(dto.Traffic.No);

            // Update
            commandAggregate = new SaveTestEntityCommandAggregate(new SaveTestEntityInputDto
            {
                TestEntityId = id,
                Enumeration1 = TestEntity.EnumerationType1.Element2,
                Text = "Some other text",
                TypeValue1 = new TypeValueInputDto
                {
                    DataType = TypeValue.DataTypes.String,
                    Data = "String data"
                },
                Url = new UrlInputDto
                {
                    Value = "https://myotherurl.dev"
                },
                Distance = new SelectionCriteriaInputDto
                {
                    Selected = false,
                    YesNoNotSure = "Y"
                },
                Time = new SelectionCriteriaInputDto
                {
                    Selected = true,
                    YesNoNotSure = "N"
                },
                Traffic = new SelectionCriteriaInputDto
                {
                    Selected = false,
                    YesNoNotSure = "?"
                }
            });

            var user = new AuthenticatedUser<int>(1);

            commandAggregate.Save(user);

            // Read
            queryAggregate = new TestEntityQueryAggregate();

            dto = queryAggregate.Get(id);

            Assert.IsTrue(dto.IsEnumeration1Element2);

            Assert.AreEqual("Some other text", dto.Text);

            Assert.AreEqual(TypeValue.DataTypes.String, dto.TypeValue1.DataType);

            Assert.AreEqual("String data", dto.TypeValue1.Data);

            Assert.AreEqual("https://myotherurl.dev", dto.Url.Value);

            Assert.IsFalse(dto.Distance.Selected.Value);

            Assert.IsTrue(dto.Distance.Yes);

            Assert.IsTrue(dto.Time.Selected.Value);

            Assert.IsTrue(dto.Time.No);

            Assert.IsFalse(dto.Traffic.Selected.Value);

            Assert.IsTrue(dto.Traffic.NotSure);
        }
    }
}
