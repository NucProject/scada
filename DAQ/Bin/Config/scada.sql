﻿
#
# ReadMe: Run Scada.DAQ.Installer.exe --init-database
# To Create Database and tables.
#

DROP TABLE IF EXISTS `HPGe_rec`;
CREATE TABLE `HPGe_rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`Sid` varchar(32) DEFAULT NULL,
`Path` varchar(256) DEFAULT NULL,
) ENGINE=MyISAM DEFAULT CHARSET=utf8 PARTITION BY RANGE (TO_DAYS(Time)) 
(
PARTITION y2014a VALUES LESS THAN (TO_DAYS('2014-05-01')),
PARTITION y2014b VALUES LESS THAN (TO_DAYS('2014-09-01')),
PARTITION y2014c VALUES LESS THAN (TO_DAYS('2015-01-01')),
PARTITION y2015a VALUES LESS THAN (TO_DAYS('2015-05-01')),
PARTITION y2015b VALUES LESS THAN (TO_DAYS('2015-09-01')),
PARTITION y2015c VALUES LESS THAN (TO_DAYS('2016-01-01')),
PARTITION y2016a VALUES LESS THAN (TO_DAYS('2016-05-01')),
PARTITION y2016b VALUES LESS THAN (TO_DAYS('2016-09-01')),
PARTITION y2016c VALUES LESS THAN (TO_DAYS('2017-01-01')),
PARTITION yend VALUES LESS THAN MAXVALUE );



DROP TABLE IF EXISTS `weather_rec`;
CREATE TABLE `weather_rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`Windspeed` varchar(8) DEFAULT NULL,
`Direction` char(5) DEFAULT NULL,
`Temperature` char(8) DEFAULT NULL,
`Humidity` char(8) DEFAULT NULL,
`Pressure` varchar(8) DEFAULT NULL,
`Raingauge` varchar(8) DEFAULT NULL,
`Rainspeed` varchar(8) DEFAULT NULL,
`Dewpoint` varchar(8) DEFAULT NULL,
`IfRain` bit(1) DEFAULT NULL,
`Alarm` bit(1) DEFAULT NULL,
PRIMARY KEY (`Time`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 PARTITION BY RANGE (TO_DAYS(Time)) 
(
PARTITION y2014a VALUES LESS THAN (TO_DAYS('2014-05-01')),
PARTITION y2014b VALUES LESS THAN (TO_DAYS('2014-09-01')),
PARTITION y2014c VALUES LESS THAN (TO_DAYS('2015-01-01')),
PARTITION y2015a VALUES LESS THAN (TO_DAYS('2015-05-01')),
PARTITION y2015b VALUES LESS THAN (TO_DAYS('2015-09-01')),
PARTITION y2015c VALUES LESS THAN (TO_DAYS('2016-01-01')),
PARTITION y2016a VALUES LESS THAN (TO_DAYS('2016-05-01')),
PARTITION y2016b VALUES LESS THAN (TO_DAYS('2016-09-01')),
PARTITION y2016c VALUES LESS THAN (TO_DAYS('2017-01-01')),
PARTITION yend VALUES LESS THAN MAXVALUE );



DROP TABLE IF EXISTS `hpic_rec`;
CREATE TABLE `hpic_rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`Doserate` varchar(18) DEFAULT NULL,
`Highvoltage` varchar(8) DEFAULT NULL,
`Battery` varchar(8) DEFAULT NULL,
`Temperature` varchar(8) DEFAULT NULL,
`Alarm` bit(1) DEFAULT NULL,
PRIMARY KEY (`Time`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 PARTITION BY RANGE (TO_DAYS(Time)) 
(
PARTITION y2014a VALUES LESS THAN (TO_DAYS('2014-05-01')),
PARTITION y2014b VALUES LESS THAN (TO_DAYS('2014-09-01')),
PARTITION y2014c VALUES LESS THAN (TO_DAYS('2015-01-01')),
PARTITION y2015a VALUES LESS THAN (TO_DAYS('2015-05-01')),
PARTITION y2015b VALUES LESS THAN (TO_DAYS('2015-09-01')),
PARTITION y2015c VALUES LESS THAN (TO_DAYS('2016-01-01')),
PARTITION y2016a VALUES LESS THAN (TO_DAYS('2016-05-01')),
PARTITION y2016b VALUES LESS THAN (TO_DAYS('2016-09-01')),
PARTITION y2016c VALUES LESS THAN (TO_DAYS('2017-01-01')),
PARTITION yend VALUES LESS THAN MAXVALUE );


DROP TABLE IF EXISTS `HVSampler_rec`;
CREATE TABLE `HVSampler_rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`Sid` varchar(16) NOT NULL, /*采样ID,唯一号*/
`Volume` varchar(8), /*真空泵开关状态，单位：无；数据格式：0或1表示开关*/
`Flow` varchar(8), /*报警，单位：无；数据格式：0、1、2,，代表不同的报警类型，保留字段*/
`Hours` varchar(8),
`Status` bit,
`BeginTime` datetime,
`EndTime` datetime,
`Alarm1` bit, /*滤纸*/
`Alarm2` bit, /*流量*/
`Alarm3` bit, /*主电源*/
PRIMARY KEY (`Time`)
)ENGINE=MyISAM DEFAULT CHARSET=utf8 PARTITION BY RANGE (TO_DAYS(Time)) 
(
PARTITION y2014 VALUES LESS THAN (TO_DAYS('2015-01-01')),
PARTITION y2015 VALUES LESS THAN (TO_DAYS('2016-01-01')),
PARTITION y2016 VALUES LESS THAN (TO_DAYS('2017-01-01')),
PARTITION yend VALUES LESS THAN MAXVALUE );

