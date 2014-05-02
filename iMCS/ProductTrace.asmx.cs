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
using System.Diagnostics;

namespace FINGU.MCS
{
    public class Product
    {
        public string ID;                      //产品序列号
        public string SN;                      //产品条码
        public string ModelID;                 //品号
        public string PlanID;                  //生产计划号
        public int ProcedureID;                //工艺流程编号
        public DateTime FoundTime;             //建立时间
        public string ManufactureState;        //产品当前生产状态即当前工序类别
        public string Remark;                  //备注

        public List<int> aa;

    }

    /// <summary>
    /// 产品追溯接口：保存产品的档案信息。保存后向调用方返回一个bool值指示保存操作是否正确完成，如不正确还将在输出参数中说明保存不正确的原因。
    /// 可能的保存操作不正确的原因包括：不是一个产品、计划品号与当前品号冲突、写数据库出错。
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class ProductTrace : System.Web.Services.WebService
    {
        protected SqlConnection sqlConnectionMain;
        protected SqlCommand sqlCommandMain;
        protected SqlDataAdapter sqlDataAdapterMain;
        protected DataSet dataSetMain;

        public ProductTrace()
        {
            string strConn = ConfigurationManager.ConnectionStrings["mcsdbConnectionString"].ConnectionString;
            this.sqlConnectionMain = new SqlConnection();
            this.sqlConnectionMain.ConnectionString = strConn;

            this.sqlCommandMain = new SqlCommand();
            this.sqlCommandMain.Connection = sqlConnectionMain;

            this.sqlDataAdapterMain = new SqlDataAdapter();
            sqlDataAdapterMain.SelectCommand = sqlCommandMain;

            this.dataSetMain = new DataSet();
        }

        // 从数据库中取得产品档案信息
        public Product GetProduct(string BarCode, out string oMsg)
        {
            Product product = new Product();
            string sql;

            sql = "SELECT * FROM TA_Product WHERE (ID = @BarCode)";
            sqlCommandMain.CommandText = sql;
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BarCode", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@BarCode"].Value = BarCode;

            try
            {
                sqlConnectionMain.Open();

                sqlDataAdapterMain.Fill(dataSetMain, "TA_Product");
            }
            finally
            {
                sqlConnectionMain.Close();
            }
            int n = dataSetMain.Tables["TA_Product"].Rows.Count;
            if (n < 1)
            {

                oMsg = "根据输入的条形码未查询到对应的产品";
                return null;
            }
            else if (n == 1)
            {
                DataRow row = dataSetMain.Tables["TA_Product"].Rows[0];

                product.ID = row["ID"].ToString();
                product.SN = row["SN"].ToString();
                product.ModelID = row["ModelID"].ToString();
                product.PlanID = row["PlanID"].ToString();
                product.ProcedureID = (int)row["ProcedureID"];
                product.FoundTime = (DateTime)row["FoundTime"];
                product.ManufactureState = row.IsNull("ManufactureState") ? null : row["ManufactureState"].ToString();
                product.Remark = row.Field<string>("Remark");// row.IsNull("Remark") ? null : row["Remark"].ToString();

                oMsg = string.Empty;
                return product;
            }
            else
            {

                oMsg = "居然发生了几乎不可能发生的事情，真是让人惊讶";
                return null;
            }
        }

        // 获取产品下属零部件信息（一级），得到组成项品号数组
        public string[] GetItem(string ModelID)
        {
            string sql;

            sql = "SELECT ID, ItemID FROM TA_Structure WHERE (ID = @ModelID)";
            sqlCommandMain.CommandText = sql;
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ModelID", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@ModelID"].Value = ModelID;

            try
            {
                sqlConnectionMain.Open();

                sqlDataAdapterMain.Fill(dataSetMain, "TA_Structure");
            }
            finally
            {
                sqlConnectionMain.Close();
            }

            int n = dataSetMain.Tables["TA_Structure"].Rows.Count;
            string[] ret = new string[n];
            n = 0;
            foreach (DataRow row in dataSetMain.Tables["TA_Structure"].Rows)
            {
                ret[n] = row["ItemID"].ToString();
                n++;
            }

