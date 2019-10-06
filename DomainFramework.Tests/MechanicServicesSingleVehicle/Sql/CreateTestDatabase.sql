
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'MechanicServicesSingleVehicle'
)
BEGIN
    DROP DATABASE [MechanicServicesSingleVehicle]
END
GO

CREATE DATABASE [MechanicServicesSingleVehicle]
GO

USE [MechanicServicesSingleVehicle]
GO

CREATE SCHEMA [GarageBoundedContext];
GO

CREATE TABLE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Mechanic]
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

CREATE TABLE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle]
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

CREATE TABLE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle_Cylinders]
(
    [VehicleId] INT NOT NULL,
    [Diameter] INT
);
GO

CREATE TABLE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck]
(
    [TruckId] INT NOT NULL,
    [Weight] INT NOT NULL
    CONSTRAINT Truck_PK PRIMARY KEY ([TruckId])
);
GO

CREATE TABLE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck_Inspections]
(
    [TruckId] INT NOT NULL,
    [Date] DATETIME
);
GO

CREATE TABLE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car]
(
    [CarId] INT NOT NULL,
    [Passengers] INT NOT NULL
    CONSTRAINT Car_PK PRIMARY KEY ([CarId])
);
GO

CREATE TABLE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car_Doors]
(
    [CarId] INT NOT NULL,
    [Number] INT
);
GO

ALTER TABLE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck]
    ADD CONSTRAINT Truck_Vehicle_IFK FOREIGN KEY ([TruckId])
        REFERENCES [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] ([VehicleId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Truck_Vehicle_IFK_IX]
    ON [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck]
    (
        [TruckId]
    );
GO

ALTER TABLE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car]
    ADD CONSTRAINT Car_Vehicle_IFK FOREIGN KEY ([CarId])
        REFERENCES [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] ([VehicleId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Car_Vehicle_IFK_IX]
    ON [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car]
    (
        [CarId]
    );
GO

ALTER TABLE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle]
    ADD CONSTRAINT Mechanic_Vehicle_FK FOREIGN KEY ([MechanicId])
        REFERENCES [MechanicServicesSingleVehicle].[GarageBoundedContext].[Mechanic] ([MechanicId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Mechanic_Vehicle_FK_IX]
    ON [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle]
    (
        [MechanicId]
    );
GO

ALTER TABLE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle_Cylinders]
    ADD CONSTRAINT Vehicle_Cylinders_FK FOREIGN KEY ([VehicleId])
        REFERENCES [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] ([VehicleId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [Vehicle_Cylinders_FK_IX]
    ON [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle_Cylinders]
    (
        [VehicleId]
    );
GO

ALTER TABLE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck_Inspections]
    ADD CONSTRAINT Truck_Inspections_FK FOREIGN KEY ([TruckId])
        REFERENCES [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck] ([TruckId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [Truck_Inspections_FK_IX]
    ON [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck_Inspections]
    (
        [TruckId]
    );
GO

ALTER TABLE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car_Doors]
    ADD CONSTRAINT Car_Doors_FK FOREIGN KEY ([CarId])
        REFERENCES [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car] ([CarId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [Car_Doors_FK_IX]
    ON [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car_Doors]
    (
        [CarId]
    );
GO

CREATE PROCEDURE [GarageBoundedContext].[pCar_DeleteDoors]
    @carId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car_Doors]
    WHERE [CarId] = @carId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pCar_Delete]
    @carId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car]
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

    INSERT INTO [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle]
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

    INSERT INTO [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car]
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
    UPDATE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle]
    SET
        [Model] = @model,
        [UpdatedBy] = @updatedBy,
        [MechanicId] = @mechanicId,
        [UpdatedDateTime] = GETDATE()
    WHERE [VehicleId] = @carId;

    UPDATE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car]
    SET
        [Passengers] = @passengers
    WHERE [CarId] = @carId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pCar_Get]
AS
BEGIN
    SELECT
        c.[CarId] AS "Id",
        v.[Model] AS "Model",
        v.[MechanicId] AS "MechanicId",
        c.[Passengers] AS "Passengers"
    FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car] c
    INNER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] v
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
    FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car] c
    INNER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] v
        ON c.[CarId] = v.[VehicleId]
    WHERE c.[CarId] = @carId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pMechanic_DeleteVehicle]
    @mechanicId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle]
    WHERE [MechanicId] = @mechanicId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pMechanic_Delete]
    @mechanicId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Mechanic]
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

    INSERT INTO [MechanicServicesSingleVehicle].[GarageBoundedContext].[Mechanic]
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
    UPDATE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Mechanic]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [MechanicId] = @mechanicId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pMechanic_Get]
