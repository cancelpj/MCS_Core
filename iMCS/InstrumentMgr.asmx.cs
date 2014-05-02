using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;

namespace FINGU.MCS
{
    /// <summary>
    /// 仪表管理接口：保存仪表的校准数据。保存后向调用方返回一个bool值指示保存操作是否正确完成，如不正确还将在输出参数中说明保存不正确的原因。
    /// 可能的保存操作不正确的原因包括：数据[Data]格式不正确、写数据库出错。
    /// </summary>
    [WebService(Namespace = "http://fingu.com/mcs")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class InstrumentMgr : System.Web.Services.WebService
    {
        protected SqlConnection sqlConnectionMain;
        protected SqlCommand sqlCommandMain;
        protected SqlDataAdapter sqlDataAdapterMain;
        protected DataSet dataSetMain;

        public InstrumentMgr()
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

        [WebMethod(Description = "供测试使用")]
        public void test()
        {
            string oMsg;
            bool ret = CheckCalibration("E5071C", "MY46104690", "029", 12, out oMsg);

        }

        [WebMethod(Description = "保存校准记录")]
        public bool SaveCalibrateRecord(string Model, string SN, string ProductModel, string EmployeeID, string Data, out string oMsg)
        {
            //数据格式检查
            try
            {
                DataFormatChecker.CheckCalFormat(Data);
            }
            catch (Exception e)
            {
                oMsg = e.Message;
                return false;
            }

            try
            {
                sqlCommandMain.CommandText = "INSERT INTO TB_InstrumentCalRecord(Model, SN, ProductModel, EmployeeID, CalTime, Data) " +
                                             "VALUES (@Model, @SN, @ProductModel, @EmployeeID, GETDATE(), @Data)";
                sqlCommandMain.Parameters.Clear();
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Model", System.Data.SqlDbType.VarChar, 50));
                sqlCommandMain.Parameters["@Model"].Value = Model;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SN", System.Data.SqlDbType.VarChar, 50));
                sqlCommandMain.Parameters["@SN"].Value = SN;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductModel", System.Data.SqlDbType.VarChar, 50));
                sqlCommandMain.Parameters["@ProductModel"].Value = ProductModel;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EmployeeID", System.Data.SqlDbType.VarChar, 50));
                sqlCommandMain.Parameters["@EmployeeID"].Value = EmployeeID;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Data", System.Data.SqlDbType.Text));
                sqlCommandMain.Parameters["@Data"].Value = (System.Data.SqlTypes.SqlString)Data;
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
            catch (Exception ex)
            {
                Trace.Write(ex.Message);

                oMsg = "写数据库出错：" + ex.Message;
                return false;
            }

            oMsg = string.Empty;
            return true;
        }

        [WebMethod(Description = "验证仪表校准")]
        public bool CheckCalibration(string Model, string SN, string ProductModel, int Interval, out string oMsg)
        {
            string sql;

            sql = "SELECT TOP 1 CalTime FROM TB_InstrumentCalRecord WHERE (Model = @Model) AND (SN = @SN) AND (ProductModel = @ProductModel) ORDER BY CalTime DESC";
            sqlCommandMain.CommandText = sql;
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Model", System.Data.SqlDbType.VarChar, 50, "Model"));
            sqlCommandMain.Parameters["@Model"].Value = Model;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SN", System.Data.SqlDbType.VarChar, 50, "SN"));
            sqlCommandMain.Parameters["@SN"].Value = SN;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductModel", System.Data.SqlDbType.VarChar, 50, "ProductModel"));
            sqlCommandMain.Parameters["@ProductModel"].Value = ProductModel;

            try
            {
                sqlConnectionMain.Open();

                sqlDataAdapterMain.Fill(dataSetMain, "TB_InstrumentCalRecord");
            }
            finally
            {
                sqlConnectionMain.Close();
            }

            if (dataSetMain.Tables["TB_InstrumentCalRecord"].Rows.Count > 0)
            {
                DataRow row = dataSetMain.Tables["TB_InstrumentCalRecord"].Rows[0];
                DateTime tmp = (DateTime)row["CalTime"];

                if (DateTime.Now - tmp < TimeSpan.FromHours(Interval))
                {
                    oMsg = string.Empty;
                    return true;
                }
                else
                {
                    oMsg = "仪表校准已超时";
                    return false;
                }
            }
            else
            {
                oMsg = "该仪表校准记录不存在";
                return false;
            }
        }

        [WebMethod(Description = "查询仪表校准数据")]
        public string GetCalibrateData(string Model, string SN, string ProductModel)
        {
            string sql;

            sql = "SELECT TOP 1 Data FROM TB_InstrumentCalRecord WHERE (Model = @Model) AND (SN = @SN) AND (ProductModel = @ProductModel) ORDER BY CalTime DESC";
            sqlCommandMain.CommandText = sql;
            sqlCommandMain.Parameters.Clear();
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Model", System.Data.SqlDbType.VarChar, 50, "Model"));
            sqlCommandMain.Parameters["@Model"].Value = Model;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@SN", System.Data.SqlDbType.VarChar, 50, "SN"));
            sqlCommandMain.Parameters["@SN"].Value = SN;
            sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductModel", System.Data.SqlDbType.VarChar, 50, "ProductModel"));
            sqlCommandMain.Parameters["@ProductModel"].Value = ProductModel;

            try
            {
                sqlConnectionMain.Open();

                sqlDataAdapterMain.Fill(dataSetMain, "TB_InstrumentCalRecord");
            }
            finally
            {
                sqlConnectionMain.Close();
            }

            if (dataSetMain.Tables["TB_InstrumentCalRecord"].Rows.Count > 0)
            {
                DataRow row = dataSetMain.Tables["TB_InstrumentCalRecord"].Rows[0];
                string tmp = row["Data"].ToString();

                return tmp;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
