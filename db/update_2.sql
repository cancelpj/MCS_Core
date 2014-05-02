PRINT '>��ʼִ��mcsdb���ݿ��������ò������ᶪʧ�������ݡ�'
PRINT '>������������Ϊ���Ӳ�Ʒ���뵵����[TA_ProductID]��'

/***********************************************
--
--������TA_ProductID
--˵������Ʒ���뵵����
--
***********************************************/
IF not exists (
	SELECT * FROM dbo.sysobjects 
	WHERE id = object_id(N'[dbo].[TA_ProductID]') and OBJECTPROPERTY(id, N'IsUserTable') = 1
)
BEGIN
	CREATE TABLE [dbo].[TA_ProductID] (
		[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	     	--��������
		[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,       --Ʒ��
		CONSTRAINT PK_TA_ProductID PRIMARY KEY (ID)
	) ON [PRIMARY]
	
	PRINT ''
	PRINT '>�Ѵ�����[TA_ProductID]'
	
	INSERT INTO TA_ProductID(ID,ModelID)
		SELECT ID, ModelID FROM TA_Product
	
	PRINT ''
	PRINT '>����ɱ�[TA_ProductID]�����ؽ���'
	
	ALTER TABLE [dbo].[TA_Product] ADD 
		CONSTRAINT [FK_TA_Product_TA_ProductID] FOREIGN KEY 
		(
			[ID]
		) REFERENCES [dbo].[TA_ProductID] (
			[ID]
		)
	
	PRINT ''
	PRINT '>����ɱ�[TA_Product]����Լ����顣'
END

PRINT ''
PRINT '>������ɡ�'
GO


