
/***********************************************
--
--���ļ�����һ��Ϊmcsdb���ݿ�����ʼ���ݵ�SQL�ű�
--��������ÿ�����еĳ�ʼ����
--��Щ���ݿɹ�����ʹ��
--��������������ִ��
--osql -U [dbuser] -n -i Data.sql
--
***********************************************/

USE mcsdb
GO

DELETE FROM TA_Product
DELETE FROM TA_Materiel
DELETE FROM TA_Relationship
DELETE FROM TA_Group
DELETE FROM TA_Plan
DELETE FROM TA_Structure
DELETE FROM TA_BugPoint
DELETE FROM TA_Model
DELETE FROM TRE_Employee_Role
DELETE FROM TA_Employee
DELETE FROM TRE_Group_Employee
DELETE FROM TB_ProcedureState
DELETE FROM TB_ProcedureHistory
DELETE FROM TA_Procedure
DELETE FROM TB_RepairRecord
DELETE FROM TA_BugType
DELETE FROM TA_BugPoint
DELETE FROM TC_PlanProcedure
DELETE FROM TB_InstrumentCalRecord
DELETE FROM TB_SoftwareVersion
GO

--################################################################################
PRINT ''
PRINT '����Ʒ������'
--################################################################################
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('80320000', 'NTGZ83AAE6', 'SP156', 'NortelSN001', 1)
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('80320001', 'NTGZ83AAE7', 'SP157', 'NortelSN002', 1)
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('80320002', 'NTGZ83AAE8', 'SP158', 'NortelSN003', 1)
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('60110000', 'SP156������', 'FG00001', NULL, 2)
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('60110001', 'SP157������', 'FG00002', NULL, 2)
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('60110002', 'SP156�����', 'FG00003', NULL, 2)
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('70110001', 'г���', 'FG00102', NULL, 3)
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('70110002', '��ͷ', 'FG00103', NULL, 3)

--��������50��Ʒ��
DECLARE @i int
DECLARE @sql nvarchar(400)

SET @i = 1

WHILE (@i <= 50)
BEGIN
  SET @sql = 'INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES (''0000' + CAST(@i AS nvarchar) + ''', ''PCB��' + CAST(@i AS nvarchar) + ''', ''FG' + CAST(@i AS nvarchar) + ''', NULL, 2)'
	
  SET @i = @i + 1
  EXEC (@sql) 
END
GO

--################################################################################
PRINT ''
PRINT '�����Ʒ�ṹ����'
--################################################################################
INSERT INTO TA_Structure(ID, ItemID, Amount) VALUES ('80320000', '60110000', 1)
INSERT INTO TA_Structure(ID, ItemID, Amount) VALUES ('80320000', '60110002', 1)
INSERT INTO TA_Structure(ID, ItemID, Amount) VALUES ('80320000', '70110001', 1)
INSERT INTO TA_Structure(ID, ItemID, Amount) VALUES ('80320000', '70110002', 1)
INSERT INTO TA_Structure(ID, ItemID, Amount) VALUES ('80320001', '60110001', 1)

--################################################################################
PRINT ''
PRINT '���������ƻ�����'
--################################################################################
INSERT INTO TA_Plan(ID, ModelID, Output, OrderID, PlanType, Remark, FoundTime, CloseTime, State) VALUES ('08080001', '80320000', 300, 'NokiaOrder0001', 1, 'Test Plan', '2008-01-01 00:01:00', NULL, 2)
INSERT INTO TA_Plan(ID, ModelID, Output, OrderID, PlanType, Remark, FoundTime, CloseTime, State) VALUES ('08080002', '80320001', 700, 'NokiaOrder0002', 1, 'Test Plan', '2008-03-01 00:01:00', NULL, 2)
INSERT INTO TA_Plan(ID, ModelID, Output, OrderID, PlanType, Remark, FoundTime, CloseTime, State) VALUES ('08080003', '80320000', 200, 'NokiaOrder0003', 1, 'Test Plan', '2008-01-01 00:01:00', NULL, 2)

INSERT INTO TA_Plan(ID, ModelID, Output, OrderID, PlanType, Remark, FoundTime, CloseTime, State) VALUES ('1', '80320000', 30, '1', 1, 'Test Plan 1', '2008-01-01 00:01:00', NULL, 1)

