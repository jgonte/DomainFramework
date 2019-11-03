
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
    [MechanicId] INT NOT NULL
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
        ON DELETE NO ACTION;
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
        ON DELETE NO ACTION;
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

CREATE PROCEDURE [GarageBoundedContext].[pCar_DeleteDoors]
    @carId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car_Doors]
    WHERE [CarId] = @carId;

END;
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
    @mechanicId INT
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
    @updatedBy INT,
    @mechanicId INT
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
        @selectList = N'c.[CarId] AS "Id",
        v.[Model] AS "Model",
        v.[MechanicId] AS "MechanicId",
        c.[Passengers] AS "Passengers"',
        @tableName = N'Car c',
        @count = @count OUTPUT

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

CREATE PROCEDURE [GarageBoundedContext].[pMechanic_DeleteVehicles]
    @mechanicId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle]
    WHERE [MechanicId] = @mechanicId;

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
    @updatedBy INT
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
        @selectList = N'm.[MechanicId] AS "Id",
        m.[Name] AS "Name"',
        @tableName = N'Mechanic m',
        @count = @count OUTPUT

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

CREATE PROCEDURE [GarageBoundedContext].[pMechanic_GetMechanic_ForVehicle]
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

CREATE PROCEDURE [GarageBoundedContext].[pTruck_DeleteInspections]
    @truckId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck_Inspections]
    WHERE [TruckId] = @truckId;

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
    @mechanicId INT
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
    @updatedBy INT,
    @mechanicId INT
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
        @selectList = N't.[TruckId] AS "Id",
        v.[Model] AS "Model",
        v.[MechanicId] AS "MechanicId",
        t.[Weight] AS "Weight"',
        @tableName = N'Truck t',
        @count = @count OUTPUT

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

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_DeleteCylinders]
    @vehicleId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle_Cylinders]
    WHERE [VehicleId] = @vehicleId;

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
    @mechanicId INT
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
    @updatedBy INT,
    @mechanicId INT
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
        @selectList = N'_q_.[Id] AS "Id",
        _q_.[Model] AS "Model",
        _q_.[MechanicId] AS "MechanicId",
        _q_.[Weight] AS "Weight",
        _q_.[Passengers] AS "Passengers",
        _q_.[_EntityType_] AS "_EntityType_"',
        @tableName = N'Vehicle _q_',
        @count = @count OUTPUT

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

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_GetVehicles_ForMechanic]
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

CREATE PROCEDURE [GarageBoundedContext].[pDelete_Cylinders_For_Vehicle]
    @vehicleId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle_Cylinders]
    WHERE [VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pInsert_Cylinders_For_Vehicle]
    @vehicleId INT,
    @diameter INT
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

CREATE PROCEDURE [GarageBoundedContext].[pGet_Cylinders_For_Vehicle]
    @vehicleId INT
AS
BEGIN
    SELECT
        [VehicleId] AS "VehicleId",
        [Diameter] AS "Diameter"
    FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Vehicle_Cylinders]
    WHERE [VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pDelete_Inspections_For_Truck]
    @truckId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck_Inspections]
    WHERE [TruckId] = @truckId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pInsert_Inspections_For_Truck]
    @truckId INT,
    @date DATETIME
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

CREATE PROCEDURE [GarageBoundedContext].[pGet_Inspections_For_Truck]
    @truckId INT
AS
BEGIN
    SELECT
        [TruckId] AS "TruckId",
        [Date] AS "Date"
    FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Truck_Inspections]
    WHERE [TruckId] = @truckId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pDelete_Doors_For_Car]
    @carId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car_Doors]
    WHERE [CarId] = @carId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pInsert_Doors_For_Car]
    @carId INT,
    @number INT
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

CREATE PROCEDURE [GarageBoundedContext].[pGet_Doors_For_Car]
    @carId INT
AS
BEGIN
    SELECT
        [CarId] AS "CarId",
        [Number] AS "Number"
    FROM [MechanicServicesSeveralVehicles].[GarageBoundedContext].[Car_Doors]
    WHERE [CarId] = @carId;

END;
GO

CREATE PROCEDURE [pExecuteDynamicQuery]
	@$select NVARCHAR(MAX) = NULL,
	@$filter NVARCHAR(MAX) = NULL,
	@$orderby NVARCHAR(MAX) = NULL,
	@$skip NVARCHAR(10) = NULL,
	@$top NVARCHAR(10) = NULL,
	@selectList NVARCHAR(MAX),
	@tableName NVARCHAR(64),
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
	FROM [' + @tableName + ']
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

	IF @$select = '*'
	BEGIN
		SET @sqlCommand = @sqlCommand + @selectList;
	END
	ELSE
	BEGIN
		SET @sqlCommand = @sqlCommand + @$select;
	END

	SET @sqlCommand = @sqlCommand +
'
	FROM [' + @tableName + '] s
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