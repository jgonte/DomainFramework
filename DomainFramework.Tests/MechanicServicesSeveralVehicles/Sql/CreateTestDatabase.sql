
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'MechanicServicesSeveralVehicles'
)
BEGIN
    DROP DATABASE [MechanicServicesSeveralVehicles]
END
GO

CREATE DATABASE [MechanicServicesSeveralVehicles]
GO

USE [MechanicServicesSeveralVehicles]
GO

CREATE SCHEMA [GarageBoundedContext];
GO

CREATE TABLE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Mechanic]
(
    [MechanicId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT Mechanic_PK PRIMARY KEY ([MechanicId])
);
GO

CREATE TABLE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
(
    [VehicleId] INT NOT NULL IDENTITY,
    [Model] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME,
    [MechanicId] INT
    CONSTRAINT Vehicle_PK PRIMARY KEY ([VehicleId])
);
GO

CREATE TABLE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle_Cylinders]
(
    [VehicleId] INT NOT NULL,
    [Diameter] INT
);
GO

CREATE TABLE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck]
(
    [TruckId] INT NOT NULL,
    [Weight] INT NOT NULL
    CONSTRAINT Truck_PK PRIMARY KEY ([TruckId])
);
GO

CREATE TABLE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck_Inspections]
(
    [TruckId] INT NOT NULL,
    [Date] DATETIME
);
GO

CREATE TABLE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car]
(
    [CarId] INT NOT NULL,
    [Passengers] INT NOT NULL
    CONSTRAINT Car_PK PRIMARY KEY ([CarId])
);
GO

CREATE TABLE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car_Doors]
(
    [CarId] INT NOT NULL,
    [Number] INT
);
GO

