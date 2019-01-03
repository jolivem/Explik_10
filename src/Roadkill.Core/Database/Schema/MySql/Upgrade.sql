DROP TABLE IF EXISTS explik_siteconfiguration;
CREATE TABLE explik_siteconfiguration
(
	Id VARCHAR(36) NOT NULL,
	Version NVARCHAR(255) NOT NULL,
	Content MEDIUMTEXT NOT NULL,
	PRIMARY KEY (Id)
);