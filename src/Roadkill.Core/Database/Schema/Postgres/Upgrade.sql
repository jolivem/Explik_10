DROP TABLE IF EXISTS explik_siteconfiguration;
CREATE TABLE explik_siteconfiguration 
(
  "id" UUID NOT NULL, 
  "version" TEXT NOT NULL, 
  "content" TEXT NOT NULL UNIQUE, 
  PRIMARY KEY("id")
);