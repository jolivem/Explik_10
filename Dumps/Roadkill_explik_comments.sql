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
-- Table structure for table `explik_comments`
--

DROP TABLE IF EXISTS `explik_comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `explik_comments` (
  `Id` varchar(36) NOT NULL,
  `PageId` int(11) DEFAULT NULL,
  `CreatedBy` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreatedOn` datetime NOT NULL,
  `Rating` int(11) DEFAULT NULL,
  `ControlledBy` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `IsControlled` tinyint(1) NOT NULL,
  `IsRejected` tinyint(1) NOT NULL,
  `Text` mediumtext
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `explik_comments`
--

LOCK TABLES `explik_comments` WRITE;
/*!40000 ALTER TABLE `explik_comments` DISABLE KEYS */;
INSERT INTO `explik_comments` VALUES ('5c60df1d-b7d1-4461-9ebc-aa6500fc686c',2,'u','2019-06-07 15:18:59',3,'',0,0,''),('7cbcbba2-703f-44b7-b7b3-aa650100f4d5',4,'u1@e','2019-06-07 15:35:33',3,'',0,0,''),('aca366b8-2c7d-42c4-968b-aa8000fbec09',6,'u','2019-07-04 15:17:13',0,'',0,0,''),('cfa9b12a-0cea-424e-bd1e-aa8001040473',7,'u','2019-07-04 15:46:42',2,'',0,0,''),('64d22c89-0be9-47cd-8faa-aa80010fa616',8,'u','2019-07-04 16:29:03',5,'',0,0,''),('84b0b764-7308-43ad-9298-aa8100c43a6b',9,'u','2019-07-05 11:54:27',1,'',0,0,'');
/*!40000 ALTER TABLE `explik_comments` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-07-10 17:24:22
