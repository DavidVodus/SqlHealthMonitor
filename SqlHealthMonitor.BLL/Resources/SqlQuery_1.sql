﻿USE [master]
GO

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [SqlHealthMonitor]    Script Date: 6/14/2018 11:40:13 AM ******/
CREATE LOGIN [SqlHealthMonitor] WITH PASSWORD=N'123', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
USE [SqlHealthMonitorContext];
GO
/****** Object:  User [SqlHealthMonitor]    Script Date: 6/14/2018 11:44:49 AM ******/
CREATE USER [SqlHealthMonitor] FOR LOGIN [SqlHealthMonitor] WITH DEFAULT_SCHEMA=[dbo]
GO
/** for cpu widget***/
USE [master];
GRANT VIEW SERVER STATE TO SqlHealthMonitor
go
/** for jobs widget***/
USE [msdb];
CREATE USER [SqlHealthMonitor] FOR LOGIN [SqlHealthMonitor] WITH DEFAULT_SCHEMA=[dbo]
GRANT SELECT ON [dbo].[sysjobhistory] TO [SqlHealthMonitor]
GRANT SELECT ON [dbo].[sysjobschedules] TO [SqlHealthMonitor]
GRANT SELECT ON [dbo].[sysjobs] TO [SqlHealthMonitor]
GO