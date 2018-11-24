CREATE TABLE roadkill_pages
(
  "id" SERIAL NOT NULL, 
  "title" TEXT NOT NULL, 
  "summary" TEXT, 
  "tags" TEXT NOT NULL, 
  "createdby" TEXT NOT NULL, 
  "createdon" TIMESTAMP(20) WITHOUT TIME ZONE NOT NULL, 
  "islocked" BOOLEAN NOT NULL, 
  "controlledby" TEXT, 
  "modifiedon" TIMESTAMP(20) WITHOUT TIME ZONE, 
  "isvideo" BOOLEAN NOT NULL, 
  "issubmitted" BOOLEAN NOT NULL, 
  "iscontrolled" BOOLEAN NOT NULL, 
  "isrejected" BOOLEAN NOT NULL,
  "nbrating" INTEGER,
  "totalrating" INTEGER,
  "nbview" INTEGER,
  "filepath" TEXT, 
  "videourl" TEXT, 
  "controllerrating" INTEGER,
PRIMARY KEY("id")
);

CREATE TABLE roadkill_pagecontent
(
  "id" UUID NOT NULL, 
  "editedby" TEXT NOT NULL, 
  "editedon" TIMESTAMP WITHOUT TIME ZONE NOT NULL, 
  "versionnumber" INTEGER NOT NULL, 
  "text" TEXT, 
  "pageid" INTEGER NOT NULL, 
  PRIMARY KEY("id")
);

CREATE TABLE roadkill_users
(
  "id" UUID NOT NULL, 
  "activationkey" TEXT, 
  "email" TEXT NOT NULL, 
  "firstname" TEXT, 
  "lastname" TEXT, 
  "iseditor" BOOLEAN NOT NULL, 
  "iscontroller" BOOLEAN NOT NULL, 
  "attachmentspath" TEXT, 
  "isadmin" BOOLEAN NOT NULL, 
  "isactivated" BOOLEAN NOT NULL, 
  "password" TEXT NOT NULL, 
  "passwordresetkey" TEXT, 
  "salt" TEXT NOT NULL, 
  "username" TEXT NOT NULL, 
  PRIMARY KEY("id")
);

CREATE TABLE roadkill_siteconfiguration 
(
  "id" UUID NOT NULL, 
  "version" TEXT NOT NULL, 
  "content" TEXT NOT NULL UNIQUE, 
  PRIMARY KEY("id")
);

CREATE TABLE roadkill_comments
(
  "id"  UUID NOT NULL,
  "pageid" INTEGER, 
  "createdby" TEXT NOT NULL, 
  "createdon" TIMESTAMP(20) WITHOUT TIME ZONE, 
  "rating" INTEGER, 
  "text" TEXT NOT NULL
);

CREATE TABLE roadkill_alerts
(
  "id" UUID NOT NULL,
  "pageid" INTEGER, 
  "commentid" UUID,
  "createdby" TEXT NOT NULL, 
  "createdon" TIMESTAMP(20) WITHOUT TIME ZONE
);
