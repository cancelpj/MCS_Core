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

namespace MCSApp.Application.QueryManage
{
    public partial class ProduceFinished : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "一级数据查看者" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
            if (!IsPostBack)
            {
                ViewState["conStr"] = "";//数据库where 部分用的字符串
                this.dt_begin_UseBox.Value = System.DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                this.dt_end_UseBox.Value = System.DateTime.Now.ToString("yyyy-MM-dd");
                this.dt_begin_UseBox.Disabled = false;
                this.dt_end_UseBox.Disabled = false;
            }

        }

        protected void ckCancelTime_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckCancelTime.Checked)
            {
                this.dt_begin_UseBox.Disabled = false;
                this.dt_end_UseBox.Disabled = false;
            }
            else
            {
                this.dt_begin_UseBox.Disabled = true;
                this.dt_end_UseBox.Disabled = true;
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string bstr = "";
            string estr = "";
            string whstr = "";
            string datestr = "";
            DateTime endTime;

            try
            {
                endTime = DateTime.Parse(this.dt_end_UseBox.Value);
            }
            catch (Exception)
            {
                Methods.AjaxMessageBox(this, "输入的日期格式不正确！"); return;
            }

            datestr = "endTime";
            if (!this.dt_begin_UseBox.Disabled)
            {
                bstr = " and " + datestr + ">='" + this.dt_begin_UseBox.Value + "'";
            }
            if (!this.dt_end_UseBox.Disabled)
            {
                estr = " and " + datestr + "<='" + endTime.AddDays(1).ToString("yyyy-MM-dd") + "'";
            }

            whstr = " and productid in (select id from ta_product where ModelType=1 and planid='" + this.txtID.Text.Trim() + "')";

            ViewState["conStr"] = whstr + bstr + estr;
            this.GridBind();

        }

        private void GridBind()
        {
            sqlaccess.Open();
            try
            {
                string tmpsql = "";
                tmpsql = " select t.id planid,t.output,t.modelid,tm.name modelName,tm.code from ta_plan t,ta_model tm where t.modelid = tm.id and t.state= 2 and t.id='" + this.txtID.Text.Trim() + "'";

                ds = sqlaccess.OpenQuerry(tmpsql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    this.lblPlanID.Text = ds.Tables[0].Rows[0][0].ToString();
                    this.lblOutput.Text = ds.Tables[0].Rows[0][1].ToString();
                    this.lblModelName.Text = ds.Tables[0].Rows[0][3].ToString();
                    this.lblCode.Text = ds.Tables[0].Rows[0][4].ToString();
                }
                else
                {
                    this.lblPlanID.Text = "----";
                    this.lblOutput.Text = "----";
                    this.lblModelName.Text = "----";
                    this.lblCode.Text = "----";
                }

                tmpsql = " select process,ISNULL(sum(case when result=0 then 1 end),0) finished,ISNULL(sum(case when result=1 then 1 end),0) repaired from tb_procedureHistory where 1=1" + ViewState["conStr"]
                        + " group by process";
                ds = sqlaccess.OpenQuerry(tmpsql); ;
                this.GridView1.DataSource = ds;
                this.GridView1.DataBind();

            }
            catch (Exception ex)
            {
                LogManager.Write(this, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }
        }


    }
}
