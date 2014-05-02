using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Xml;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

namespace FINGU.MCS
{
    /// <summary>
    /// 过程控制接口：此接口函数接收调用方输入的品号、作业员工号，查询数据库后判定作业员当前所执行的生产计划是否是针对该品号，并向调用方返回一个bool值指示判定结果。
    /// 如果是部件，也能够判定该部件是否是生产计划中产品下的部件。
    /// </summary>
    [WebService(Namespace = "http://fingu.com/mcs")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class ProcedureCtrl : System.Web.Services.WebService
    {
        protected SqlConnection sqlConnectionMain;
        protected SqlCommand sqlCommandMain;
        protected SqlDataReader DataReaderMain;

        public ProcedureCtrl()
        {
            string strConn = ConfigurationManager.ConnectionStrings["mcsdbConnectionString"].ConnectionString;
            this.sqlConnectionMain = new SqlConnection();
            this.sqlConnectionMain.ConnectionString = strConn;

            this.sqlCommandMain = new SqlCommand();
            this.sqlCommandMain.Connection = sqlConnectionMain;

        }


        [WebMethod(Description = "品号检查")]
        public bool CheckModel(string ModelID, string UID, int Flag)
        {
            if (Flag == 1)
            {
                return true;
            }

            // 根据员工工号查询该员工当前应生产的产品的品号
            try
            {
                sqlConnectionMain.Open();

                string sql;

                sql = "SELECT TA_Plan.ModelID " +
                        "FROM TRE_Group_Employee INNER JOIN " +
                        "TA_Group ON TRE_Group_Employee.GroupID = TA_Group.ID INNER JOIN " +
                        "TRE_Group_Plan ON TRE_Group_Plan.GroupID = TA_Group.ID INNER JOIN " +
                        "TA_Plan ON TRE_Group_Plan.PlanID = TA_Plan.ID " +
                        "WHERE (TRE_Group_Employee.EmployeeID = @EmployeeID) AND (TA_Plan.State = 2) AND TA_Plan.ModelID = @ModelID";
                sqlCommandMain.CommandText = sql;
                sqlCommandMain.Parameters.Clear();
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EmployeeID", System.Data.SqlDbType.VarChar, 50, "EmployeeID"));
                sqlCommandMain.Parameters["@EmployeeID"].Value = UID;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ModelID", System.Data.SqlDbType.VarChar, 50, "ModelID"));
                sqlCommandMain.Parameters["@ModelID"].Value = ModelID;

                DataReaderMain = sqlCommandMain.ExecuteReader();
                if (DataReaderMain.Read())
                {
                    DataReaderMain.Close();
                    return true;
                    ////从返回数据集中获得ModelID字段的值
                    //string mID = DataReaderMain.GetString(DataReaderMain.GetOrdinal("ModelID"));

                    //if (mID == ModelID)
                    //{
                    //    DataReaderMain.Close();
                    //    return true;
                    //}
                    //else
                    //{
                    //    // 检查是否是部件品号，如果下属零部件有多级，是否会漏掉？？
                    //    ProductTrace pt = new ProductTrace();
                    //    string[] Items = pt.GetItem(mID);

                    //    foreach (string item in Items)
                    //    {
                    //        if (item == ModelID)
                    //        {
                    //            DataReaderMain.Close();
                    //            return true;
                    //        }
                    //    }
                    //}
                }

                DataReaderMain.Close();
                return false;
            }
            finally
            {
                sqlConnectionMain.Close();
            }
        }

        [WebMethod(Description = "通过条码来做品号检查")]
        public bool CheckModelByBarCode(string BarCode, string UID, int Flag)
        {
            // 获取品号
            string oMsg;
            ProductTrace pt = new ProductTrace();
            string ModelID = pt.GetModelID(BarCode, out oMsg);
            if (ModelID == string.Empty)
            {
                return false;
            }

            return CheckModel(ModelID, UID, Flag);
        }

        [WebMethod(Description = "查询产品是否处于返修状态")]
        public bool RepairProduct(string BarCode)
        {
            // 获取产品序列号
            string oMsg;
            ProductTrace pt = new ProductTrace();
            string ProductID = pt.GetProductID(BarCode, out oMsg);
            if (ProductID == string.Empty)
            {
                return false;
            }

            try
            {
                sqlConnectionMain.Open();

                string sql;

                sql = "SELECT TOP 1 Result FROM TB_ProcedureHistory WHERE (ProductID = @ProductID) " +
                        "ORDER BY BeginTime DESC";
                sqlCommandMain.CommandText = sql;
                sqlCommandMain.Parameters.Clear();
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50, "ProductID"));
                sqlCommandMain.Parameters["@ProductID"].Value = ProductID;

                DataReaderMain = sqlCommandMain.ExecuteReader();
                if (DataReaderMain.Read())
                {
                    if (!DataReaderMain.IsDBNull(DataReaderMain.GetOrdinal("Result")))
                    {
                        int Result = DataReaderMain.GetInt32(DataReaderMain.GetOrdinal("Result"));

                        if (Result == 1)
                        {
                            DataReaderMain.Close();
                            return true;
                        }
                    }
                }

                DataReaderMain.Close();
                return false;
            }
            finally
            {
                sqlConnectionMain.Close();
            }
        }

