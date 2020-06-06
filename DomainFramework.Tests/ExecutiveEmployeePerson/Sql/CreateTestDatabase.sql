
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'ExecutiveEmployeePerson'
)
BEGIN
    DROP DATABASE [ExecutiveEmployeePerson]
END
GO

CREATE DATABASE [ExecutiveEmployeePerson]
GO

USE [ExecutiveEmployeePerson]
GO

CREATE SCHEMA [ExecutiveBoundedContext];
GO

CREATE TABLE [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive]
(
    [ExecutiveId] INT NOT NULL,
    [Bonus] MONEY NOT NULL
    CONSTRAINT Executive_PK PRIMARY KEY ([ExecutiveId])
);
GO

CREATE TABLE [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee]
(
    [EmployeeId] INT NOT NULL,
    [HireDate] DATETIME NOT NULL
    CONSTRAINT Employee_PK PRIMARY KEY ([EmployeeId])
);
GO

CREATE TABLE [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person]
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

ALTER TABLE [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee]
    ADD CONSTRAINT Employee_Person_IFK FOREIGN KEY ([EmployeeId])
        REFERENCES [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [Employee_Person_IFK_IX]
    ON [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee]
    (
        [EmployeeId]
    );
GO

ALTER TABLE [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive]
    ADD CONSTRAINT Executive_Employee_IFK FOREIGN KEY ([ExecutiveId])
        REFERENCES [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] ([EmployeeId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [Executive_Employee_IFK_IX]
    ON [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive]
    (
        [ExecutiveId]
    );
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pEmployee_Delete]
    @employeeId INT
AS
BEGIN
    DELETE FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee]
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

    INSERT INTO [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person]
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

    INSERT INTO [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee]
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
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [PersonId] = @employeeId;

    UPDATE [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee]
    SET
        [HireDate] = @hireDate
    WHERE [EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pExecutive_Delete]
    @executiveId INT
AS
BEGIN
    DELETE FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive]
    WHERE [ExecutiveId] = @executiveId;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pExecutive_Insert]
    @bonus MONEY,
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

    INSERT INTO [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person]
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

    INSERT INTO [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee]
    (
        [EmployeeId],
        [HireDate]
    )
    VALUES
    (
        @personId,
        @hireDate
    );

    INSERT INTO [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive]
    (
        [ExecutiveId],
        [Bonus]
    )
    VALUES
    (
        @personId,
        @bonus
    );

    SELECT
        [PersonId]
    FROM @personOutputData;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pExecutive_Update]
    @executiveId INT,
    @bonus MONEY,
    @hireDate DATETIME,
    @name VARCHAR(50),
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [PersonId] = @executiveId;

    UPDATE [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee]
    SET
        [HireDate] = @hireDate
    WHERE [EmployeeId] = @executiveId;

    UPDATE [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive]
    SET
        [Bonus] = @bonus
    WHERE [ExecutiveId] = @executiveId;

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pPerson_Delete]
    @personId INT
AS
BEGIN
    DELETE FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person]
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

    INSERT INTO [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person]
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
    @updatedBy INT = NULL
AS
BEGIN
    UPDATE [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [UpdatedDateTime] = GETDATE()
    WHERE [PersonId] = @personId;

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
    _q_.[_EntityType_] AS "_EntityType_"',
        @from = N'
(
    SELECT
        e1.[ExecutiveId] AS "Id",
        p.[Name] AS "Name",
        e.[HireDate] AS "HireDate",
        e1.[Bonus] AS "Bonus",
        1 AS "_EntityType_"
    FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
    INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
        ON e1.[ExecutiveId] = e.[EmployeeId]
    INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
        ON e.[EmployeeId] = p.[PersonId]
    UNION ALL
    (
        SELECT
            e.[EmployeeId] AS "Id",
            p.[Name] AS "Name",
            e.[HireDate] AS "HireDate",
            NULL AS "Bonus",
            2 AS "_EntityType_"
        FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
        INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        LEFT OUTER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
            ON e1.[ExecutiveId] = e.[EmployeeId]
        WHERE e1.[ExecutiveId] IS NULL
    )
) _q_',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pEmployee_GetAll]
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Name] AS "Name",
        _q_.[HireDate] AS "HireDate",
        _q_.[Bonus] AS "Bonus",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            e1.[ExecutiveId] AS "Id",
            p.[Name] AS "Name",
            e.[HireDate] AS "HireDate",
            e1.[Bonus] AS "Bonus",
            1 AS "_EntityType_"
        FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
        INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
            ON e1.[ExecutiveId] = e.[EmployeeId]
        INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        UNION ALL
        (
            SELECT
                e.[EmployeeId] AS "Id",
                p.[Name] AS "Name",
                e.[HireDate] AS "HireDate",
                NULL AS "Bonus",
                2 AS "_EntityType_"
            FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
            INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
                ON e.[EmployeeId] = p.[PersonId]
            LEFT OUTER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
                ON e1.[ExecutiveId] = e.[EmployeeId]
            WHERE e1.[ExecutiveId] IS NULL
        )
    ) _q_;

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
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            e1.[ExecutiveId] AS "Id",
            p.[Name] AS "Name",
            e.[HireDate] AS "HireDate",
            e1.[Bonus] AS "Bonus",
            1 AS "_EntityType_"
        FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
        INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
            ON e1.[ExecutiveId] = e.[EmployeeId]
        INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        UNION ALL
        (
            SELECT
                e.[EmployeeId] AS "Id",
                p.[Name] AS "Name",
                e.[HireDate] AS "HireDate",
                NULL AS "Bonus",
                2 AS "_EntityType_"
            FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
            INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
                ON e.[EmployeeId] = p.[PersonId]
            LEFT OUTER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
                ON e1.[ExecutiveId] = e.[EmployeeId]
            WHERE e1.[ExecutiveId] IS NULL
        )
    ) _q_
    WHERE _q_.[Id] = @employeeId;

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
    e.[Bonus] AS "Bonus"',
        @from = N'[ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e
INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e1
    ON e.[ExecutiveId] = e1.[EmployeeId]
INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
    ON e1.[EmployeeId] = p.[PersonId]',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pExecutive_GetAll]
AS
BEGIN
    SELECT
        e.[ExecutiveId] AS "Id",
        e1.[HireDate] AS "HireDate",
        p.[Name] AS "Name",
        e.[Bonus] AS "Bonus"
    FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e
    INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e1
        ON e.[ExecutiveId] = e1.[EmployeeId]
    INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
        ON e1.[EmployeeId] = p.[PersonId];

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
        e.[Bonus] AS "Bonus"
    FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e
    INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e1
        ON e.[ExecutiveId] = e1.[EmployeeId]
    INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
        ON e1.[EmployeeId] = p.[PersonId]
    WHERE e.[ExecutiveId] = @executiveId;

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
    _q_.[_EntityType_] AS "_EntityType_"',
        @from = N'
(
    SELECT
        e.[EmployeeId] AS "Id",
        p.[Name] AS "Name",
        e.[HireDate] AS "HireDate",
        NULL AS "Bonus",
        1 AS "_EntityType_"
    FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
    INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
        ON e.[EmployeeId] = p.[PersonId]
    LEFT OUTER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
        ON e1.[ExecutiveId] = e.[EmployeeId]
    WHERE e1.[ExecutiveId] IS NULL
    UNION ALL
    (
        SELECT
            e1.[ExecutiveId] AS "Id",
            p.[Name] AS "Name",
            e.[HireDate] AS "HireDate",
            e1.[Bonus] AS "Bonus",
            2 AS "_EntityType_"
        FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
        INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
            ON e1.[ExecutiveId] = e.[EmployeeId]
        INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
    )
    UNION ALL
    (
        SELECT
            p.[PersonId] AS "Id",
            p.[Name] AS "Name",
            NULL AS "HireDate",
            NULL AS "Bonus",
            3 AS "_EntityType_"
        FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
        LEFT OUTER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
            ON e.[EmployeeId] = p.[PersonId]
        LEFT OUTER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
            ON e1.[ExecutiveId] = e.[EmployeeId]
        WHERE e.[EmployeeId] IS NULL
        AND e1.[ExecutiveId] IS NULL
    )
) _q_',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [ExecutiveBoundedContext].[pPerson_GetAll]
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Name] AS "Name",
        _q_.[HireDate] AS "HireDate",
        _q_.[Bonus] AS "Bonus",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            e.[EmployeeId] AS "Id",
            p.[Name] AS "Name",
            e.[HireDate] AS "HireDate",
            NULL AS "Bonus",
            1 AS "_EntityType_"
        FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
        INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        LEFT OUTER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
            ON e1.[ExecutiveId] = e.[EmployeeId]
        WHERE e1.[ExecutiveId] IS NULL
        UNION ALL
        (
            SELECT
                e1.[ExecutiveId] AS "Id",
                p.[Name] AS "Name",
                e.[HireDate] AS "HireDate",
                e1.[Bonus] AS "Bonus",
                2 AS "_EntityType_"
            FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
            INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
                ON e1.[ExecutiveId] = e.[EmployeeId]
            INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
                ON e.[EmployeeId] = p.[PersonId]
        )
        UNION ALL
        (
            SELECT
                p.[PersonId] AS "Id",
                p.[Name] AS "Name",
                NULL AS "HireDate",
                NULL AS "Bonus",
                3 AS "_EntityType_"
            FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
            LEFT OUTER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
                ON e.[EmployeeId] = p.[PersonId]
            LEFT OUTER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
                ON e1.[ExecutiveId] = e.[EmployeeId]
            WHERE e.[EmployeeId] IS NULL
            AND e1.[ExecutiveId] IS NULL
        )
    ) _q_;

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
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            e.[EmployeeId] AS "Id",
            p.[Name] AS "Name",
            e.[HireDate] AS "HireDate",
            NULL AS "Bonus",
            1 AS "_EntityType_"
        FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
        INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        LEFT OUTER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
            ON e1.[ExecutiveId] = e.[EmployeeId]
        WHERE e1.[ExecutiveId] IS NULL
        UNION ALL
        (
            SELECT
                e1.[ExecutiveId] AS "Id",
                p.[Name] AS "Name",
                e.[HireDate] AS "HireDate",
                e1.[Bonus] AS "Bonus",
                2 AS "_EntityType_"
            FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
            INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
                ON e1.[ExecutiveId] = e.[EmployeeId]
            INNER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
                ON e.[EmployeeId] = p.[PersonId]
        )
        UNION ALL
        (
            SELECT
                p.[PersonId] AS "Id",
                p.[Name] AS "Name",
                NULL AS "HireDate",
                NULL AS "Bonus",
                3 AS "_EntityType_"
            FROM [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Person] p
            LEFT OUTER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Employee] e
                ON e.[EmployeeId] = p.[PersonId]
            LEFT OUTER JOIN [ExecutiveEmployeePerson].[ExecutiveBoundedContext].[Executive] e1
                ON e1.[ExecutiveId] = e.[EmployeeId]
            WHERE e.[EmployeeId] IS NULL
            AND e1.[ExecutiveId] IS NULL
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

