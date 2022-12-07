CREATE TABLE [dbo].[Events]
(
	[EventId] char(16) NOT NULL,
	[EventName] varchar(max) not null,
	[PublishDate] datetime not null,
	[Queue] int not null,
	[Params] varchar(max)
)
