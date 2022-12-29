CREATE TABLE [dbo].[Orders]
(
	[OrderId] uniqueidentifier NOT NULL PRIMARY KEY,
	[Amount]  decimal(18,2) not null,
	[ItemCount] int not null,
	[AccountId] uniqueidentifier,
	[State] int not null,

)
