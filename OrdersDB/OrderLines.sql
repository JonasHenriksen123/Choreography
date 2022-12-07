CREATE TABLE [dbo].[OrderLines]
(
	[OrderLineId] char(16) NOT NULL PRIMARY KEY,
	[OrderId] char(16) not null foreign key references [Orders]([OrderId]),
	[ItemId] char(16) not null,
	[Count] int not null,
	[Amount] decimal(18,2) not null,
	[Total] decimal(18,2) not null,

)