DROP TABLE IF EXISTS `mds_rec`;
CREATE TABLE `mds_rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`Sid` char(48) NOT NULL, /*采样ID,唯一号*/
`BeginTime` datetime,
`EndTime` datetime,
`Volume` char(8), /*真空泵开关状态，单位：无；数据格式：0或1表示开关*/
`Flow` char(8), /*报警，单位：无；数据格式：0、1、2,，代表不同的报警类型，保留字段*/
`Hours` char(8),
`Status` bit,
`Alarm1` bit,
`Alarm2` bit,
`Alarm3` bit,
PRIMARY KEY (`Time`)
)ENGINE=MyISAM DEFAULT CHARSET=utf8 PARTITION BY RANGE (TO_DAYS(Time)) 
(
PARTITION y2014 VALUES LESS THAN (TO_DAYS('2015-01-01')),
PARTITION y2015 VALUES LESS THAN (TO_DAYS('2016-01-01')),
PARTITION y2016 VALUES LESS THAN (TO_DAYS('2017-01-01')),
PARTITION yend VALUES LESS THAN MAXVALUE );

DROP TABLE IF EXISTS `ISampler_rec`;
CREATE TABLE `ISampler_rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`Sid` varchar(16) NOT NULL, /*采样ID,唯一号*/
`Volume` varchar(8), /*真空泵开关状态，单位：无；数据格式：0或1表示开关*/
`Flow` varchar(8), /*报警，单位：无；数据格式：0、1、2,，代表不同的报警类型，保留字段*/
`Hours` varchar(8),
`Status` bit,
`BeginTime` datetime,
`EndTime` datetime,
`Alarm1` bit,
`Alarm2` bit,
`Alarm3` bit,
PRIMARY KEY (`Time`)
)ENGINE=MyISAM DEFAULT CHARSET=utf8 PARTITION BY RANGE (TO_DAYS(Time)) 
(
PARTITION y2014 VALUES LESS THAN (TO_DAYS('2015-01-01')),
PARTITION y2015 VALUES LESS THAN (TO_DAYS('2016-01-01')),
PARTITION y2016 VALUES LESS THAN (TO_DAYS('2017-01-01')),
PARTITION yend VALUES LESS THAN MAXVALUE );

