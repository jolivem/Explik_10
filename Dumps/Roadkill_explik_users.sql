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
-- Table structure for table `explik_users`
--

DROP TABLE IF EXISTS `explik_users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `explik_users` (
  `Id` varchar(36) NOT NULL,
  `ActivationKey` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `Email` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Firstname` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `Lastname` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `IsEditor` tinyint(1) NOT NULL,
  `IsController` tinyint(1) NOT NULL,
  `AttachmentsPath` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `IsAdmin` tinyint(1) NOT NULL,
  `IsActivated` tinyint(1) NOT NULL,
  `Contribution` int(11) DEFAULT NULL,
  `DisplayFlags` int(11) DEFAULT NULL,
  `Password` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `PasswordResetKey` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `Salt` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Username` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `explik_users`
--

LOCK TABLES `explik_users` WRITE;
/*!40000 ALTER TABLE `explik_users` DISABLE KEYS */;
INSERT INTO `explik_users` VALUES ('2e61e12b-d87b-402b-88bd-aa7e011c28ee',NULL,'e@e',NULL,NULL,1,0,'2019-07/e',0,1,0,0,'ECE6F6238FA6E9261ABA0E7E969B5A5513623499',NULL,'9hK;q_D3E%7h{B\"Z','e'),('6d74f459-bd50-4e88-ad4e-aa6501007047',NULL,'u2',NULL,NULL,1,0,'2019-06/u2@e',0,1,0,0,'FDE8B0B3DF448EA17B1C80F692C8D156918C8B35',NULL,'Szzw.Z2g^8Q^8T{2','u2@e'),('8ab3cdeb-12df-4813-8d3a-aa6501007046',NULL,'u1',NULL,NULL,1,0,'2019-06/u1@e',0,1,0,0,'A0BDCC784C075F3DC37536B33A5F506EB8459E63',NULL,'\';5q_x-*Ng1I,0wA','u1@e'),('d07800de-eff8-41df-a3c2-aa6501007043',NULL,'u0',NULL,NULL,1,0,'2019-06/u0@e',0,1,0,0,'3DD2CAED118AC1100C44227D0314F0AC91305A26',NULL,'@e%IJ*)7)gQjK%`1','u0@e'),('e3356d9d-373b-4ff2-b07f-aa6500fb24a8',NULL,'Admin@explik.fr',NULL,NULL,1,0,'2019-06/admin',1,1,0,0,'8FE03F1093639784C997351511A31D4B59726703',NULL,'j?{DtG\"@/wM2P]QO','admin'),('fae942d4-a0a6-43bc-9949-aa6500fb69e6',NULL,'u@e',NULL,NULL,1,1,'2019-06/u',0,1,0,0,'41A32A41D501B1B37A40D94C750F5D2A7069D907',NULL,'*Z)y>`qAz\\=K&hg>','u');
/*!40000 ALTER TABLE `explik_users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-07-10 17:24:20
