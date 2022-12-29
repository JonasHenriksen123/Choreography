CREATE TABLE [dbo].[Invoices]
(
	[InvoiceId] uniqueidentifier NOT NULL PRIMARY KEY,
	[AccountId] uniqueidentifier not null foreign key references [Accounts]([AccountId]),
	[Amount] decimal(18, 2) not null,
	[OrderId] uniqueidentifier not null,
	[State] int not null
)
