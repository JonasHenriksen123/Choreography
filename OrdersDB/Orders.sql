CREATE TABLE [dbo].[Orders]
(
	[OrderId] char(16) NOT NULL PRIMARY KEY,
	[Amount]  decimal(18,2) not null,
	[ItemCount] int not null,
	[AccountId] char(16),
	[State] int not null,

)