ALTER TABLE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck]
    ADD CONSTRAINT Truck_Vehicle_IFK FOREIGN KEY ([TruckId])
        REFERENCES [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] ([VehicleId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [Truck_Vehicle_IFK_IX]
    ON [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck]
    (
        [TruckId]
    );
GO

ALTER TABLE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car]
    ADD CONSTRAINT Car_Vehicle_IFK FOREIGN KEY ([CarId])
        REFERENCES [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] ([VehicleId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [Car_Vehicle_IFK_IX]
    ON [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car]
    (
        [CarId]
    );
GO

ALTER TABLE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
    ADD CONSTRAINT Mechanic_Vehicle_FK FOREIGN KEY ([MechanicId])
        REFERENCES [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Mechanic] ([MechanicId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Mechanic_Vehicle_FK_IX]
    ON [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
    (
        [MechanicId]
    );
GO

ALTER TABLE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle_Cylinders]
    ADD CONSTRAINT Vehicle_Cylinders_FK FOREIGN KEY ([VehicleId])
        REFERENCES [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] ([VehicleId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [Vehicle_Cylinders_FK_IX]
    ON [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle_Cylinders]
    (
        [VehicleId]
    );
GO

ALTER TABLE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck_Inspections]
    ADD CONSTRAINT Truck_Inspections_FK FOREIGN KEY ([TruckId])
        REFERENCES [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck] ([TruckId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [Truck_Inspections_FK_IX]
    ON [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck_Inspections]
    (
        [TruckId]
    );
GO

ALTER TABLE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car_Doors]
    ADD CONSTRAINT Car_Doors_FK FOREIGN KEY ([CarId])
        REFERENCES [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car] ([CarId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [Car_Doors_FK_IX]
    ON [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car_Doors]
    (
        [CarId]
    );
GO

CREATE PROCEDURE [GarageBoundedContext].[pCar_Delete]
    @carId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car]
    WHERE [CarId] = @carId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pCar_Insert]
    @passengers INT,
    @model VARCHAR(50),
    @createdBy INT,
    @mechanicId INT = NULL
AS
BEGIN
    DECLARE @vehicleId INT;

    DECLARE @vehicleOutputData TABLE
    (
        [VehicleId] INT
    );

    INSERT INTO [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
    (
        [Model],
        [CreatedBy],
        [MechanicId]
    )
    OUTPUT
        INSERTED.[VehicleId]
        INTO @vehicleOutputData
    VALUES
    (
        @model,
        @createdBy,
        @mechanicId
    );

    SELECT
        @vehicleId = [VehicleId]
    FROM @vehicleOutputData;

    INSERT INTO [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car]
    (
        [CarId],
        [Passengers]
    )
    VALUES
    (
        @vehicleId,
        @passengers
    );

    SELECT
        [VehicleId]
    FROM @vehicleOutputData;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pCar_Update]
    @carId INT,
    @passengers INT,
    @model VARCHAR(50),
    @updatedBy INT = NULL,
    @mechanicId INT = NULL
AS
BEGIN
    UPDATE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
    SET
        [Model] = @model,
        [UpdatedBy] = @updatedBy,
        [MechanicId] = @mechanicId,
        [UpdatedDateTime] = GETDATE()
    WHERE [VehicleId] = @carId;

    UPDATE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car]
    SET
        [Passengers] = @passengers
    WHERE [CarId] = @carId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_AddMechanic]
    @vehicleId INT,
    @mechanicId INT = NULL
AS
BEGIN
    UPDATE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
    SET
        [MechanicId] = @mechanicId
    WHERE [VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_LinkMechanic]
    @vehicleId INT,
    @mechanicId INT = NULL
AS
BEGIN
    UPDATE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
    SET
        [MechanicId] = @mechanicId
    WHERE [VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_UnlinkMechanic]
    @vehicleId INT
AS
BEGIN
    UPDATE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
    SET
        [MechanicId] = NULL
    WHERE [VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_AddCylinders]
    @vehicleId INT,
    @diameter INT = NULL
AS
BEGIN
    INSERT INTO [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle_Cylinders]
    (
        [VehicleId],
        [Diameter]
    )
    VALUES
    (
        @vehicleId,
        @diameter
    );

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pCar_AddDoors]
    @carId INT,
    @number INT = NULL
AS
BEGIN
    INSERT INTO [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car_Doors]
    (
        [CarId],
        [Number]
    )
    VALUES
    (
        @carId,
        @number
    );

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_DeleteCylinders]
    @vehicleId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle_Cylinders]
    WHERE [VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pCar_DeleteDoors]
    @carId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car_Doors]
    WHERE [CarId] = @carId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pMechanic_Delete]
    @mechanicId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Mechanic]
    WHERE [MechanicId] = @mechanicId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pMechanic_Insert]
    @name VARCHAR(50),
    @createdBy INT
AS
BEGIN
    DECLARE @mechanicOutputData TABLE
    (
        [MechanicId] INT
    );

    INSERT INTO [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Mechanic]
    (
        [Name],
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[MechanicId]
        INTO @mechanicOutputData
    VALUES
    (
        @name,
        @createdBy
    );

    SELECT
        [MechanicId]
    FROM @mechanicOutputData;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pMechanic_Update]
    @mechanicId INT,
    @name VARCHAR(50),
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Mechanic]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [MechanicId] = @mechanicId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pMechanic_UnlinkVehicles]
    @mechanicId INT
AS
BEGIN
    UPDATE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
    SET
        [MechanicId] = NULL
    WHERE [MechanicId] = @mechanicId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pTruck_Delete]
    @truckId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck]
    WHERE [TruckId] = @truckId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pTruck_Insert]
    @weight INT,
    @model VARCHAR(50),
    @createdBy INT,
    @mechanicId INT = NULL
