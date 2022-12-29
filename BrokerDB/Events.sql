CREATE TABLE [dbo].[Events]
(
	[EventId] UNIQUEIDENTIFIER NOT NULL,
	[EventName] varchar(40) not null,
	[PublishDate] datetime not null,
	[Queue] int not null,
	[Params] varchar(max), 
)
