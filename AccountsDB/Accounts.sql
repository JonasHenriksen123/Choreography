CREATE TABLE [dbo].[Accounts]
(
	[AccountId] CHAR(16) NOT NULL PRIMARY KEY,
	[Name] varchar(40) NOT NULL ,
	[CreditAmount] decimal(18,2) not null,
	[Credit] decimal(18,2) not null
)