AS
BEGIN
    DECLARE @vehicleId INT;

    DECLARE @vehicleOutputData TABLE
    (
        [VehicleId] INT
    );

    INSERT INTO [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
    (
        [Model],
        [CreatedBy],
        [MechanicId]
    )
    OUTPUT
        INSERTED.[VehicleId]
        INTO @vehicleOutputData
    VALUES
    (
        @model,
        @createdBy,
        @mechanicId
    );

    SELECT
        @vehicleId = [VehicleId]
    FROM @vehicleOutputData;

    INSERT INTO [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck]
    (
        [TruckId],
        [Weight]
    )
    VALUES
    (
        @vehicleId,
        @weight
    );

    SELECT
        [VehicleId]
    FROM @vehicleOutputData;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pTruck_Update]
    @truckId INT,
    @weight INT,
    @model VARCHAR(50),
    @updatedBy INT = NULL,
    @mechanicId INT = NULL
AS
BEGIN
    UPDATE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
    SET
        [Model] = @model,
        [UpdatedBy] = @updatedBy,
        [MechanicId] = @mechanicId,
        [UpdatedDateTime] = GETDATE()
    WHERE [VehicleId] = @truckId;

    UPDATE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck]
    SET
        [Weight] = @weight
    WHERE [TruckId] = @truckId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pTruck_AddInspections]
    @truckId INT,
    @date DATETIME = NULL
AS
BEGIN
    INSERT INTO [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck_Inspections]
    (
        [TruckId],
        [Date]
    )
    VALUES
    (
        @truckId,
        @date
    );

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pTruck_DeleteInspections]
    @truckId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck_Inspections]
    WHERE [TruckId] = @truckId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_Delete]
    @vehicleId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
    WHERE [VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_Insert]
    @model VARCHAR(50),
    @createdBy INT,
    @mechanicId INT = NULL
AS
BEGIN
    DECLARE @vehicleOutputData TABLE
    (
        [VehicleId] INT
    );

    INSERT INTO [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
    (
        [Model],
        [CreatedBy],
        [MechanicId]
    )
    OUTPUT
        INSERTED.[VehicleId]
        INTO @vehicleOutputData
    VALUES
    (
        @model,
        @createdBy,
        @mechanicId
    );

    SELECT
        [VehicleId]
    FROM @vehicleOutputData;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_Update]
    @vehicleId INT,
    @model VARCHAR(50),
    @updatedBy INT = NULL,
    @mechanicId INT = NULL
AS
BEGIN
    UPDATE [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
    SET
        [Model] = @model,
        [UpdatedBy] = @updatedBy,
        [MechanicId] = @mechanicId,
        [UpdatedDateTime] = GETDATE()
    WHERE [VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pCar_Get]
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    c.[CarId] AS "Id",
    v.[Model] AS "Model",
    v.[MechanicId] AS "MechanicId",
    c.[Passengers] AS "Passengers"',
        @from = N'[MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car] c
INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
    ON c.[CarId] = v.[VehicleId]',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pCar_GetAll]
AS
BEGIN
    SELECT
        c.[CarId] AS "Id",
        v.[Model] AS "Model",
        v.[MechanicId] AS "MechanicId",
        c.[Passengers] AS "Passengers"
    FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car] c
    INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
        ON c.[CarId] = v.[VehicleId];

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pCar_GetById]
    @carId INT
