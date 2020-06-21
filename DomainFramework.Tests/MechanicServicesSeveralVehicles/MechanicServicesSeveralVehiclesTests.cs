using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlServerScriptRunner;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MechanicServicesSeveralVehicles.GarageBoundedContext
{
    [TestClass]
    public class MechanicServicesSeveralVehicles
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
                @"C:\tmp\Dev\Projects\Development\DomainFramework.Solution\DomainFramework.Tests\MechanicServicesSeveralVehicles\Sql\CreateTestDatabase.sql");

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

            Assert.AreEqual(0, mechanicDto.Vehicles.Count());

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

            Assert.AreEqual(0, mechanicDto.Vehicles.Count());

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
        public void Save_Mechanic_With_Vehicles_Tests()
        {
            // Insert
            var commandAggregate = new SaveMechanicCommandAggregate(new SaveMechanicInputDto
            {
                Name = "Sasha",
                Vehicles = new List<VehicleInputDto>
                {
                    new CarInputDto
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
                        },
                        Doors = new List<DoorInputDto>
                        {
                            new DoorInputDto
                            {
                                Number = 1
                            },
                            new DoorInputDto
                            {
                                Number = 2
                            }
                        },
                        Passengers = 4
                    },
                    new TruckInputDto
                    {
                        Model = "Chevrolet",
                        Weight = 4000,
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
                            },
                            new CylinderInputDto
                            {
                                Diameter = 5
                            },
                            new CylinderInputDto
                            {
                                Diameter = 6
                            }
                        },
                        Inspections = new List<InspectionInputDto>
                        {
                            new InspectionInputDto
                            {
                                Date = new System.DateTime(2012, 1, 18)
                            },
                            new InspectionInputDto
                            {
                                Date = new System.DateTime(2016, 12, 23)
                            },
                            new InspectionInputDto
                            {
                                Date = new System.DateTime(2019, 2, 13)
                            }
                        }
                    },
                    new VehicleInputDto
                    {
                        Model = "Huyndai",
                        Cylinders = new List<CylinderInputDto>
                        {
                            new CylinderInputDto
                            {
                                Diameter = 5
                            },
                            new CylinderInputDto
                            {
                                Diameter = 6
                            }
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

            Assert.AreEqual(3, mechanicDto.Vehicles.Count());

            var vehicleDto = mechanicDto.Vehicles.ElementAt(0);

            Assert.AreEqual("Chevrolet", vehicleDto.Model);

            Assert.AreEqual(6, vehicleDto.Cylinders.Count());

            Assert.AreEqual(3, ((TruckOutputDto)vehicleDto).Inspections.Count());

            Assert.AreEqual(mechanicDto.MechanicId, vehicleDto.MechanicId);

            vehicleDto = mechanicDto.Vehicles.ElementAt(1);

            Assert.AreEqual("Honda", vehicleDto.Model);

            Assert.AreEqual(4, vehicleDto.Cylinders.Count());

            Assert.AreEqual(2, ((CarOutputDto)vehicleDto).Doors.Count());

            Assert.AreEqual(mechanicDto.MechanicId, vehicleDto.MechanicId);

            vehicleDto = mechanicDto.Vehicles.ElementAt(2);

            Assert.AreEqual("Huyndai", vehicleDto.Model);

            Assert.AreEqual(2, vehicleDto.Cylinders.Count());

            Assert.AreEqual(mechanicDto.MechanicId, vehicleDto.MechanicId);

            // Update
            commandAggregate = new SaveMechanicCommandAggregate(new SaveMechanicInputDto
            {
                MechanicId = mechanicId,
                Name = "Alexander",
                Vehicles = new List<VehicleInputDto>
                {
                    new VehicleInputDto
                    {
                        Model = "Toyota",
                        Cylinders = new List<CylinderInputDto>
                        {
                            new CylinderInputDto
                            {
                                Diameter = 4
                            }
                        }

                    },
                    new VehicleInputDto
                    {
                        Model = "BMW",
                        Cylinders = new List<CylinderInputDto>
                        {
                            new CylinderInputDto
                            {
                                Diameter = 3
                            }
                        }
                    },
                    new VehicleInputDto
                    {
                        Model = "Ford",
                        Cylinders = new List<CylinderInputDto>
                        {
                            new CylinderInputDto
                            {
                                Diameter = 6
                            }
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

            vehicleDto = mechanicDto.Vehicles.ElementAt(0);

            Assert.AreEqual("Toyota", vehicleDto.Model);

            Assert.AreEqual(mechanicDto.MechanicId, vehicleDto.MechanicId);

            vehicleDto = mechanicDto.Vehicles.ElementAt(1);

            Assert.AreEqual("BMW", vehicleDto.Model);

            Assert.AreEqual(mechanicDto.MechanicId, vehicleDto.MechanicId);

            vehicleDto = mechanicDto.Vehicles.ElementAt(2);

            Assert.AreEqual("Ford", vehicleDto.Model);

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
