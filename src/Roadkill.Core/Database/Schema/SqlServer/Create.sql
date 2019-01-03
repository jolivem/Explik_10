CREATE TABLE [dbo].[explik_pages]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Tags] [nvarchar](255) NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[ControlledBy] [nvarchar](255) NULL,
	[PublishedOn] [datetime] NULL,
	[NbRating] [int] NOT NULL,
	[TotalRating] [int] NOT NULL,
	[NbView] [int] NOT NULL,
	PRIMARY KEY CLUSTERED (Id)
);

CREATE TABLE [dbo].[explik_pagecontent]
(
	[Id] [uniqueidentifier] NOT NULL,
	[EditedBy] [nvarchar](255) NOT NULL,
	[EditedOn] [datetime] NOT NULL,
	[VersionNumber] [int] NOT NULL,
	[Text] [nvarchar](MAX) NULL,
	[PageId] [int] NOT NULL,
	PRIMARY KEY NONCLUSTERED (Id)
);

ALTER TABLE [dbo].[explik_pagecontent] ADD CONSTRAINT [FK_explik_pageid] FOREIGN KEY([pageid]) REFERENCES [dbo].[explik_pages] ([id]);

CREATE TABLE [dbo].explik_users
(
	[Id] [uniqueidentifier] NOT NULL,
	[ActivationKey] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Firstname] [nvarchar](255) NULL,
	[Lastname] [nvarchar](255) NULL,
	[IsEditor] [bit] NOT NULL,
	[IsAdmin] [bit] NOT NULL,
	[IsActivated] [bit] NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[PasswordResetKey] [nvarchar](255) NULL,
	[Salt] [nvarchar](255) NOT NULL,
	[Username] [nvarchar](255) NOT NULL,
	PRIMARY KEY NONCLUSTERED (Id)
);

CREATE TABLE [dbo].[explik_siteconfiguration]
(
	[Id] [uniqueidentifier] NOT NULL,
	[Version] [nvarchar](255) NOT NULL,
	[Content] [nvarchar](MAX) NOT NULL,
	PRIMARY KEY NONCLUSTERED (Id)
);

CREATE CLUSTERED INDEX [explik_pagecontent_idx] ON [dbo].[explik_pagecontent] (PageId, VersionNumber);
CREATE CLUSTERED INDEX [explik_users_idx] ON [dbo].explik_users (Email);
CREATE CLUSTERED INDEX [explik_siteconfiguration_idx] ON [dbo].[explik_siteconfiguration] ([Version]);