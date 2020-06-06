
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'EmployeePerson'
)
BEGIN
    DROP DATABASE [EmployeePerson]
END
GO

CREATE DATABASE [EmployeePerson]
GO

USE [EmployeePerson]
GO

CREATE SCHEMA [EmployeeBoundedContext];
GO

CREATE TABLE [EmployeePerson].[EmployeeBoundedContext].[Employee]
(
    [EmployeeId] INT NOT NULL,
    [HireDate] DATETIME NOT NULL
    CONSTRAINT Employee_PK PRIMARY KEY ([EmployeeId])
);
GO

CREATE TABLE [EmployeePerson].[EmployeeBoundedContext].[Person]
(
    [PersonId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [Gender] CHAR(1) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME,
    [CellPhone_AreaCode] VARCHAR(3) NOT NULL,
    [CellPhone_Exchange] VARCHAR(3) NOT NULL,
    [CellPhone_Number] VARCHAR(4) NOT NULL
    CONSTRAINT Person_PK PRIMARY KEY ([PersonId])
);
GO

CREATE TABLE [EmployeePerson].[EmployeeBoundedContext].[GenderLov]
(
    [Value] CHAR(1),
    [Description] VARCHAR(6)
    CONSTRAINT GenderLov_PK PRIMARY KEY ([Value])
);
GO

ALTER TABLE [EmployeePerson].[EmployeeBoundedContext].[Employee]
    ADD CONSTRAINT Employee_Person_IFK FOREIGN KEY ([EmployeeId])
        REFERENCES [EmployeePerson].[EmployeeBoundedContext].[Person] ([PersonId])
        ON UPDATE NO ACTION
        ON DELETE CASCADE;
GO

CREATE INDEX [Employee_Person_IFK_IX]
    ON [EmployeePerson].[EmployeeBoundedContext].[Employee]
    (
        [EmployeeId]
    );
GO

ALTER TABLE [EmployeePerson].[EmployeeBoundedContext].[Person]
    ADD CONSTRAINT GenderLov_Person_FK FOREIGN KEY ([Gender])
        REFERENCES [EmployeePerson].[EmployeeBoundedContext].[GenderLov] ([Value])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Person_Gender_IX]
    ON [EmployeePerson].[EmployeeBoundedContext].[Person]
    (
        [Gender]
    );
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pEmployee_Delete]
    @employeeId INT
AS
BEGIN
    DELETE FROM [EmployeePerson].[EmployeeBoundedContext].[Employee]
    WHERE [EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pEmployee_Insert]
    @hireDate DATETIME,
    @name VARCHAR(50),
    @gender CHAR(1),
    @createdBy INT,
    @cellPhone_AreaCode VARCHAR(3),
    @cellPhone_Exchange VARCHAR(3),
    @cellPhone_Number VARCHAR(4)
AS
BEGIN
    DECLARE @personId INT;

    DECLARE @personOutputData TABLE
    (
        [PersonId] INT
    );

    INSERT INTO [EmployeePerson].[EmployeeBoundedContext].[Person]
    (
        [Name],
        [Gender],
        [CreatedBy],
        [CellPhone_AreaCode],
        [CellPhone_Exchange],
        [CellPhone_Number]
    )
    OUTPUT
        INSERTED.[PersonId]
        INTO @personOutputData
    VALUES
    (
        @name,
        @gender,
        @createdBy,
        @cellPhone_AreaCode,
        @cellPhone_Exchange,
        @cellPhone_Number
    );

    SELECT
        @personId = [PersonId]
    FROM @personOutputData;

    INSERT INTO [EmployeePerson].[EmployeeBoundedContext].[Employee]
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

CREATE PROCEDURE [EmployeeBoundedContext].[pEmployee_Update]
    @employeeId INT,
    @hireDate DATETIME,
    @name VARCHAR(50),
    @gender CHAR(1),
    @updatedBy INT = NULL,
    @cellPhone_AreaCode VARCHAR(3),
    @cellPhone_Exchange VARCHAR(3),
    @cellPhone_Number VARCHAR(4)
AS
BEGIN
    UPDATE [EmployeePerson].[EmployeeBoundedContext].[Person]
    SET
        [Name] = @name,
        [Gender] = @gender,
        [UpdatedBy] = @updatedBy,
        [CellPhone_AreaCode] = @cellPhone_AreaCode,
        [CellPhone_Exchange] = @cellPhone_Exchange,
        [CellPhone_Number] = @cellPhone_Number,
        [UpdatedDateTime] = GETDATE()
    WHERE [PersonId] = @employeeId;

    UPDATE [EmployeePerson].[EmployeeBoundedContext].[Employee]
    SET
        [HireDate] = @hireDate
    WHERE [EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_Delete]
    @personId INT
