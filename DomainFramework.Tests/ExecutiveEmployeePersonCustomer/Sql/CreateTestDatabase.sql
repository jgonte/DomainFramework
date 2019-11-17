
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'ExecutiveEmployeePersonCustomer'
)
BEGIN
    DROP DATABASE [ExecutiveEmployeePersonCustomer]
END
GO

CREATE DATABASE [ExecutiveEmployeePersonCustomer]
GO

USE [ExecutiveEmployeePersonCustomer]
GO

CREATE SCHEMA [ExecutiveBoundedContext];
GO

CREATE TABLE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive]
(
    [ExecutiveId] INT NOT NULL,
    [Bonus] MONEY NOT NULL,
    [Asset_Number] INT
    CONSTRAINT Executive_PK PRIMARY KEY ([ExecutiveId])
);
GO

CREATE TABLE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee]
(
    [EmployeeId] INT NOT NULL,
    [HireDate] DATETIME NOT NULL
    CONSTRAINT Employee_PK PRIMARY KEY ([EmployeeId])
);
GO

CREATE TABLE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person]
(
    [PersonId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME
    CONSTRAINT Person_PK PRIMARY KEY ([PersonId])
);
GO

CREATE TABLE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Customer]
(
    [CustomerId] INT NOT NULL,
    [Rating] INT NOT NULL
    CONSTRAINT Customer_PK PRIMARY KEY ([CustomerId])
);
GO

ALTER TABLE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee]
    ADD CONSTRAINT Employee_Person_IFK FOREIGN KEY ([EmployeeId])
        REFERENCES [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Employee_Person_IFK_IX]
    ON [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee]
    (
        [EmployeeId]
    );
GO

ALTER TABLE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive]
    ADD CONSTRAINT Executive_Employee_IFK FOREIGN KEY ([ExecutiveId])
        REFERENCES [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee] ([EmployeeId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Executive_Employee_IFK_IX]
    ON [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive]
    (
        [ExecutiveId]
    );
GO

ALTER TABLE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Customer]
    ADD CONSTRAINT Customer_Person_IFK FOREIGN KEY ([CustomerId])
        REFERENCES [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Customer_Person_IFK_IX]
    ON [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Customer]
    (
        [CustomerId]
    );
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pCustomer_Delete]
    @customerId INT
AS
BEGIN
    DELETE FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Customer]
    WHERE [CustomerId] = @customerId;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pCustomer_Insert]
    @rating INT,
    @name VARCHAR(50),
    @createdBy INT
AS
BEGIN
    DECLARE @personId INT;

    DECLARE @personOutputData TABLE
    (
        [PersonId] INT
    );

    INSERT INTO [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person]
    (
        [Name],
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[PersonId]
        INTO @personOutputData
    VALUES
    (
        @name,
        @createdBy
    );

    SELECT
        @personId = [PersonId]
    FROM @personOutputData;

    INSERT INTO [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Customer]
    (
        [CustomerId],
        [Rating]
    )
    VALUES
    (
        @personId,
        @rating
    );

    SELECT
        [PersonId]
    FROM @personOutputData;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pCustomer_Update]
    @customerId INT,
    @rating INT,
    @name VARCHAR(50),
    @updatedBy INT
AS
BEGIN
    UPDATE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [PersonId] = @customerId;

    UPDATE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Customer]
    SET
        [Rating] = @rating
    WHERE [CustomerId] = @customerId;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pCustomer_Get]
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
        @selectList = N'    c.[CustomerId] AS "Id",
    p.[Name] AS "Name",
    c.[Rating] AS "Rating"',
        @from = N'[ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Customer] c
INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
    ON c.[CustomerId] = p.[PersonId]',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pCustomer_GetById]
    @customerId INT
AS
BEGIN
    SELECT
        c.[CustomerId] AS "Id",
        p.[Name] AS "Name",
        c.[Rating] AS "Rating"
    FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Customer] c
    INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
        ON c.[CustomerId] = p.[PersonId]
    WHERE c.[CustomerId] = @customerId;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pEmployee_Delete]
    @employeeId INT