--################################################################################
PRINT ''
PRINT '���빤�����̵�������'
--################################################################################
DECLARE @ProcessConfig varchar(8000)
DECLARE @ProcessConfig2 varchar(8000)
DECLARE @ProcessGraph varchar(8000)

SET @ProcessConfig = '<?xml version="1.0"?>
<NewDataSet>
  <process>
    <Name>װ��</Name>
    <Range>1װ��</Range>
  </process>
  <process>
    <Name>����</Name>
    <Range>2����</Range>
  </process>
  <process>
    <Name>�ϻ�</Name>
    <Range>3�ϻ�</Range>
  </process>
  <process>
    <Name>��װ</Name>
    <Range>4��װ</Range>
  </process>
  <process>
    <Name>�����������</Name>
    <Range>5���</Range>
  </process>
  <process>
    <Name>IP3����</Name>
    <Range>5���</Range>
  </process>
  <process>
    <Name>1dBѹ�������</Name>
    <Range>5���</Range>
  </process>
  <process>
    <Name>��������</Name>
    <Range>5���</Range>
  </process>
  <process>
    <Name>��װ</Name>
    <Range>6��װ</Range>
  </process>
  <process>
    <Name>���</Name>
    <Range>7���</Range>
  </process>
  <Connection>
    <From>װ��</From>
    <To>����</To>
  </Connection>
  <Connection>
    <From>����</From>
    <To>�ϻ�</To>
  </Connection>
  <Connection>
    <From>�ϻ�</From>
    <To>��װ</To>
  </Connection>
  <Connection>
    <From>��װ</From>
    <To>�����������</To>
  </Connection>
  <Connection>
    <From>��װ</From>
    <To>IP3����</To>
  </Connection>
  <Connection>
    <From>��װ</From>
    <To>1dBѹ�������</To>
  </Connection>
  <Connection>
    <From>��װ</From>
    <To>��������</To>
  </Connection>
  <Connection>
    <From>�����������</From>
    <To>��װ</To>
  </Connection>
  <Connection>
    <From>IP3����</From>
    <To>��װ</To>
  </Connection>
  <Connection>
    <From>1dBѹ�������</From>
    <To>��װ</To>
  </Connection>
  <Connection>
    <From>��������</From>
    <To>��װ</To>
  </Connection>
  <Connection>
    <From>��װ</From>
    <To>���</To>
  </Connection>
</NewDataSet>'

SET @ProcessConfig2 = '<?xml version="1.0"?>
<NewDataSet>
  <process>
    <Name>װ��</Name>
    <Range>1װ��</Range>
  </process>
  <process>
    <Name>����</Name>
    <Range>2����</Range>
  </process>
  <process>
    <Name>�ϻ�</Name>
    <Range>3�ϻ�</Range>
  </process>
  <process>
    <Name>��װ</Name>
    <Range>4��װ</Range>
  </process>
  <Connection>
    <From>װ��</From>
    <To>����</To>
  </Connection>
  <Connection>
    <From>����</From>
    <To>�ϻ�</To>
  </Connection>
  <Connection>
    <From>�ϻ�</From>
    <To>��װ</To>
  </Connection>
</NewDataSet>'

