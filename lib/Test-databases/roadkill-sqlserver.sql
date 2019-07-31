USE Roadkill;

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'explik_pagecontent')
    DROP TABLE [dbo].[explik_pagecontent];

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'explik_pages')
    DROP TABLE [dbo].[explik_pages];

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'explik_users')
    DROP TABLE [dbo].[explik_users];

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'explik_siteconfiguration')
    DROP TABLE [dbo].[explik_siteconfiguration];

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'explik_comments')
    DROP TABLE [dbo].[explik_comments];

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'explik_alerts')
    DROP TABLE [dbo].[explik_alerts];

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'explik_competition')
    DROP TABLE [dbo].[explik_competition];

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'explik_competitionpage')
    DROP TABLE [dbo].[explik_competitionpage];

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'explik_competitionuser')
    DROP TABLE [dbo].[explik_competitionuser];

-- SCHEMA (taken from Core/Database/Schema/SqlServer)
CREATE TABLE [dbo].[explik_pages]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Summary] [nvarchar](MAX) NULL,
	[Tags] [nvarchar](255) NULL,
	[CreatedBy] [nvarchar](255) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[ControlledBy] [nvarchar](255) NULL,
	[PublishedOn] [datetime] NULL,
    [IsVideo] [bit] NOT NULL, 
    [IsSubmitted] [bit] NOT NULL, 
    [IsControlled] [bit] NOT NULL, 
    [IsRejected] [bit] NOT NULL,
    [IsCopied] [bit] NOT NULL,
	[NbRating] [int] NOT NULL,
	[TotalRating] [int] NOT NULL,
	[NbView] [int] NOT NULL,
    [FilePath] [nvarchar](255) NULL, 
    [VideoUrl] [nvarchar](255) NULL,
    [Pseudonym] [nvarchar](255) NULL,
    [ControllerRating] [int],
    [CompetitionId] [int],
	PRIMARY KEY CLUSTERED (Id)
);

CREATE TABLE [dbo].[explik_pagecontent]
(
	[Id] [uniqueidentifier] NOT NULL,
	[ControlledBy] [nvarchar](255) NOT NULL,
	[EditedOn] [datetime] NOT NULL,
	[VersionNumber] [int] NOT NULL,
	[Text] [nvarchar](MAX) NULL,
	[PageId] [int] NOT NULL,
	PRIMARY KEY NONCLUSTERED (Id)
);

ALTER TABLE [dbo].[explik_pagecontent] ADD CONSTRAINT [FK_explik_pageid] FOREIGN KEY([pageid]) REFERENCES [dbo].[explik_pages] ([id]);

CREATE TABLE [dbo].[explik_users]
(
	[Id] [uniqueidentifier] NOT NULL,
	[ActivationKey] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Firstname] [nvarchar](255) NULL,
	[Lastname] [nvarchar](255) NULL,
	[IsEditor] [bit] NOT NULL,
    [IsController] [bit] NOT NULL, 
    [AttachmentsPath] [nvarchar](255), 
	[IsAdmin] [bit] NOT NULL,
	[IsActivated] [bit] NOT NULL,
	[Contribution] [int],
	[DisplayFlags] [int],
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

CREATE TABLE [dbo].[explik_comments]
(
  [Id] [uniqueidentifier] NOT NULL,
  [PageId] [int], 
  [CreatedBy] [nvarchar](255) NOT NULL, 
  [CreatedOn] [datetime] NOT NULL, 
  [Rating] [int], 
  [ControlledBy] [nvarchar](255) NOT NULL, 
  [IsControlled] [bit] NOT NULL,
  [IsRejected] [bit] NOT NULL,
  [Text] [nvarchar](MAX) NULL
);

