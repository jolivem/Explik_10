CREATE DATABASE  IF NOT EXISTS `Roadkill` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `Roadkill`;
-- MySQL dump 10.13  Distrib 8.0.16, for Win64 (x86_64)
--
-- Host: localhost    Database: Roadkill
-- ------------------------------------------------------
-- Server version	8.0.16

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `explik_pages`
--

DROP TABLE IF EXISTS `explik_pages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `explik_pages` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Summary` mediumtext,
  `Tags` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `CreatedBy` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreatedOn` datetime NOT NULL,
  `IsLocked` tinyint(1) NOT NULL,
  `ControlledBy` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `PublishedOn` datetime DEFAULT NULL,
  `IsVideo` tinyint(1) NOT NULL,
  `IsSubmitted` tinyint(1) NOT NULL,
  `IsControlled` tinyint(1) NOT NULL,
  `IsRejected` tinyint(1) NOT NULL,
  `IsCopied` tinyint(1) NOT NULL,
  `NbRating` int(11) DEFAULT NULL,
  `TotalRating` int(11) DEFAULT NULL,
  `NbView` int(11) DEFAULT NULL,
  `FilePath` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `VideoUrl` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `Pseudonym` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `ControllerRating` int(11) DEFAULT NULL,
  `CompetitionId` int(11) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `explik_pages`
--

LOCK TABLES `explik_pages` WRITE;
/*!40000 ALTER TABLE `explik_pages` DISABLE KEYS */;
INSERT INTO `explik_pages` VALUES (1,'Nouvelle competition 1',NULL,'__Competition_190607.031535','Admin','2019-06-07 15:15:39',1,'','2019-07-02 15:04:59',0,0,0,0,0,0,0,2,NULL,NULL,NULL,0,-1),(2,'Titre ma clé',NULL,'tag1, tag2','User1','2019-06-07 15:15:39',0,NULL,'0001-01-01 00:00:00',0,0,0,0,0,1,3,1,NULL,NULL,NULL,0,0),(3,'Titre ma clé',NULL,'tag1, tag2','u1','2019-06-07 15:15:39',0,NULL,'2019-01-02 00:00:00',0,0,1,0,0,0,0,1,NULL,NULL,NULL,0,0),(4,'Titre ma clé',NULL,'tag3','u2','2019-06-07 15:15:39',0,NULL,'2019-01-03 00:00:00',0,0,1,0,0,1,3,2,NULL,NULL,NULL,0,0),(5,'Nouvelle competition 2',NULL,'__Competition_190607.033339','Admin','2019-06-07 15:33:40',1,NULL,'2019-01-04 00:00:00',0,0,1,0,0,0,0,0,NULL,NULL,NULL,0,2),(6,'Titre ma clé',NULL,'tag1, tag2','u0','2019-06-07 15:33:40',0,NULL,'2019-01-05 00:00:00',0,0,0,0,0,4,13,13,NULL,NULL,NULL,0,3),(7,'Titre ma clé A',NULL,'tag1, tag2','u1','2019-06-07 15:33:40',0,NULL,'0001-01-01 00:00:00',0,0,1,0,0,10,28,17,NULL,NULL,NULL,0,3),(8,'Titre ma clé B',NULL,'tag3','u2','2019-06-07 15:33:40',0,NULL,'0001-01-01 00:00:00',0,0,1,0,0,1,3,9,NULL,NULL,NULL,0,3),(9,'Titre pour competition 2',NULL,'Math','e','2019-07-02 15:16:15',0,'u','2019-07-02 15:18:27',0,0,0,0,0,2,5,7,'2019-07/e',NULL,NULL,2,2),(10,'Titre pour competition',NULL,'','u','2019-07-02 15:21:20',0,'u','2019-07-02 15:26:05',0,0,1,0,0,1,4,2,'2019-07/u',NULL,NULL,4,-1),(11,'Competition 3',NULL,'__competition_190704.085522','admin','2019-07-04 06:57:01',1,'','2019-07-04 06:57:28',0,0,0,0,0,0,0,0,'2019-07/admin',NULL,NULL,0,-1),(12,'fghjkqsdhkjfg lkqjsdhgf lqskdjhf qsdf q',NULL,'','u','2019-07-08 09:36:44',0,'','2019-07-10 14:17:11',0,0,0,0,0,0,0,4,'2019-07/u',NULL,NULL,0,2),(13,'A controller un cinq',NULL,'un,deux,trois,cinq','u','2019-07-08 09:48:32',0,'u','2019-07-08 14:22:07',0,0,1,0,0,1,1,2,'2019-07/u',NULL,NULL,1,2);
/*!40000 ALTER TABLE `explik_pages` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-07-10 17:24:21