SET @ProcessGraph = '<ProcedureflowProcess>
<Activities>
<Activitie id="2" type="START_NODE" name="װ��" xCoordinate="314" yCoordinate="27" width="80" height="30" range="1װ��"/>
<Activitie id="3" type="NODE" name="����" xCoordinate="496" yCoordinate="123" width="80" height="30" range="2����"/>
<Activitie id="4" type="NODE" name="�ϻ�" xCoordinate="205" yCoordinate="204" width="80" height="30" range="3�ϻ�"/>
<Activitie id="6" type="FORK_NODE" name="��װ" xCoordinate="400" yCoordinate="318" width="80" height="30" range="4��װ"/>
<Activitie id="7" type="NODE" name="�����������" xCoordinate="159" yCoordinate="439" width="80" height="30" range="5���"/>
<Activitie id="8" type="NODE" name="IP3����" xCoordinate="279" yCoordinate="439" width="80" height="30" range="5���"/>
<Activitie id="9" type="NODE" name="1dBѹ�������" xCoordinate="519" yCoordinate="438" width="80" height="30" range="5���"/>
<Activitie id="10" type="NODE" name="��������" xCoordinate="640" yCoordinate="439" width="80" height="30" range="5���"/>
<Activitie id="11" type="JOIN_NODE" name="��װ" xCoordinate="399" yCoordinate="538" width="80" height="30" range="6��װ"/>
<Activitie id="26" type="END_NODE" name="���" xCoordinate="399" yCoordinate="638" width="80" height="30" range="7���"/>
</Activities>
<Transitions>
<Transition id="14" name="" from="4" to="6"/><Transition id="15" name="" from="6" to="7"/>
<Transition id="16" name="" from="6" to="8"/><Transition id="17" name="" from="6" to="9"/>
<Transition id="18" name="" from="6" to="10"/><Transition id="19" name="" from="7" to="11"/>
<Transition id="20" name="" from="8" to="11"/><Transition id="21" name="" from="9" to="11"/>
<Transition id="22" name="" from="10" to="11"/><Transition id="25" name="" from="2" to="3"/>
<Transition id="26" name="" from="3" to="4"/><Transition id="27" name="" from="11" to="26"/>
</Transitions>
</ProcedureflowProcess>'

INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP156������������', '80320000', @ProcessConfig, @ProcessGraph)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP156������������2', '80320000', @ProcessConfig2, @ProcessGraph)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP157������������', '80320001', @ProcessConfig, @ProcessGraph)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP157������������2', '80320001', NULL, @ProcessGraph)

INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP156������������������', '60110000', '1', NULL)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP156������������������2', '60110000', '1', NULL)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP157������������������', '60110001', '1', NULL)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP157������������������2', '60110001', '1', NULL)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP156�����������������', '60110002', '1', NULL)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP156�����������������2', '60110002', '1', NULL)

--################################################################################
PRINT ''
PRINT '����ƻ�������������'
--################################################################################
DECLARE @ProcedureID int
DECLARE @ProcedureID2 int
DECLARE @ProcedureID3 int

SET @ProcedureID = (SELECT ID FROM TA_Procedure WHERE (Name = 'SP156������������'))
SET @ProcedureID2 = (SELECT ID FROM TA_Procedure WHERE (Name = 'SP157������������'))
SET @ProcedureID3 = (SELECT ID FROM TA_Procedure WHERE (Name = 'SP156������������2'))

INSERT INTO TC_PlanProcedure(PlanID, ModelID, ProcedureID) VALUES ('08080001', '80320000', @ProcedureID)
INSERT INTO TC_PlanProcedure(PlanID, ModelID, ProcedureID) VALUES ('08080001', '60110000', @ProcedureID)
INSERT INTO TC_PlanProcedure(PlanID, ModelID, ProcedureID) VALUES ('08080002', '80320001', @ProcedureID2)
INSERT INTO TC_PlanProcedure(PlanID, ModelID, ProcedureID) VALUES ('08080003', '80320000', @ProcedureID3)

GO

--################################################################################
PRINT ''
PRINT '����Ա���ʺ�����'
--################################################################################
INSERT INTO TA_Employee(ID, Name, Password, State, Remark) VALUES ('07033', '�潣', '39pkUkLLbiI=', 1, '�������Ա')
INSERT INTO TA_Employee(ID, Name, Password, State, Remark) VALUES ('07032', '����', '39pkUkLLbiI=', 1, '�������Ա')
INSERT INTO TA_Employee(ID, Name, Password, State, Remark) VALUES ('14146', '���汦', 'XQLzAejM1i8knefIVrBAcQ==', 1, '�������Ա')
INSERT INTO TA_Employee(ID, Name, Password, State, Remark) VALUES ('00000', '�����ʺ�0', '39pkUkLLbiI=', 2, '���ڲ���')
--INSERT INTO TA_Employee(ID, Name, Password, State, Remark) VALUES ('00002', '�����ʺ�2', '39pkUkLLbiI=', 1, '���ڲ���')
--INSERT INTO TA_Employee(ID, Name, Password, State, Remark) VALUES ('00003', '�����ʺ�3', '39pkUkLLbiI=', 1, '���ڲ���')
GO

--��������50��Ա���ʺ�
DECLARE @i int
DECLARE @sql nvarchar(400)

