PRINT '>��ʼִ��mcsdb���ݿ��������ò������ᶪʧ�������ݡ�'
PRINT '>������������Ϊ������Ʒ������[TA_Product]������'

/***********************************************
--
--������TA_Product
--˵������Ʒ������
--
***********************************************/
IF exists (
	SELECT * FROM dbo.sysobjects 
	WHERE id = object_id(N'[dbo].[IX_TA_Product]')
)
BEGIN
	DROP INDEX TA_Product.IX_TA_Product
	
	PRINT ''
	PRINT '>��ȥ����[TA_Product]��SN�������ظ�������'
END

PRINT ''
PRINT '>������ɡ�'
GO


