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
using MCSApp.Common;
using MCSApp.DataAccess.LogManage;
using MCSApp.DataAccess;
using FINGU.MCS;
using MCSApp.WSSecurity;

namespace MCSApp.Application.ProductManage
{
    public partial class QueryModel : PageBase
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
                string modeltype = (Session["__ModelType"].ToString() =="2") ? ("modeltype='2' or  modeltype='3'") :("modeltype='"+ Session["__ModelType"].ToString()+"'");
                sqlaccess.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ID,name,case when modeltype=1 then '产品' when modeltype=2 then '部件' else '物料' end type from TA_Model where (" +modeltype +") "+ ViewState["conStr"].ToString());//只能选择出状态为激活状态的计划

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