SET @i = 1

WHILE (@i <= 50)
BEGIN
  SET @sql = 'INSERT INTO TA_Employee(ID, Name, Password, State, Remark) VALUES (''0000' + CAST(@i AS nvarchar) + ''', ''�����ʺ�' + CAST(@i AS nvarchar) + ''', ''39pkUkLLbiI='', 1, ''���ڲ���'')'
	
  SET @i = @i + 1
  EXEC (@sql) 
END
GO

--################################################################################
PRINT ''
PRINT '������ҵԱ���ɫ�Ĺ�ϵ'
--################################################################################
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('00000', '��ҵԱ')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('07032', '��ҵԱ')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('07032', 'һ�����ݲ鿴��')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('07032', '�������ݲ鿴��')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('07032', '�������ݲ鿴��')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('14146', '��ҵԱ')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('14146', '���鳤')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('00002', '��ҵԱ')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('00003', '��ҵԱ')

INSERT INTO TRE_Employee_Role(EmployeeID, Role) SELECT '07033', Role FROM TA_Role

--################################################################################
PRINT ''
PRINT '�����������'
--################################################################################
INSERT INTO TA_Group(Name, LeaderID, PlanID, WorkDispatch) VALUES ('ERװ��һ��', '07033', '08080001', '���ǰ壺07033;���ݶ���00001')
INSERT INTO TA_Group(Name, LeaderID, PlanID, WorkDispatch) VALUES ('ERװ�����', '07033', '08080002', '���ǰ壺07033;���ݶ���07032')
INSERT INTO TA_Group(Name, LeaderID, PlanID, WorkDispatch) VALUES ('ER��װһ��', '07033', '08080001', NULL)
INSERT INTO TA_Group(Name, LeaderID, PlanID, WorkDispatch) VALUES ('ER���һ��', '07033', '08080003', NULL)

--################################################################################
PRINT ''
PRINT '�������Ա������'
--################################################################################
INSERT INTO TRE_Group_Employee(GroupID, EmployeeID) SELECT ID, '00001' AS EmployeeID FROM TA_Group WHERE (Name = 'ERװ��һ��')
INSERT INTO TRE_Group_Employee(GroupID, EmployeeID) SELECT ID, '00002' AS EmployeeID FROM TA_Group WHERE (Name = 'ERװ�����')
INSERT INTO TRE_Group_Employee(GroupID, EmployeeID) SELECT ID, '00003' AS EmployeeID FROM TA_Group WHERE (Name = 'ER��װһ��')
INSERT INTO TRE_Group_Employee(GroupID, EmployeeID) SELECT ID, '07033' AS EmployeeID FROM TA_Group WHERE (Name = 'ERװ��һ��')
INSERT INTO TRE_Group_Employee(GroupID, EmployeeID) SELECT ID, '07032' AS EmployeeID FROM TA_Group WHERE (Name = 'ER���һ��')
INSERT INTO TRE_Group_Employee(GroupID, EmployeeID) SELECT ID, '14146' AS EmployeeID FROM TA_Group WHERE (Name = 'ERװ��һ��')

--################################################################################
PRINT ''
PRINT '�����Ʒ��������'
--################################################################################
DECLARE @ProcedureID int
DECLARE @ProcedureID2 int

SET @ProcedureID = (SELECT ID FROM TA_Procedure WHERE (Name = 'SP156������������'))
SET @ProcedureID2 = (SELECT ID FROM TA_Procedure WHERE (Name = 'SP156������������������'))

INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('8032000008160011', 'NOKIA001', '80320000', 1, '08080001', @ProcedureID, '2008-05-01 08:01:00', NULL, 'TestProduct')
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('8032000008160012', 'NOKIA002', '80320000', 1, '08080001', @ProcedureID, '2008-05-01 08:01:00', NULL, 'TestProduct')
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('8032000008160013', 'NOKIA003', '80320000', 1, '08080001', @ProcedureID, '2008-05-01 08:01:00', '3�ϻ�', 'TestProduct')
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('8032000008160014', 'NOKIA004', '80320000', 1, '08080001', @ProcedureID, '2008-05-01 08:01:00', NULL, 'TestProduct')
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('8032000008160015', 'NOKIA005', '80320000', 1, '08080001', @ProcedureID, '2008-05-01 08:01:00', '6��װ', 'TestProduct')
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('8032000008160019', 'NOKIA009', '80320000', 1, '08080001', @ProcedureID, '2008-05-01 08:01:00', '7���', 'TestProduct')
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('8032000008160020', 'NOKIA019', '80320000', 1, '08080003', @ProcedureID, '2008-05-01 08:01:00', NULL, 'TestProduct')
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('8032000108160011', 'NOKIA101', '80320001', 1, '08080002', @ProcedureID, '2008-05-01 08:01:00', NULL, 'TestProduct')
            
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('6011000008160011', NEWID(), '60110000', 2, '08080001', @ProcedureID2, '2008-05-01 08:01:00', NULL, 'TestComponent')
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('6011000008160012', NEWID(), '60110000', 2, '08080001', @ProcedureID2, '2008-05-01 08:01:00', NULL, 'TestComponent')
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('6011000208160012', NEWID(), '60110002', 2, '08080001', @ProcedureID2, '2008-05-01 08:01:00', NULL, 'TestComponent')
GO

--################################################################################
PRINT ''
PRINT '�������ϵ�������'
--################################################################################
INSERT INTO TA_Materiel(ID, ModelID, Batch, Vendor, EmployeeID, FoundTime, Remark) 
            VALUES ('7011000108160011', '70110001', '0834091', '���ȵ���', NULL, '2008-05-01 08:01:00', 'TestMateriel')
INSERT INTO TA_Materiel(ID, ModelID, Batch, Vendor, EmployeeID, FoundTime, Remark) 
            VALUES ('7011000108160012', '70110001', '0834092', '���ȵ���', NULL, '2008-05-01 08:01:00', 'TestMateriel')
INSERT INTO TA_Materiel(ID, ModelID, Batch, Vendor, EmployeeID, FoundTime, Remark) 
            VALUES ('7011000208160011', '70110002', '0844091', '���ȵ���', '00001', '2008-06-01 08:01:00', 'TestMateriel')
INSERT INTO TA_Materiel(ID, ModelID, Batch, Vendor, EmployeeID, FoundTime, Remark) 
            VALUES ('7011000208160012', '70110002', '0844092', '���ȵ���', '00002', '2008-07-01 08:01:00', 'TestMateriel')

--################################################################################
PRINT ''
PRINT '�����Ʒ�����������ϵĹ�������'
--################################################################################
INSERT INTO TA_Relationship(ID, ItemID) VALUES ('8032000008160011', '6011000008160011')
INSERT INTO TA_Relationship(ID, ItemID) VALUES ('8032000008160011', '6011000208160012')
INSERT INTO TA_Relationship(ID, ItemID) VALUES ('8032000008160011', '7011000108160011')
INSERT INTO TA_Relationship(ID, ItemID) VALUES ('8032000108160011', '6011000208160012')
INSERT INTO TA_Relationship(ID, ItemID) VALUES ('8032000108160011', '7011000108160011')

INSERT INTO TA_Relationship(ID, ItemID) VALUES ('6011000008160011', '7011000108160011')

--################################################################################
PRINT ''
PRINT '��������������ʷ����'
--################################################################################
DECLARE @xmlData varchar(8000)

SET @xmlData = '<?xml version="1.0"?>
<NewDataSet>
  <Info>
    <Content>IP3����</Content>
    <ver>1</ver>
    <Description>����IP3��������</Description>
  </Info>
  <fieldname>
    <field1>�ֶ�1</field1>
    <field2>�ֶ�2</field2>
    <field3>�ֶ�3</field3>
    <field4>�ֶ�4</field4>
    <field11>�ֶ�11</field11>
    <field12>�ֶ�12</field12>
    <field13>�ֶ�13</field13>
    <field14>�ֶ�14</field14>
    <field21>�ֶ�21</field21>
    <field22>�ֶ�22</field22>
    <field23>�ֶ�23</field23>
    <field24>�ֶ�24</field24>
    <field31>�ֶ�31</field31>
    <field32>�ֶ�32</field32>
    <field33>�ֶ�33</field33>
    <field34>�ֶ�34</field34>
    <field41>�ֶ�41</field41>
    <field42>�ֶ�42</field42>
    <field43>�ֶ�43</field43>
    <field44>�ֶ�44</field44>
  </fieldname>
  <Data>
    <field1>����11</field1>
    <field2>����12</field2>
    <field3>����13</field3>
    <field4>����14</field4>
    <field11>����11</field11>
    <field12>����12</field12>
    <field13>����13</field13>
    <field14>����14</field14>
    <field21>����21</field21>
    <field22>����22</field22>
    <field23>����23</field23>
    <field24>����24</field24>
    <field31>����31</field31>
    <field32>����32</field32>
    <field33>����33</field33>
    <field34>����34</field34>
    <field41>����41</field41>
    <field42>����42</field42>
    <field43>����43</field43>
    <field44>����44</field44>
  </Data>
