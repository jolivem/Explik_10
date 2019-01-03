IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[explik_siteconfiguration]') AND type in (N'U'))
DROP TABLE [dbo].[explik_siteconfiguration];

CREATE TABLE [dbo].[explik_siteconfiguration]
(
	[Id] [uniqueidentifier] NOT NULL,
	[Version] [nvarchar](255) NOT NULL,
	[Content] [nvarchar](MAX) NOT NULL,
	PRIMARY KEY NONCLUSTERED (Id)
);

CREATE CLUSTERED INDEX [explik_siteconfiguration_idx] ON [dbo].[explik_siteconfiguration] ([Version]);

-- DROP the bad constraints NHibernate sql generator made
declare @constraintName varchar(max)

SELECT @constraintName=CONSTRAINT_NAME
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE CONSTRAINT_TYPE='PRIMARY KEY' AND TABLE_SCHEMA='dbo' AND TABLE_NAME='explik_pagecontent';
exec('ALTER TABLE [dbo].[explik_pagecontent] DROP CONSTRAINT ' +@constraintName);

SELECT @constraintName=CONSTRAINT_NAME
		FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
		WHERE CONSTRAINT_TYPE='PRIMARY KEY' AND TABLE_SCHEMA='dbo' AND TABLE_NAME='explik_users';
exec('ALTER TABLE [dbo].[explik_users] DROP CONSTRAINT ' +@constraintName);


-- Create new ones, to be Azure-friendly
CREATE CLUSTERED INDEX [explik_pagecontent_idx] ON [dbo].[explik_pagecontent] (PageId, VersionNumber);
CREATE CLUSTERED INDEX [explik_users_idx] ON [dbo].explik_users (Email);