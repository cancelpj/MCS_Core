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
    public partial class RepairRecordInfo : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "维修员" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
            if (!IsPostBack)
            {
                ViewState["conStr"] = "";//数据库where 部分用的字符串
                //this.GridBind();
                this.dt_begin_UseBox.Value = System.DateTime.Now.AddMonths(-1).ToShortDateString();
                this.dt_end_UseBox.Value = System.DateTime.Now.ToShortDateString();
                this.dt_begin_UseBox.Disabled = true;
                this.dt_end_UseBox.Disabled = true; 
            }
        }

        protected void GridBind()
        {
            sqlaccess.Open();
            try
            {
                string tmpsql = "  select t.productid,t.DetectProcess,t.detectemployeeid,tdu.Name dEmpName,t.DetectTime FindTime,t.Exception ,tt.Bug,t.BugPointCode,t.RepairEmployeeID,tru.name repairer,t.repairTime,t.repairInfo"
                                + "  from TB_RepairRecord t,ta_employee tdu,ta_employee tru ,ta_bugtype tt,ta_product tpr "
                                + "  where t.productid =tpr.id and t.detectemployeeid=tdu.id  and t.RepairEmployeeID=tru.id and t.bugid=tt.id " + ViewState["conStr"].ToString()
                                + "  order by t.detecttime desc";

                ds = sqlaccess.OpenQuerry(tmpsql);
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

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string bstr = "";
            string estr = "";
            string whstr = "";
            string datestr = "";
            if (this.ddlDate.SelectedIndex == 0)
            {
                datestr = "DetectTime";
            }
            else { datestr = "RepairTime"; }
            if (!this.dt_begin_UseBox.Disabled)
            {
                bstr = " and "+datestr+">='" + this.dt_begin_UseBox.Value + "'";
            }
            if (!this.dt_end_UseBox.Disabled)
            {
                estr = " and "+datestr+"<='" + this.dt_end_UseBox.Value + "'";
            }

            if (this.txtID.Text.Trim() != string.Empty)
            {
                //ViewState["conStr"] = whstr + bstr + estr + " and EmplyeeID like '%" + this.txtcondition.Text.Trim() + "%'";
                //已改过 按产品中的计划号取得
                if(this.RadioButton1.Checked)
                {
                    ViewState["conStr"] = " and productid in (select id from ta_product where planid='" + this.txtID.Text.Trim() + "')" + bstr + estr;
                }
                if(this.RadioButton2.Checked)
                {
                    ViewState["conStr"] = " and productid in (select id from ta_product where modelid='" + this.txtID.Text.Trim() + "')" + bstr + estr;
                }
                if(this.RadioButton3.Checked)
                {
                    ViewState["conStr"] = " and productid='" + this.txtID.Text.Trim() + "'"+ bstr + estr;
                }
                if(this.RadioButton4.Checked)
                {
                    ViewState["conStr"] = " and productid in (select id from ta_product where sn='" + this.txtID.Text.Trim() + "')" + bstr + estr;
                }
            }
            else
                ViewState["conStr"] = whstr + bstr + estr;
            this.GridBind();
        }

        protected void ckCancelTime_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckCancelTime.Checked)
            {
                this.dt_begin_UseBox.Disabled = false; 
                this.dt_end_UseBox.Disabled = false;
                this.ddlDate.Enabled = true;
            }
            else
            {
                this.dt_begin_UseBox.Disabled = true;
                this.dt_end_UseBox.Disabled = true;
                this.ddlDate.Enabled = false;
            }
        }
    }
}