AS
BEGIN
    SELECT
        c.[CarId] AS "Id",
        v.[Model] AS "Model",
        v.[MechanicId] AS "MechanicId",
        c.[Passengers] AS "Passengers"
    FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car] c
    INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
        ON c.[CarId] = v.[VehicleId]
    WHERE c.[CarId] = @carId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pMechanic_GetAllVehicles]
    @mechanicId INT
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Model] AS "Model",
        _q_.[MechanicId] AS "MechanicId",
        _q_.[Weight] AS "Weight",
        _q_.[Passengers] AS "Passengers",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            t.[TruckId] AS "Id",
            v.[Model] AS "Model",
            v.[MechanicId] AS "MechanicId",
            t.[Weight] AS "Weight",
            NULL AS "Passengers",
            1 AS "_EntityType_"
        FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck] t
        INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
            ON t.[TruckId] = v.[VehicleId]
        UNION ALL
        (
            SELECT
                c.[CarId] AS "Id",
                v.[Model] AS "Model",
                v.[MechanicId] AS "MechanicId",
                NULL AS "Weight",
                c.[Passengers] AS "Passengers",
                2 AS "_EntityType_"
            FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car] c
            INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
                ON c.[CarId] = v.[VehicleId]
        )
        UNION ALL
        (
            SELECT
                v.[VehicleId] AS "Id",
                v.[Model] AS "Model",
                v.[MechanicId] AS "MechanicId",
                NULL AS "Weight",
                NULL AS "Passengers",
                3 AS "_EntityType_"
            FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
            LEFT OUTER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck] t
                ON t.[TruckId] = v.[VehicleId]
            LEFT OUTER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car] c
                ON c.[CarId] = v.[VehicleId]
            WHERE t.[TruckId] IS NULL
            AND c.[CarId] IS NULL
        )
    ) _q_
    WHERE _q_.[MechanicId] = @mechanicId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_GetCylinders]
    @vehicleId INT,
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
    IF @$filter IS NULL
    BEGIN
        SET @$filter = N'v.[VehicleId] = @vehicleId';
    END
    ELSE
    BEGIN
        SET @$filter = N'v.[VehicleId] = @vehicleId AND ' + @$filter;
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    v.[Diameter] AS "Diameter"',
        @from = N'[MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle_Cylinders] v',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pCar_GetDoors]
    @carId INT,
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
    IF @$filter IS NULL
    BEGIN
        SET @$filter = N'c.[CarId] = @carId';
    END
    ELSE
    BEGIN
        SET @$filter = N'c.[CarId] = @carId AND ' + @$filter;
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    c.[Number] AS "Number"',
        @from = N'[MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car_Doors] c',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_GetAllCylinders]
    @vehicleId INT
AS
BEGIN
    SELECT
        v.[Diameter] AS "Diameter"
    FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle_Cylinders] v
    WHERE v.[VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pCar_GetAllDoors]
    @carId INT
AS
BEGIN
    SELECT
        c.[Number] AS "Number"
    FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car_Doors] c
    WHERE c.[CarId] = @carId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pMechanic_Get]
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    m.[MechanicId] AS "Id",
    m.[Name] AS "Name"',
        @from = N'[MechanicServicesSeveralVehicles].[GarageBoundedContext].[Mechanic] m',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pMechanic_GetAll]
AS
BEGIN
    SELECT
        m.[MechanicId] AS "Id",
        m.[Name] AS "Name"
    FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Mechanic] m;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pMechanic_GetById]
    @mechanicId INT
AS
BEGIN
    SELECT
        m.[MechanicId] AS "Id",
        m.[Name] AS "Name"
    FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Mechanic] m
    WHERE m.[MechanicId] = @mechanicId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_GetMechanic]
    @vehicleId INT
AS
BEGIN
    SELECT
        m.[MechanicId] AS "Id",
        m.[Name] AS "Name"
    FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Mechanic] m
    INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
        ON m.[MechanicId] = v.[MechanicId]
    WHERE v.[VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pTruck_Get]
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    t.[TruckId] AS "Id",
    v.[Model] AS "Model",
    v.[MechanicId] AS "MechanicId",
    t.[Weight] AS "Weight"',
        @from = N'[MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck] t
INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
    ON t.[TruckId] = v.[VehicleId]',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pTruck_GetAll]
AS
BEGIN
    SELECT
        t.[TruckId] AS "Id",
        v.[Model] AS "Model",
        v.[MechanicId] AS "MechanicId",
        t.[Weight] AS "Weight"
    FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck] t
    INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
        ON t.[TruckId] = v.[VehicleId];

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pTruck_GetById]
    @truckId INT
