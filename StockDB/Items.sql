CREATE TABLE [dbo].[Items]
(
	[ItemId] char(16) NOT NULL PRIMARY KEY,
	[Name] varchar(max) not null,
	[Price] decimal(18,2) not null,
	[Description] varchar(max),
	[Amount] int not null
)
