CREATE TABLE [explik_pages]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Tags] [nvarchar](255) NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[ControlledBy] [nvarchar](255) NULL,
	[PublishedOn] [datetime] NULL
);