AS
BEGIN
    SELECT
        t.[TruckId] AS "Id",
        v.[Model] AS "Model",
        v.[MechanicId] AS "MechanicId",
        t.[Weight] AS "Weight"
    FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck] t
    INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
        ON t.[TruckId] = v.[VehicleId]
    WHERE t.[TruckId] = @truckId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pTruck_GetInspections]
    @truckId INT,
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
    IF @$filter IS NULL
    BEGIN
        SET @$filter = N't.[TruckId] = @truckId';
    END
    ELSE
    BEGIN
        SET @$filter = N't.[TruckId] = @truckId AND ' + @$filter;
    END;

    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    t.[Date] AS "Date"',
        @from = N'[MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck_Inspections] t',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pTruck_GetAllInspections]
    @truckId INT
AS
BEGIN
    SELECT
        t.[Date] AS "Date"
    FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck_Inspections] t
    WHERE t.[TruckId] = @truckId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_Get]
    @$select NVARCHAR(MAX) = NULL,
    @$filter NVARCHAR(MAX) = NULL,
    @$orderby NVARCHAR(MAX) = NULL,
    @$skip NVARCHAR(10) = NULL,
    @$top NVARCHAR(10) = NULL,
    @count INT OUTPUT
AS
BEGIN
    EXEC [dbo].[pExecuteDynamicQuery]
        @$select = @$select,
        @$filter = @$filter,
        @$orderby = @$orderby,
        @$skip = @$skip,
        @$top = @$top,
        @selectList = N'    _q_.[Id] AS "Id",
    _q_.[Model] AS "Model",
    _q_.[MechanicId] AS "MechanicId",
    _q_.[Weight] AS "Weight",
    _q_.[Passengers] AS "Passengers",
    _q_.[_EntityType_] AS "_EntityType_"',
        @from = N'
(
    SELECT
        t.[TruckId] AS "Id",
        v.[Model] AS "Model",
        v.[MechanicId] AS "MechanicId",
        t.[Weight] AS "Weight",
        NULL AS "Passengers",
        1 AS "_EntityType_"
    FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck] t
    INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
        ON t.[TruckId] = v.[VehicleId]
    UNION ALL
    (
        SELECT
            c.[CarId] AS "Id",
            v.[Model] AS "Model",
            v.[MechanicId] AS "MechanicId",
            NULL AS "Weight",
            c.[Passengers] AS "Passengers",
            2 AS "_EntityType_"
        FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car] c
        INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
            ON c.[CarId] = v.[VehicleId]
    )
    UNION ALL
    (
        SELECT
            v.[VehicleId] AS "Id",
            v.[Model] AS "Model",
            v.[MechanicId] AS "MechanicId",
            NULL AS "Weight",
            NULL AS "Passengers",
            3 AS "_EntityType_"
        FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
        LEFT OUTER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck] t
            ON t.[TruckId] = v.[VehicleId]
        LEFT OUTER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car] c
            ON c.[CarId] = v.[VehicleId]
        WHERE t.[TruckId] IS NULL
        AND c.[CarId] IS NULL
    )
) _q_',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_GetAll]
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Model] AS "Model",
        _q_.[MechanicId] AS "MechanicId",
        _q_.[Weight] AS "Weight",
        _q_.[Passengers] AS "Passengers",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            t.[TruckId] AS "Id",
            v.[Model] AS "Model",
            v.[MechanicId] AS "MechanicId",
            t.[Weight] AS "Weight",
            NULL AS "Passengers",
            1 AS "_EntityType_"
        FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck] t
        INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
            ON t.[TruckId] = v.[VehicleId]
        UNION ALL
        (
            SELECT
                c.[CarId] AS "Id",
                v.[Model] AS "Model",
                v.[MechanicId] AS "MechanicId",
                NULL AS "Weight",
                c.[Passengers] AS "Passengers",
                2 AS "_EntityType_"
            FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car] c
            INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
                ON c.[CarId] = v.[VehicleId]
        )
        UNION ALL
        (
            SELECT
                v.[VehicleId] AS "Id",
                v.[Model] AS "Model",
                v.[MechanicId] AS "MechanicId",
                NULL AS "Weight",
                NULL AS "Passengers",
                3 AS "_EntityType_"
            FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
            LEFT OUTER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck] t
                ON t.[TruckId] = v.[VehicleId]
            LEFT OUTER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car] c
                ON c.[CarId] = v.[VehicleId]
            WHERE t.[TruckId] IS NULL
            AND c.[CarId] IS NULL
        )
    ) _q_;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_GetById]
    @vehicleId INT
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Model] AS "Model",
        _q_.[MechanicId] AS "MechanicId",
        _q_.[Weight] AS "Weight",
        _q_.[Passengers] AS "Passengers",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            t.[TruckId] AS "Id",
            v.[Model] AS "Model",
            v.[MechanicId] AS "MechanicId",
            t.[Weight] AS "Weight",
            NULL AS "Passengers",
            1 AS "_EntityType_"
        FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck] t
        INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
            ON t.[TruckId] = v.[VehicleId]
        UNION ALL
        (
            SELECT
                c.[CarId] AS "Id",
                v.[Model] AS "Model",
                v.[MechanicId] AS "MechanicId",
                NULL AS "Weight",
                c.[Passengers] AS "Passengers",
                2 AS "_EntityType_"
            FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car] c
            INNER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
                ON c.[CarId] = v.[VehicleId]
        )
        UNION ALL
        (
            SELECT
                v.[VehicleId] AS "Id",
                v.[Model] AS "Model",
                v.[MechanicId] AS "MechanicId",
                NULL AS "Weight",
                NULL AS "Passengers",
                3 AS "_EntityType_"
            FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle] v
            LEFT OUTER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck] t
                ON t.[TruckId] = v.[VehicleId]
            LEFT OUTER JOIN [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car] c
                ON c.[CarId] = v.[VehicleId]
            WHERE t.[TruckId] IS NULL
            AND c.[CarId] IS NULL
        )
    ) _q_
    WHERE _q_.[Id] = @vehicleId;