DROP TABLE IF EXISTS `ais_rec`;
CREATE TABLE `ais_rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`Sid` char(48) NOT NULL, /*采样ID,唯一号*/
`BeginTime` datetime,
`EndTime` datetime,
`Volume` char(8), /*真空泵开关状态，单位：无；数据格式：0或1表示开关*/
`Flow` char(8), /*报警，单位：无；数据格式：0、1、2,，代表不同的报警类型，保留字段*/
`Hours` char(8),
`Status` bit,
`Alarm1` bit,
`Alarm2` bit,
`Alarm3` bit,
PRIMARY KEY (`Time`)
)ENGINE=MyISAM DEFAULT CHARSET=utf8 PARTITION BY RANGE (TO_DAYS(Time)) 
(
PARTITION y2014 VALUES LESS THAN (TO_DAYS('2015-01-01')),
PARTITION y2015 VALUES LESS THAN (TO_DAYS('2016-01-01')),
PARTITION y2016 VALUES LESS THAN (TO_DAYS('2017-01-01')),
PARTITION yend VALUES LESS THAN MAXVALUE );



DROP TABLE IF EXISTS `RDSampler_rec`;
CREATE TABLE `RDSampler_rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`IfRain` bit, /*感雨，单位：无，数据格式：0或1表示是否下雨*/
`Barrel` tinyint, /*桶状态，单位：无，数据格式：0、1、2表示哪个桶正在使用*/
`Alarm` bit,/*报警，单位：无；数据格式：0、1、2,，代表不同的报警类型，保留字段*/
`IsLidOpen` bit,
`CurrentRainTime` varchar(10) ,
`BeginTime` datetime DEFAULT NULL,/*开始时间，保留字段*/
`Endtime` datetime DEFAULT NULL,/*开始时间，保留字段*/
PRIMARY KEY (`Time`)
)ENGINE=MyISAM DEFAULT CHARSET=utf8 PARTITION BY RANGE (TO_DAYS(Time)) 
(
PARTITION y2014a VALUES LESS THAN (TO_DAYS('2014-05-01')),
PARTITION y2014b VALUES LESS THAN (TO_DAYS('2014-09-01')),
PARTITION y2014c VALUES LESS THAN (TO_DAYS('2015-01-01')),
PARTITION y2015a VALUES LESS THAN (TO_DAYS('2015-05-01')),
PARTITION y2015b VALUES LESS THAN (TO_DAYS('2015-09-01')),
PARTITION y2015c VALUES LESS THAN (TO_DAYS('2016-01-01')),
PARTITION y2016a VALUES LESS THAN (TO_DAYS('2016-05-01')),
PARTITION y2016b VALUES LESS THAN (TO_DAYS('2016-09-01')),
PARTITION y2016c VALUES LESS THAN (TO_DAYS('2017-01-01')),
PARTITION yend VALUES LESS THAN MAXVALUE );



DROP TABLE IF EXISTS `environment_rec`;
CREATE TABLE `environment_rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`Temperature` tinyint, /*温度，单位：℃，数据格式：N8*/
`Humidity` tinyint, /*湿度，单位：%，数据格式：N8*/
`IfMainPowerOff` bit,
`BatteryHours` varchar(10),
`IfSmoke` bit, /*烟感报警，单位：无，数据格式：0或1表示tinyint否报警*/
`IfWater` bit, /*浸水报警，单位：无，数据格式：0或1表示是否报警*/
`IfDoorOpen` bit, /*门禁报警，单位：无，数据格式：0或1表示是否报警*/
`Alarm` bit, /*报警，单位：无；数据格式：0、1、2,，代表不同的报警类型，保留字段*/
PRIMARY KEY (`Time`)
)ENGINE=MyISAM DEFAULT CHARSET=utf8 PARTITION BY RANGE (TO_DAYS(Time)) 
(
PARTITION y2014a VALUES LESS THAN (TO_DAYS('2014-05-01')),
PARTITION y2014b VALUES LESS THAN (TO_DAYS('2014-09-01')),
PARTITION y2014c VALUES LESS THAN (TO_DAYS('2015-01-01')),
PARTITION y2015a VALUES LESS THAN (TO_DAYS('2015-05-01')),
PARTITION y2015b VALUES LESS THAN (TO_DAYS('2015-09-01')),
PARTITION y2015c VALUES LESS THAN (TO_DAYS('2016-01-01')),
PARTITION y2016a VALUES LESS THAN (TO_DAYS('2016-05-01')),
PARTITION y2016b VALUES LESS THAN (TO_DAYS('2016-09-01')),
PARTITION y2016c VALUES LESS THAN (TO_DAYS('2017-01-01')),
PARTITION yend VALUES LESS THAN MAXVALUE );

DROP TABLE IF EXISTS `environment_door_rec`;
CREATE TABLE `environment_door_rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`IfDoorOpen` bit(1) DEFAULT NULL,
PRIMARY KEY (`Time`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;


DROP TABLE IF EXISTS `NaI_Rec`;
CREATE TABLE `NaI_Rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`StartTime` datetime, 
`EndTime` datetime, 
`Coefficients` varchar(48), 
`ChannelData` varchar(8000), 
`DoseRate` varchar(16), 
`Temperature` tinyint, 
`HighVoltage` varchar(16),
`NuclideFound` bit(1),
`EnergyFromPosition` varchar(16),
PRIMARY KEY (`Time`)
)ENGINE=MyISAM DEFAULT CHARSET=utf8 PARTITION BY RANGE (TO_DAYS(Time)) 
(
PARTITION y2014 VALUES LESS THAN (TO_DAYS('2015-01-01')),
PARTITION y2015 VALUES LESS THAN (TO_DAYS('2016-01-01')),
PARTITION y2016 VALUES LESS THAN (TO_DAYS('2017-01-01')),
PARTITION yend VALUES LESS THAN MAXVALUE );

DROP TABLE IF EXISTS `Labr_Rec`;
CREATE TABLE `Labr_Rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`StartTime` datetime, 
`EndTime` datetime, 
`Coefficients` varchar(48), 
`ChannelData` varchar(8000), 
`DoseRate` varchar(16), 
`Temperature` tinyint, 
`HighVoltage` varchar(16),
`NuclideFound` bit(1),
`EnergyFromPosition` varchar(16),
PRIMARY KEY (`Time`)
)ENGINE=MyISAM DEFAULT CHARSET=utf8 PARTITION BY RANGE (TO_DAYS(Time)) 
(
PARTITION y2014 VALUES LESS THAN (TO_DAYS('2015-01-01')),
PARTITION y2015 VALUES LESS THAN (TO_DAYS('2016-01-01')),
PARTITION y2016 VALUES LESS THAN (TO_DAYS('2017-01-01')),
PARTITION yend VALUES LESS THAN MAXVALUE );



