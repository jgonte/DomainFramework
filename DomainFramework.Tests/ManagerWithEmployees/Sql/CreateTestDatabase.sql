
USE master
GO

IF EXISTS
(
    SELECT NAME
    FROM Sys.Databases
    WHERE Name = N'ManagerWithEmployees'
)
BEGIN
    DROP DATABASE [ManagerWithEmployees]
END
GO

CREATE DATABASE [ManagerWithEmployees]
GO

USE [ManagerWithEmployees]
GO

CREATE SCHEMA [ManagerBoundedContext];
GO

CREATE TABLE [ManagerWithEmployees].[ManagerBoundedContext].[Manager]
(
    [ManagerId] INT NOT NULL,
    [Department] VARCHAR(50) NOT NULL
    CONSTRAINT Manager_PK PRIMARY KEY ([ManagerId])
);
GO

CREATE TABLE [ManagerWithEmployees].[ManagerBoundedContext].[Employee]
(
    [EmployeeId] INT NOT NULL IDENTITY,
    [Name] VARCHAR(50) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(),
    [UpdatedBy] INT,
    [UpdatedDateTime] DATETIME,
    [SupervisorId] INT
    CONSTRAINT Employee_PK PRIMARY KEY ([EmployeeId])
);
GO

ALTER TABLE [ManagerWithEmployees].[ManagerBoundedContext].[Manager]
    ADD CONSTRAINT Manager_Employee_IFK FOREIGN KEY ([ManagerId])
        REFERENCES [ManagerWithEmployees].[ManagerBoundedContext].[Employee] ([EmployeeId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Manager_Employee_IFK_IX]
    ON [ManagerWithEmployees].[ManagerBoundedContext].[Manager]
    (
        [ManagerId]
    );
GO

ALTER TABLE [ManagerWithEmployees].[ManagerBoundedContext].[Employee]
    ADD CONSTRAINT Manager_Employees_FK FOREIGN KEY ([SupervisorId])
        REFERENCES [ManagerWithEmployees].[ManagerBoundedContext].[Manager] ([ManagerId])
        ON UPDATE NO ACTION
        ON DELETE NO ACTION;
GO

CREATE INDEX [Manager_Employees_FK_IX]
    ON [ManagerWithEmployees].[ManagerBoundedContext].[Employee]
    (
        [SupervisorId]
    );
GO

CREATE PROCEDURE [ManagerBoundedContext].[pEmployee_Delete]
    @employeeId INT
AS
BEGIN
    DELETE FROM [ManagerWithEmployees].[ManagerBoundedContext].[Employee]
    WHERE [EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [ManagerBoundedContext].[pEmployee_Insert]
    @name VARCHAR(50),
    @createdBy INT,
    @supervisorId INT
AS
BEGIN
    DECLARE @employeeOutputData TABLE
    (
        [EmployeeId] INT
    );

    INSERT INTO [ManagerWithEmployees].[ManagerBoundedContext].[Employee]
    (
        [Name],
        [CreatedBy],
        [SupervisorId]
    )
    OUTPUT
        INSERTED.[EmployeeId]
        INTO @employeeOutputData
    VALUES
    (
        @name,
        @createdBy,
        @supervisorId
    );

    SELECT
        [EmployeeId]
    FROM @employeeOutputData;

END;
GO

CREATE PROCEDURE [ManagerBoundedContext].[pEmployee_Update]
    @employeeId INT,
    @name VARCHAR(50),
    @updatedBy INT,
    @supervisorId INT
AS
BEGIN
    UPDATE [ManagerWithEmployees].[ManagerBoundedContext].[Employee]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [SupervisorId] = @supervisorId,
        [UpdatedDateTime] = GETDATE()
    WHERE [EmployeeId] = @employeeId;

END;
GO

CREATE PROCEDURE [ManagerBoundedContext].[pEmployee_Get]
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
    _q_.[SupervisorId] AS "SupervisorId",
    _q_.[Department] AS "Department",
    _q_.[_EntityType_] AS "_EntityType_"',
        @from = N'
(
    SELECT
        m.[ManagerId] AS "Id",
        e.[Name] AS "Name",
        e.[SupervisorId] AS "SupervisorId",
        m.[Department] AS "Department",
        1 AS "_EntityType_"
    FROM [ManagerWithEmployees].[ManagerBoundedContext].[Manager] m
    INNER JOIN [ManagerWithEmployees].[ManagerBoundedContext].[Employee] e
        ON m.[ManagerId] = e.[EmployeeId]
    UNION ALL
    (
        SELECT
            e.[EmployeeId] AS "Id",
            e.[Name] AS "Name",
            e.[SupervisorId] AS "SupervisorId",
            NULL AS "Department",
            2 AS "_EntityType_"
        FROM [ManagerWithEmployees].[ManagerBoundedContext].[Employee] e
        LEFT OUTER JOIN [ManagerWithEmployees].[ManagerBoundedContext].[Manager] m
            ON m.[ManagerId] = e.[EmployeeId]
        WHERE m.[ManagerId] IS NULL
    )
) _q_',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [ManagerBoundedContext].[pEmployee_GetAll]
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Name] AS "Name",
        _q_.[SupervisorId] AS "SupervisorId",
        _q_.[Department] AS "Department",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            m.[ManagerId] AS "Id",
            e.[Name] AS "Name",
            e.[SupervisorId] AS "SupervisorId",
            m.[Department] AS "Department",
            1 AS "_EntityType_"
        FROM [ManagerWithEmployees].[ManagerBoundedContext].[Manager] m
        INNER JOIN [ManagerWithEmployees].[ManagerBoundedContext].[Employee] e
            ON m.[ManagerId] = e.[EmployeeId]
        UNION ALL
        (
            SELECT
                e.[EmployeeId] AS "Id",
                e.[Name] AS "Name",
                e.[SupervisorId] AS "SupervisorId",
                NULL AS "Department",
                2 AS "_EntityType_"
            FROM [ManagerWithEmployees].[ManagerBoundedContext].[Employee] e
            LEFT OUTER JOIN [ManagerWithEmployees].[ManagerBoundedContext].[Manager] m
                ON m.[ManagerId] = e.[EmployeeId]
            WHERE m.[ManagerId] IS NULL
        )
    ) _q_;

