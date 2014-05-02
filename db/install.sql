
/***********************************************
--
--本文件包含一套创建mcsdb数据库的SQL脚本
--用来创建一个全新数据库及所有数据库对象
--可用如下命令来执行
--osql -U [dbuser] -n -i install.sql
--
--<<<<约定>>>>
--1)	表和视图命名原则
--档案类表  ：TA_
--基本数据表：TB_
--分析数据表：TD_
--标准配置表: TC_
--关联数据表：TRE__表A_表B 
--视图：在表命名的基础上加前缀改为V_
--后缀：主-明细结构的表，主表为 _M；明细表为 _L
--单词首写字母要大写，多个单词间不用任何连接符号如：TCRM_UserType
--
--2)	列命名规则
--采用第一个字母大写，命名规则只来自于业务，尽量表达出列的含义
--
--3)	过程、函数、触发器命名原则
--过程：SP_
--函数：FN_
--触发器：TR_表名_后面插入加I,修改加U,删除加D
--
--4)	自定义数据类型、默认、规则命名原则
--自定义数据类型：UD_
--默认: DF_，对于非绑定的默认可取系统默认的名字
--规则：RU_，对于非绑定规则(约束) 可取系统默认的名字
--
--5)	主键、外键关系、索引命名原则
--主键: PK_表名
--外键：FK_主表_从表
--索引：ID_列名,复合索引列名间用_隔开
--
***********************************************/

/***********************************************
--
--创建数据库
--
***********************************************/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'mcsdb')
	DROP DATABASE [mcsdb]
GO

IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'mcsdb')
BEGIN
CREATE DATABASE [mcsdb]  ON (NAME = N'mcsdb_Data', FILENAME = N'E:\DBdata\mcsdb_Data.MDF' , SIZE = 10, FILEGROWTH = 10%)
 LOG ON (NAME = N'mcsdb_Log', FILENAME = N'E:\DBdata\mcsdb_Log.LDF' , SIZE = 2, FILEGROWTH = 10%)
 COLLATE Chinese_PRC_CI_AS

PRINT ''
PRINT '>已创建数据库[mcsdb]'
END
GO

use [mcsdb]
GO

/***********************************************
--
--创建登录用户
--
***********************************************/
if not exists (select * from master.dbo.syslogins where loginname = N'mcsuser')
BEGIN
	declare @logindb nvarchar(132), @loginlang nvarchar(132) select @logindb = N'mcsdb', @loginlang = N'简体中文'
	if @logindb is null or not exists (select * from master.dbo.sysdatabases where name = @logindb)
		select @logindb = N'master'
	if @loginlang is null or (not exists (select * from master.dbo.syslanguages where name = @loginlang) and @loginlang <> N'us_english')
		select @loginlang = @@language
	exec sp_addlogin N'mcsuser', '123', @logindb, @loginlang
END
GO

if not exists (select * from dbo.sysusers where name = N'mcsuser1' and uid < 16382)
	EXEC sp_grantdbaccess N'mcsuser', N'mcsuser1'
GO

exec sp_addrolemember N'db_datareader', N'mcsuser1'
GO

exec sp_addrolemember N'db_datawriter', N'mcsuser1'
GO

/***********************************************
--
--创建数据库中的对象
--
***********************************************/

