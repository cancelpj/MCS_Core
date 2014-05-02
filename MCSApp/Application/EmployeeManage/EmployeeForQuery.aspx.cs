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

namespace MCSApp.Application.EmployeeManage
{
    public partial class EmployeeForQuery : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            //string ID = SessionUser.ID;
            //string password = SessionUser.Password;
            //string[] Roles = { "系统管理员" };
            //string outStr;

            //if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            //{
            //    return;
            //}
            this.btnOK.Attributes.Add("onclick", "setContolValue();return false;");
            if (!IsPostBack)
            {
                ViewState["conStr"] = string.Empty;
                this.GridBind();
            }
        }

        protected void GridView1_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {
            this.GridView1.PageIndex = e.NewPageIndex;
            this.GridBind();
        }
        private void GridBind()
        {
            try
            {
                sqlaccess.Open();
                StringBuilder sb = new StringBuilder();
                //sb.Append("SELECT * from TA_Employee,TRE_Employee_Role  where TRE_Employee_Role.EmployeeID=TA_Employee.id and TRE_Employee_Role.role='班组长' and state<>2 and id not in(select distinct(leaderid) from ta_group) and id not in(select distinct(Employeeid) from TRE_Group_Employee)" + ViewState["conStr"].ToString());
                sb.Append("SELECT * from TA_Employee,TRE_Employee_Role  where TRE_Employee_Role.EmployeeID=TA_Employee.id and TRE_Employee_Role.role='班组长' and state<>2 " + ViewState["conStr"].ToString());

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

        protected void btnquery_Click(object sender, EventArgs e)
        {
            if (txtcondition.Text.Trim() != string.Empty)
            {
                ViewState["conStr"] = " and " + (RadioButton1.Checked == true ? "id" : "name") + " like '%" + txtcondition.Text.Trim() + "%'";
            }
            else
                ViewState["conStr"] = string.Empty;

            this.GridBind();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