            return ret;
        }

        public string GetProductID(string BarCode, out string oMsg)
        {
            Product product = GetProduct(BarCode, out oMsg);

            if (product == null)
            {
                return string.Empty;
            }
            else
            {
                return product.ID;
            }

        }

        [WebMethod(Description = "查询产品品号")]
        public string GetModelID(string BarCode, out string oMsg)
        {
            Product product = GetProduct(BarCode, out oMsg);

            if (product == null)
            {
                return string.Empty;
            }
            else
            {
                return product.ModelID;
            }

        }

        [WebMethod(Description = "查询客户产品条码")]
        public string GetSN(string BarCode, out string oMsg)
        {
            Product product = GetProduct(BarCode, out oMsg);

            if (product == null)
            {
                return string.Empty;
            }
            else
            {
                return product.SN;
            }

        }

        //查询品号类型
        public int GetModelType(string ModelID)
        {
            string sql;

            sql = "SELECT ModelType FROM TA_Model WHERE (ID = @ID)";
            sqlCommandMain.CommandText = sql;
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@ID"].Value = ModelID;

            try
            {
                sqlConnectionMain.Open();

                sqlDataAdapterMain.Fill(dataSetMain, "TA_Model");
            }
            finally
            {
                sqlConnectionMain.Close();
            }

            int n = dataSetMain.Tables["TA_Model"].Rows.Count;
            if (n > 0)
            {
                DataRow row = dataSetMain.Tables["TA_Model"].Rows[0];

                int ModelType = (int)row["ModelType"];

                return ModelType;
            }
            else
            {
                return int.MaxValue;
            }
        }

        //根据用户ID和品号查询生产计划及流程（不支持同一品号的多个计划）
        public bool GetPlanProcedure(string ModelID, string UID, out string PlanID, out int ProcedureID)
        {
            string sql;

            sql = "SELECT TC_PlanProcedure.PlanID, TC_PlanProcedure.ProcedureID FROM TA_Group INNER JOIN " +
                  "TRE_Group_Employee ON TA_Group.ID = TRE_Group_Employee.GroupID INNER JOIN " +
                  "TA_Group_Plan ON TA_Group_Plan.GroupID = TA_Group.ID INNER JOIN " +
                  "TC_PlanProcedure ON TA_Group_Plan.PlanID = TC_PlanProcedure.PlanID " +
                  "WHERE (TC_PlanProcedure.ModelID = @ModelID) AND (TRE_Group_Employee.EmployeeID = @UID)";
            sqlCommandMain.CommandText = sql;
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@UID", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@UID"].Value = UID;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ModelID", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@ModelID"].Value = ModelID;

            dataSetMain.Clear();
            try
            {
                sqlConnectionMain.Open();

                sqlDataAdapterMain.Fill(dataSetMain, "TC_PlanProcedure");
            }
            finally
            {
                sqlConnectionMain.Close();
            }

            int n = dataSetMain.Tables["TC_PlanProcedure"].Rows.Count;
            if (n > 0)
            {
                DataRow row = dataSetMain.Tables["TC_PlanProcedure"].Rows[0];

                PlanID = row["PlanID"].ToString();
                ProcedureID = (int)row["ProcedureID"];

                return true;
            }
            else
            {
                PlanID = string.Empty;
                ProcedureID = 0;

                return false;
            }
        }

        [WebMethod(Description = "查询生产计划号")]
        public string GetPlanID(string BarCode, out string oMsg)
        {
            Product product = GetProduct(BarCode, out oMsg);

            if (product == null)
            {
                return string.Empty;
            }
            else
            {
                return product.PlanID;
            }
        }

        
        //根据品号和计划号查询工艺流程
        public bool GetProcedure(string ModelID, string PlanID, out int ProcedureID)
        {
            string sql;

            sql = "SELECT ProcedureID FROM TC_PlanProcedure WHERE (ModelID = @ModelID) AND (PlanID = @PlanID)";
            sqlCommandMain.CommandText = sql;
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ModelID", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@ModelID"].Value = ModelID;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PlanID", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@PlanID"].Value = PlanID;

            dataSetMain.Clear();
            try
            {
                sqlConnectionMain.Open();

                sqlDataAdapterMain.Fill(dataSetMain, "TC_PlanProcedure");
            }
            finally
            {
                sqlConnectionMain.Close();
            }

            int n = dataSetMain.Tables["TC_PlanProcedure"].Rows.Count;
            if (n > 0)
            {
                DataRow row = dataSetMain.Tables["TC_PlanProcedure"].Rows[0];

                //PlanID = row["PlanID"].ToString();
                ProcedureID = (int)row["ProcedureID"];

                return true;
            }
            else
            {
                //PlanID = string.Empty;
                ProcedureID = 0;

                return false;
            }
        }

        //新建产品记录 
        protected void InsertProduct(string ID, string ModelID, int ModelType, string PlanID, int ProcedureID, string Remark)
        {

            sqlCommandMain.CommandText = "INSERT INTO TA_Product(ID, SN, ModelID, ModelType, PlanID, ProcedureID, FoundTime, ManufactureState, Remark) " +
                         "VALUES (@ID, '', @ModelID, @ModelType, @PlanID, @ProcedureID, GETDATE(), NULL, @Remark)";
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@ID"].Value = ID;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ModelID", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@ModelID"].Value = ModelID;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ModelType", System.Data.SqlDbType.Int, 50));
            sqlCommandMain.Parameters["@ModelType"].Value = ModelType;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PlanID", System.Data.SqlDbType.VarChar, 50));
            sqlCommandMain.Parameters["@PlanID"].Value = PlanID;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProcedureID", System.Data.SqlDbType.Int, 50));
            sqlCommandMain.Parameters["@ProcedureID"].Value = ProcedureID;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Remark", System.Data.SqlDbType.VarChar, 100));
            sqlCommandMain.Parameters["@Remark"].Value = Remark;
            try
            {
                sqlConnectionMain.Open();

                sqlCommandMain.ExecuteNonQuery();
            }
            finally
            {
                sqlConnectionMain.Close();
            }
        }

        [WebMethod(Description = "保存产品信息，加入计划号")]
        public bool SaveProduct(string PlanID, string UID, string ID, string ModelID, string Remark, out string oMsg)
        {
            //判断是否为一个产品
            if (GetModelType(ModelID) != 1)
            {
                oMsg = "不是一个产品";
                return false;
            }

            //检查品号
            ProcedureCtrl PC = new ProcedureCtrl();
            if (!PC.CheckModel(ModelID, UID, 0))
            {
                oMsg = "计划品号与当前品号冲突";
                return false;
            }

            //string PlanID;
            int ProcedureID;
            if (!GetProcedure(ModelID, PlanID, out ProcedureID))
            {
                oMsg = "未定义计划工艺流程";
                return false;
            }

            try
            {
                InsertProduct(ID, ModelID, 1, PlanID, ProcedureID, Remark);
            }
            catch (Exception ex)
            {
                Trace.Write(ex.Message);

                oMsg = "写数据库出错：" + ex.Message;
                return false;
            }

            oMsg = string.Empty;
            return true;
        }

        [WebMethod(Description = "保存部件信息，加入计划号")]
        public bool SaveComponent(string PlanID, string UID, string ID, string ModelID, string Remark, out string oMsg)
        {
            //判断是否为一个部件
            if (GetModelType(ModelID) != 2)
            {
                oMsg = "不是一个部件";
                return false;
            }

            //检查品号
            ProcedureCtrl PC = new ProcedureCtrl();
            if (!PC.CheckModel(ModelID, UID, 0))
            {
                oMsg = "计划品号与当前品号冲突";
                return false;
            }

            //string PlanID;
            int ProcedureID;
            if (!GetProcedure(ModelID, PlanID, out ProcedureID))
            {
                oMsg = "未定义计划工艺流程";
                return false;
            }

            try
            {
                InsertProduct(ID, ModelID, 2, PlanID, ProcedureID, Remark);
            }
            catch (Exception ex)
            {
                Trace.Write(ex.Message);

                oMsg = "写数据库出错";
                return false;
            }

            oMsg = string.Empty;
            return true;
        }
    }
}
