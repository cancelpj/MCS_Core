
/***********************************************
--
--���ļ�����һ�״���mcsdb���ݿ��SQL�ű�
--��������һ��ȫ�����ݿ⼰�������ݿ����
--��������������ִ��
--osql -U [dbuser] -n -i install.sql
--
--<<<<Լ��>>>>
--1)	�����ͼ����ԭ��
--�������  ��TA_
--�������ݱ�TB_
--�������ݱ�TD_
--��׼���ñ�: TC_
--�������ݱ�TRE__��A_��B 
--��ͼ���ڱ������Ļ����ϼ�ǰ׺��ΪV_
--��׺����-��ϸ�ṹ�ı�����Ϊ _M����ϸ��Ϊ _L
--������д��ĸҪ��д��������ʼ䲻���κ����ӷ����磺TCRM_UserType
--
--2)	����������
--���õ�һ����ĸ��д����������ֻ������ҵ�񣬾��������еĺ���
--
--3)	���̡�����������������ԭ��
--���̣�SP_
--������FN_
--��������TR_����_��������I,�޸ļ�U,ɾ����D
--
--4)	�Զ����������͡�Ĭ�ϡ���������ԭ��
--�Զ����������ͣ�UD_
--Ĭ��: DF_�����ڷǰ󶨵�Ĭ�Ͽ�ȡϵͳĬ�ϵ�����
--����RU_�����ڷǰ󶨹���(Լ��) ��ȡϵͳĬ�ϵ�����
--
--5)	�����������ϵ����������ԭ��
--����: PK_����
--�����FK_����_�ӱ�
--������ID_����,����������������_����
--
***********************************************/

/***********************************************
--
--�������ݿ�
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
PRINT '>�Ѵ������ݿ�[mcsdb]'
END
GO

use [mcsdb]
GO

/***********************************************
--
--������¼�û�
--
***********************************************/
if not exists (select * from master.dbo.syslogins where loginname = N'mcsuser')
BEGIN
	declare @logindb nvarchar(132), @loginlang nvarchar(132) select @logindb = N'mcsdb', @loginlang = N'��������'
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
--�������ݿ��еĶ���
--
***********************************************/

