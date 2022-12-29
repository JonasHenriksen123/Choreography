CREATE TABLE [dbo].[Accounts]
(
	[AccountId] uniqueidentifier NOT NULL PRIMARY KEY,
	[Name] varchar(40) NOT NULL ,
	[CreditAmount] decimal(18,2) not null,
	[Credit] decimal(18,2) not null
)
