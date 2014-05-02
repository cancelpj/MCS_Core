PRINT '>��ʼִ��mcsdb���ݿ��������ò������ᶪʧ�������ݡ�'
PRINT '>������������Ϊ���Ӱ���ƻ���[TRE_Group_Plan]��'

/***********************************************
--
--������TRE_Group_Plan
--˵��������ƻ���
--
***********************************************/
IF not exists (
	SELECT * FROM dbo.sysobjects 
	WHERE id = object_id(N'[dbo].[TRE_Group_Plan]') and OBJECTPROPERTY(id, N'IsUserTable') = 1
)
BEGIN
	CREATE TABLE [dbo].[TRE_Group_Plan] (
		[GroupID] [int] NOT NULL ,								--������
		[PlanID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL				--�ƻ����
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
	PRINT '>�Ѵ�����[TRE_Group_Plan]��'

	INSERT INTO TRE_Group_Plan(GroupID,PlanID)
		SELECT ID, PlanID
		FROM TA_Group

	PRINT ''
	PRINT '>����ɱ�[TRE_Group_Plan]�����ؽ���'

	ALTER TABLE TA_Group DROP CONSTRAINT FK_TA_Group_TA_Plan
	ALTER TABLE TA_Group DROP COLUMN PlanID

	PRINT ''
	PRINT '>����ɱ�[TA_Group]�ṹ���¡�'
END

PRINT ''
PRINT '>������ɡ�'
GO
