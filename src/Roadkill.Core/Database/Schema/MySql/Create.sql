CREATE TABLE explik_pages
(
	Id INT AUTO_INCREMENT NOT NULL,
	Title NVARCHAR(255) NOT NULL,
	Summary MEDIUMTEXT NULL,
	Tags NVARCHAR(255) NULL,
	CreatedBy NVARCHAR(255) NOT NULL,
	CreatedOn DATETIME NOT NULL,
	IsLocked BOOLEAN NOT NULL,
	ControlledBy NVARCHAR(255) NULL,
	PublishedOn DATETIME NULL,
    IsVideo BOOLEAN NOT NULL, 
    IsSubmitted BOOLEAN NOT NULL, 
    IsControlled BOOLEAN NOT NULL, 
    IsRejected BOOLEAN NOT NULL,
    IsCopied BOOLEAN NOT NULL,
    NbRating INT,
    TotalRating INT,
    NbView INT,
    FilePath NVARCHAR(255) NULL, 
    VideoUrl NVARCHAR(255) NULL,
    Pseudonym NVARCHAR(255) NULL,
    ControllerRating INT,
    CompetitionId INT,
	PRIMARY KEY (Id)
);

CREATE TABLE explik_pagecontent
(
	Id NVARCHAR(36) NOT NULL,
	ControlledBy NVARCHAR(255) NOT NULL,
	EditedOn DATETIME NOT NULL,
	VersionNumber INT NOT NULL,
	Text MEDIUMTEXT NULL,
	PageId INT NOT NULL,
	PRIMARY KEY (Id)
);

/*ALTER TABLE explik_pagecontent ADD CONSTRAINT FK_explik_pageid FOREIGN KEY(pageid) REFERENCES explik_pages (id);*/

CREATE TABLE explik_users
(
	Id VARCHAR(36) NOT NULL,
	ActivationKey NVARCHAR(255) NULL,
	Email NVARCHAR(255) NOT NULL,
	Firstname NVARCHAR(255) NULL,
	Lastname NVARCHAR(255) NULL,
	IsEditor BOOLEAN NOT NULL,
    IsController BOOLEAN NOT NULL, 
    AttachmentsPath NVARCHAR(255), 
	IsAdmin BOOLEAN NOT NULL,
	IsActivated BOOLEAN NOT NULL,
	Contribution INT,
	DisplayFlags INT,
	Password NVARCHAR(255) NOT NULL,
	PasswordResetKey NVARCHAR(255) NULL,
	Salt NVARCHAR(255) NOT NULL,
	Username NVARCHAR(255) NOT NULL,
	PRIMARY KEY (Id)
);

CREATE TABLE explik_siteconfiguration
(
	Id VARCHAR(36) NOT NULL,
	Version NVARCHAR(255) NOT NULL,
	Content MEDIUMTEXT NOT NULL,
	PRIMARY KEY (Id)
);

CREATE TABLE explik_comments
(
  Id VARCHAR(36) NOT NULL,
  PageId INT, 
  CreatedBy NVARCHAR(255) NOT NULL, 
  CreatedOn DATETIME  NOT NULL, 
  Rating INT, 
  ControlledBy NVARCHAR(255) NOT NULL, 
  IsControlled BOOLEAN NOT NULL,
  IsRejected BOOLEAN NOT NULL,
  Text MEDIUMTEXT NULL
);

CREATE TABLE explik_alerts
(
  Id VARCHAR(36) NOT NULL,
  PageId INT, 
  CommentId VARCHAR(36) NOT NULL,
  CreatedBy NVARCHAR(255) NOT NULL, 
  CreatedOn DATETIME  NOT NULL,
  Ilk NVARCHAR(255) NOT NULL
);

CREATE TABLE explik_competition
(
  Id INT AUTO_INCREMENT NOT NULL,
  PublicationStart DATETIME  NOT NULL,
  PublicationStop DATETIME  NOT NULL,
  RatingStart DATETIME  NOT NULL,
  RatingStop DATETIME  NOT NULL,
  PageTag NVARCHAR(100) NOT NULL, 
  PageId INT NOT NULL,
  Status INT NOT NULL,
  PRIMARY KEY (Id)
);

CREATE TABLE explik_competitionpage
(
  Id INT AUTO_INCREMENT NOT NULL,
  CompetitionId INT NOT NULL,
  PageId INT NOT NULL,
  NbRating INT,
  TotalRating INT,
  UserName NVARCHAR(255) NOT NULL, 
  Ranking INT,
  PRIMARY KEY (Id)
);