AS
BEGIN
    DELETE FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee]
    WHERE [EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pEmployee_Insert]
    @hireDate DATETIME,
    @name VARCHAR(50),
    @createdBy INT
AS
BEGIN
    DECLARE @personId INT;

    DECLARE @personOutputData TABLE
    (
        [PersonId] INT
    );

    INSERT INTO [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person]
    (
        [Name],
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[PersonId]
        INTO @personOutputData
    VALUES
    (
        @name,
        @createdBy
    );

    SELECT
        @personId = [PersonId]
    FROM @personOutputData;

    INSERT INTO [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee]
    (
        [EmployeeId],
        [HireDate]
    )
    VALUES
    (
        @personId,
        @hireDate
    );

    SELECT
        [PersonId]
    FROM @personOutputData;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pEmployee_Update]
    @employeeId INT,
    @hireDate DATETIME,
    @name VARCHAR(50),
    @updatedBy INT
AS
BEGIN
    UPDATE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [PersonId] = @employeeId;

    UPDATE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee]
    SET
        [HireDate] = @hireDate
    WHERE [EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pEmployee_Get]
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
    _q_.[Name] AS "Name",
    _q_.[HireDate] AS "HireDate",
    _q_.[Bonus] AS "Bonus",
    _q_.[Asset.Number] AS "Asset.Number",
    _q_.[_EntityType_] AS "_EntityType_"',
        @from = N'
(
    SELECT
        e1.[ExecutiveId] AS "Id",
        p.[Name] AS "Name",
        e.[HireDate] AS "HireDate",
        e1.[Bonus] AS "Bonus",
        e1.[Asset_Number] AS "Asset.Number",
        1 AS "_EntityType_"
    FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive] e1
    INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee] e
        ON e1.[ExecutiveId] = e.[EmployeeId]
    INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
        ON e.[EmployeeId] = p.[PersonId]
    UNION ALL
    (
        SELECT
            e.[EmployeeId] AS "Id",
            p.[Name] AS "Name",
            e.[HireDate] AS "HireDate",
            NULL AS "Bonus",
            NULL AS "Asset.Number",
            2 AS "_EntityType_"
        FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee] e
        INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        LEFT OUTER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive] e1
            ON e1.[ExecutiveId] = e.[EmployeeId]
        WHERE e1.[ExecutiveId] IS NULL
    )
) _q_',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pEmployee_GetById]
    @employeeId INT
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Name] AS "Name",
        _q_.[HireDate] AS "HireDate",
        _q_.[Bonus] AS "Bonus",
        _q_.[Asset.Number] AS "Asset.Number",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            e1.[ExecutiveId] AS "Id",
            p.[Name] AS "Name",
            e.[HireDate] AS "HireDate",
            e1.[Bonus] AS "Bonus",
            e1.[Asset_Number] AS "Asset.Number",
            1 AS "_EntityType_"
        FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive] e1
        INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee] e
            ON e1.[ExecutiveId] = e.[EmployeeId]
        INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        UNION ALL
        (
            SELECT
                e.[EmployeeId] AS "Id",
                p.[Name] AS "Name",
                e.[HireDate] AS "HireDate",
                NULL AS "Bonus",
                NULL AS "Asset.Number",
                2 AS "_EntityType_"
            FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee] e
            INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
                ON e.[EmployeeId] = p.[PersonId]
            LEFT OUTER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive] e1
                ON e1.[ExecutiveId] = e.[EmployeeId]
            WHERE e1.[ExecutiveId] IS NULL
        )
    ) _q_
    WHERE _q_.[Id] = @employeeId;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pExecutive_Delete]
    @executiveId INT
AS
BEGIN
    DELETE FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive]
    WHERE [ExecutiveId] = @executiveId;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pExecutive_Insert]
    @bonus MONEY,
    @asset_Number INT,
    @hireDate DATETIME,
    @name VARCHAR(50),
    @createdBy INT
