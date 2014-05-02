DELETE FROM TA_Product WHERE ID='029070033A1088008800'
DELETE FROM TB_ProcedureHistory WHERE ProductID='029070033A1088008800'

--################################################################################
PRINT ''
PRINT '�����Ʒ��������'
--################################################################################
DECLARE @ProcedureID int
DECLARE @ProcedureID2 int

SET @ProcedureID = (SELECT ID FROM TA_Procedure WHERE (Name = '����������'))

INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('029070033A1088008800', 'NOKIA001', '029070033A10', 1, '123', @ProcedureID, '2008-11-01 08:01:00', '4��װ', 'TestProduct')

INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('029070033A1088008800', '��װ', 'HR007033', 0, NULL, 'Test Data', '2151D7E5-C0E5-47AE-9DDF-7950536ACFB8', 'װ�ɸˣ�00001��װ���ذ壺00002', '2008-08-01 08:01:00', '2008-08-01 08:11:00')

INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('029070033A1088008800', '��װ', NEWID())