        //加入，判断该产品的流程是否发生变更，仿RepaiProduct
        [WebMethod(Description = "查询产品生产流程是否发生更改")]
        public bool ProcedureChanged(string BarCode)
        {
            // 获取产品序列号
            string oMsg;
            ProductTrace pt = new ProductTrace();
            string ProductID = pt.GetProductID(BarCode, out oMsg);
            if (ProductID == string.Empty)
            {
                return false;
            }

            try
            {
                sqlConnectionMain.Open();

                string sql;

                sql = "SELECT TOP 1 Result FROM TB_ProcedureHistory WHERE (ProductID = @ProductID) " +
                        "ORDER BY BeginTime DESC";
                sqlCommandMain.CommandText = sql;
                sqlCommandMain.Parameters.Clear();
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50, "ProductID"));
                sqlCommandMain.Parameters["@ProductID"].Value = ProductID;
                //sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PlanID", System.Data.SqlDbType.VarChar, 50, "PlanID"));
                //sqlCommandMain.Parameters["@PlanID"].Value = PlanID;

                DataReaderMain = sqlCommandMain.ExecuteReader();
                if (DataReaderMain.Read())
                {
                    if (!DataReaderMain.IsDBNull(DataReaderMain.GetOrdinal("Result")))
                    {
                        int Result = DataReaderMain.GetInt32(DataReaderMain.GetOrdinal("Result"));

                        if (Result == 2)
                        {
                            DataReaderMain.Close();
                            return true;
                        }
                    }
                }