AS
BEGIN
    DECLARE @personId INT;

    DECLARE @personOutputData TABLE
    (
        [PersonId] INT
    );

    INSERT INTO [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person]
    (
        [Name],
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[PersonId]
        INTO @personOutputData
    VALUES
    (
        @name,
        @createdBy
    );

    SELECT
        @personId = [PersonId]
    FROM @personOutputData;

    INSERT INTO [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee]
    (
        [EmployeeId],
        [HireDate]
    )
    VALUES
    (
        @personId,
        @hireDate
    );

    INSERT INTO [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive]
    (
        [ExecutiveId],
        [Bonus],
        [Asset_Number]
    )
    VALUES
    (
        @personId,
        @bonus,
        @asset_Number
    );

    SELECT
        [PersonId]
    FROM @personOutputData;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pExecutive_Update]
    @executiveId INT,
    @bonus MONEY,
    @asset_Number INT,
    @hireDate DATETIME,
    @name VARCHAR(50),
    @updatedBy INT
AS
BEGIN
    UPDATE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [PersonId] = @executiveId;

    UPDATE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee]
    SET
        [HireDate] = @hireDate
    WHERE [EmployeeId] = @executiveId;

    UPDATE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive]
    SET
        [Bonus] = @bonus,
        [Asset_Number] = @asset_Number
    WHERE [ExecutiveId] = @executiveId;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pExecutive_Get]
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
        @selectList = N'    e.[ExecutiveId] AS "Id",
    e1.[HireDate] AS "HireDate",
    p.[Name] AS "Name",
    e.[Bonus] AS "Bonus",
    e.[Asset_Number] AS "Asset.Number"',
        @from = N'[ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive] e
INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee] e1
    ON e.[ExecutiveId] = e1.[EmployeeId]
INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
    ON e1.[EmployeeId] = p.[PersonId]',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pExecutive_GetById]
    @executiveId INT
AS
BEGIN
    SELECT
        e.[ExecutiveId] AS "Id",
        e1.[HireDate] AS "HireDate",
        p.[Name] AS "Name",
        e.[Bonus] AS "Bonus",
        e.[Asset_Number] AS "Asset.Number"
    FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive] e
    INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee] e1
        ON e.[ExecutiveId] = e1.[EmployeeId]
    INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
        ON e1.[EmployeeId] = p.[PersonId]
    WHERE e.[ExecutiveId] = @executiveId;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pPerson_Delete]
    @personId INT
AS
BEGIN
    DELETE FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person]
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pPerson_Insert]
    @name VARCHAR(50),
    @createdBy INT
AS
BEGIN
    DECLARE @personOutputData TABLE
    (
        [PersonId] INT
    );

    INSERT INTO [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person]
    (
        [Name],
        [CreatedBy]
    )
    OUTPUT
        INSERTED.[PersonId]
        INTO @personOutputData
    VALUES
    (
        @name,
        @createdBy
    );

    SELECT
        [PersonId]
    FROM @personOutputData;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pPerson_Update]
    @personId INT,
    @name VARCHAR(50),
    @updatedBy INT
AS
BEGIN
    UPDATE [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pPerson_Get]
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
    _q_.[Name] AS "Name",
    _q_.[HireDate] AS "HireDate",
    _q_.[Bonus] AS "Bonus",
    _q_.[Asset.Number] AS "Asset.Number",
    _q_.[Rating] AS "Rating",
    _q_.[_EntityType_] AS "_EntityType_"',
        @from = N'