AS
BEGIN
    DELETE FROM [EmployeePerson].[EmployeeBoundedContext].[Person]
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_Insert]
    @name VARCHAR(50),
    @gender CHAR(1),
    @createdBy INT,
    @cellPhone_AreaCode VARCHAR(3),
    @cellPhone_Exchange VARCHAR(3),
    @cellPhone_Number VARCHAR(4)
AS
BEGIN
    DECLARE @personOutputData TABLE
    (
        [PersonId] INT
    );

    INSERT INTO [EmployeePerson].[EmployeeBoundedContext].[Person]
    (
        [Name],
        [Gender],
        [CreatedBy],
        [CellPhone_AreaCode],
        [CellPhone_Exchange],
        [CellPhone_Number]
    )
    OUTPUT
        INSERTED.[PersonId]
        INTO @personOutputData
    VALUES
    (
        @name,
        @gender,
        @createdBy,
        @cellPhone_AreaCode,
        @cellPhone_Exchange,
        @cellPhone_Number
    );

    SELECT
        [PersonId]
    FROM @personOutputData;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_Update]
    @personId INT,
    @name VARCHAR(50),
    @gender CHAR(1),
    @updatedBy INT = NULL,
    @cellPhone_AreaCode VARCHAR(3),
    @cellPhone_Exchange VARCHAR(3),
    @cellPhone_Number VARCHAR(4)
AS
BEGIN
    UPDATE [EmployeePerson].[EmployeeBoundedContext].[Person]
    SET
        [Name] = @name,
        [Gender] = @gender,
        [UpdatedBy] = @updatedBy,
        [CellPhone_AreaCode] = @cellPhone_AreaCode,
        [CellPhone_Exchange] = @cellPhone_Exchange,
        [CellPhone_Number] = @cellPhone_Number,
        [UpdatedDateTime] = GETDATE()
    WHERE [PersonId] = @personId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pEmployee_Get]
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
        @selectList = N'    e.[EmployeeId] AS "Id",
    p.[Name] AS "Name",
    p.[Gender] AS "Gender",
    p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
    p.[CellPhone_Exchange] AS "CellPhone.Exchange",
    p.[CellPhone_Number] AS "CellPhone.Number",
    e.[HireDate] AS "HireDate"',
        @from = N'[EmployeePerson].[EmployeeBoundedContext].[Employee] e
INNER JOIN [EmployeePerson].[EmployeeBoundedContext].[Person] p
    ON e.[EmployeeId] = p.[PersonId]',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pEmployee_GetAll]
AS
BEGIN
    SELECT
        e.[EmployeeId] AS "Id",
        p.[Name] AS "Name",
        p.[Gender] AS "Gender",
        p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
        p.[CellPhone_Exchange] AS "CellPhone.Exchange",
        p.[CellPhone_Number] AS "CellPhone.Number",
        e.[HireDate] AS "HireDate"
    FROM [EmployeePerson].[EmployeeBoundedContext].[Employee] e
    INNER JOIN [EmployeePerson].[EmployeeBoundedContext].[Person] p
        ON e.[EmployeeId] = p.[PersonId];

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pEmployee_GetById]
    @employeeId INT
AS
BEGIN
    SELECT
        e.[EmployeeId] AS "Id",
        p.[Name] AS "Name",
        p.[Gender] AS "Gender",
        p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
        p.[CellPhone_Exchange] AS "CellPhone.Exchange",
        p.[CellPhone_Number] AS "CellPhone.Number",
        e.[HireDate] AS "HireDate"
    FROM [EmployeePerson].[EmployeeBoundedContext].[Employee] e
    INNER JOIN [EmployeePerson].[EmployeeBoundedContext].[Person] p
        ON e.[EmployeeId] = p.[PersonId]
    WHERE e.[EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_Get]
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
    _q_.[Gender] AS "Gender",
    _q_.[CellPhone.AreaCode] AS "CellPhone.AreaCode",
    _q_.[CellPhone.Exchange] AS "CellPhone.Exchange",
    _q_.[CellPhone.Number] AS "CellPhone.Number",
    _q_.[HireDate] AS "HireDate",
    _q_.[_EntityType_] AS "_EntityType_"',
        @from = N'
