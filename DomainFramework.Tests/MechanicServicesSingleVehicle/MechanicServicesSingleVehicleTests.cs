using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MechanicServicesSingleVehicle.GarageBoundedContext
{
    [TestClass]
    public class MechanicServicesSingleVehicle
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\MechanicServicesSingleVehicle\Sql\CreateTestDatabase.sql");

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
        public void Save_Mechanic_Only_Tests()
        {
            // Insert
            var commandAggregate = new SaveMechanicCommandAggregate(new SaveMechanicInputDto
            {
                Name = "Sasha"
            });

            commandAggregate.Save();

            var mechanicId = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new MechanicQueryAggregate();

            var mechanicDto = queryAggregate.Get(mechanicId);

            Assert.AreEqual(mechanicId, mechanicDto.MechanicId);

            Assert.AreEqual("Sasha", mechanicDto.Name);

            Assert.IsNull(mechanicDto.Vehicle);

            // Update
            commandAggregate = new SaveMechanicCommandAggregate(new SaveMechanicInputDto
            {
                MechanicId = mechanicId,
                Name = "Alexander"
            });

            commandAggregate.Save();

            // Read
            queryAggregate = new MechanicQueryAggregate();

            mechanicDto = queryAggregate.Get(mechanicId);

            Assert.AreEqual(mechanicId, mechanicDto.MechanicId);

            Assert.AreEqual("Alexander", mechanicDto.Name);

            Assert.IsNull(mechanicDto.Vehicle);

            // Delete
            //var deleteAggregate = new DeleteMechanicCommandAggregate(new DeleteMechanicInputDto
            //{
            //    Id = mechanicId.Value
            //});

            //deleteAggregate.Save();

            //mechanicDto = queryAggregate.Get(mechanicId);

            //Assert.IsNull(mechanicDto);
        }

        [TestMethod]
        public void Save_Mechanic_With_Vehicle_Tests()
        {
            // Insert
            var commandAggregate = new SaveMechanicCommandAggregate(new SaveMechanicInputDto
            {
                Name = "Sasha",
                Vehicle = new VehicleInputDto
                    {
                        Model = "Honda",
                        Cylinders = new List<CylinderInputDto>
                        {
                            new CylinderInputDto
                            {
                                Diameter = 1
                            },
                            new CylinderInputDto
                            {
                                Diameter = 2
                            },
                            new CylinderInputDto
                            {
                                Diameter = 3
                            },
                            new CylinderInputDto
                            {
                                Diameter = 4
                            }

                        }
                    }
            });

            commandAggregate.Save();

            var mechanicId = commandAggregate.RootEntity.Id;

            // Read
            var queryAggregate = new MechanicQueryAggregate();

            var mechanicDto = queryAggregate.Get(mechanicId);

            Assert.AreEqual(mechanicId, mechanicDto.MechanicId);

            Assert.AreEqual("Sasha", mechanicDto.Name);

            var vehicleDto = mechanicDto.Vehicle;

            Assert.AreEqual("Honda", vehicleDto.Model);

            Assert.AreEqual(4, vehicleDto.Cylinders.Count());

            Assert.AreEqual(mechanicDto.MechanicId, vehicleDto.MechanicId);

            // Update
            commandAggregate = new SaveMechanicCommandAggregate(new SaveMechanicInputDto
            {
                MechanicId = mechanicId,
                Name = "Alexander",
                Vehicle = new VehicleInputDto
                {
                    Model = "Toyota",
                    Cylinders = new List<CylinderInputDto>
                        {
                            new CylinderInputDto
                            {
                                Diameter = 4
                            }
                        }

                }
            });

            commandAggregate.Save();

            // Read
            queryAggregate = new MechanicQueryAggregate();

            mechanicDto = queryAggregate.Get(mechanicId);

            Assert.AreEqual(mechanicId, mechanicDto.MechanicId);

            Assert.AreEqual("Alexander", mechanicDto.Name);

            vehicleDto = mechanicDto.Vehicle;

            Assert.AreEqual("Toyota", vehicleDto.Model);

            Assert.AreEqual(mechanicDto.MechanicId, vehicleDto.MechanicId);

            // Delete
            //var deleteAggregate = new DeleteMechanicCommandAggregate(new DeleteMechanicInputDto
            //{
            //    Id = mechanicId.Value
            //});

            //deleteAggregate.Save();

            //mechanicDto = queryAggregate.Get(mechanicId);

            //Assert.IsNull(mechanicDto);
        }
    }
}
