PRINT '>开始执行mcsdb数据库升级，该操作不会丢失现有数据。'
PRINT '>本次升级内容为调整产品档案表[TA_Product]索引。'

/***********************************************
--
--表名：TA_Product
--说明：产品档案表
--
***********************************************/
IF exists (
	SELECT * FROM dbo.sysobjects 
	WHERE id = object_id(N'[dbo].[IX_TA_Product]')
)
BEGIN
	DROP INDEX TA_Product.IX_TA_Product
	
	PRINT ''
	PRINT '>已去掉表[TA_Product]的SN不允许重复索引。'
END

PRINT ''
PRINT '>升级完成。'
GO