/***********************************************
--
--������TB_Example1
--˵��������ʾ��
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_Example1]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_Example1]
GO

CREATE TABLE [dbo].[TB_Example1] (
	[field1] [int] NOT NULL ,                                           --ÿ���ֶ�Ҫ�����ֶ�˵��
	[field2] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,            --ÿ���ֶ�Ҫ�����ֶ�˵��
	[field3] [float] NULL ,                                             --ÿ���ֶ�Ҫ�����ֶ�˵��
	[xmlfield] [varchar] (7900) COLLATE Chinese_PRC_CI_AS NULL ,        --ÿ���ֶ�Ҫ�����ֶ�˵��
	[xmlfield1] [text] COLLATE Chinese_PRC_CI_AS NULL                   --ÿ���ֶ�Ҫ�����ֶ�˵��
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TB_Example1] WITH NOCHECK ADD 
	CONSTRAINT [PK_TB_Example1] PRIMARY KEY  CLUSTERED 
	(
		[field1]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>�Ѵ�����[TB_Example1]'

/***********************************************
--
--������TB_Event
--˵�����¼���¼��
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_Event]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_Event]
GO

CREATE TABLE [dbo].[TB_Event] (
	[EmplyeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,          --Ա����
	[EventTime] [DateTime] NOT NULL ,                                        --ʱ��
	[EventRecord] [varchar] (2000) COLLATE Chinese_PRC_CI_AS NOT NULL        --�¼���¼
) ON [PRIMARY]
GO

CREATE  INDEX [IX_TB_Event] ON [dbo].[TB_Event]([EventTime]) ON [PRIMARY]
GO

PRINT ''
PRINT '>�Ѵ�����[TB_Event]'

/***********************************************
--
--������TA_Model
--˵����Ʒ�ŵ�����
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Model]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Model]
GO

CREATE TABLE [dbo].[TA_Model] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	    --Ʒ��
	[Name] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,          --����
	[Code] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,          --����
	[CustomerID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,	    --�ͻ�Ʒ��
	[ModelType] [int] NOT NULL                                          --Ʒ�����ͣ�1-��Ʒ��2-������3-���ϣ�
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_Model] WITH NOCHECK ADD 
	CONSTRAINT [PK_TA_Model] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>�Ѵ�����[TA_Model]'

/***********************************************
--
--������TA_Plan
--˵���������ƻ�����
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Plan]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Plan]
GO

CREATE TABLE [dbo].[TA_Plan] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,          --�����ƻ���
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,     --��ƷƷ��
	[Output] [int] NOT NULL ,                                         --����
	[OrderID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,         --�ͻ�������
	[PlanType] [int] NOT NULL ,                                       --�����ƻ������ͣ�1-����;2-���ˣ�
	[Remark] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL ,         --��ע
	[FoundTime] [DateTime] NOT NULL ,				  --����ʱ��
	[CloseTime] [DateTime] NULL ,					  --�ر�ʱ��
	[State] [int] NOT NULL                                            --�����ƻ���״̬��1-��ʼ;2-����;3-�رգ�
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
PRINT '>�Ѵ�����[TA_Plan]'

/***********************************************
--
--������TC_PlanProcedure
--˵�����ƻ��������̱�
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TC_PlanProcedure]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TC_PlanProcedure]
GO

CREATE TABLE [dbo].[TC_PlanProcedure] (
	[PlanID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,        --�����ƻ���
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,       --Ʒ��
	[ProcedureID] [int] NOT NULL                                        --�������̱��
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
PRINT '>�Ѵ�����[TC_PlanProcedure]'

/***********************************************
--
--������TA_Structure
--˵������Ʒ�ṹ�����
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Structure]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Structure]
GO

CREATE TABLE [dbo].[TA_Structure] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	--Ʒ��
	[ItemID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,    --�����Ʒ��
	[Amount] [int] NOT NULL                                         --���������
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
PRINT '>�Ѵ�����[TA_Structure]'

/***********************************************
--
--������TA_Employee
--˵����Ա��������
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Employee]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Employee]
GO

CREATE TABLE [dbo].[TA_Employee] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	--����
	[Name] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,      --Ա������
	[Password] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,  --��¼����
	[State] [int] NOT NULL ,                                        --״̬��1-����;2-ͣ�ã�
	[Remark] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL         --��ע
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_Employee] WITH NOCHECK ADD 
	CONSTRAINT [PK_TA_Employee] PRIMARY KEY  CLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>�Ѵ�����[TA_Employee]'

/***********************************************
--
--������TA_Role
--˵������ɫ������
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Role]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Role]
GO

CREATE TABLE [dbo].[TA_Role] (
	[Role] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,      --��ɫ
	[Remark] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL         --��ע
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TA_Role] WITH NOCHECK ADD 
	CONSTRAINT [PK_TA_Role] PRIMARY KEY  CLUSTERED 
	(
		[Role]
	)  ON [PRIMARY] 
GO

INSERT INTO TA_Role(Role) VALUES ('��ҵԱ')
INSERT INTO TA_Role(Role) VALUES ('ά��Ա')
INSERT INTO TA_Role(Role) VALUES ('���Ϲ���Ա')
INSERT INTO TA_Role(Role) VALUES ('��������')
INSERT INTO TA_Role(Role) VALUES ('���鳤')
INSERT INTO TA_Role(Role) VALUES ('���¹���Ա')
INSERT INTO TA_Role(Role) VALUES ('�ƻ�Ա')
INSERT INTO TA_Role(Role) VALUES ('���칤��ʦ')
INSERT INTO TA_Role(Role) VALUES ('ϵͳ����Ա')
--INSERT INTO TA_Role(Role) VALUES ('��������Ա')
INSERT INTO TA_Role(Role) VALUES ('һ�����ݲ鿴��')
INSERT INTO TA_Role(Role) VALUES ('�������ݲ鿴��')
INSERT INTO TA_Role(Role) VALUES ('�������ݲ鿴��')
GO

PRINT ''
PRINT '>�Ѵ�����[TA_Role]'

/***********************************************
--
--������TRE_Employee_Role
--˵������ҵԱ���ɫ�Ĺ�ϵ��
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TRE_Employee_Role]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TRE_Employee_Role]
GO

CREATE TABLE [dbo].[TRE_Employee_Role] (
	[EmployeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,				--����
	[Role] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL        				--��ɫ
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
PRINT '>�Ѵ�����[TRE_Employee_Role]'

/***********************************************
--
--������TA_Group
--˵�������鵵����
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Group]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Group]
GO

CREATE TABLE [dbo].[TA_Group] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,				      --��ţ����ݿ��ڲ���ʶ��
	[Name] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,            --����
	[LeaderID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,        --���鳤����
	[WorkDispatch] [varchar] (2000) COLLATE Chinese_PRC_CI_AS NULL	      --Ա����λ����
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
PRINT '>�Ѵ�����[TA_Group]'

/***********************************************
--
--������TRE_Group_Employee
--˵������������ҵԱ�Ĺ�ϵ��
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TRE_Group_Employee]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TRE_Group_Employee]
GO

CREATE TABLE [dbo].[TRE_Group_Employee] (
	[GroupID] [int] NOT NULL ,								--������
	[EmployeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL				--Ա������
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
PRINT '>�Ѵ�����[TRE_Group_Employee]'

/***********************************************
--
--������TA_BugType
--˵����ȱ����𵵰���
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_BugType]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_BugType]
GO

CREATE TABLE [dbo].[TA_BugType] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,          --ȱ������
	[Bug] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL           --ȱ��
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
PRINT '>�Ѵ�����[TA_BugType]'

/***********************************************
--
--������TA_BugPoint
--˵����ȱ�ݶ�λ�㵵����
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_BugPoint]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_BugPoint]
GO

CREATE TABLE [dbo].[TA_BugPoint] (
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,        --Ʒ��
	[BugPointCode] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,   --ȱ�ݶ�λ�����
	[BugPointDsc] [varchar] (100) COLLATE Chinese_PRC_CI_AS NOT NULL     --ȱ�ݶ�λ��
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
PRINT '>�Ѵ�����[TA_BugPoint]'

/***********************************************
--
--������TA_Product
--˵������Ʒ������
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Product]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Product]
GO

CREATE TABLE [dbo].[TA_Product] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	     	--��Ʒ���к�|��������
	[SN] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,  	     	--��Ʒ����|�ͻ�����
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,        	--Ʒ��
	[ModelType] [int] NOT NULL ,                                         	--Ʒ�����ͣ�1-��Ʒ��2-������
	[PlanID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL , 	    	--�����ƻ���
	[ProcedureID] [int] NOT NULL ,                                       	--�������̱��
	[FoundTime] [DateTime] NOT NULL ,					--����ʱ��
	[ManufactureState] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,   	--��Ʒ��ǰ����״̬����ǰ������𣨷��ޡ����ϡ�1װ�䡢2��װ��3���ԡ�4���顢5��װ��6���ȣ�ΪNULL��ʾ��ʼ̬��
	[Remark] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL 		--��ע
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
PRINT '>�Ѵ�����[TA_Product]'

/***********************************************
--
--������TA_Materiel
--˵�������ϵ�����
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Materiel]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Materiel]
GO

CREATE TABLE [dbo].[TA_Materiel] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		--��������
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,        	--Ʒ��
	[Batch] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,          	--���κ�
	[Vendor] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,         	--��Ӧ��
	[EmployeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,         	--����Ա�����ţ�ֻ����Բ����ϣ�
	[FoundTime] [DateTime] NOT NULL ,					--����ʱ��
	[Remark] [varchar] (100) COLLATE Chinese_PRC_CI_AS NULL 		--��ע
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
PRINT '>�Ѵ�����[TA_Materiel]'

/***********************************************
--
--������TA_Relationship
--˵������Ʒ�����������ϵĹ�����
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Relationship]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Relationship]
GO

CREATE TABLE [dbo].[TA_Relationship] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		     --�����к����루��Ʒ�򲿼����кţ�
	[ItemID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		     --��������루�������������룩
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
PRINT '>�Ѵ�����[TA_Relationship]'

/***********************************************
--
--������TA_Procedure
--˵�����������̵�����
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_Procedure]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_Procedure]
GO

CREATE TABLE [dbo].[TA_Procedure] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,				     --��ţ����ݿ��ڲ���ʶ��
	[Name] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,           --����
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,        --Ʒ��
	[ProcessConfig] [text] COLLATE Chinese_PRC_CI_AS NULL ,              --�������ã�XML��ʽ������μ���ϸ���˵���飩
	[ProcessGraph] [text] COLLATE Chinese_PRC_CI_AS NULL                 --������ӻ���Ϣ��XML��ʽ������μ���ϸ���˵���飩
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
PRINT '>�Ѵ�����[TA_Procedure]'


/***********************************************
--
--������TB_GroupMateriel
--˵�����������ϱ�
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_GroupMateriel]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_GroupMateriel]
GO

CREATE TABLE [dbo].[TB_GroupMateriel] (
	[GroupID] [int] NOT NULL ,						--������
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,         	--Ʒ��
	[MaterielID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL        	--��������
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
PRINT '>�Ѵ�����[TB_GroupMateriel]'

/***********************************************
--
--������TB_ProcedureState
--˵������������״̬��
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_ProcedureState]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_ProcedureState]
GO

CREATE TABLE [dbo].[TB_ProcedureState] (
	[ProductID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		   --��Ʒ���к�
	[Process] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		   --����ɹ�������
--	[Data] [text] COLLATE Chinese_PRC_CI_AS NULL ,                             --����
	[DataID] [uniqueidentifier] NOT NULL                                       --���ݱ�ʶ��ָ��TB_ProcedureHistory��Ӧ�ֶ�
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
PRINT '>�Ѵ�����[TB_ProcedureState]'

/***********************************************
--
--������TB_ProcedureHistory
--˵��������������ʷ��
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_ProcedureHistory]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_ProcedureHistory]
GO

CREATE TABLE [dbo].[TB_ProcedureHistory] (
	[ProductID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		        --��Ʒ���к�
	[Process] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		        --��������
	[EmployeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		--Ա������
	[Result] [int] NULL ,								--��ҵ���|����״̬��NULL������δ������0��������1���践�ޣ�2�����̱����
	[Exception] [varchar] (2000) COLLATE Chinese_PRC_CI_AS NULL ,              	--�쳣��������
	[Data] [text] COLLATE Chinese_PRC_CI_AS NULL ,                             	--����
	[DataID] [uniqueidentifier] NOT NULL ,                                     	--���ݱ�ʶ��Ϊһ��GUIDֵ
	[Dispatch] [varchar] (2000) COLLATE Chinese_PRC_CI_AS NULL ,		        --Ա����λ����
	[BeginTime] [DateTime] NOT NULL ,				                --����ʼʱ��
	[EndTime] [DateTime] NULL  						        --�������ʱ��
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
PRINT '>�Ѵ�����[TB_ProcedureHistory]'

/***********************************************
--
--������TB_RepairRecord
--˵������Ʒ���޼�¼��
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_RepairRecord]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_RepairRecord]
GO

CREATE TABLE [dbo].[TB_RepairRecord] (
	[ProductID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		        --��Ʒ���к�
	[DetectTime] [DateTime] NOT NULL ,						--�����쳣��ʱ��
	[DetectProcess] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		--�����쳣�Ĺ���
	[DetectEmployeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		--�����쳣��Ա������
	[Exception] [varchar] (2000) COLLATE Chinese_PRC_CI_AS NOT NULL ,          	--�쳣��������
	[BugID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,                    	--ȱ������
	[BugPointCode] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,             	--ȱ�ݶ�λ��
	[RepairTime] [DateTime]  NULL ,						        --����ʱ��
	[RepairEmployeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NULL ,		--����Ա������
	[RepairInfo] [varchar] (2000) COLLATE Chinese_PRC_CI_AS NULL ,             	--������Ϣ
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
PRINT '>�Ѵ�����[TB_RepairRecord]'

/***********************************************
--
--������TB_InstrumentCalRecord
--˵�����Ǳ�У׼��¼��
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_InstrumentCalRecord]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_InstrumentCalRecord]
GO

CREATE TABLE [dbo].[TB_InstrumentCalRecord] (
	[Model] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		--�Ǳ��ͺ�
	[SN] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		--�Ǳ����к�
	[ProductModel] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	--��ƷƷ��
	[EmployeeID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	--У׼�߹���
	[CalTime] [DateTime] NOT NULL ,  					--У׼ʱ��
	[Data] [text] COLLATE Chinese_PRC_CI_AS NOT NULL                       	--У׼����
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
PRINT '>�Ѵ�����[TB_InstrumentCalRecord]'

/***********************************************
--
--������TB_SoftwareVersion
--˵��������汾��
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_SoftwareVersion]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_SoftwareVersion]
GO

CREATE TABLE [dbo].[TB_SoftwareVersion] (
	[Name] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		         --�������
	[Version] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL		         --����汾��
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TB_SoftwareVersion] ADD 
	CONSTRAINT [PK_TB_SoftwareVersion] PRIMARY KEY  CLUSTERED 
	(
		[Name]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>�Ѵ�����[TB_SoftwareVersion]'

/***********************************************
--
--������TB_TestConfigVer
--˵������������汾��
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TB_TestConfigVer]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TB_TestConfigVer]
GO

CREATE TABLE [dbo].[TB_TestConfigVer] (
	[Name] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,		         --�������
	[Version] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL		         --����汾��
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TB_TestConfigVer] ADD 
	CONSTRAINT [PK_TB_TestConfigVer] PRIMARY KEY  CLUSTERED 
	(
		[Name]
	)  ON [PRIMARY] 
GO

PRINT ''
PRINT '>�Ѵ�����[TB_TestConfigVer]'

/***********************************************
--
--������TRE_Group_Plan
--˵��������ƻ���
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TRE_Group_Plan]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TRE_Group_Plan]
GO

CREATE TABLE [dbo].[TRE_Group_Plan] (
	[GroupID] [int] NOT NULL ,								--������
	[PlanID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL				--�ƻ����
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
PRINT '>�Ѵ�����[TRE_Group_Plan]'

/***********************************************
--
--������TA_ProductID
--˵������Ʒ���뵵����
--
***********************************************/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TA_ProductID]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TA_ProductID]
GO

CREATE TABLE [dbo].[TA_ProductID] (
	[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	     	--��������
	[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,       --Ʒ��
	CONSTRAINT PK_TA_ProductID PRIMARY KEY (ID)
) ON [PRIMARY]
GO

PRINT ''
PRINT '>�Ѵ�����[TA_ProductID]'
