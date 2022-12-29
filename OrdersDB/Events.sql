CREATE TABLE [dbo].[Events]
(
	[EventId] uniqueidentifier NOT NULL,
	[EventName] varchar(max) not null, 
    [Queue] INT NOT NULL
)