END;
GO

CREATE PROCEDURE [ManagerBoundedContext].[pGetAll_Employees_For_Manager]
    @managerId INT
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Name] AS "Name",
        _q_.[SupervisorId] AS "SupervisorId",
        _q_.[Department] AS "Department",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            m.[ManagerId] AS "Id",
            e.[Name] AS "Name",
            e.[SupervisorId] AS "SupervisorId",
            m.[Department] AS "Department",
            1 AS "_EntityType_"
        FROM [ManagerWithEmployees].[ManagerBoundedContext].[Manager] m
        INNER JOIN [ManagerWithEmployees].[ManagerBoundedContext].[Employee] e
            ON m.[ManagerId] = e.[EmployeeId]
        UNION ALL
        (
            SELECT
                e.[EmployeeId] AS "Id",
                e.[Name] AS "Name",
                e.[SupervisorId] AS "SupervisorId",
                NULL AS "Department",
                2 AS "_EntityType_"
            FROM [ManagerWithEmployees].[ManagerBoundedContext].[Employee] e
            LEFT OUTER JOIN [ManagerWithEmployees].[ManagerBoundedContext].[Manager] m
                ON m.[ManagerId] = e.[EmployeeId]
            WHERE m.[ManagerId] IS NULL
        )
    ) _q_
    WHERE _q_.[SupervisorId] = @managerId;

END;
GO

CREATE PROCEDURE [ManagerBoundedContext].[pEmployee_GetById]
    @employeeId INT
AS
BEGIN
    SELECT
        _q_.[Id] AS "Id",
        _q_.[Name] AS "Name",
        _q_.[SupervisorId] AS "SupervisorId",
        _q_.[Department] AS "Department",
        _q_.[_EntityType_] AS "_EntityType_"
    FROM 
    (
        SELECT
            m.[ManagerId] AS "Id",
            e.[Name] AS "Name",
            e.[SupervisorId] AS "SupervisorId",
            m.[Department] AS "Department",
            1 AS "_EntityType_"
        FROM [ManagerWithEmployees].[ManagerBoundedContext].[Manager] m
        INNER JOIN [ManagerWithEmployees].[ManagerBoundedContext].[Employee] e
            ON m.[ManagerId] = e.[EmployeeId]
        UNION ALL
        (
            SELECT
                e.[EmployeeId] AS "Id",
                e.[Name] AS "Name",
                e.[SupervisorId] AS "SupervisorId",
                NULL AS "Department",
                2 AS "_EntityType_"
            FROM [ManagerWithEmployees].[ManagerBoundedContext].[Employee] e
            LEFT OUTER JOIN [ManagerWithEmployees].[ManagerBoundedContext].[Manager] m
                ON m.[ManagerId] = e.[EmployeeId]
            WHERE m.[ManagerId] IS NULL
        )
    ) _q_
    WHERE _q_.[Id] = @employeeId;

