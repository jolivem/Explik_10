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
-- Table structure for table `explik_siteconfiguration`
--

DROP TABLE IF EXISTS `explik_siteconfiguration`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `explik_siteconfiguration` (
  `Id` varchar(36) NOT NULL,
  `Version` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Content` mediumtext NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `explik_siteconfiguration`
--

LOCK TABLES `explik_siteconfiguration` WRITE;
/*!40000 ALTER TABLE `explik_siteconfiguration` DISABLE KEYS */;
INSERT INTO `explik_siteconfiguration` VALUES ('2e60def0-5729-0000-0000-000000000000','1.0','{\r\n  \"PluginId\": \"ExternalLinksInNewWindow\",\r\n  \"Version\": \"1.0\",\r\n  \"IsEnabled\": false,\r\n  \"Values\": []\r\n}'),('3069cf0e-5729-0000-0000-000000000000','1.0','{\r\n  \"PluginId\": \"Jumbotron\",\r\n  \"Version\": \"1.0\",\r\n  \"IsEnabled\": false,\r\n  \"Values\": []\r\n}'),('501abe5f-5729-0000-0000-000000000000','1.0','{\r\n  \"PluginId\": \"MathJax\",\r\n  \"Version\": \"1.0\",\r\n  \"IsEnabled\": false,\r\n  \"Values\": []\r\n}'),('821cf995-5729-0000-0000-000000000000','1.0','{\r\n  \"PluginId\": \"ClickableImages\",\r\n  \"Version\": \"1.0\",\r\n  \"IsEnabled\": false,\r\n  \"Values\": []\r\n}'),('83c4b2c8-5729-0000-0000-000000000000','1.0','{\r\n  \"PluginId\": \"ResizeImages\",\r\n  \"Version\": \"1.0\",\r\n  \"IsEnabled\": false,\r\n  \"Values\": []\r\n}'),('b68bb256-5729-0000-0000-000000000000','1.0','{\r\n  \"PluginId\": \"SyntaxHighlighter\",\r\n  \"Version\": \"1.0\",\r\n  \"IsEnabled\": false,\r\n  \"Values\": []\r\n}'),('b960e8e5-529f-4f7c-aee4-28eb23e13dbd','2.0.275','{\r\n  \"AllowedFileTypes\": \"jpg,png,gif,zip,xml,pdf\",\r\n  \"AllowUserSignup\": false,\r\n  \"IsRecaptchaEnabled\": false,\r\n  \"MarkupType\": \"Creole\",\r\n  \"RecaptchaPrivateKey\": null,\r\n  \"RecaptchaPublicKey\": null,\r\n  \"SiteUrl\": \"http://localhost:3588\",\r\n  \"SiteName\": \"Explik\",\r\n  \"Theme\": \"Responsive\",\r\n  \"OverwriteExistingFiles\": false,\r\n  \"HeadContent\": \"\",\r\n  \"MenuMarkup\": \"* %mainpage%\\r\\n* %categories%\\r\\n* %allpages%\\r\\n* %allnewpages%\\r\\n* %allnewcomments%\\r\\n* %mypages%\\r\\n* %alerts%\\r\\n* %newpage%\\r\\n* %managefiles%\\r\\n* %competitions%\\r\\n* %sitesettings%\\r\\n\\r\\n\",\r\n  \"PluginLastSaveDate\": \"2019-06-07T13:14:22.66991Z\"\r\n}'),('cb1644e7-5729-0000-0000-000000000000','1.0','{\r\n  \"PluginId\": \"ToC\",\r\n  \"Version\": \"1.0\",\r\n  \"IsEnabled\": false,\r\n  \"Values\": []\r\n}');
/*!40000 ALTER TABLE `explik_siteconfiguration` ENABLE KEYS */;
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