</NewDataSet>'

INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('8032000008160011', 'װ��', '00001', 0, NULL, 'Test Data', '2151D7E5-C0E5-47AE-9DDF-7950596ACFB8', 'װ�ɸˣ�00001��װ���ذ壺00002', '2008-08-01 08:01:00', '2008-08-01 08:11:00')
INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('8032000008160011', '����', '00001', 1, '���߲���', '������', '57965653-D022-4473-A1BF-3647A4BCE389', NULL, '2008-08-01 08:12:00', '2008-08-01 08:15:00')
INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('8032000008160011', '�����������', '00001', 1, '�����', @xmlData, '363490F6-33C8-4DED-9EA9-A283D5B51076', '', '2008-08-03 11:01:00', '2008-08-03 11:11:00')
INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('8032000008160012', 'װ��', '00001', 0, NULL, 'Test Data', NEWID(), 'װ�ɸˣ�00001��װ���ذ壺00002', '2008-08-02 08:01:00', '2008-08-02 08:11:00')
INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('8032000008160012', '�ϻ�', '00001', NULL, NULL, NULL, NEWID(), NULL, '2008-08-02 09:01:00', NULL)
INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('8032000008160013', 'װ��', '00001', 0, NULL, 'Test Data', NEWID(), 'װ�ɸˣ�00001��װ���ذ壺00002', '2008-08-01 08:01:00', '2008-08-01 08:11:00')

SET @xmlData = '<?xml version="1.0"?>
<NewDataSet>
  <Info>
    <Content>�����������</Content>
    <ver>1</ver>
    <Description>���ǳ���������Բ�������</Description>
  </Info>
  <fieldname>
    <field1>����</field1>
    <field2>�ز�</field2>
    <field3>����</field3>
    <field4>����</field4>
  </fieldname>
  <Data>
    <field1>����111</field1>
    <field2>����112</field2>
    <field3>����113</field3>
    <field4>����114</field4>
  </Data>
  <Data>
    <field1>����121</field1>
    <field2>����122</field2>
    <field3>����123</field3>
    <field4>����124</field4>
  </Data>
  <Data>
    <field1>����131</field1>
    <field2>����132</field2>
    <field3>����133</field3>
    <field4>����134</field4>
  </Data>
</NewDataSet>'

INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('8032000008160011', '�����������', '00001', 1, NULL, @xmlData, '363490F6-33C8-4DED-9EA9-A283D5B51176', '', '2008-07-03 11:01:00', '2008-07-03 11:11:28')

--################################################################################
PRINT ''
PRINT '������������״̬����'
--################################################################################
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160011', 'װ��', '2151D7E5-C0E5-47AE-9DDF-7950596ACFB8')
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160011', '����', '57965653-D022-4473-A1BF-3647A4BCE389')
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160011', '�ϻ�', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160011', '��װ', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160011', '�����������', '363490F6-33C8-4DED-9EA9-A283D5B51076')
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160011', '1dBѹ�������', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160012', 'װ��', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160012', '����', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160013', 'װ��', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160014', 'װ��', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160014', '����', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160014', '�ϻ�', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160014', '��װ', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160014', '�����������', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160020', '��װ', NEWID())

