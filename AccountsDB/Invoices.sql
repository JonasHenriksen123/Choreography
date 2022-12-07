CREATE TABLE [dbo].[Invoices]
(
	[InvoiceId] char(16) NOT NULL PRIMARY KEY,
	[AccountId] char(16) not null foreign key references [Accounts]([AccountId]),
	[Amount] decimal(18, 2) not null,
	[OrderId] char(16) not null,
	[State] int not null
)