CREATE TABLE [dbo].[explik_alerts]
(
  [Id] [uniqueidentifier] NOT NULL,
  [PageId] [int], 
  [CommentId VARCHAR(36) NOT NULL,
  [CreatedBy] [nvarchar](255) NOT NULL, 
  [CreatedOn] [datetime] NOT NULL,
  [Ilk] [nvarchar](255) NOT NULL
);

CREATE TABLE [dbo].[explik_competition]
(
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [PublicationStart] [datetime] NOT NULL,
  [PublicationStop] [datetime] NOT NULL,
  [RatingStart] [datetime] NOT NULL,
  [RatingStop] [datetime] NOT NULL,
  [PageTag] [nvarchar](100) NOT NULL, 
  [PageId] [int] NOT NULL,
  [Status] [int] NOT NULL,
  PRIMARY KEY CLUSTERED (Id)
);

CREATE TABLE [dbo].[explik_competitionpage]
(
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [CompetitionId] [int] NOT NULL,
  [PageId] [int] NOT NULL,
  [NbRating] [int],
  [TotalRating] [int],
  PRIMARY KEY CLUSTERED (Id)
);

CREATE TABLE [dbo].[explik_competitionuser]
(
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [CompetitionId] [int] NOT NULL, 
  [UserName] [nvarchar](255) NOT NULL, 
  [Ranking] [int],
  PRIMARY KEY CLUSTERED (Id)
);



CREATE CLUSTERED INDEX [explik_pagecontent_idx] ON [dbo].[explik_pagecontent] (PageId, VersionNumber);
CREATE CLUSTERED INDEX [explik_users_idx] ON [dbo].explik_users (Email);
CREATE CLUSTERED INDEX [explik_siteconfiguration_idx] ON [dbo].[explik_siteconfiguration] ([Version]);

-- DATA
SET IDENTITY_INSERT explik_pages ON;

-- Users
INSERT INTO explik_users (id, activationkey, email, firstname, iseditor, iscontroller, attachmentspath, isadmin, isactivated, contribution, displayflags, lastname, password, passwordresetkey, salt, username) VALUES ('aabd5468-1c0e-4277-ae10-a0ce00d2fefc','','admin@localhost','Chris','0','0','pathA','1','1','0','0','Admin','C882A7933951FCC4197718B104AECC53564FC205','','J::]T!>k5LR|.{U9','admin');
INSERT INTO explik_users (id, activationkey, email, firstname, iseditor, iscontroller, attachmentspath, isadmin, isactivated, contribution, displayflags, lastname, password, passwordresetkey, salt, username) VALUES ('4d0bc016-1d47-4ad3-a6fe-a11a013ef9c8','3d12daea-16d0-4bd6-9e0c-347f14e0d97d','editor@localhost','','1','0','pathE','0','1','0','0','','7715C929E99254C117657B0937E97926443FDAF6','','fO)M`*QU:eH''Xl_%','editor');

-- Configuration
INSERT INTO explik_siteconfiguration (id, version, content) VALUES ('b960e8e5-529f-4f7c-aee4-28eb23e13dbd','2.0.0.0','{
  "AllowedFileTypes": "jpg,png,gif,zip,xml,pdf",
  "AllowUserSignup": true,
  "IsRecaptchaEnabled": false,
  "MarkupType": "Creole",
  "RecaptchaPrivateKey": "recaptcha-private-key",
  "RecaptchaPublicKey": "recaptcha-public-key",
  "SiteUrl": "http://localhost:9876",
  "SiteName": "Acceptance Tests",
  "Theme": "Responsive",
  "OverwriteExistingFiles": false,
  "HeadContent": "",
  "MenuMarkup": "* %mainpage%\r\n* %categories%\r\n* %allpages%\r\n* %newpage%\r\n* %managefiles%\r\n* %sitesettings%\r\n\r\n",
  "PluginLastSaveDate": "2013-12-28T16:00:54.408505Z"
}');
INSERT INTO explik_siteconfiguration (id, version, content) VALUES ('8050978c-80fb-0000-0000-000000000000','1.0','{
  "PluginId": "ClickableImages",
  "Version": "1.0",
  "IsEnabled": false,
  "Values": []
}');
INSERT INTO explik_siteconfiguration (id, version, content) VALUES ('208af9dc-80fb-0000-0000-000000000000','1.0','{
  "PluginId": "ResizeImages",
  "Version": "1.0",
  "IsEnabled": false,
  "Values": []
}');
INSERT INTO explik_siteconfiguration (id, version, content) VALUES ('b35f5545-80fb-0000-0000-000000000000','1.0','{
  "PluginId": "ExternalLinksInNewWindow",
  "Version": "1.0",
  "IsEnabled": false,
  "Values": []
}');
INSERT INTO explik_siteconfiguration (id, version, content) VALUES ('b20d067e-80fb-0000-0000-000000000000','1.0','{
  "PluginId": "Jumbotron",
  "Version": "1.0",
  "IsEnabled": false,
  "Values": []
}');
INSERT INTO explik_siteconfiguration (id, version, content) VALUES ('b970504f-80fb-0000-0000-000000000000','1.0','{
  "PluginId": "MathJax",
  "Version": "1.0",
  "IsEnabled": false,
  "Values": []
}');
INSERT INTO explik_siteconfiguration (id, version, content) VALUES ('598fdb04-80fb-0000-0000-000000000000','1.0','{
  "PluginId": "ToC",
  "Version": "1.0",
  "IsEnabled": false,
  "Values": []
}');
INSERT INTO explik_siteconfiguration (id, version, content) VALUES ('92e641c4-80fb-0000-0000-000000000000','1.0','{
  "PluginId": "SyntaxHighlighter",
  "Version": "1.0",
  "IsEnabled": false,
  "Values": []
}');
