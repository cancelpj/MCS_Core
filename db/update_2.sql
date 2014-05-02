PRINT '>开始执行mcsdb数据库升级，该操作不会丢失现有数据。'
PRINT '>本次升级内容为增加产品条码档案表[TA_ProductID]。'

/***********************************************
--
--表名：TA_ProductID
--说明：产品条码档案表
--
***********************************************/
IF not exists (
	SELECT * FROM dbo.sysobjects 
	WHERE id = object_id(N'[dbo].[TA_ProductID]') and OBJECTPROPERTY(id, N'IsUserTable') = 1
)
BEGIN
	CREATE TABLE [dbo].[TA_ProductID] (
		[ID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,	     	--生产条码
		[ModelID] [varchar] (50) COLLATE Chinese_PRC_CI_AS NOT NULL ,       --品号
		CONSTRAINT PK_TA_ProductID PRIMARY KEY (ID)
	) ON [PRIMARY]
	
	PRINT ''
	PRINT '>已创建表[TA_ProductID]'
	
	INSERT INTO TA_ProductID(ID,ModelID)
		SELECT ID, ModelID FROM TA_Product
	
	PRINT ''
	PRINT '>已完成表[TA_ProductID]数据重建。'
	
	ALTER TABLE [dbo].[TA_Product] ADD 
		CONSTRAINT [FK_TA_Product_TA_ProductID] FOREIGN KEY 
		(
			[ID]
		) REFERENCES [dbo].[TA_ProductID] (
			[ID]
		)
	
	PRINT ''
	PRINT '>已完成表[TA_Product]数据约束检查。'
END

PRINT ''
PRINT '>升级完成。'
GO