                DataReaderMain.Close();
                return false;
            }
            finally
            {
                sqlConnectionMain.Close();
            }
        }

        [WebMethod(Description = "工艺流程检查")]
        public bool CheckProcedure(string BarCode, string Process, int BeginOrEnd, out string oMsg)
        {
            // 根据条码获得产品序列号
            ProductTrace pt = new ProductTrace();
            string ProductID = pt.GetProductID(BarCode, out oMsg);
            if (ProductID == string.Empty)
            {
                return false;
            }

            string sql;
            bool ret;

            //首先判断产品是否处于返修状态
            if (RepairProduct(ProductID))
            {
                oMsg = "产品需返修";
                return false;
            }

            //查工序配置表
            DataSet dataSet = GetProcedure(ProductID);
            if (dataSet == null)
            {
                oMsg = "工艺流程不存在或定义错误";
                return false;
            }

            //查输入的工序参数是否存在
            DataView vProcess = new DataView(dataSet.Tables["Process"]);
            vProcess.RowFilter = string.Format("Name = '{0}'", Process);// 注意：此处的比较为大小写不敏感的，故有下一句的再次比较
            if (vProcess.Count == 0 || vProcess[0]["Name"].ToString() != Process)
            {
                oMsg = "输入参数错误（工序不存在）";
                return false;
            }

            //加入，判断产品流程是否发生变更
            if (ProcedureChanged(ProductID))
            {
                oMsg = "因流程变更，本工序不受控";
                return true;
            }

            try
            {
                sqlConnectionMain.Open();

                switch (BeginOrEnd)
                {
                    case 0:     //begin
                        // 判断上一道工序是否未结束
                        sql = "SELECT * FROM ("
                            +"    SELECT TOP 1 Result FROM TB_ProcedureHistory WHERE (ProductID = @ProductID) ORDER BY BeginTime DESC"
                            + ") A WHERE Result is NULL";
                        sqlCommandMain.CommandText = sql;
                        sqlCommandMain.Parameters.Clear();
                        sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50, "ProductID"));
                        sqlCommandMain.Parameters["@ProductID"].Value = ProductID;

                        DataReaderMain = sqlCommandMain.ExecuteReader();
                        if (DataReaderMain.Read())
                        {
                            oMsg = "上一道工序未结束";
                            ret = false;
                            break;
                        }
                        DataReaderMain.Close();
                        sqlConnectionMain.Close();

                        //下面代码对于有重名工序的流程无法通过
                        //得到所有已完成的工序，并比较被检查的工序是否已包含在已完成工序中
                        string[] FinishedProcess = GetFinishedProcess(ProductID);
                        if (FinishedProcess.Contains(Process))
                        {
                            oMsg = "本工序已完成";
                            ret = false;
                        }
                        else
                        {
                            //获得当前工序的所有入口工序
                            DataView view = new DataView(dataSet.Tables["Connection"]);
                            view.RowFilter = string.Format("To = '{0}'", Process);
                            ArrayList InProcess = new ArrayList();
                            for (int i = 0; i < view.Count; i++)
                            {
                                InProcess.Add(view[i]["From"]);
                            }

                            //比较后可知当前工序的所有入口工序是否都已完成
                            string[] tmpIn = (string[])InProcess.ToArray(typeof(string));
                            string[] tmp = FinishedProcess.Intersect(tmpIn).ToArray();
                            tmp = tmpIn.Except(tmp).ToArray();
                            //tmp = tmp.Count() == 0 ? tmpIn : tmp.Except(tmpIn).ToArray();

                            if (tmp.Count() == 0)
                            {
                                oMsg = string.Empty;
                                ret = true;
                            }
                            else
                            {
                                oMsg = "尚未执行到本工序";
                                ret = false;
                            }
                        }
                        break;
                    case 1:     //end
                        sql = "SELECT TOP 1 Process, Result FROM TB_ProcedureHistory WHERE (ProductID = @ProductID)" +
                                "ORDER BY BeginTime DESC";
                        sqlCommandMain.CommandText = sql;
                        sqlCommandMain.Parameters.Clear();
                        sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50, "ProductID"));
                        sqlCommandMain.Parameters["@ProductID"].Value = ProductID;

                        DataReaderMain = sqlCommandMain.ExecuteReader();
                        if (DataReaderMain.Read())
                        {
                            string QueryProcess = DataReaderMain.GetString(DataReaderMain.GetOrdinal("Process"));
                            if (QueryProcess == Process)
                            {
                                if (DataReaderMain.IsDBNull(DataReaderMain.GetOrdinal("Result")))
                                {
                                    oMsg = string.Empty;
                                    ret = true;
                                    break;
                                }
                                else
                                {
                                    oMsg = "本工序已完成";
                                    ret = false;
                                    break;
                                }
                            }
                        }

                        oMsg = "本工序不是当前工序";
                        ret = false;
                        break;
                    default:
                        oMsg = "输入参数错误（无效的工序状态）";
                        ret = false;
                        break;
                }

                DataReaderMain.Close();
                return ret;
            }
            finally
            {
                sqlConnectionMain.Close();
            }
        }

        public string GetProcessGraph(string ProductID)
        {
            DataSet ds = new DataSet();
            string sql;
            SqlDataAdapter adapter = new SqlDataAdapter();

            sql = "SELECT ProcessGraph FROM TA_Procedure WHERE (ID = (SELECT procedureid FROM TA_Product WHERE TA_Product.ID = @ProductID))";
            sqlCommandMain.CommandText = sql;
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50, "ProductID"));
            sqlCommandMain.Parameters["@ProductID"].Value = ProductID;

            adapter.SelectCommand = sqlCommandMain;

            DataSet dataSet = new DataSet();
            try
            {
                sqlConnectionMain.Open();

                adapter.Fill(dataSet, "TA_Procedure");
            }
            finally
            {
                sqlConnectionMain.Close();
            }

            if (dataSet.Tables["TA_Procedure"].Rows.Count > 0)
            {
                DataRow row = dataSet.Tables["TA_Procedure"].Rows[0];
                string tmp = row["ProcessGraph"].ToString();

                return tmp;
            }
            else
            {
                return string.Empty;
            }

        }

        //获取流程定义，转换为数据集输出
        public DataSet GetProcedure(string ProductID)
        {
            DataSet ds = new DataSet();
            string sql;
            SqlDataAdapter adapter = new SqlDataAdapter();

            //这里改为直接从计划单获取最新关联的工艺流程号（原来是从产品档案表中获取首次上线时的关联的流程）
            sql = "SELECT ProcessConfig FROM TA_Procedure WHERE " +
                "(ID = (SELECT C.ProcedureID FROM TC_PlanProcedure C, TA_Product A WHERE C.PlanID = A.PlanID AND C.ModelID = A.ModelID AND A.ID = @ProductID))";
            sqlCommandMain.CommandText = sql;
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50, "ProductID"));
            sqlCommandMain.Parameters["@ProductID"].Value = ProductID;

            adapter.SelectCommand = sqlCommandMain;

            DataSet dataSet = new DataSet();
            try
            {
                sqlConnectionMain.Open();

                adapter.Fill(dataSet, "TA_Procedure");
            }
            finally
            {
                sqlConnectionMain.Close();
            }

            if (dataSet.Tables["TA_Procedure"].Rows.Count > 0)
            {
                string tmp;

                DataRow row = dataSet.Tables["TA_Procedure"].Rows[0];
                if (!row.IsNull("ProcessConfig"))
                {
                    tmp = row["ProcessConfig"].ToString();
                }
                else
                {
                    return null;
                }

                try
                {
                    XmlTextReader reader = new XmlTextReader(tmp, XmlNodeType.Document, null);
                    ds.ReadXml(reader);

                    return ds;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        [WebMethod(Description = "是否可进入下一工序")]
        public bool CanDoNext(string BarCode, string Process)
        {
            // 获取产品序列号
            string oMsg;
            ProductTrace pt = new ProductTrace();
            string ProductID = pt.GetProductID(BarCode, out oMsg);
            if (ProductID == string.Empty)
            {
                return false;
            }

            // 获取所有下道工序
            DataSet ProcessConfig = GetProcedure(ProductID);
            if (ProcessConfig == null)
            {
                return false;
            }

            DataView view = new DataView(ProcessConfig.Tables["Connection"]);
            view.RowFilter = string.Format("From = '{0}'", Process);
            ArrayList OutProcess = new ArrayList();
            for (int i = 0; i < view.Count; i++)
            {
                OutProcess.Add(view[i]["To"]);
            }

            // 逐个比较这些工序的上道工序是否已完成，只要有一个完成就认为可进入下道工序
            foreach (string tmp in OutProcess)
            {

                if (CheckProcedure(BarCode, tmp, 0, out oMsg))
                    return true;
            }

            return false;
        }

        [WebMethod(Description = "查询产品已作业工序")]
        public string[] GetFinishedProcess(string BarCode)
        {
            //string[] id1 = { "装配", "总装", "老化", "测试" };
            //string[] id2 = { "装配1", "总装1" };
            //ArrayList al1 = new ArrayList(id1);
            //ArrayList al2 = new ArrayList(id2);

            //string[] id3 = id1.Intersect(id2).ToArray();
            //string[] id4 = id2.Except(id3).ToArray();
            //string[] id5 = id3.Except(id2).ToArray();
            //string[] id6 = id3.Intersect(id2).ToArray();
            //string[] id7 = id2.Intersect(id3).ToArray();

            //if (id3.Count() == 0)
            //{
            //    return id2;
            //}
            //{
            //    return id1;
            //}
            //foreach (string id in both)
            //    Console.WriteLine(id);

            // 获取产品序列号
            string oMsg;
            ProductTrace pt = new ProductTrace();
            string ProductID = pt.GetProductID(BarCode, out oMsg);
            if (ProductID == string.Empty)
            {
                string[] ret1 = new string[0];
                return ret1;
            }

            string sql;
            SqlDataAdapter adapter = new SqlDataAdapter();

            sql = "SELECT Process FROM TB_ProcedureState WHERE (ProductID = @ProductID)";
            sqlCommandMain.CommandText = sql;
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50, "ProductID"));
            sqlCommandMain.Parameters["@ProductID"].Value = ProductID;

            adapter.SelectCommand = sqlCommandMain;

            DataSet dataSet = new DataSet();
            try
            {
                sqlConnectionMain.Open();

                adapter.Fill(dataSet, "TB_ProcedureState");
            }
            finally
            {
                sqlConnectionMain.Close();
            }

            int n = dataSet.Tables["TB_ProcedureState"].Rows.Count;
            string[] ret = new string[n];
            n = 0;
            foreach (DataRow row in dataSet.Tables["TB_ProcedureState"].Rows)
            {
                ret[n] = row["Process"].ToString();
                n++;
            }

            return ret;
        }

        protected string GetDispatch(string EmployeeID)
        {
            string sql;
            SqlDataAdapter adapter = new SqlDataAdapter();

            sql = "SELECT TA_Group.WorkDispatch FROM TA_Group INNER JOIN TRE_Group_Employee ON TA_Group.ID = TRE_Group_Employee.GroupID " +
                  "WHERE (TRE_Group_Employee.EmployeeID = @EmployeeID)";
            sqlCommandMain.CommandText = sql;
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EmployeeID", System.Data.SqlDbType.VarChar, 50, "EmployeeID"));
            sqlCommandMain.Parameters["@EmployeeID"].Value = EmployeeID;

            adapter.SelectCommand = sqlCommandMain;

            DataSet dataSet = new DataSet();
            try
            {
                sqlConnectionMain.Open();

                adapter.Fill(dataSet, "TA_Group");
            }
            finally
            {
                sqlConnectionMain.Close();
            }

            if (dataSet.Tables["TA_Group"].Rows.Count > 0)
            {
                DataRow row = dataSet.Tables["TA_Group"].Rows[0];
                string tmp = row["WorkDispatch"].ToString();

                return tmp;
            }
            else
            {
                return string.Empty;
            }
        }

        public string GetRange(string ProductID, string Process)
        {
            string Range;

            DataSet dataSet = GetProcedure(ProductID);
            if (dataSet == null)
            {
                return string.Empty;
            }

            //查输入的工序参数是否存在
            DataView vProcess = new DataView(dataSet.Tables["Process"]);
            vProcess.RowFilter = string.Format("Name = '{0}'", Process);
            if (vProcess.Count > 0)
            {
                Range = vProcess[0]["Range"].ToString();
            }
            else
            {
                return string.Empty;
            }

            //得到前一工序范围
            var query =
                from row in dataSet.Tables["Process"].AsEnumerable()
                group row by row.Field<string>("Range") into range
                orderby range.Key
                //where range.Key != Range
                select range.Key;

            List<string> aaa = query.ToList<string>();
            string preRange = "";
            foreach (string r in query)
            {
                if (r != Range)
                {
                    preRange = r;
                }
                else
                    break;
            }

            //得到所有同一工序范围内的工序
            var query1 =
                from row in dataSet.Tables["Process"].AsEnumerable()
                where row["Range"].ToString() == Range
                select row["Name"].ToString();

            List<string> AllProcessInRange = query1.ToList<string>();
            //List<string> AllProcessInRange = new List<string>(new string[]{"IIC读写","点频互调测试","前向功率测试","整机IM测试","VSWR告警测试"});
            AllProcessInRange.Remove(Process);

            //得到所有已完成的工序
            string[] FinishedProcess = GetFinishedProcess(ProductID);

            //比较后可知当前工序的所有同范围工序是否都已完成
            string[] tmp = FinishedProcess.Intersect(AllProcessInRange).ToArray();
            tmp = AllProcessInRange.Except(tmp).ToArray();

            if (tmp.Count() == 0)
            {
                return Range;
            }
            else
            {
                return preRange;
            }
        }

        //记录生产流程历史 
        protected void InsertProcedureHistory(string ProductID, string Process, string EmployeeID, int Result,
                                string Exception, string Data, Guid DataID, DateTime BeginTime, DateTime EndTime, string WorkDispatch)
        {
            sqlCommandMain.CommandText = "INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, " +
                                         "Result, [Exception], Data, DataID, Dispatch, BeginTime, EndTime) " +
                                         "VALUES (@ProductID, @Process, @EmployeeID, @Result, @Exception, " +
                                         "@Data, @DataID, @Dispatch, @BeginTime, @EndTime)";
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@ProductID"].Value = ProductID;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Process", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@Process"].Value = Process;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EmployeeID", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@EmployeeID"].Value = EmployeeID;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Result", System.Data.SqlDbType.Int, 50));
            sqlCommandMain.Parameters["@Result"].Value = Result != int.MaxValue ? Result : System.Data.SqlTypes.SqlInt32.Null;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Exception", System.Data.SqlDbType.VarChar, 2000));
            sqlCommandMain.Parameters["@Exception"].Value = (System.Data.SqlTypes.SqlString)Exception;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Data", System.Data.SqlDbType.Text));
            sqlCommandMain.Parameters["@Data"].Value = (System.Data.SqlTypes.SqlString)Data;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DataID", System.Data.SqlDbType.UniqueIdentifier));
            sqlCommandMain.Parameters["@DataID"].Value = DataID;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Dispatch", System.Data.SqlDbType.VarChar, 2000));
            sqlCommandMain.Parameters["@Dispatch"].Value = (System.Data.SqlTypes.SqlString)WorkDispatch;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginTime", System.Data.SqlDbType.DateTime));
            sqlCommandMain.Parameters["@BeginTime"].Value = EndTime != DateTime.MaxValue ? DateTime.Now - (EndTime - BeginTime) : DateTime.Now;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndTime", System.Data.SqlDbType.DateTime));
            sqlCommandMain.Parameters["@EndTime"].Value = EndTime != DateTime.MaxValue ? DateTime.Now : System.Data.SqlTypes.SqlDateTime.Null;
            sqlCommandMain.ExecuteNonQuery();
        }

        //更新生产流程历史 
        protected void UpdateProcedureHistory(string ProductID, string Process, int Result, string Exception,
                                string Data, DateTime EndTime, out Guid DataID)
        {
            string sql;
            SqlDataAdapter adapter = new SqlDataAdapter();

            sql = "SELECT TOP 1 DataID FROM TB_ProcedureHistory WHERE (ProductID = @ProductID) AND (Process = @Process) AND (Result IS NULL)"
                + " ORDER BY BeginTime DESC";
            sqlCommandMain.CommandText = sql;
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50, "ProductID"));
            sqlCommandMain.Parameters["@ProductID"].Value = ProductID;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Process", System.Data.SqlDbType.VarChar, 50, "Process"));
            sqlCommandMain.Parameters["@Process"].Value = Process;

            adapter.SelectCommand = sqlCommandMain;

            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet, "TB_ProcedureHistory");

            if (dataSet.Tables["TB_ProcedureHistory"].Rows.Count == 1)
            {
                DataRow row = dataSet.Tables["TB_ProcedureHistory"].Rows[0];
                DataID = row.Field<Guid>("DataID");//row["DataID"].ToString();

                sqlCommandMain.CommandText = "UPDATE TB_ProcedureHistory SET Result =@Result, [Exception] =@Exception, " +
                                             "Data =@Data, EndTime =@EndTime WHERE (ProductID = @ProductID) AND " +
                                             "(Process = @Process) AND (Result IS NULL) AND (DataID = @DataID)";
                sqlCommandMain.Parameters.Clear();
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DataID", System.Data.SqlDbType.UniqueIdentifier));
                sqlCommandMain.Parameters["@DataID"].Value = DataID;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50));
                sqlCommandMain.Parameters["@ProductID"].Value = ProductID;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Process", System.Data.SqlDbType.VarChar, 50));
                sqlCommandMain.Parameters["@Process"].Value = Process;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Result", System.Data.SqlDbType.Int, 50));
                sqlCommandMain.Parameters["@Result"].Value = Result;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Exception", System.Data.SqlDbType.VarChar, 2000));
                sqlCommandMain.Parameters["@Exception"].Value = (System.Data.SqlTypes.SqlString)Exception;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Data", System.Data.SqlDbType.Text));
                sqlCommandMain.Parameters["@Data"].Value = (System.Data.SqlTypes.SqlString)Data;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndTime", System.Data.SqlDbType.DateTime));
                sqlCommandMain.Parameters["@EndTime"].Value = DateTime.Now;
                sqlCommandMain.ExecuteNonQuery();
            }
            else
            {
                throw new Exception("当前工序还未扫入，因此不能扫出！");
            }

        }

        //记录生产流程状态 
        private void InsertProcedureState(string ProductID, string Process, Guid DataID)
        {
            //如果有同名工序的旧记录，先删除，再插入新的
            sqlCommandMain.CommandText = "DELETE TB_ProcedureState WHERE ProductID = @ProductID AND Process = @Process";
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@ProductID"].Value = ProductID;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Process", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@Process"].Value = Process;
            sqlCommandMain.ExecuteNonQuery();
                        
            sqlCommandMain.CommandText =
                "INSERT INTO TB_ProcedureState (ProductID, Process, DataID) VALUES (@ProductID, @Process, @DataID)";
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@ProductID"].Value = ProductID;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Process", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@Process"].Value = Process;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DataID", System.Data.SqlDbType.UniqueIdentifier));
            sqlCommandMain.Parameters["@DataID"].Value = DataID;// DBNull.Value;// System.Data.SqlTypes.SqlString.Null;// null;//.Value = Data;
            //sqlCommandMain.Parameters["@Data"].IsNullable = true;//.Value = Data;
            sqlCommandMain.ExecuteNonQuery();
        }

        //更新产品档案表中的字段[ManufactureState]
        private void UpdateManufactureState(string ProductID, string Range)
        {
            sqlCommandMain.CommandText =
                "UPDATE TA_Product SET ManufactureState = @ManufactureState WHERE (ID = @ProductID)";
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@ProductID"].Value = ProductID;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ManufactureState", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@ManufactureState"].Value = (System.Data.SqlTypes.SqlString)Range;
            sqlCommandMain.ExecuteNonQuery();
        }

        [WebMethod(Description = "只保存工序记录及数据，不检查工序流程")]
        public bool SaveProcessPurely(string BarCode, string Process, string EmployeeID, int Result,
                                string Exception, string Data, DateTime BeginTime, DateTime EndTime,
                                out string oMsg)
        {
            //作业结果检查
            if (Result != 0 && Result != 1 && Result != 2)
            {
                oMsg = "作业结果中输入未定义值";
                return false;
            }

            //数据格式检查
            try
            {
                DataFormatChecker.CheckFormat(Data);
            }
            catch (Exception e)
            {
                oMsg = e.Message;
                return false;
            }

            //写数据库
            try
            {
                sqlConnectionMain.Open();

                SqlTransaction transaction = sqlConnectionMain.BeginTransaction();
                sqlCommandMain.Transaction = transaction;

                try
                {
                    Guid DataID = Guid.NewGuid();

                    //记录生产流程历史 
                    InsertProcedureHistory(BarCode, Process, EmployeeID, Result, Exception, Data, DataID, BeginTime, EndTime, "NoWorkDispatch");

                    // Attempt to commit the transaction.
                    transaction.Commit();

                    oMsg = string.Empty;
                    return true;
                }
                catch (Exception ex)
                {
                    Trace.Write(ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Trace.Write(e.Message);
                    }

                    oMsg = "写数据库出错";
                    return false;
                }
            }
            finally
            {
                sqlConnectionMain.Close();
            }
        }

        [WebMethod(Description = "保存工序记录及数据")]
        public bool SaveProcess(string BarCode, string Process, string EmployeeID, int Result,
                                string Exception, string Data, DateTime BeginTime, DateTime EndTime,
                                out string oMsg)
        {
            //作业结果检查
            if (Result != 0 && Result != 1)
            {
                oMsg = "作业结果中输入未定义值";
                return false;
            }

            //数据格式检查
            try
            {
                DataFormatChecker.CheckFormat(Data);
            }
            catch (Exception e)
            {
                oMsg = e.Message;
                return false;
            }

            //工艺流程检查
            if (!CheckProcedure(BarCode, Process, 0, out oMsg))
            {
                return false;
            }

            // 获取产品序列号
            ProductTrace pt = new ProductTrace();
            string ProductID = pt.GetProductID(BarCode, out oMsg);
            if (ProductID == string.Empty)
            {
                return false;
            }

            //获取班组排工信息
            string WorkDispatch = GetDispatch(EmployeeID);

            //获取工序的工段范围
            string Range;
            if (Result == 0)
                Range = GetRange(ProductID, Process);
            else
                Range = "返修";

            //写数据库
            try
            {
                sqlConnectionMain.Open();

                SqlTransaction transaction = sqlConnectionMain.BeginTransaction();
                sqlCommandMain.Transaction = transaction;

                try
                {
                    Guid DataID = Guid.NewGuid();

                    //记录生产流程历史 
                    InsertProcedureHistory(ProductID, Process, EmployeeID, Result, Exception, Data, DataID, BeginTime, EndTime, WorkDispatch);

                    //记录生产流程状态 
                    InsertProcedureState(ProductID, Process, DataID);

                    //更新产品档案表中的字段[ManufactureState]
                    UpdateManufactureState(ProductID, Range);

                    // Attempt to commit the transaction.
                    transaction.Commit();

                    oMsg = string.Empty;
                    return true;
                }
                catch (Exception ex)
                {
                    Trace.Write(ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Trace.Write(e.Message);
                    }

                    oMsg = "写数据库出错";
                    return false;
                }
            }
            finally
            {
                sqlConnectionMain.Close();
            }
        }

        [WebMethod(Description = "保存工序信息(工序开始)")]
        public bool SaveProcessBegin(string BarCode, string Process, string EmployeeID,
                                DateTime BeginTime, out string oMsg)
        {
            //工艺流程检查
            if (!CheckProcedure(BarCode, Process, 0, out oMsg))
            {
                return false;
            }

            // 获取产品序列号
            ProductTrace pt = new ProductTrace();
            string ProductID = pt.GetProductID(BarCode, out oMsg);
            if (ProductID == string.Empty)
            {
                return false;
            }

            //获取班组排工信息
            string WorkDispatch = GetDispatch(EmployeeID);

            //写数据库
            try
            {
                sqlConnectionMain.Open();

                SqlTransaction transaction = sqlConnectionMain.BeginTransaction();
                sqlCommandMain.Transaction = transaction;

                try
                {
                    Guid DataID = Guid.NewGuid();

                    //记录生产流程历史 
                    InsertProcedureHistory(ProductID, Process, EmployeeID, int.MaxValue, null, null, DataID, BeginTime, DateTime.MaxValue, WorkDispatch);

                    ////记录生产流程状态 
                    //InsertProcedureState(ProductID, Process, null);

                    ////更新产品档案表中的字段[ManufactureState]
                    //UpdateManufactureState(ProductID, null);

                    // Attempt to commit the transaction.
                    transaction.Commit();

                    oMsg = string.Empty;
                    return true;
                }
                catch (Exception ex)
                {
                    Trace.Write(ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Trace.Write(e.Message);
                    }

                    oMsg = "写数据库出错";
                    return false;
                }
            }
            finally
            {
                sqlConnectionMain.Close();
            }
        }

        [WebMethod(Description = "保存工序信息(工序结束)")]
        public bool SaveProcessEnd(string BarCode, string Process, int Result, string Exception,
                                string Data, DateTime EndTime, out string oMsg)
        {
            //作业结果检查
            if (Result != 0 && Result != 1)
            {
                oMsg = "作业结果中输入未定义值";
                return false;
            }

            //数据格式检查
            try
            {
                DataFormatChecker.CheckFormat(Data);
            }
            catch (Exception e)
            {
                oMsg = e.Message;
                return false;
            }

            //工艺流程检查
            if (!CheckProcedure(BarCode, Process, 1, out oMsg))
            {
                return false;
            }

            // 获取产品序列号
            ProductTrace pt = new ProductTrace();
            string ProductID = pt.GetProductID(BarCode, out oMsg);
            if (ProductID == string.Empty)
            {
                return false;
            }

            //获取工序的工段范围
            string Range;
            if (Result == 0)
                Range = GetRange(ProductID, Process);
            else
                Range = "返修";

            //写数据库
            try
            {
                sqlConnectionMain.Open();

                SqlTransaction transaction = sqlConnectionMain.BeginTransaction();
                sqlCommandMain.Transaction = transaction;

                try
                {
                    Guid DataID;

                    //记录生产流程历史 
                    UpdateProcedureHistory(ProductID, Process, Result, Exception, Data, EndTime, out DataID);

                    //记录生产流程状态 
                    InsertProcedureState(ProductID, Process, DataID);

                    //更新产品档案表中的字段[ManufactureState]
                    UpdateManufactureState(ProductID, Range);

                    // Attempt to commit the transaction.
                    transaction.Commit();

                    oMsg = string.Empty;
                    return true;
                }
                catch (Exception ex)
                {
                    Trace.Write(ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Trace.Write(e.Message);
                    }

                    oMsg = "写数据库出错：" + ex.Message;
                    return false;
                }
            }
            finally
            {
                sqlConnectionMain.Close();
            }
        }

        [WebMethod(Description = "查询产品数据")]
        public string GetData(string BarCode, string Process)
        {
            // 获取产品序列号
            string oMsg;
            ProductTrace pt = new ProductTrace();
            string ProductID = pt.GetProductID(BarCode, out oMsg);
            if (ProductID == string.Empty)
            {
                return string.Empty;
            }

            string sql;
            SqlDataAdapter adapter = new SqlDataAdapter();

            sql = "SELECT Data FROM TB_ProcedureHistory  " +
                  "WHERE (ProductID = @ProductID) AND (Process = @Process)";
            sqlCommandMain.CommandText = sql;
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50, "ProductID"));
            sqlCommandMain.Parameters["@ProductID"].Value = ProductID;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Process", System.Data.SqlDbType.VarChar, 50, "Process"));
            sqlCommandMain.Parameters["@Process"].Value = Process;

            adapter.SelectCommand = sqlCommandMain;

            DataSet dataSet = new DataSet();
            try
            {
                sqlConnectionMain.Open();

                adapter.Fill(dataSet, "TB_ProcedureHistory");
            }
            finally
            {
                sqlConnectionMain.Close();
            }

            if (dataSet.Tables["TB_ProcedureHistory"].Rows.Count > 0)
            {
                DataRow row = dataSet.Tables["TB_ProcedureHistory"].Rows[0];
                string tmp = row["Data"].ToString();

                return tmp;
            }
            else
            {
                return string.Empty;
            }
        }

        [WebMethod(Description = "查询自动化测试功能的配置信息")]
        public string GetTestingConfiguration(string ConfigurationName)
        {
            try
            {
                string configPath = ConfigurationManager.AppSettings["ConfigPath"].ToString();
                return File.ReadAllText(configPath + ConfigurationName + ".config");
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