/***********************************************
--
--表名：TB_Example1
--说明：用作示例
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_Example1]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_Example1]
GO

CREATE TABLE [dbo].[TB_Example1] (
	[field1] [int] NOT NULL ,                                           --每个字段要给出字段说明
	[field2] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,            --每个字段要给出字段说明
	[field3] [float] NULL ,                                             --每个字段要给出字段说明
	[xmlfield] [varchar] (7900) COLLATE Chinese_PRC_CI_AS NULL ,        --每个字段要给出字段说明
	[xmlfield1] [text] COLLATE Chinese_PRC_CI_AS NULL                   --每个字段要给出字段说明
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TB_Example1] WITH NOCHECK ADD 
	CONSTRAINT [PK_TB_Example1] PRIMARY KEY  CLUSTERED 
	(
		[field1]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>已创建表[TB_Example1]'

/***********************************************
--
--表名：TB_Event
--说明：事件记录表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_Event]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_Event]
GO

CREATE TABLE [dbo].[TB_Event] (
	[EmplyeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,          --员工号
	[EventTime] [DateTime] NOT NULL ,                                        --时间
	[EventRecord] [varchar] (2000) COLLATE Chinese_PRC_CI_AS NOT NULL        --事件记录
) ON [PRIMARY]
GO

CREATE  INDEX [IX_TB_Event] ON [dbo].[TB_Event]([EventTime]) ON [PRIMARY]
GO

PRINT ''
PRINT '>已创建表[TB_Event]'

/***********************************************
--
--表名：TA_Model
--说明：品号档案表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Model]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Model]
GO

CREATE TABLE [dbo].[TA_Model] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	    --品号
	[Name] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,          --名称
	[Code] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,          --代号
	[CustomerID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,	    --客户品号
	[ModelType] [int] NOT NULL                                          --品号类型（1-产品；2-部件；3-物料）
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_Model] WITH NOCHECK ADD 
	CONSTRAINT [PK_TA_Model] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>已创建表[TA_Model]'

/***********************************************
--
--表名：TA_Plan
--说明：生产计划单表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Plan]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Plan]
GO

CREATE TABLE [dbo].[TA_Plan] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,          --生产计划号
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,     --产品品号
	[Output] [int] NOT NULL ,                                         --产量
	[OrderID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,         --客户订单号
	[PlanType] [int] NOT NULL ,                                       --生产计划单类型（1-常规;2-客退）
	[Remark] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL ,         --备注
	[FoundTime] [DateTime] NOT NULL ,				  --建立时间
	[CloseTime] [DateTime] NULL ,					  --关闭时间
	[State] [int] NOT NULL                                            --生产计划单状态（1-初始;2-激活;3-关闭）
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_Plan] WITH NOCHECK ADD 
	CONSTRAINT [DF_TA_Plan_State] DEFAULT (1) FOR [State],
	CONSTRAINT [PK_TA_Plan] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TA_Plan] ADD 
	CONSTRAINT [FK_TA_Plan_TA_Model] FOREIGN KEY 
	(
		[ModelID]
	) REFERENCES [dbo].[TA_Model] (
		[ID]
	)
GO

PRINT ''
PRINT '>已创建表[TA_Plan]'

/***********************************************
--
--表名：TC_PlanProcedure
--说明：计划工艺流程表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TC_PlanProcedure]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TC_PlanProcedure]
GO

CREATE TABLE [dbo].[TC_PlanProcedure] (
	[PlanID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,        --生产计划号
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,       --品号
	[ProcedureID] [int] NOT NULL                                        --工艺流程编号
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TC_PlanProcedure] WITH NOCHECK ADD 
	CONSTRAINT [PK_TC_PlanProcedure] PRIMARY KEY  CLUSTERED 
	(
		[PlanID],
		[ModelID]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>已创建表[TC_PlanProcedure]'

/***********************************************
--
--表名：TA_Structure
--说明：产品结构定义表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Structure]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Structure]
GO

CREATE TABLE [dbo].[TA_Structure] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	--品号
	[ItemID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,    --组成项品号
	[Amount] [int] NOT NULL                                         --组成项数量
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_Structure] WITH NOCHECK ADD 
	CONSTRAINT [PK_TA_Structure] PRIMARY KEY  CLUSTERED 
	(
		[ID],
		[ItemID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TA_Structure] ADD 
	CONSTRAINT [FK_TA_Structure_TA_Model] FOREIGN KEY 
	(
		[ID]
	) REFERENCES [dbo].[TA_Model] (
		[ID]
	),
	CONSTRAINT [FK_TA_Structure_TA_Model1] FOREIGN KEY 
	(
		[ItemID]
	) REFERENCES [dbo].[TA_Model] (
		[ID]
	)
GO

PRINT ''
PRINT '>已创建表[TA_Structure]'

/***********************************************
--
--表名：TA_Employee
--说明：员工档案表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Employee]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Employee]
GO

CREATE TABLE [dbo].[TA_Employee] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	--工号
	[Name] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,      --员工名称
	[Password] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,  --登录密码
	[State] [int] NOT NULL ,                                        --状态（1-启用;2-停用）
	[Remark] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL         --备注
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_Employee] WITH NOCHECK ADD 
	CONSTRAINT [PK_TA_Employee] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>已创建表[TA_Employee]'

/***********************************************
--
--表名：TA_Role
--说明：角色档案表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Role]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Role]
GO

CREATE TABLE [dbo].[TA_Role] (
	[Role] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,      --角色
	[Remark] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL         --备注
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_Role] WITH NOCHECK ADD 
	CONSTRAINT [PK_TA_Role] PRIMARY KEY  CLUSTERED 
	(
		[Role]
	)  ON [PRIMARY] 
GO

INSERT INTO TA_Role(Role) VALUES ('作业员')
INSERT INTO TA_Role(Role) VALUES ('维修员')
INSERT INTO TA_Role(Role) VALUES ('物料管理员')
INSERT INTO TA_Role(Role) VALUES ('生产主管')
INSERT INTO TA_Role(Role) VALUES ('班组长')
INSERT INTO TA_Role(Role) VALUES ('人事管理员')
INSERT INTO TA_Role(Role) VALUES ('计划员')
INSERT INTO TA_Role(Role) VALUES ('制造工程师')
INSERT INTO TA_Role(Role) VALUES ('系统管理员')
--INSERT INTO TA_Role(Role) VALUES ('超级管理员')
INSERT INTO TA_Role(Role) VALUES ('一级数据查看者')
INSERT INTO TA_Role(Role) VALUES ('二级数据查看者')
INSERT INTO TA_Role(Role) VALUES ('三级数据查看者')
GO

PRINT ''
PRINT '>已创建表[TA_Role]'

/***********************************************
--
--表名：TRE_Employee_Role
--说明：作业员与角色的关系表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TRE_Employee_Role]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TRE_Employee_Role]
GO

CREATE TABLE [dbo].[TRE_Employee_Role] (
	[EmployeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,				--工号
	[Role] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL        				--角色
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TRE_Employee_Role] WITH NOCHECK ADD 
	CONSTRAINT [PK_TRE_Employee_Role] PRIMARY KEY  CLUSTERED 
	(
		[EmployeeID],
		[Role]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TRE_Employee_Role] ADD 
	CONSTRAINT [FK_TRE_Employee_Role_TA_Employee] FOREIGN KEY 
	(
		[EmployeeID]
	) REFERENCES [dbo].[TA_Employee] (
		[ID]
	),
	CONSTRAINT [FK_TRE_Employee_Role_TA_Role] FOREIGN KEY 
	(
		[Role]
	) REFERENCES [dbo].[TA_Role] (
		[Role]
	)
GO

PRINT ''
PRINT '>已创建表[TRE_Employee_Role]'

/***********************************************
--
--表名：TA_Group
--说明：班组档案表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Group]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Group]
GO

CREATE TABLE [dbo].[TA_Group] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,				      --编号（数据库内部标识）
	[Name] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,            --名称
	[LeaderID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,        --班组长工号
	[WorkDispatch] [varchar] (2000) COLLATE Chinese_PRC_CI_AS NULL	      --员工工位安排
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_Group] WITH NOCHECK ADD 
	CONSTRAINT [PK_TA_Group] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TA_Group] ADD 
	CONSTRAINT [IX_TA_Group] UNIQUE  NONCLUSTERED 
	(
		[Name]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TA_Group] ADD 
	CONSTRAINT [FK_TA_Group_TA_Employee] FOREIGN KEY 
	(
		[LeaderID]
	) REFERENCES [dbo].[TA_Employee] (
		[ID]
	)
GO

PRINT ''
PRINT '>已创建表[TA_Group]'

/***********************************************
--
--表名：TRE_Group_Employee
--说明：班组与作业员的关系表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TRE_Group_Employee]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TRE_Group_Employee]
GO

CREATE TABLE [dbo].[TRE_Group_Employee] (
	[GroupID] [int] NOT NULL ,								--班组编号
	[EmployeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL				--员工工号
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TRE_Group_Employee] WITH NOCHECK ADD 
	CONSTRAINT [PK_TRE_Group_Employee] PRIMARY KEY  CLUSTERED 
	(
		[GroupID],
		[EmployeeID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TRE_Group_Employee] ADD 
	CONSTRAINT [FK_TRE_Group_Employee_TA_Employee] FOREIGN KEY 
	(
		[EmployeeID]
	) REFERENCES [dbo].[TA_Employee] (
		[ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_TRE_Group_Employee_TA_Group] FOREIGN KEY 
	(
		[GroupID]
	) REFERENCES [dbo].[TA_Group] (
		[ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

PRINT ''
PRINT '>已创建表[TRE_Group_Employee]'

/***********************************************
--
--表名：TA_BugType
--说明：缺陷类别档案表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_BugType]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_BugType]
GO

CREATE TABLE [dbo].[TA_BugType] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,          --缺陷条码
	[Bug] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL           --缺陷
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_BugType] WITH NOCHECK ADD 
	CONSTRAINT [PK_TA_BugType] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TA_BugType] ADD 
	CONSTRAINT [IX_TA_BugType] UNIQUE  NONCLUSTERED 
	(
		[Bug]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>已创建表[TA_BugType]'

/***********************************************
--
--表名：TA_BugPoint
--说明：缺陷定位点档案表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_BugPoint]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_BugPoint]
GO

CREATE TABLE [dbo].[TA_BugPoint] (
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,        --品号
	[BugPointCode] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,   --缺陷定位点代码
	[BugPointDsc] [varchar] (100) COLLATE Chinese_PRC_CI_AS NOT NULL     --缺陷定位点
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_BugPoint] WITH NOCHECK ADD 
	CONSTRAINT [PK_TA_BugPoint] PRIMARY KEY  CLUSTERED 
	(
		[ModelID],
		[BugPointCode]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TA_BugPoint] ADD 
	CONSTRAINT [IX_TA_BugPoint] UNIQUE  NONCLUSTERED 
	(
		[ModelID],
		[BugPointDsc]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TA_BugPoint] ADD 
	CONSTRAINT [FK_TA_BugPoint_TA_Model] FOREIGN KEY 
	(
		[ModelID]
	) REFERENCES [dbo].[TA_Model] (
		[ID]
	)
GO

PRINT ''
PRINT '>已创建表[TA_BugPoint]'

/***********************************************
--
--表名：TA_Product
--说明：产品档案表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Product]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Product]
GO

CREATE TABLE [dbo].[TA_Product] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	     	--产品序列号|生产条码
	[SN] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,  	     	--产品条码|客户条码
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,        	--品号
	[ModelType] [int] NOT NULL ,                                         	--品号类型（1-产品；2-部件）
	[PlanID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL , 	    	--生产计划号
	[ProcedureID] [int] NOT NULL ,                                       	--工艺流程编号
	[FoundTime] [DateTime] NOT NULL ,					--建立时间
	[ManufactureState] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,   	--产品当前生产状态即当前工序类别（返修、报废、1装配、2总装、3调试、4检验、5包装、6入库等；为NULL表示初始态）
	[Remark] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL 		--备注
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_Product] WITH NOCHECK ADD 
	CONSTRAINT [PK_TA_Product] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TA_Product] ADD 
	CONSTRAINT [FK_TA_Product_TA_Model] FOREIGN KEY 
	(
		[ModelID]
	) REFERENCES [dbo].[TA_Model] (
		[ID]
	),
	CONSTRAINT [FK_TA_Product_TA_Plan] FOREIGN KEY 
	(
		[PlanID]
	) REFERENCES [dbo].[TA_Plan] (
		[ID]
	),
	CONSTRAINT [FK_TA_Product_TA_ProductID] FOREIGN KEY 
	(
		[ID]
	) REFERENCES [dbo].[TA_ProductID] (
		[ID]
	)
GO

PRINT ''
PRINT '>已创建表[TA_Product]'

/***********************************************
--
--表名：TA_Materiel
--说明：物料档案表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Materiel]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Materiel]
GO

CREATE TABLE [dbo].[TA_Materiel] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		--物料条码
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,        	--品号
	[Batch] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,          	--批次号
	[Vendor] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,         	--供应商
	[EmployeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,         	--生产员工工号（只针对自产物料）
	[FoundTime] [DateTime] NOT NULL ,					--建立时间
	[Remark] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL 		--备注
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_Materiel] WITH NOCHECK ADD 
	CONSTRAINT [PK_TA_Materiel] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TA_Materiel] ADD 
	CONSTRAINT [FK_TA_Materiel_TA_Model] FOREIGN KEY 
	(
		[ModelID]
	) REFERENCES [dbo].[TA_Model] (
		[ID]
	)
GO

PRINT ''
PRINT '>已创建表[TA_Materiel]'

/***********************************************
--
--表名：TA_Relationship
--说明：产品、部件及物料的关联表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Relationship]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Relationship]
GO

CREATE TABLE [dbo].[TA_Relationship] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		     --主序列号条码（产品或部件序列号）
	[ItemID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		     --组成项条码（部件或物料条码）
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_Relationship] WITH NOCHECK ADD 
	CONSTRAINT [PK_TA_Relationship] PRIMARY KEY  CLUSTERED 
	(
		[ID],
		[ItemID]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>已创建表[TA_Relationship]'

/***********************************************
--
--表名：TA_Procedure
--说明：工艺流程档案表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Procedure]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Procedure]
GO

CREATE TABLE [dbo].[TA_Procedure] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,				     --编号（数据库内部标识）
	[Name] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,           --名称
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,        --品号
	[ProcessConfig] [text] COLLATE Chinese_PRC_CI_AS NULL ,              --工序配置（XML格式，具体参见详细设计说明书）
	[ProcessGraph] [text] COLLATE Chinese_PRC_CI_AS NULL                 --工序可视化信息（XML格式，具体参见详细设计说明书）
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_Procedure] WITH NOCHECK ADD 
	CONSTRAINT [PK_TA_Procedure] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TA_Procedure] ADD 
	CONSTRAINT [IX_TA_Procedure] UNIQUE  NONCLUSTERED 
	(
		[Name]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>已创建表[TA_Procedure]'


/***********************************************
--
--表名：TB_GroupMateriel
--说明：班组物料表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_GroupMateriel]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_GroupMateriel]
GO

CREATE TABLE [dbo].[TB_GroupMateriel] (
	[GroupID] [int] NOT NULL ,						--班组编号
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,         	--品号
	[MaterielID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL        	--物料条码
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TB_GroupMateriel] WITH NOCHECK ADD 
	CONSTRAINT [PK_TB_GroupMateriel] PRIMARY KEY  CLUSTERED 
	(
		[GroupID],
		[ModelID],
		[MaterielID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TB_GroupMateriel] ADD 
	CONSTRAINT [FK_TB_GroupMateriel_TA_Group] FOREIGN KEY 
	(
		[GroupID]
	) REFERENCES [dbo].[TA_Group] (
		[ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

PRINT ''
PRINT '>已创建表[TB_GroupMateriel]'

/***********************************************
--
--表名：TB_ProcedureState
--说明：生产流程状态表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_ProcedureState]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_ProcedureState]
GO

CREATE TABLE [dbo].[TB_ProcedureState] (
	[ProductID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		   --产品序列号
	[Process] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		   --已完成工序名称
--	[Data] [text] COLLATE Chinese_PRC_CI_AS NULL ,                             --数据
	[DataID] [uniqueidentifier] NOT NULL                                       --数据标识，指向TB_ProcedureHistory相应字段
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TB_ProcedureState] WITH NOCHECK ADD 
	CONSTRAINT [PK_TB_ProcedureState] PRIMARY KEY  CLUSTERED 
	(
		[ProductID],
		[Process]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>已创建表[TB_ProcedureState]'

/***********************************************
--
--表名：TB_ProcedureHistory
--说明：生产流程历史表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_ProcedureHistory]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_ProcedureHistory]
GO

CREATE TABLE [dbo].[TB_ProcedureHistory] (
	[ProductID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		        --产品序列号
	[Process] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		        --工序名称
	[EmployeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		--员工工号
	[Result] [int] NULL ,								--作业结果|生产状态（NULL－工序未结束；0－正常；1－需返修；2－流程变更）
	[Exception] [varchar] (2000) COLLATE Chinese_PRC_CI_AS NULL ,              	--异常现象描述
	[Data] [text] COLLATE Chinese_PRC_CI_AS NULL ,                             	--数据
	[DataID] [uniqueidentifier] NOT NULL ,                                     	--数据标识，为一个GUID值
	[Dispatch] [varchar] (2000) COLLATE Chinese_PRC_CI_AS NULL ,		        --员工工位安排
	[BeginTime] [DateTime] NOT NULL ,				                --工序开始时间
	[EndTime] [DateTime] NULL  						        --工序结束时间
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TB_ProcedureHistory] ADD 
	CONSTRAINT [PK_TB_ProcedureHistory] PRIMARY KEY  CLUSTERED 
	(
		[ProductID],
		[Process],
		[BeginTime]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>已创建表[TB_ProcedureHistory]'

/***********************************************
--
--表名：TB_RepairRecord
--说明：产品返修记录表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_RepairRecord]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_RepairRecord]
GO

CREATE TABLE [dbo].[TB_RepairRecord] (
	[ProductID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		        --产品序列号
	[DetectTime] [DateTime] NOT NULL ,						--发现异常的时间
	[DetectProcess] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		--发现异常的工序
	[DetectEmployeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		--发现异常的员工工号
	[Exception] [varchar] (2000) COLLATE Chinese_PRC_CI_AS NOT NULL ,          	--异常现象描述
	[BugID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,                    	--缺陷条码
	[BugPointCode] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,             	--缺陷定位点
	[RepairTime] [DateTime]  NULL ,						        --修理时间
	[RepairEmployeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,		--修理员工工号
	[RepairInfo] [varchar] (2000) COLLATE Chinese_PRC_CI_AS NULL ,             	--修理信息
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TB_RepairRecord] WITH NOCHECK ADD 
	CONSTRAINT [PK_TB_RepairRecord] PRIMARY KEY  CLUSTERED 
	(
		[ProductID],
		[DetectTime]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>已创建表[TB_RepairRecord]'

/***********************************************
--
--表名：TB_InstrumentCalRecord
--说明：仪表校准记录表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_InstrumentCalRecord]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_InstrumentCalRecord]
GO

CREATE TABLE [dbo].[TB_InstrumentCalRecord] (
	[Model] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		--仪表型号
	[SN] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		--仪表序列号
	[ProductModel] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	--产品品号
	[EmployeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	--校准者工号
	[CalTime] [DateTime] NOT NULL ,  					--校准时间
	[Data] [text] COLLATE Chinese_PRC_CI_AS NOT NULL                       	--校准数据
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TB_InstrumentCalRecord] ADD 
	CONSTRAINT [PK_TB_InstrumentCalRecord] PRIMARY KEY  CLUSTERED 
	(
		[Model],
		[SN],
		[CalTime]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>已创建表[TB_InstrumentCalRecord]'

/***********************************************
--
--表名：TB_SoftwareVersion
--说明：软件版本表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_SoftwareVersion]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_SoftwareVersion]
GO

CREATE TABLE [dbo].[TB_SoftwareVersion] (
	[Name] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		         --软件名称
	[Version] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL		         --软件版本号
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TB_SoftwareVersion] ADD 
	CONSTRAINT [PK_TB_SoftwareVersion] PRIMARY KEY  CLUSTERED 
	(
		[Name]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>已创建表[TB_SoftwareVersion]'

/***********************************************
--
--表名：TB_TestConfigVer
--说明：中试软件版本表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_TestConfigVer]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_TestConfigVer]
GO

CREATE TABLE [dbo].[TB_TestConfigVer] (
	[Name] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		         --软件名称
	[Version] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL		         --软件版本号
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TB_TestConfigVer] ADD 
	CONSTRAINT [PK_TB_TestConfigVer] PRIMARY KEY  CLUSTERED 
	(
		[Name]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>已创建表[TB_TestConfigVer]'

/***********************************************
--
--表名：TRE_Group_Plan
--说明：班组计划表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TRE_Group_Plan]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TRE_Group_Plan]
GO

CREATE TABLE [dbo].[TRE_Group_Plan] (
	[GroupID] [int] NOT NULL ,								--班组编号
	[PlanID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL				--计划编号
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TRE_Group_Plan] WITH NOCHECK ADD 
	CONSTRAINT [PK_TRE_Group_Plan] PRIMARY KEY  CLUSTERED 
	(
		[GroupID],
		[PlanID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TRE_Group_Plan] ADD 
	CONSTRAINT [FK_TRE_Group_Plan_TA_Plan] FOREIGN KEY 
	(
		[PlanID]
	) REFERENCES [dbo].[TA_Plan] (
		[ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_TRE_Group_Plan_TA_Group] FOREIGN KEY 
	(
		[GroupID]
	) REFERENCES [dbo].[TA_Group] (
		[ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

PRINT ''
PRINT '>已创建表[TRE_Group_Plan]'

/***********************************************
--
--表名：TA_ProductID
--说明：产品条码档案表
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_ProductID]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_ProductID]
GO

CREATE TABLE [dbo].[TA_ProductID] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	     	--生产条码
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,       --品号
	CONSTRAINT PK_TA_ProductID PRIMARY KEY (ID)
) ON [PRIMARY]
GO

PRINT ''
PRINT '>已创建表[TA_ProductID]'