AS
BEGIN
    SELECT
        m.[MechanicId] AS "Id",
        m.[Name] AS "Name"
    FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Mechanic] m;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pMechanic_GetById]
    @mechanicId INT
AS
BEGIN
    SELECT
        m.[MechanicId] AS "Id",
        m.[Name] AS "Name"
    FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Mechanic] m
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
    FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Mechanic] m
    INNER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] v
        ON m.[MechanicId] = v.[MechanicId]
    WHERE v.[VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pTruck_DeleteInspections]
    @truckId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck_Inspections]
    WHERE [TruckId] = @truckId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pTruck_Delete]
    @truckId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck]
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

    INSERT INTO [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle]
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

    INSERT INTO [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck]
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
    UPDATE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle]
    SET
        [Model] = @model,
        [UpdatedBy] = @updatedBy,
        [MechanicId] = @mechanicId,
        [UpdatedDateTime] = GETDATE()
    WHERE [VehicleId] = @truckId;

    UPDATE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck]
    SET
        [Weight] = @weight
    WHERE [TruckId] = @truckId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pTruck_Get]
AS
BEGIN
    SELECT
        t.[TruckId] AS "Id",
        v.[Model] AS "Model",
        v.[MechanicId] AS "MechanicId",
        t.[Weight] AS "Weight"
    FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck] t
    INNER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] v
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
    FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck] t
    INNER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] v
        ON t.[TruckId] = v.[VehicleId]
    WHERE t.[TruckId] = @truckId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_DeleteCylinders]
    @vehicleId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle_Cylinders]
    WHERE [VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_Delete]
    @vehicleId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle]
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

    INSERT INTO [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle]
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
    UPDATE [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle]
    SET
        [Model] = @model,
        [UpdatedBy] = @updatedBy,
        [MechanicId] = @mechanicId,
        [UpdatedDateTime] = GETDATE()
    WHERE [VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_Get]
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
        FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck] t
        INNER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] v
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
            FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car] c
            INNER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] v
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
            FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] v
            LEFT OUTER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck] t
                ON t.[TruckId] = v.[VehicleId]
            LEFT OUTER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car] c
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
        FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck] t
        INNER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] v
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
            FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car] c
            INNER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] v
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
            FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] v
            LEFT OUTER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck] t
                ON t.[TruckId] = v.[VehicleId]
            LEFT OUTER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car] c
                ON c.[CarId] = v.[VehicleId]
            WHERE t.[TruckId] IS NULL
            AND c.[CarId] IS NULL
        )
    ) _q_
    WHERE _q_.[Id] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pVehicle_GetVehicle_ForMechanic]
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
        FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck] t
        INNER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] v
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
            FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car] c
            INNER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] v
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
            FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle] v
            LEFT OUTER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck] t
                ON t.[TruckId] = v.[VehicleId]
            LEFT OUTER JOIN [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car] c
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
    DELETE FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle_Cylinders]
    WHERE [VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pInsert_Cylinders_For_Vehicle]
    @vehicleId INT,
    @diameter INT
AS
BEGIN
    INSERT INTO [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle_Cylinders]
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
    FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Vehicle_Cylinders]
    WHERE [VehicleId] = @vehicleId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pDelete_Inspections_For_Truck]
    @truckId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck_Inspections]
    WHERE [TruckId] = @truckId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pInsert_Inspections_For_Truck]
    @truckId INT,
    @date DATETIME
AS
BEGIN
    INSERT INTO [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck_Inspections]
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
    FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Truck_Inspections]
    WHERE [TruckId] = @truckId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pDelete_Doors_For_Car]
    @carId INT
AS
BEGIN
    DELETE FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car_Doors]
    WHERE [CarId] = @carId;

END;
GO

CREATE PROCEDURE [GarageBoundedContext].[pInsert_Doors_For_Car]
    @carId INT,
    @number INT
AS
BEGIN
    INSERT INTO [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car_Doors]
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
    FROM [MechanicServicesSingleVehicle].[GarageBoundedContext].[Car_Doors]
    WHERE [CarId] = @carId;

END;
GO

