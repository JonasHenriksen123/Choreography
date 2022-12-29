CREATE TABLE [dbo].[OrderLines]
(
	[OrderLineId] uniqueidentifier NOT NULL PRIMARY KEY,
	[OrderId] uniqueidentifier not null foreign key references [Orders]([OrderId]),
	[ItemId] uniqueidentifier not null,
	[Count] int not null,
	[Amount] decimal(18,2) not null,
	[Total] decimal(18,2) not null,

)
