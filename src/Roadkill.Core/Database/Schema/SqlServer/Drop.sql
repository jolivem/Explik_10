IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[explik_pagecontent_fk_pageid]') AND parent_object_id = OBJECT_ID(N'[dbo].[explik_pagecontent]'))
ALTER TABLE [dbo].[explik_pagecontent] DROP CONSTRAINT [FK_explik_pageid];

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[explik_pagecontent]') AND type in (N'U'))
DROP TABLE [dbo].[explik_pagecontent];

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[explik_pages]') AND type in (N'U'))
DROP TABLE [dbo].[explik_pages];

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[explik_users]') AND type in (N'U'))
DROP TABLE [dbo].explik_users;

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[explik_siteconfiguration]') AND type in (N'U'))
DROP TABLE [dbo].[explik_siteconfiguration];