DROP TABLE IF EXISTS `NaINuclide_Rec`;
CREATE TABLE `NaINuclide_Rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`Name` varchar(16),
`Activity` varchar(32),
`Indication` varchar(16),
`DoseRate` varchar(16),
`Channel` varchar(16), 
`Energy` varchar(16)
)ENGINE=MyISAM DEFAULT CHARSET=utf8 PARTITION BY RANGE (TO_DAYS(Time)) 
(
PARTITION y2014 VALUES LESS THAN (TO_DAYS('2015-01-01')),
PARTITION y2015 VALUES LESS THAN (TO_DAYS('2016-01-01')),
PARTITION y2016 VALUES LESS THAN (TO_DAYS('2017-01-01')),
PARTITION yend VALUES LESS THAN MAXVALUE );


DROP TABLE IF EXISTS `LabrNuclide_Rec`;
CREATE TABLE `LabrNuclide_Rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`Name` varchar(16),
`Activity` varchar(32),
`Indication` varchar(16),
`DoseRate` varchar(16),
`Channel` varchar(16), 
`Energy` varchar(16)
)ENGINE=MyISAM DEFAULT CHARSET=utf8 PARTITION BY RANGE (TO_DAYS(Time)) 
(
PARTITION y2014 VALUES LESS THAN (TO_DAYS('2015-01-01')),
PARTITION y2015 VALUES LESS THAN (TO_DAYS('2016-01-01')),
PARTITION y2016 VALUES LESS THAN (TO_DAYS('2017-01-01')),
PARTITION yend VALUES LESS THAN MAXVALUE );


#
#
DROP TABLE IF EXISTS `CinderellaData_Rec`;
CREATE TABLE `CinderellaData_Rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`DeviceTime` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`Sid` char(32),
`barcode` varchar(128),
`BeginTime` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`WorkTime` varchar(16),
`Flow` varchar(16),
`FlowPerHour` varchar(16), 
`Pressure` varchar(8) DEFAULT NULL,
`PressureDiff` varchar(8) DEFAULT NULL,
`Temperature` tinyint DEFAULT NULL,
PRIMARY KEY (`Time`)
)ENGINE=MyISAM DEFAULT CHARSET=utf8;

#
#
DROP TABLE IF EXISTS `CinderellaStatus_Rec`;
CREATE TABLE `CinderellaStatus_Rec` (
`Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00',
`StateBits` char(30)
)ENGINE=MyISAM DEFAULT CHARSET=utf8;