--################################################################################
PRINT ''
PRINT '�����¼���¼����'
--################################################################################
INSERT INTO TB_Event(EmplyeeID, EventTime, EventRecord) VALUES ('00001', '2008-07-21 09:01:00', '�½�Ʒ��')
INSERT INTO TB_Event(EmplyeeID, EventTime, EventRecord) VALUES ('00002', '2008-08-1 09:01:00', '�½��ƻ���')
INSERT INTO TB_Event(EmplyeeID, EventTime, EventRecord) VALUES ('07033', GETDATE(), '�½�Ʒ��')
INSERT INTO TB_Event(EmplyeeID, EventTime, EventRecord) VALUES ('07032', GETDATE(), 'ǻ����')

--################################################################################
PRINT ''
PRINT '����ȱ����𵵰�����'
--################################################################################
INSERT INTO TA_BugType(ID, Bug) VALUES ('1000', 'פ������')
INSERT INTO TA_BugType(ID, Bug) VALUES ('1001', '��������')
INSERT INTO TA_BugType(ID, Bug) VALUES ('1002', 'ȱ�����2')
INSERT INTO TA_BugType(ID, Bug) VALUES ('1003', 'ȱ�����3')
INSERT INTO TA_BugType(ID, Bug) VALUES ('1004', 'ȱ�����4')
INSERT INTO TA_BugType(ID, Bug) VALUES ('1100', '����')

--################################################################################
PRINT ''
PRINT '����ȱ�ݶ�λ�㵵������'
--################################################################################
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320000', '2000', '���ذ�')
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320000', '2001', '��λ��1')
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320000', '2002', '��λ��2')
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320000', '2003', '��λ��3')
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320000', '2004', '��λ��4')
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320001', '2001', 'Ʒ��1��λ��1')
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320001', '2002', 'Ʒ��1��λ��2')
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320001', '2003', 'Ʒ��1��λ��3')

--################################################################################
PRINT ''
PRINT '�����Ʒ���޼�¼����'
--################################################################################
INSERT INTO TB_RepairRecord(ProductID, DetectTime, DetectProcess, DetectEmployeeID, [Exception], BugID, BugPointCode, RepairTime, RepairEmployeeID, RepairInfo)
       VALUES ('8032000008160011', '2008-07-21 09:01:00', '�����������', '07032', 'פ����', '1000', '2000', '2008-08-08 08:08:08', '00001', '��123����123����123����123����123����123����123����123����123��')
INSERT INTO TB_RepairRecord(ProductID, DetectTime, DetectProcess, DetectEmployeeID, [Exception], BugID, BugPointCode, RepairTime, RepairEmployeeID, RepairInfo)
       VALUES ('8032000008160011', '2008-08-21 09:01:00', 'IP3����', '07033', 'ָ�겻����', '1001', '2002', '2008-09-08 08:08:08', '00002', '�㶨abc�˸㶨abc�˸㶨abc�˸㶨abc�˸㶨abc��')
INSERT INTO TB_RepairRecord(ProductID, DetectTime, DetectProcess, DetectEmployeeID, [Exception], BugID, BugPointCode, RepairTime, RepairEmployeeID, RepairInfo)
       VALUES ('8032000008160012', '2008-07-21 09:01:00', '�����������', '07032', '������', '1001', '2001', '2008-08-08 08:08:08', '00001', '��1a��')
INSERT INTO TB_RepairRecord(ProductID, DetectTime, DetectProcess, DetectEmployeeID, [Exception], BugID, BugPointCode, RepairTime, RepairEmployeeID, RepairInfo)
       VALUES ('8032000008160020', '2008-07-21 09:01:00', '�����������', '07032', 'פ����', '1000', '2000', '2008-08-08 08:08:08', '00001', '��2b��')

--################################################################################
PRINT ''
PRINT '�����Ǳ�У׼��¼����'
--################################################################################
INSERT INTO TB_InstrumentCalRecord(Model, SN, ProductModel, EmployeeID, CalTime, Data)
       VALUES ('Aglient E5071B', '12345678', '80320000', '07032', '2008-07-21 09:01:00', 'abcdefg')
INSERT INTO TB_InstrumentCalRecord(Model, SN, ProductModel, EmployeeID, CalTime, Data)
       VALUES ('Aglient E5071B', '12345678', '80320000', '07032', '2008-07-21 09:00:59', 'hijklmn')

--################################################################################
PRINT ''
PRINT '��������汾����'
--################################################################################
INSERT INTO TB_SoftwareVersion(Name, Version) VALUES ('IIP3���Գ���', '1.8')