(
    SELECT
        e.[EmployeeId] AS "Id",
        p.[Name] AS "Name",
        e.[HireDate] AS "HireDate",
        NULL AS "Bonus",
        NULL AS "Asset.Number",
        NULL AS "Rating",
        1 AS "_EntityType_"
    FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee] e
    INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
        ON e.[EmployeeId] = p.[PersonId]
    LEFT OUTER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive] e1
        ON e1.[ExecutiveId] = e.[EmployeeId]
    WHERE e1.[ExecutiveId] IS NULL
    UNION ALL
    (
        SELECT
            e1.[ExecutiveId] AS "Id",
            p.[Name] AS "Name",
            e.[HireDate] AS "HireDate",
            e1.[Bonus] AS "Bonus",
            e1.[Asset_Number] AS "Asset.Number",
            NULL AS "Rating",
            2 AS "_EntityType_"
        FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive] e1
        INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee] e
            ON e1.[ExecutiveId] = e.[EmployeeId]
        INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
    )
    UNION ALL
    (
        SELECT
            c.[CustomerId] AS "Id",
            p.[Name] AS "Name",
            NULL AS "HireDate",
            NULL AS "Bonus",
            NULL AS "Asset.Number",
            c.[Rating] AS "Rating",
            3 AS "_EntityType_"
        FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Customer] c
        INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
            ON c.[CustomerId] = p.[PersonId]
    )
    UNION ALL
    (
        SELECT
            p.[PersonId] AS "Id",
            p.[Name] AS "Name",
            NULL AS "HireDate",
            NULL AS "Bonus",
            NULL AS "Asset.Number",
            NULL AS "Rating",
            4 AS "_EntityType_"
        FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
        LEFT OUTER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee] e
            ON e.[EmployeeId] = p.[PersonId]
        LEFT OUTER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive] e1
            ON e1.[ExecutiveId] = e.[EmployeeId]
        LEFT OUTER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Customer] c
            ON c.[CustomerId] = p.[PersonId]
        WHERE e.[EmployeeId] IS NULL
        AND e1.[ExecutiveId] IS NULL
        AND c.[CustomerId] IS NULL
    )
) _q_',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pPerson_GetById]
    @personId INT
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Name] AS "Name",
        _q_.[HireDate] AS "HireDate",
        _q_.[Bonus] AS "Bonus",
        _q_.[Asset.Number] AS "Asset.Number",
        _q_.[Rating] AS "Rating",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            e.[EmployeeId] AS "Id",
            p.[Name] AS "Name",
            e.[HireDate] AS "HireDate",
            NULL AS "Bonus",
            NULL AS "Asset.Number",
            NULL AS "Rating",
            1 AS "_EntityType_"
        FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee] e
        INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        LEFT OUTER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive] e1
            ON e1.[ExecutiveId] = e.[EmployeeId]
        WHERE e1.[ExecutiveId] IS NULL
        UNION ALL
        (
            SELECT
                e1.[ExecutiveId] AS "Id",
                p.[Name] AS "Name",
                e.[HireDate] AS "HireDate",
                e1.[Bonus] AS "Bonus",
                e1.[Asset_Number] AS "Asset.Number",
                NULL AS "Rating",
                2 AS "_EntityType_"
            FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive] e1
            INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee] e
                ON e1.[ExecutiveId] = e.[EmployeeId]
            INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
                ON e.[EmployeeId] = p.[PersonId]
        )
        UNION ALL
        (
            SELECT
                c.[CustomerId] AS "Id",
                p.[Name] AS "Name",
                NULL AS "HireDate",
                NULL AS "Bonus",
                NULL AS "Asset.Number",
                c.[Rating] AS "Rating",
                3 AS "_EntityType_"
            FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Customer] c
            INNER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
                ON c.[CustomerId] = p.[PersonId]
        )
        UNION ALL
        (
            SELECT
                p.[PersonId] AS "Id",
                p.[Name] AS "Name",
                NULL AS "HireDate",
                NULL AS "Bonus",
                NULL AS "Asset.Number",
                NULL AS "Rating",
                4 AS "_EntityType_"
            FROM [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Person] p
            LEFT OUTER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Employee] e
                ON e.[EmployeeId] = p.[PersonId]
            LEFT OUTER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Executive] e1
                ON e1.[ExecutiveId] = e.[EmployeeId]
            LEFT OUTER JOIN [ExecutiveEmployeePersonCustomer].[ExecutiveBoundedContext].[Customer] c
                ON c.[CustomerId] = p.[PersonId]
            WHERE e.[EmployeeId] IS NULL
            AND e1.[ExecutiveId] IS NULL
            AND c.[CustomerId] IS NULL
        )
    ) _q_
    WHERE _q_.[Id] = @personId;

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