(
    SELECT
        e.[EmployeeId] AS "Id",
        p.[Name] AS "Name",
        p.[Gender] AS "Gender",
        p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
        p.[CellPhone_Exchange] AS "CellPhone.Exchange",
        p.[CellPhone_Number] AS "CellPhone.Number",
        e.[HireDate] AS "HireDate",
        1 AS "_EntityType_"
    FROM [EmployeePerson].[EmployeeBoundedContext].[Employee] e
    INNER JOIN [EmployeePerson].[EmployeeBoundedContext].[Person] p
        ON e.[EmployeeId] = p.[PersonId]
    UNION ALL
    (
        SELECT
            p.[PersonId] AS "Id",
            p.[Name] AS "Name",
            p.[Gender] AS "Gender",
            p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
            p.[CellPhone_Exchange] AS "CellPhone.Exchange",
            p.[CellPhone_Number] AS "CellPhone.Number",
            NULL AS "HireDate",
            2 AS "_EntityType_"
        FROM [EmployeePerson].[EmployeeBoundedContext].[Person] p
        LEFT OUTER JOIN [EmployeePerson].[EmployeeBoundedContext].[Employee] e
            ON e.[EmployeeId] = p.[PersonId]
        WHERE e.[EmployeeId] IS NULL
    )
) _q_',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_GetAll]
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Name] AS "Name",
        _q_.[Gender] AS "Gender",
        _q_.[CellPhone.AreaCode] AS "CellPhone.AreaCode",
        _q_.[CellPhone.Exchange] AS "CellPhone.Exchange",
        _q_.[CellPhone.Number] AS "CellPhone.Number",
        _q_.[HireDate] AS "HireDate",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            e.[EmployeeId] AS "Id",
            p.[Name] AS "Name",
            p.[Gender] AS "Gender",
            p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
            p.[CellPhone_Exchange] AS "CellPhone.Exchange",
            p.[CellPhone_Number] AS "CellPhone.Number",
            e.[HireDate] AS "HireDate",
            1 AS "_EntityType_"
        FROM [EmployeePerson].[EmployeeBoundedContext].[Employee] e
        INNER JOIN [EmployeePerson].[EmployeeBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        UNION ALL
        (
            SELECT
                p.[PersonId] AS "Id",
                p.[Name] AS "Name",
                p.[Gender] AS "Gender",
                p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
                p.[CellPhone_Exchange] AS "CellPhone.Exchange",
                p.[CellPhone_Number] AS "CellPhone.Number",
                NULL AS "HireDate",
                2 AS "_EntityType_"
            FROM [EmployeePerson].[EmployeeBoundedContext].[Person] p
            LEFT OUTER JOIN [EmployeePerson].[EmployeeBoundedContext].[Employee] e
                ON e.[EmployeeId] = p.[PersonId]
            WHERE e.[EmployeeId] IS NULL
        )
    ) _q_;

END;
GO

CREATE PROCEDURE [EmployeeBoundedContext].[pPerson_GetById]
    @personId INT
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Name] AS "Name",
        _q_.[Gender] AS "Gender",
        _q_.[CellPhone.AreaCode] AS "CellPhone.AreaCode",
        _q_.[CellPhone.Exchange] AS "CellPhone.Exchange",
        _q_.[CellPhone.Number] AS "CellPhone.Number",
        _q_.[HireDate] AS "HireDate",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            e.[EmployeeId] AS "Id",
            p.[Name] AS "Name",
            p.[Gender] AS "Gender",
            p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
            p.[CellPhone_Exchange] AS "CellPhone.Exchange",
            p.[CellPhone_Number] AS "CellPhone.Number",
            e.[HireDate] AS "HireDate",
            1 AS "_EntityType_"
        FROM [EmployeePerson].[EmployeeBoundedContext].[Employee] e
        INNER JOIN [EmployeePerson].[EmployeeBoundedContext].[Person] p
            ON e.[EmployeeId] = p.[PersonId]
        UNION ALL
        (
            SELECT
                p.[PersonId] AS "Id",
                p.[Name] AS "Name",
                p.[Gender] AS "Gender",
                p.[CellPhone_AreaCode] AS "CellPhone.AreaCode",
                p.[CellPhone_Exchange] AS "CellPhone.Exchange",
                p.[CellPhone_Number] AS "CellPhone.Number",
                NULL AS "HireDate",
                2 AS "_EntityType_"
            FROM [EmployeePerson].[EmployeeBoundedContext].[Person] p
            LEFT OUTER JOIN [EmployeePerson].[EmployeeBoundedContext].[Employee] e
                ON e.[EmployeeId] = p.[PersonId]
            WHERE e.[EmployeeId] IS NULL
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

INSERT INTO [EmployeePerson].[EmployeeBoundedContext].[GenderLov]
(
    [Value],
    [Description]
)
VALUES
(
    'M',
    'Male'
);

INSERT INTO [EmployeePerson].[EmployeeBoundedContext].[GenderLov]
(
    [Value],
    [Description]
)
VALUES
(
    'F',
    'Female'
);

