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

namespace MCSApp.Application.SystemManage
{
    public partial class EventPage : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "系统管理员" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
            if(!IsPostBack)
            {
                ViewState["conStr"] = "";//是否为新建标记
                this.dt_begin_UseBox.Value = System.DateTime.Now.AddMonths(-1).ToShortDateString();
                this.dt_end_UseBox.Value = System.DateTime.Now.ToShortDateString();
                this.BindGrid();
                this.dt_begin_UseBox.Disabled = true; 
                this.dt_end_UseBox.Disabled = true; 
                
            }
        }

        protected void btnquery_Click(object sender, EventArgs e)
        {
            string bstr = "";
            string estr = "";
            string whstr = " where 1=1 ";
            if (!this.dt_begin_UseBox.Disabled)
            {
                bstr = " and EventTime>='" + this.dt_begin_UseBox.Value + "'";
            }
            if (!this.dt_end_UseBox.Disabled)
            {
                estr = " and EventTime<='" + this.dt_end_UseBox.Value+ " 23:59:59'";
            }
            if (this.txtcondition.Text.Trim() != string.Empty)
                ViewState["conStr"] = whstr + bstr + estr + " and EmplyeeID like '%" + this.txtcondition.Text.Trim() + "%'";
            else
                ViewState["conStr"] = whstr+bstr + estr;
            this.BindGrid();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridView1.PageIndex = e.NewPageIndex;
            this.BindGrid();
        }

        private void BindGrid()
        {
            try
            {
                sqlaccess.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * from TB_Event " + ViewState["conStr"].ToString()+" order by eventtime desc");

                ds = sqlaccess.OpenQuerry(sb.ToString());

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

        protected void ckCancelTime_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckCancelTime.Checked)
            {
                this.dt_begin_UseBox.Disabled = false; ;
                this.dt_end_UseBox.Disabled = false; ;
            }
            else
            {
                this.dt_begin_UseBox.Disabled = true;
                this.dt_end_UseBox.Disabled = true; 
            }
        }
    }
}
