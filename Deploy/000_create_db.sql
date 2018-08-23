CREATE DATABASE OrleDB;
GO

USE OrleDB;

CREATE TABLE A(id int primary key identity(1,1), b varchar(255));

GO

INSERT INTO A(b) Values('init');
INSERT INTO A(b) Values('init22');

GO