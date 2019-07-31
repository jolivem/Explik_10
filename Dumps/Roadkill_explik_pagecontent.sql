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
-- Table structure for table `explik_pagecontent`
--

DROP TABLE IF EXISTS `explik_pagecontent`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `explik_pagecontent` (
  `Id` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `ControlledBy` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `EditedOn` datetime NOT NULL,
  `VersionNumber` int(11) NOT NULL,
  `Text` mediumtext,
  `PageId` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `explik_pagecontent`
--

LOCK TABLES `explik_pagecontent` WRITE;
/*!40000 ALTER TABLE `explik_pagecontent` DISABLE KEYS */;
INSERT INTO `explik_pagecontent` VALUES ('3b2e25b7-e66d-4986-8358-0906c86e824b','','2019-07-08 14:21:27',14,'sdfgsdfgsdfg\\\\\r\n**baba**\\\\\r\ntutu\\\\\r\ncinq\r\n',13),('3fdd97ce-7b2e-4be5-bf49-d404bfe39bb8','','2019-07-08 11:17:17',7,'sdfgsdfgsdfg\\\\\r\n**baba**',13),('4709c012-ea35-4b86-92c3-ee9ae030e78c','','2019-06-07 13:33:40',1,'clé controllée 22',7),('579a9700-7f4c-4807-8b7f-e11d782d31e7','','2019-06-07 13:15:39',1,'clé controllée 22',3),('5b682f2f-bf97-4c39-813e-6940119270cc','','2019-07-08 13:30:09',9,'sdfgsdfgsdfg\\\\\r\n**baba**\\\\\r\ntutu\r\n',13),('5d577c15-fdce-4f8b-aad0-87e60ba8e656','','2019-07-08 09:48:32',1,'sdfgsdfgsdfg****',13),('7a71bc4f-1a90-4ca1-8b6f-7fe3f22dbf0b','','2019-07-02 15:23:49',2,'Contenu de la page pour la competition',10),('7e718742-519a-4079-8650-aad3b94b8061','','2019-07-08 11:07:34',6,'sdfgsdfgsdfg****',13),('8291bbec-639b-4cad-8b26-e8744b99ea35','','2019-07-02 15:16:15',1,'ceci est une page pour la competition 2',9),('85c96efe-c965-409c-b947-6747eae35444','','2019-07-04 06:57:28',2,'Description de la competition 3',11),('8aef365c-9adc-456d-8f63-0ecd8fe269d7','','2019-06-07 13:33:40',1,'clé controllée 23',8),('8ff41665-d0c8-4db5-9ee3-be18e292765a','','2019-07-08 09:36:44',1,' qssdflkj  **qmslkdf **qsd\r\nf qsdf\r\n qsdf qdsf qsdf qsdklfj qsdf\r\n qsdf qsdfs\\\\ sdf \r\nsdf qsdf\r\n qsdf qsdf\r\n',12),('90fb51b9-3024-499f-a4e3-519d62b9fe02','','2019-07-08 13:53:55',12,'sdfgsdfgsdfg\\\\\r\n**baba**\\\\\r\ntutu\r\n',13),('929c3203-311a-4705-a03b-ee4103a39616','','2019-06-07 13:15:39',1,'clé not controllée 21',2),('931551a0-98b8-4be9-a02b-0ec2f85a4fe9','','2019-06-07 13:17:31',2,'Bienvenue pour cette nouvelle competition',1),('9f35b400-9b91-47af-a458-d8a3ad8e11cc','','2019-07-08 13:12:05',8,'sdfgsdfgsdfg\\\\\r\n**baba**\\\\\r\ntutu\r\n',13),('a09d924b-7694-4c4d-b1fa-6df2c4228884','','2019-07-08 14:21:49',15,'sdfgsdfgsdfg\\\\\r\n**baba**\\\\\r\ntutu\\\\\r\ncinq\r\n',13),('a64ac180-9105-4697-886e-9b888f405a88','','2019-07-08 11:02:58',2,'sdfgsdfgsdfg****',13),('aed9e2c2-b912-4bbd-89e0-4ba8b66b82e6','','2019-07-02 15:21:20',1,'fghjfghj',10),('b90b6e4d-5a74-46af-a9ed-90b3dc00ccb5','','2019-07-08 13:32:56',10,'sdfgsdfgsdfg\\\\\r\n**baba**\\\\\r\ntutu\r\n',13),('c083b7b3-80ab-4d63-bac9-0be7438e5d72','','2019-06-07 13:15:39',1,'Bienvenue pour cette noubelle competition',1),('c2bd6f76-26f9-457a-9c7e-f16fbf1be9d5','','2019-07-08 11:04:03',3,'sdfgsdfgsdfg****',13),('d019bc77-426a-46de-bcde-e6747ed3703f','','2019-06-07 13:15:39',1,'clé controllée 23',4),('d18e21b6-4652-4af4-b92e-2153ead429a8','','2019-06-07 13:33:40',1,'Bienvenue pour cette noubelle competition',5),('d7958951-eff7-456d-82a2-15ad0d950fad','','2019-07-04 06:57:01',1,'Description de la competition 3',11),('d9612bac-e490-489c-87b7-e9e4e7e63814','','2019-07-08 13:52:39',11,'sdfgsdfgsdfg\\\\\r\n**baba**\\\\\r\ntutu\r\n',13),('de88ded9-768a-4b08-aea3-cfc24f235673','','2019-07-02 15:04:59',3,'Bienvenue pour cette nouvelle competition',1),('e4fd87e5-37b3-4465-aca5-79eb2a8890cc','','2019-07-08 11:06:58',5,'sdfgsdfgsdfg****',13),('f3a761f5-2553-4db4-9217-84a09847ca82','','2019-07-08 13:56:05',13,'sdfgsdfgsdfg\\\\\r\n**baba**\\\\\r\ntutu\\\\\r\ncinq\r\n',13),('f52cb129-1cf6-4ae5-bae2-0dac3156f25f','','2019-07-08 11:06:20',4,'sdfgsdfgsdfg****',13),('fbdb599e-25e2-49a7-8d4f-2b1823f5ae6a','','2019-07-10 14:17:11',2,' qssdflkj  **qmslkdf **qsd\r\nf qsdf\r\n qsdf qdsf qsdf qsdklfj qsdf\r\n qsdf qsdfs\\\\ sdf \r\nsdf qsdf\r\n qsdf qsdf\r\n',12),('fe9216e2-15f3-48af-a625-6920ece0959e','','2019-06-07 13:33:40',1,'clé not controllée 21',6);
/*!40000 ALTER TABLE `explik_pagecontent` ENABLE KEYS */;
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
