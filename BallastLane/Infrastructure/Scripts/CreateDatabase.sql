IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE name='Employees' and xtype='U')
BEGIN
    CREATE TABLE Employees (
        EmployeeId INT PRIMARY KEY IDENTITY,
        FirstName NVARCHAR(100),
        LastName NVARCHAR(100),
        DateOfBirth DATE
    )
    ;
END

IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE name='Users' and xtype='U')
BEGIN
    CREATE TABLE Users (
        UserId INT PRIMARY KEY IDENTITY,
        Username NVARCHAR(100),
        PasswordHash NVARCHAR(255),
        Email NVARCHAR(100)
    )
    ;

    INSERT INTO Users (Username, PasswordHash) VALUES ('Admin','123');
END