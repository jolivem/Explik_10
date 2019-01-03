CREATE TABLE explik_pages
(
  "id" SERIAL NOT NULL, 
  "title" TEXT NOT NULL, 
  "summary" TEXT, 
  "tags" TEXT NOT NULL, 
  "createdby" TEXT NOT NULL, 
  "createdon" TIMESTAMP(20) WITHOUT TIME ZONE NOT NULL, 
  "islocked" BOOLEAN NOT NULL, 
  "controlledby" TEXT, 
  "publishedon" TIMESTAMP(20) WITHOUT TIME ZONE, 
  "isvideo" BOOLEAN NOT NULL, 
  "issubmitted" BOOLEAN NOT NULL, 
  "iscontrolled" BOOLEAN NOT NULL, 
  "isrejected" BOOLEAN NOT NULL,
  "iscopied" BOOLEAN NOT NULL,
  "nbrating" INTEGER,
  "totalrating" INTEGER,
  "nbview" INTEGER,
  "filepath" TEXT, 
  "videourl" TEXT, 
  "pseudonym" TEXT, 
  "controllerrating" INTEGER,
PRIMARY KEY("id")
);

CREATE TABLE explik_pagecontent
(
  "id" UUID NOT NULL, 
  "controlledby" TEXT NOT NULL, 
  "editedon" TIMESTAMP WITHOUT TIME ZONE NOT NULL, 
  "versionnumber" INTEGER NOT NULL, 
  "text" TEXT, 
  "pageid" INTEGER NOT NULL, 
  PRIMARY KEY("id")
);

CREATE TABLE explik_users
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
  "contributionlevel" INTEGER,
  "displayflags" INTEGER,
  "password" TEXT NOT NULL, 
  "passwordresetkey" TEXT, 
  "salt" TEXT NOT NULL, 
  "username" TEXT NOT NULL, 
  PRIMARY KEY("id")
);

CREATE TABLE explik_siteconfiguration 
(
  "id" UUID NOT NULL, 
  "version" TEXT NOT NULL, 
  "content" TEXT NOT NULL UNIQUE, 
  PRIMARY KEY("id")
);

CREATE TABLE explik_comments
(
  "id"  UUID NOT NULL,
  "pageid" INTEGER, 
  "createdby" TEXT NOT NULL, 
  "createdon" TIMESTAMP(20) WITHOUT TIME ZONE, 
  "rating" INTEGER, 
  "controlledby" TEXT NOT NULL, 
  "iscontrolled" BOOLEAN NOT NULL, 
  "isrejected" BOOLEAN NOT NULL, 
  "text" TEXT NOT NULL
);

CREATE TABLE explik_alerts
(
  "id" UUID NOT NULL,
  "pageid" INTEGER, 
  "commentid" UUID,
  "createdby" TEXT NOT NULL, 
  "createdon" TIMESTAMP(20) WITHOUT TIME ZONE
);
