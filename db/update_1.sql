PRINT '>开始执行mcsdb数据库升级，该操作不会丢失现有数据。'
PRINT '>本次升级内容为增加班组计划表[TRE_Group_Plan]。'

/***********************************************
--
--表名：TRE_Group_Plan
--说明：班组计划表
--
***********************************************/
IF not exists (
	SELECT * FROM dbo.sysobjects 
	WHERE id = object_id(N'[dbo].[TRE_Group_Plan]') and OBJECTPROPERTY(id, N'IsUserTable') = 1
)
BEGIN
	CREATE TABLE [dbo].[TRE_Group_Plan] (
		[GroupID] [int] NOT NULL ,								--班组编号
		[PlanID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL				--计划编号
	) ON [PRIMARY]
	
	ALTER TABLE [dbo].[TRE_Group_Plan] WITH NOCHECK ADD 
		CONSTRAINT [PK_TRE_Group_Plan] PRIMARY KEY  CLUSTERED 
		(
			[GroupID],
			[PlanID]
		)  ON [PRIMARY] 
	
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
	
	PRINT ''
	PRINT '>已创建表[TRE_Group_Plan]。'

	INSERT INTO TRE_Group_Plan(GroupID,PlanID)
		SELECT ID, PlanID
		FROM TA_Group

	PRINT ''
	PRINT '>已完成表[TRE_Group_Plan]数据重建。'

	ALTER TABLE TA_Group DROP CONSTRAINT FK_TA_Group_TA_Plan
	ALTER TABLE TA_Group DROP COLUMN PlanID

	PRINT ''
	PRINT '>已完成表[TA_Group]结构更新。'
END

PRINT ''
PRINT '>升级完成。'
GO