END;
GO

CREATE PROCEDURE [pExecuteDynamicQuery]
	@$select NVARCHAR(MAX) = NULL,
	@$filter NVARCHAR(MAX) = NULL,
	@$orderby NVARCHAR(MAX) = NULL,
	@$skip NVARCHAR(10) = NULL,
	@$top NVARCHAR(10) = NULL,
	@selectList NVARCHAR(MAX),
	@from NVARCHAR(MAX),
	@count INT OUTPUT
AS
BEGIN

	DECLARE @sqlCommand NVARCHAR(MAX);
	DECLARE @paramDefinition NVARCHAR(100);

	SET @paramDefinition = N'@cnt INT OUTPUT'

	SET @sqlCommand = 
'
	SELECT
		 @cnt = COUNT(1)
	FROM ' + @from + '
';

	IF @$filter IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	WHERE ' + @$filter;
	END

	SET @sqlCommand = @sqlCommand + 
'
	SELECT
	';

	IF ISNULL(@$select, '*') = '*'
	BEGIN
		SET @sqlCommand = @sqlCommand + @selectList;
	END
	ELSE
	BEGIN
		SET @sqlCommand = @sqlCommand + @$select;
	END

	SET @sqlCommand = @sqlCommand +
'
	FROM ' + @from + '
';

	IF @$filter IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	WHERE ' + @$filter;
	END

	IF @$orderby IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	ORDER BY ' + @$orderby;
	END
	ELSE
	BEGIN

	-- At least a dummy order by is required is $skip and $top are provided
		IF @$skip IS NOT NULL OR @$top IS NOT NULL
		BEGIN  
			SET @sqlCommand = @sqlCommand + 
' 
	ORDER BY 1 ASC';
		END
	END

	IF @$skip IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	OFFSET ' + @$skip + ' ROWS';
	END

	IF @$top IS NOT NULL
	BEGIN 
		SET @sqlCommand = @sqlCommand + 
' 
	FETCH NEXT ' + @$top + ' ROWS ONLY';
	END

	EXECUTE sp_executesql @sqlCommand, @paramDefinition, @cnt = @count OUTPUT

END;
GO

