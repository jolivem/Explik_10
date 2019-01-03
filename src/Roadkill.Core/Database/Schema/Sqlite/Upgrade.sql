DROP TABLE IF EXISTS explik_siteconfiguration;
CREATE TABLE [explik_siteconfiguration] 
(
  [id] UNIQUEIDENTIFIER NOT NULL, 
  [version] TEXT, 
  [content] NTEXT, 
  PRIMARY KEY (Id)
);