Use master
Go
IF EXISTS (SELECT * FROM sys.databases WHERE name = N'TamiDB')
BEGIN
    DROP DATABASE TamiDB;
END
Go
Create Database TamiDB
Go
Use TamiDB
Go
Create Table AppUsers
(
	Id int Primary Key Identity,
	UserName nvarchar(50) Not Null,
	UserLastName nvarchar(50) Not Null,
	UserEmail nvarchar(50) Unique Not Null,
	UserPassword nvarchar(50) Not Null,
	ProfilePicture NVARCHAR(255) DEFAULT 'anonimuspic.jpg',
	IsManager bit Not Null Default 0
)
Go

Insert Into AppUsers (UserName, UserLastName, UserEmail, UserPassword, IsManager) 
Values ('admin', 'admin', 'ofer@gmauil.com', '1234', 1)
Go

-- Create a login for the admin user
CREATE LOGIN [TaskAdminLogin] WITH PASSWORD = 'kukuPassword';
Go

-- Create a user in the TamiDB database for the login
CREATE USER [TaskAdminUser] FOR LOGIN [TaskAdminLogin];
Go

-- Add the user to the db_owner role to grant admin privileges
ALTER ROLE db_owner ADD MEMBER [TaskAdminUser];
Go

--EF Code
/*
scaffold-DbContext "Server = (localdb)\MSSQLLocalDB;Initial Catalog=TamiDB;User ID=TaskAdminLogin;Password=kukuPassword;" Microsoft.EntityFrameworkCore.SqlServer -OutPutDir Models -Context TamiDBContext -DataAnnotations -force
*/

select * from AppUsers

ALTER TABLE AppUsers
ADD UserPhone nvarchar(20) NOT NULL DEFAULT '--' ;
Go

