CREATE TABLE roadkill_pages 
(
	[id] integer primary key autoincrement, 
	[title] TEXT, 
	[summary] TEXT, 
	[tags] TEXT, 
	[createdby] TEXT, 
	[createdon] DATETIME, 
	[islocked] BOOL, 
	[modifiedby] TEXT, 
	[modifiedon] DATETIME,
	[isvideo] BOOL, 
	[ispublished] BOOL, 
	[iscontrolled] BOOL, 
	[isrejected] BOOL, 
	[nbrating] integer,
	[totalrating] integer,
	[nbview] integer,
    [nbalert] INT
);

CREATE TABLE roadkill_pagecontent 
(
	[id] CHAR(36) not null, 
	[editedby] TEXT,
	[editedon] DATETIME, 
	[versionnumber] INT, 
	[text] NTEXT, 
	[pageid] INT, 
	PRIMARY KEY (Id)
	/*,constraint fk_roadkillpageid foreign key (pageid) references roadkill_pages*/
);

CREATE INDEX pageid on roadkill_pagecontent (pageid);

CREATE TABLE roadkill_users 
(
	[id] CHAR(36) not null, 
	[activationkey] TEXT, 
	[email] NTEXT, 
	[firstname] NTEXT, 
	[isblacklisted] BOOL,
	[iseditor] BOOL, 
	[isadmin] BOOL, 
	[iscontroller] BOOL, 
	[isdummy] BOOL, 
	[editorlevel] BOOL, 
	[isactivated] BOOL, 
	[lastname] NTEXT, 
	[password] NTEXT, 
	[passwordresetkey] NTEXT, 
	[salt] NTEXT, 
	[username] NTEXT, 
	PRIMARY KEY (Id)
);

CREATE INDEX email on roadkill_users (email);

CREATE TABLE [roadkill_siteconfiguration] 
(
  [id] CHAR(36) NOT NULL, 
  [version] TEXT, 
  [content] NTEXT, 
  PRIMARY KEY (Id)
);

CREATE TABLE [roadkill_comments] 
(
  [id] integer primary key autoincrement,
  [pageid] INT, 
  [createdby] NTEXT, 
  [createdon] DATETIME, 
  [rating] INT, 
  [text] NTEXT,
  [nbalert] INT

);