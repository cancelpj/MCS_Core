
/***********************************************
--
--本文件包含一套为mcsdb数据库加入初始数据的SQL脚本
--用来插入每个表中的初始数据
--这些数据可供测试使用
--可用如下命令来执行
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
PRINT '插入品号数据'
--################################################################################
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('80320000', 'NTGZ83AAE6', 'SP156', 'NortelSN001', 1)
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('80320001', 'NTGZ83AAE7', 'SP157', 'NortelSN002', 1)
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('80320002', 'NTGZ83AAE8', 'SP158', 'NortelSN003', 1)
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('60110000', 'SP156报警板', 'FG00001', NULL, 2)
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('60110001', 'SP157报警板', 'FG00002', NULL, 2)
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('60110002', 'SP156低噪放', 'FG00003', NULL, 2)
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('70110001', '谐振杆', 'FG00102', NULL, 3)
INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES ('70110002', '抽头', 'FG00103', NULL, 3)

--批量生成50个品号
DECLARE @i int
DECLARE @sql nvarchar(400)

SET @i = 1

WHILE (@i <= 50)
BEGIN
  SET @sql = 'INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) VALUES (''0000' + CAST(@i AS nvarchar) + ''', ''PCB板' + CAST(@i AS nvarchar) + ''', ''FG' + CAST(@i AS nvarchar) + ''', NULL, 2)'
	
  SET @i = @i + 1
  EXEC (@sql) 
END
GO

--################################################################################
PRINT ''
PRINT '插入产品结构数据'
--################################################################################
INSERT INTO TA_Structure(ID, ItemID, Amount) VALUES ('80320000', '60110000', 1)
INSERT INTO TA_Structure(ID, ItemID, Amount) VALUES ('80320000', '60110002', 1)
INSERT INTO TA_Structure(ID, ItemID, Amount) VALUES ('80320000', '70110001', 1)
INSERT INTO TA_Structure(ID, ItemID, Amount) VALUES ('80320000', '70110002', 1)
INSERT INTO TA_Structure(ID, ItemID, Amount) VALUES ('80320001', '60110001', 1)

--################################################################################
PRINT ''
PRINT '插入生产计划数据'
--################################################################################
INSERT INTO TA_Plan(ID, ModelID, Output, OrderID, PlanType, Remark, FoundTime, CloseTime, State) VALUES ('08080001', '80320000', 300, 'NokiaOrder0001', 1, 'Test Plan', '2008-01-01 00:01:00', NULL, 2)
INSERT INTO TA_Plan(ID, ModelID, Output, OrderID, PlanType, Remark, FoundTime, CloseTime, State) VALUES ('08080002', '80320001', 700, 'NokiaOrder0002', 1, 'Test Plan', '2008-03-01 00:01:00', NULL, 2)
INSERT INTO TA_Plan(ID, ModelID, Output, OrderID, PlanType, Remark, FoundTime, CloseTime, State) VALUES ('08080003', '80320000', 200, 'NokiaOrder0003', 1, 'Test Plan', '2008-01-01 00:01:00', NULL, 2)

INSERT INTO TA_Plan(ID, ModelID, Output, OrderID, PlanType, Remark, FoundTime, CloseTime, State) VALUES ('1', '80320000', 30, '1', 1, 'Test Plan 1', '2008-01-01 00:01:00', NULL, 1)

--################################################################################
PRINT ''
PRINT '插入工艺流程档案数据'
--################################################################################
DECLARE @ProcessConfig varchar(8000)
DECLARE @ProcessConfig2 varchar(8000)
DECLARE @ProcessGraph varchar(8000)

SET @ProcessConfig = '<?xml version="1.0"?>
<NewDataSet>
  <process>
    <Name>装配</Name>
    <Range>1装配</Range>
  </process>
  <process>
    <Name>调试</Name>
    <Range>2调试</Range>
  </process>
  <process>
    <Name>老化</Name>
    <Range>3老化</Range>
  </process>
  <process>
    <Name>总装</Name>
    <Range>4总装</Range>
  </process>
  <process>
    <Name>常规电气测试</Name>
    <Range>5电检</Range>
  </process>
  <process>
    <Name>IP3测试</Name>
    <Range>5电检</Range>
  </process>
  <process>
    <Name>1dB压缩点测试</Name>
    <Range>5电检</Range>
  </process>
  <process>
    <Name>噪声测试</Name>
    <Range>5电检</Range>
  </process>
  <process>
    <Name>包装</Name>
    <Range>6包装</Range>
  </process>
  <process>
    <Name>入库</Name>
    <Range>7入库</Range>
  </process>
  <Connection>
    <From>装配</From>
    <To>调试</To>
  </Connection>
  <Connection>
    <From>调试</From>
    <To>老化</To>
  </Connection>
  <Connection>
    <From>老化</From>
    <To>总装</To>
  </Connection>
  <Connection>
    <From>总装</From>
    <To>常规电气测试</To>
  </Connection>
  <Connection>
    <From>总装</From>
    <To>IP3测试</To>
  </Connection>
  <Connection>
    <From>总装</From>
    <To>1dB压缩点测试</To>
  </Connection>
  <Connection>
    <From>总装</From>
    <To>噪声测试</To>
  </Connection>
  <Connection>
    <From>常规电气测试</From>
    <To>包装</To>
  </Connection>
  <Connection>
    <From>IP3测试</From>
    <To>包装</To>
  </Connection>
  <Connection>
    <From>1dB压缩点测试</From>
    <To>包装</To>
  </Connection>
  <Connection>
    <From>噪声测试</From>
    <To>包装</To>
  </Connection>
  <Connection>
    <From>包装</From>
    <To>入库</To>
  </Connection>
</NewDataSet>'

SET @ProcessConfig2 = '<?xml version="1.0"?>
<NewDataSet>
  <process>
    <Name>装配</Name>
    <Range>1装配</Range>
  </process>
  <process>
    <Name>调试</Name>
    <Range>2调试</Range>
  </process>
  <process>
    <Name>老化</Name>
    <Range>3老化</Range>
  </process>
  <process>
    <Name>总装</Name>
    <Range>4总装</Range>
  </process>
  <Connection>
    <From>装配</From>
    <To>调试</To>
  </Connection>
  <Connection>
    <From>调试</From>
    <To>老化</To>
  </Connection>
  <Connection>
    <From>老化</From>
    <To>总装</To>
  </Connection>
</NewDataSet>'

SET @ProcessGraph = '<ProcedureflowProcess>
<Activities>
<Activitie id="2" type="START_NODE" name="装配" xCoordinate="314" yCoordinate="27" width="80" height="30" range="1装配"/>
<Activitie id="3" type="NODE" name="调试" xCoordinate="496" yCoordinate="123" width="80" height="30" range="2调试"/>
<Activitie id="4" type="NODE" name="老化" xCoordinate="205" yCoordinate="204" width="80" height="30" range="3老化"/>
<Activitie id="6" type="FORK_NODE" name="总装" xCoordinate="400" yCoordinate="318" width="80" height="30" range="4总装"/>
<Activitie id="7" type="NODE" name="常规电气测试" xCoordinate="159" yCoordinate="439" width="80" height="30" range="5电检"/>
<Activitie id="8" type="NODE" name="IP3测试" xCoordinate="279" yCoordinate="439" width="80" height="30" range="5电检"/>
<Activitie id="9" type="NODE" name="1dB压缩点测试" xCoordinate="519" yCoordinate="438" width="80" height="30" range="5电检"/>
<Activitie id="10" type="NODE" name="噪声测试" xCoordinate="640" yCoordinate="439" width="80" height="30" range="5电检"/>
<Activitie id="11" type="JOIN_NODE" name="包装" xCoordinate="399" yCoordinate="538" width="80" height="30" range="6包装"/>
<Activitie id="26" type="END_NODE" name="入库" xCoordinate="399" yCoordinate="638" width="80" height="30" range="7入库"/>
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

INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP156生产工艺流程', '80320000', @ProcessConfig, @ProcessGraph)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP156生产工艺流程2', '80320000', @ProcessConfig2, @ProcessGraph)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP157生产工艺流程', '80320001', @ProcessConfig, @ProcessGraph)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP157生产工艺流程2', '80320001', NULL, @ProcessGraph)

INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP156报警板生产工艺流程', '60110000', '1', NULL)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP156报警板生产工艺流程2', '60110000', '1', NULL)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP157报警板生产工艺流程', '60110001', '1', NULL)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP157报警板生产工艺流程2', '60110001', '1', NULL)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP156低噪放生产工艺流程', '60110002', '1', NULL)
INSERT INTO TA_Procedure(Name, ModelID, ProcessConfig, ProcessGraph) VALUES ('SP156低噪放生产工艺流程2', '60110002', '1', NULL)

--################################################################################
PRINT ''
PRINT '插入计划工艺流程数据'
--################################################################################
DECLARE @ProcedureID int
DECLARE @ProcedureID2 int
DECLARE @ProcedureID3 int

SET @ProcedureID = (SELECT ID FROM TA_Procedure WHERE (Name = 'SP156生产工艺流程'))
SET @ProcedureID2 = (SELECT ID FROM TA_Procedure WHERE (Name = 'SP157生产工艺流程'))
SET @ProcedureID3 = (SELECT ID FROM TA_Procedure WHERE (Name = 'SP156生产工艺流程2'))

INSERT INTO TC_PlanProcedure(PlanID, ModelID, ProcedureID) VALUES ('08080001', '80320000', @ProcedureID)
INSERT INTO TC_PlanProcedure(PlanID, ModelID, ProcedureID) VALUES ('08080001', '60110000', @ProcedureID)
INSERT INTO TC_PlanProcedure(PlanID, ModelID, ProcedureID) VALUES ('08080002', '80320001', @ProcedureID2)
INSERT INTO TC_PlanProcedure(PlanID, ModelID, ProcedureID) VALUES ('08080003', '80320000', @ProcedureID3)

GO

--################################################################################
PRINT ''
PRINT '插入员工帐号数据'
--################################################################################
INSERT INTO TA_Employee(ID, Name, Password, State, Remark) VALUES ('07033', '舒剑', '39pkUkLLbiI=', 1, '开发组成员')
INSERT INTO TA_Employee(ID, Name, Password, State, Remark) VALUES ('07032', '刘亮', '39pkUkLLbiI=', 1, '开发组成员')
INSERT INTO TA_Employee(ID, Name, Password, State, Remark) VALUES ('14146', '徐祖宝', 'XQLzAejM1i8knefIVrBAcQ==', 1, '开发组成员')
INSERT INTO TA_Employee(ID, Name, Password, State, Remark) VALUES ('00000', '测试帐号0', '39pkUkLLbiI=', 2, '用于测试')
--INSERT INTO TA_Employee(ID, Name, Password, State, Remark) VALUES ('00002', '测试帐号2', '39pkUkLLbiI=', 1, '用于测试')
--INSERT INTO TA_Employee(ID, Name, Password, State, Remark) VALUES ('00003', '测试帐号3', '39pkUkLLbiI=', 1, '用于测试')
GO

--批量生成50个员工帐号
DECLARE @i int
DECLARE @sql nvarchar(400)

SET @i = 1

WHILE (@i <= 50)
BEGIN
  SET @sql = 'INSERT INTO TA_Employee(ID, Name, Password, State, Remark) VALUES (''0000' + CAST(@i AS nvarchar) + ''', ''测试帐号' + CAST(@i AS nvarchar) + ''', ''39pkUkLLbiI='', 1, ''用于测试'')'
	
  SET @i = @i + 1
  EXEC (@sql) 
END
GO

--################################################################################
PRINT ''
PRINT '插入作业员与角色的关系'
--################################################################################
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('00000', '作业员')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('07032', '作业员')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('07032', '一级数据查看者')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('07032', '二级数据查看者')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('07032', '三级数据查看者')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('14146', '作业员')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('14146', '班组长')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('00002', '作业员')
INSERT INTO TRE_Employee_Role(EmployeeID, Role) VALUES ('00003', '作业员')

INSERT INTO TRE_Employee_Role(EmployeeID, Role) SELECT '07033', Role FROM TA_Role

--################################################################################
PRINT ''
PRINT '插入班组数据'
--################################################################################
INSERT INTO TA_Group(Name, LeaderID, PlanID, WorkDispatch) VALUES ('ER装配一班', '07033', '08080001', '开盖板：07033;打螺钉：00001')
INSERT INTO TA_Group(Name, LeaderID, PlanID, WorkDispatch) VALUES ('ER装配二班', '07033', '08080002', '开盖板：07033;打螺钉：07032')
INSERT INTO TA_Group(Name, LeaderID, PlanID, WorkDispatch) VALUES ('ER总装一班', '07033', '08080001', NULL)
INSERT INTO TA_Group(Name, LeaderID, PlanID, WorkDispatch) VALUES ('ER电检一班', '07033', '08080003', NULL)

--################################################################################
PRINT ''
PRINT '插入班组员工数据'
--################################################################################
INSERT INTO TRE_Group_Employee(GroupID, EmployeeID) SELECT ID, '00001' AS EmployeeID FROM TA_Group WHERE (Name = 'ER装配一班')
INSERT INTO TRE_Group_Employee(GroupID, EmployeeID) SELECT ID, '00002' AS EmployeeID FROM TA_Group WHERE (Name = 'ER装配二班')
INSERT INTO TRE_Group_Employee(GroupID, EmployeeID) SELECT ID, '00003' AS EmployeeID FROM TA_Group WHERE (Name = 'ER总装一班')
INSERT INTO TRE_Group_Employee(GroupID, EmployeeID) SELECT ID, '07033' AS EmployeeID FROM TA_Group WHERE (Name = 'ER装配一班')
INSERT INTO TRE_Group_Employee(GroupID, EmployeeID) SELECT ID, '07032' AS EmployeeID FROM TA_Group WHERE (Name = 'ER电检一班')
INSERT INTO TRE_Group_Employee(GroupID, EmployeeID) SELECT ID, '14146' AS EmployeeID FROM TA_Group WHERE (Name = 'ER装配一班')

--################################################################################
PRINT ''
PRINT '插入产品档案数据'
--################################################################################
DECLARE @ProcedureID int
DECLARE @ProcedureID2 int

SET @ProcedureID = (SELECT ID FROM TA_Procedure WHERE (Name = 'SP156生产工艺流程'))
SET @ProcedureID2 = (SELECT ID FROM TA_Procedure WHERE (Name = 'SP156报警板生产工艺流程'))

INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('8032000008160011', 'NOKIA001', '80320000', 1, '08080001', @ProcedureID, '2008-05-01 08:01:00', NULL, 'TestProduct')
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('8032000008160012', 'NOKIA002', '80320000', 1, '08080001', @ProcedureID, '2008-05-01 08:01:00', NULL, 'TestProduct')
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('8032000008160013', 'NOKIA003', '80320000', 1, '08080001', @ProcedureID, '2008-05-01 08:01:00', '3老化', 'TestProduct')
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('8032000008160014', 'NOKIA004', '80320000', 1, '08080001', @ProcedureID, '2008-05-01 08:01:00', NULL, 'TestProduct')
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('8032000008160015', 'NOKIA005', '80320000', 1, '08080001', @ProcedureID, '2008-05-01 08:01:00', '6包装', 'TestProduct')
INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) 
            VALUES ('8032000008160019', 'NOKIA009', '80320000', 1, '08080001', @ProcedureID, '2008-05-01 08:01:00', '7入库', 'TestProduct')
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
PRINT '插入物料档案数据'
--################################################################################
INSERT INTO TA_Materiel(ID, ModelID, Batch, Vendor, EmployeeID, FoundTime, Remark) 
            VALUES ('7011000108160011', '70110001', '0834091', '凡谷电子', NULL, '2008-05-01 08:01:00', 'TestMateriel')
INSERT INTO TA_Materiel(ID, ModelID, Batch, Vendor, EmployeeID, FoundTime, Remark) 
            VALUES ('7011000108160012', '70110001', '0834092', '凡谷电子', NULL, '2008-05-01 08:01:00', 'TestMateriel')
INSERT INTO TA_Materiel(ID, ModelID, Batch, Vendor, EmployeeID, FoundTime, Remark) 
            VALUES ('7011000208160011', '70110002', '0844091', '凡谷电子', '00001', '2008-06-01 08:01:00', 'TestMateriel')
INSERT INTO TA_Materiel(ID, ModelID, Batch, Vendor, EmployeeID, FoundTime, Remark) 
            VALUES ('7011000208160012', '70110002', '0844092', '凡谷电子', '00002', '2008-07-01 08:01:00', 'TestMateriel')

--################################################################################
PRINT ''
PRINT '插入产品、部件及物料的关联数据'
--################################################################################
INSERT INTO TA_Relationship(ID, ItemID) VALUES ('8032000008160011', '6011000008160011')
INSERT INTO TA_Relationship(ID, ItemID) VALUES ('8032000008160011', '6011000208160012')
INSERT INTO TA_Relationship(ID, ItemID) VALUES ('8032000008160011', '7011000108160011')
INSERT INTO TA_Relationship(ID, ItemID) VALUES ('8032000108160011', '6011000208160012')
INSERT INTO TA_Relationship(ID, ItemID) VALUES ('8032000108160011', '7011000108160011')

INSERT INTO TA_Relationship(ID, ItemID) VALUES ('6011000008160011', '7011000108160011')

--################################################################################
PRINT ''
PRINT '插入生产流程历史数据'
--################################################################################
DECLARE @xmlData varchar(8000)

SET @xmlData = '<?xml version="1.0"?>
<NewDataSet>
  <Info>
    <Content>IP3测试</Content>
    <ver>1</ver>
    <Description>这是IP3测试数据</Description>
  </Info>
  <fieldname>
    <field1>字段1</field1>
    <field2>字段2</field2>
    <field3>字段3</field3>
    <field4>字段4</field4>
    <field11>字段11</field11>
    <field12>字段12</field12>
    <field13>字段13</field13>
    <field14>字段14</field14>
    <field21>字段21</field21>
    <field22>字段22</field22>
    <field23>字段23</field23>
    <field24>字段24</field24>
    <field31>字段31</field31>
    <field32>字段32</field32>
    <field33>字段33</field33>
    <field34>字段34</field34>
    <field41>字段41</field41>
    <field42>字段42</field42>
    <field43>字段43</field43>
    <field44>字段44</field44>
  </fieldname>
  <Data>
    <field1>数据11</field1>
    <field2>数据12</field2>
    <field3>数据13</field3>
    <field4>数据14</field4>
    <field11>数据11</field11>
    <field12>数据12</field12>
    <field13>数据13</field13>
    <field14>数据14</field14>
    <field21>数据21</field21>
    <field22>数据22</field22>
    <field23>数据23</field23>
    <field24>数据24</field24>
    <field31>数据31</field31>
    <field32>数据32</field32>
    <field33>数据33</field33>
    <field34>数据34</field34>
    <field41>数据41</field41>
    <field42>数据42</field42>
    <field43>数据43</field43>
    <field44>数据44</field44>
  </Data>
</NewDataSet>'

INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('8032000008160011', '装配', '00001', 0, NULL, 'Test Data', '2151D7E5-C0E5-47AE-9DDF-7950596ACFB8', '装飞杆：00001；装主控板：00002', '2008-08-01 08:01:00', '2008-08-01 08:11:00')
INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('8032000008160011', '调试', '00001', 1, '曲线不好', '无数据', '57965653-D022-4473-A1BF-3647A4BCE389', NULL, '2008-08-01 08:12:00', '2008-08-01 08:15:00')
INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('8032000008160011', '常规电气测试', '00001', 1, '增益差', @xmlData, '363490F6-33C8-4DED-9EA9-A283D5B51076', '', '2008-08-03 11:01:00', '2008-08-03 11:11:00')
INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('8032000008160012', '装配', '00001', 0, NULL, 'Test Data', NEWID(), '装飞杆：00001；装主控板：00002', '2008-08-02 08:01:00', '2008-08-02 08:11:00')
INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('8032000008160012', '老化', '00001', NULL, NULL, NULL, NEWID(), NULL, '2008-08-02 09:01:00', NULL)
INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('8032000008160013', '装配', '00001', 0, NULL, 'Test Data', NEWID(), '装飞杆：00001；装主控板：00002', '2008-08-01 08:01:00', '2008-08-01 08:11:00')

SET @xmlData = '<?xml version="1.0"?>
<NewDataSet>
  <Info>
    <Content>常规电气测试</Content>
    <ver>1</ver>
    <Description>这是常规电气测试测试数据</Description>
  </Info>
  <fieldname>
    <field1>插损</field1>
    <field2>回波</field2>
    <field3>抑制</field3>
    <field4>增益</field4>
  </fieldname>
  <Data>
    <field1>数据111</field1>
    <field2>数据112</field2>
    <field3>数据113</field3>
    <field4>数据114</field4>
  </Data>
  <Data>
    <field1>数据121</field1>
    <field2>数据122</field2>
    <field3>数据123</field3>
    <field4>数据124</field4>
  </Data>
  <Data>
    <field1>数据131</field1>
    <field2>数据132</field2>
    <field3>数据133</field3>
    <field4>数据134</field4>
  </Data>
</NewDataSet>'

INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, Dispatch, BeginTime, EndTime) 
            VALUES ('8032000008160011', '常规电气测试', '00001', 1, NULL, @xmlData, '363490F6-33C8-4DED-9EA9-A283D5B51176', '', '2008-07-03 11:01:00', '2008-07-03 11:11:28')

--################################################################################
PRINT ''
PRINT '插入生产流程状态数据'
--################################################################################
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160011', '装配', '2151D7E5-C0E5-47AE-9DDF-7950596ACFB8')
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160011', '调试', '57965653-D022-4473-A1BF-3647A4BCE389')
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160011', '老化', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160011', '总装', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160011', '常规电气测试', '363490F6-33C8-4DED-9EA9-A283D5B51076')
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160011', '1dB压缩点测试', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160012', '装配', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160012', '调试', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160013', '装配', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160014', '装配', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160014', '调试', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160014', '老化', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160014', '总装', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160014', '常规电气测试', NEWID())
INSERT INTO TB_ProcedureState(ProductID, Process, DataID) VALUES ('8032000008160020', '总装', NEWID())

--################################################################################
PRINT ''
PRINT '插入事件记录数据'
--################################################################################
INSERT INTO TB_Event(EmplyeeID, EventTime, EventRecord) VALUES ('00001', '2008-07-21 09:01:00', '新建品号')
INSERT INTO TB_Event(EmplyeeID, EventTime, EventRecord) VALUES ('00002', '2008-08-1 09:01:00', '新建计划单')
INSERT INTO TB_Event(EmplyeeID, EventTime, EventRecord) VALUES ('07033', GETDATE(), '新建品号')
INSERT INTO TB_Event(EmplyeeID, EventTime, EventRecord) VALUES ('07032', GETDATE(), '腔体拆空')

--################################################################################
PRINT ''
PRINT '插入缺陷类别档案数据'
--################################################################################
INSERT INTO TA_BugType(ID, Bug) VALUES ('1000', '驻波问题')
INSERT INTO TA_BugType(ID, Bug) VALUES ('1001', '噪声问题')
INSERT INTO TA_BugType(ID, Bug) VALUES ('1002', '缺陷类别2')
INSERT INTO TA_BugType(ID, Bug) VALUES ('1003', '缺陷类别3')
INSERT INTO TA_BugType(ID, Bug) VALUES ('1004', '缺陷类别4')
INSERT INTO TA_BugType(ID, Bug) VALUES ('1100', '其它')

--################################################################################
PRINT ''
PRINT '插入缺陷定位点档案数据'
--################################################################################
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320000', '2000', '主控板')
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320000', '2001', '定位点1')
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320000', '2002', '定位点2')
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320000', '2003', '定位点3')
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320000', '2004', '定位点4')
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320001', '2001', '品号1定位点1')
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320001', '2002', '品号1定位点2')
INSERT INTO TA_BugPoint(ModelID, BugPointCode, BugPointDsc) VALUES ('80320001', '2003', '品号1定位点3')

--################################################################################
PRINT ''
PRINT '插入产品返修记录数据'
--################################################################################
INSERT INTO TB_RepairRecord(ProductID, DetectTime, DetectProcess, DetectEmployeeID, [Exception], BugID, BugPointCode, RepairTime, RepairEmployeeID, RepairInfo)
       VALUES ('8032000008160011', '2008-07-21 09:01:00', '常规电气测试', '07032', '驻波差', '1000', '2000', '2008-08-08 08:08:08', '00001', '搞123定搞123定搞123定搞123定搞123定搞123定搞123定搞123定搞123定')
INSERT INTO TB_RepairRecord(ProductID, DetectTime, DetectProcess, DetectEmployeeID, [Exception], BugID, BugPointCode, RepairTime, RepairEmployeeID, RepairInfo)
       VALUES ('8032000008160011', '2008-08-21 09:01:00', 'IP3测试', '07033', '指标不正常', '1001', '2002', '2008-09-08 08:08:08', '00002', '搞定abc了搞定abc了搞定abc了搞定abc了搞定abc了')
INSERT INTO TB_RepairRecord(ProductID, DetectTime, DetectProcess, DetectEmployeeID, [Exception], BugID, BugPointCode, RepairTime, RepairEmployeeID, RepairInfo)
       VALUES ('8032000008160012', '2008-07-21 09:01:00', '常规电气测试', '07032', '噪声差', '1001', '2001', '2008-08-08 08:08:08', '00001', '搞1a定')
INSERT INTO TB_RepairRecord(ProductID, DetectTime, DetectProcess, DetectEmployeeID, [Exception], BugID, BugPointCode, RepairTime, RepairEmployeeID, RepairInfo)
       VALUES ('8032000008160020', '2008-07-21 09:01:00', '常规电气测试', '07032', '驻波差', '1000', '2000', '2008-08-08 08:08:08', '00001', '搞2b定')

--################################################################################
PRINT ''
PRINT '插入仪表校准记录数据'
--################################################################################
INSERT INTO TB_InstrumentCalRecord(Model, SN, ProductModel, EmployeeID, CalTime, Data)
       VALUES ('Aglient E5071B', '12345678', '80320000', '07032', '2008-07-21 09:01:00', 'abcdefg')
INSERT INTO TB_InstrumentCalRecord(Model, SN, ProductModel, EmployeeID, CalTime, Data)
       VALUES ('Aglient E5071B', '12345678', '80320000', '07032', '2008-07-21 09:00:59', 'hijklmn')

--################################################################################
PRINT ''
PRINT '插入软件版本数据'
--################################################################################
INSERT INTO TB_SoftwareVersion(Name, Version) VALUES ('IIP3测试程序', '1.8')