END;
GO

CREATE PROCEDURE [ManagerBoundedContext].[pManager_DeleteEmployees]
    @supervisorId INT
AS
BEGIN
    DELETE FROM [ManagerWithEmployees].[ManagerBoundedContext].[Employee]
    WHERE [SupervisorId] = @supervisorId;

END;
GO

CREATE PROCEDURE [ManagerBoundedContext].[pManager_Delete]
    @managerId INT
AS
BEGIN
    DELETE FROM [ManagerWithEmployees].[ManagerBoundedContext].[Manager]
    WHERE [ManagerId] = @managerId;

END;
GO

CREATE PROCEDURE [ManagerBoundedContext].[pManager_Insert]
    @department VARCHAR(50),
    @name VARCHAR(50),
    @createdBy INT,
    @supervisorId INT
AS
BEGIN
    DECLARE @employeeId INT;

    DECLARE @employeeOutputData TABLE
    (
        [EmployeeId] INT
    );

    INSERT INTO [ManagerWithEmployees].[ManagerBoundedContext].[Employee]
    (
        [Name],
        [CreatedBy],
        [SupervisorId]
    )
    OUTPUT
        INSERTED.[EmployeeId]
        INTO @employeeOutputData
    VALUES
    (
        @name,
        @createdBy,
        @supervisorId
    );

    SELECT
        @employeeId = [EmployeeId]
    FROM @employeeOutputData;

    INSERT INTO [ManagerWithEmployees].[ManagerBoundedContext].[Manager]
    (
        [ManagerId],
        [Department]
    )
    VALUES
    (
        @employeeId,
        @department
    );

    SELECT
        [EmployeeId]
    FROM @employeeOutputData;

END;
GO

CREATE PROCEDURE [ManagerBoundedContext].[pManager_Update]
    @managerId INT,
    @department VARCHAR(50),
    @name VARCHAR(50),
    @updatedBy INT,
    @supervisorId INT
AS
BEGIN
    UPDATE [ManagerWithEmployees].[ManagerBoundedContext].[Employee]
    SET
        [Name] = @name,
        [UpdatedBy] = @updatedBy,
        [SupervisorId] = @supervisorId,
        [UpdatedDateTime] = GETDATE()
    WHERE [EmployeeId] = @managerId;

    UPDATE [ManagerWithEmployees].[ManagerBoundedContext].[Manager]
    SET
        [Department] = @department
    WHERE [ManagerId] = @managerId;

END;
GO

CREATE PROCEDURE [ManagerBoundedContext].[pManager_Get]
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
        @selectList = N'    m.[ManagerId] AS "Id",
    e.[Name] AS "Name",
    e.[SupervisorId] AS "SupervisorId",
    m.[Department] AS "Department"',
        @from = N'[ManagerWithEmployees].[ManagerBoundedContext].[Manager] m
INNER JOIN [ManagerWithEmployees].[ManagerBoundedContext].[Employee] e
    ON m.[ManagerId] = e.[EmployeeId]',
        @count = @count OUTPUT

END;
GO

CREATE PROCEDURE [ManagerBoundedContext].[pManager_GetAll]
AS
BEGIN
    SELECT
        m.[ManagerId] AS "Id",
        e.[Name] AS "Name",
        e.[SupervisorId] AS "SupervisorId",
        m.[Department] AS "Department"
    FROM [ManagerWithEmployees].[ManagerBoundedContext].[Manager] m
    INNER JOIN [ManagerWithEmployees].[ManagerBoundedContext].[Employee] e
        ON m.[ManagerId] = e.[EmployeeId];

END;
GO

CREATE PROCEDURE [ManagerBoundedContext].[pManager_GetById]
    @managerId INT
AS
BEGIN
    SELECT
        m.[ManagerId] AS "Id",
        e.[Name] AS "Name",
        e.[SupervisorId] AS "SupervisorId",
        m.[Department] AS "Department"
    FROM [ManagerWithEmployees].[ManagerBoundedContext].[Manager] m
    INNER JOIN [ManagerWithEmployees].[ManagerBoundedContext].[Employee] e
        ON m.[ManagerId] = e.[EmployeeId]
    WHERE m.[ManagerId] = @managerId;

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

