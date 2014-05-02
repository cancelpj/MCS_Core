using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.Data.SqlClient;
using MCSApp.DataAccess;
using MCSApp.DataAccess.LogManage;
using MCSApp.Common;


namespace MCSApp.Application.ProcedureManage
{
    public partial class PermitRetesting : PageBase
    {
        //DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "班组长" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
            if (!IsPostBack)
            {
                //Methods.DDLBind(this.ddltype, "select id value,Bug text  from ta_bugType");
            }
        }

        protected void btnAddRcd_Click(object sender, EventArgs e)
        {
            string tmpPid = this.txtID.Text.Trim();
            if (this.RadioButton2.Checked)
            {
                tmpPid = Methods.getProductIDFromPSN(this.txtID.Text.Trim());//由SN找产品序列号 
            }

            if (!Methods.HasExceptonInHistory(tmpPid))
            {
                Methods.AjaxMessageBox(this, "此产品无故障工序记录！");

                this.lblEProcess.Text = "";
                this.lblFinder.Text = "";
                this.lblFinderID.Text = "";
                this.lblFindTime.Text = "";
                this.lblException.Text = "";
                this.ctrlEnable(false); 
                return;
            }
            this.ctrlEnable(true);

            string tmpModelID = Methods.getModelIDByProductID(tmpPid);
            int tmpProcedureID = Methods.getProcedureIDByProductID(tmpPid);

            ViewState["tmpPid"] = tmpPid;//本页全局产品ID
            ViewState["tmpModelID"] = tmpModelID;//本页全局品号ID
            ViewState["tmpProcedureID"] = tmpProcedureID;//本页全局工艺流程ID

            //Methods.DDLBugPointBind(this.ddlPoint, tmpModelID);
            //这里添加设置的发现异常的部分实现 

            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.AddWithValue("@p1", this.txtID.Text.Trim());
            cmd.CommandText = " select top 1 t.productid,t.process,t.result, t.employeeid,te.name,t.exception,t.begintime from tb_procedureHistory t,ta_employee te where t.employeeid=te.id and t.productid = @p1 order by t.begintime desc";
            DataSet tmpds = sqlaccess.OpenQuerry(cmd);
            if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
            {
                DataRow r = tmpds.Tables[0].Rows[0];
                int result = r.IsNull("result") ? 0 : (int)r["result"];

                if (result == 1)
                {
                    this.lblEProcess.Text = tmpds.Tables[0].Rows[0]["process"].ToString();
                    this.lblFinder.Text = tmpds.Tables[0].Rows[0]["name"].ToString();
                    this.lblFinderID.Text = tmpds.Tables[0].Rows[0]["employeeid"].ToString();
                    this.lblFindTime.Text = tmpds.Tables[0].Rows[0]["begintime"].ToString();
                    this.lblException.Text = tmpds.Tables[0].Rows[0]["exception"].ToString();
                }
                else
                {
                    Methods.AjaxMessageBox(this, "此产品没有异常记录!"); return;
                }

            }
            else
            {
                Methods.AjaxMessageBox(this, "此产品没有异常记录!"); return;
            }


        }

        private void ctrlEnable(bool enable)
        {
            this.btnSave.Enabled = enable;
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["tmpPid"] == null || ViewState["tmpPid"].ToString() == string.Empty)
                {
                    Methods.AjaxMessageBox(this, "无产品序列号，不能完成操作!"); return;

                }
                if (this.lblFindTime.Text.Trim() == string.Empty)
                {
                    Methods.AjaxMessageBox(this, "无发现时间，不能完成操作!"); return;
                }

                sqlaccess.Open();

                sqlaccess.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO TB_ProcedureHistory(ProductID, Process, EmployeeID, " +
                             "Result, [Exception], Data, DataID, Dispatch, BeginTime, EndTime) " +
                             "VALUES (@ProductID, @Process, @EmployeeID, @Result, @Exception, " +
                             "@Data, @DataID, @Dispatch, @BeginTime, @EndTime)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ProductID", System.Data.SqlDbType.VarChar, 50));
                cmd.Parameters["@ProductID"].Value = ViewState["tmpPid"].ToString();
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Process", System.Data.SqlDbType.VarChar, 50));
                cmd.Parameters["@Process"].Value = "批准复检";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EmployeeID", System.Data.SqlDbType.VarChar, 50));
                cmd.Parameters["@EmployeeID"].Value = SessionUser.ID;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Result", System.Data.SqlDbType.Int, 50));
                cmd.Parameters["@Result"].Value = 0;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Exception", System.Data.SqlDbType.VarChar, 2000));
                cmd.Parameters["@Exception"].Value = "";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Data", System.Data.SqlDbType.Text));
                cmd.Parameters["@Data"].Value = "";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@DataID", System.Data.SqlDbType.UniqueIdentifier));
                cmd.Parameters["@DataID"].Value = Guid.NewGuid();
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Dispatch", System.Data.SqlDbType.VarChar, 2000));
                cmd.Parameters["@Dispatch"].Value = "";
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@BeginTime", System.Data.SqlDbType.DateTime));
                cmd.Parameters["@BeginTime"].Value = DateTime.Now;
                cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EndTime", System.Data.SqlDbType.DateTime));
                cmd.Parameters["@EndTime"].Value = DateTime.Now;

                sqlaccess.ExecuteQuerry(cmd);

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@p1", ViewState["tmpPid"].ToString());
                cmd.Parameters.AddWithValue("@p2", this.lblEProcess.Text);

                cmd.CommandText = "delete TB_ProcedureState where productid=@p1 and process=@p2";
                sqlaccess.ExecuteQuerry(cmd);

                sqlaccess.Commit();

                this.lblEProcess.Text = "";
                this.lblFinder.Text = "";
                this.lblFinderID.Text = "";
                this.lblFindTime.Text = "";
                this.lblException.Text = "";
                this.ctrlEnable(false);
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                Methods.AjaxMessageBox(this, ex.Message); return;
            }
            finally
            {
                sqlaccess.Close();
            }

        }

    }
}
