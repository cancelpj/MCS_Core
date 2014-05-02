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
    public partial class patchAddEmployee : PageBase
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
            //this.btnOK.Attributes.Add("onclick", "setContolValue();return false;");//这里不添加 使其执行服务器端
            if (!IsPostBack)
            {
                ViewState["conStr"] = string.Empty;
                ViewState["groupID"] = Session["Groupid"];
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
                //sb.Append("SELECT * from TA_Employee where state<>2 and id not in(select distinct(leaderid) from ta_group) and id not in(select distinct(Employeeid) from TRE_Group_Employee)"+ViewState["conStr"].ToString() );//
                sb.Append("SELECT * from TA_Employee where state<>2 and id not in(select distinct(Employeeid) from TRE_Group_Employee)" + ViewState["conStr"].ToString());//

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
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                foreach (GridViewRow row in this.GridView1.Rows)
                {
                    CheckBox ck = (CheckBox)row.FindControl("ckid");
                    if (ck.Checked)
                    {
                        string tmpstr=ViewState["groupID"].ToString();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Parameters.AddWithValue("@p1", this.GridView1.DataKeys[row.RowIndex].Value.ToString());
                        cmd.Parameters.AddWithValue("@p2", tmpstr);
                        cmd.CommandText = "insert into TRE_Group_Employee (groupid,employeeid) values(@p2,@p1)";
                        sqlaccess.ExecuteQuerry(cmd);                        
                        
                        //添加的班组成员自动设为“作业员”角色
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p1", this.GridView1.DataKeys[row.RowIndex].Value.ToString());
                        cmd.Parameters.AddWithValue("@p2", "作业员");
                        cmd.CommandText = "select * from TRE_Employee_Role where Role=@p2 and employeeid=@p1";
                        ds= sqlaccess.OpenQuerry(cmd);
                        if(ds!=null&&ds.Tables[0].Rows.Count>0)
                        {
                            continue;
                        }
                        
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p1", this.GridView1.DataKeys[row.RowIndex].Value.ToString());
                        cmd.Parameters.AddWithValue("@p2", "作业员");
                        cmd.CommandText = "insert into TRE_Employee_Role (EmployeeID,Role) values(@p1,@p2)";
                        sqlaccess.ExecuteQuerry(cmd);
                    }

                }
                sqlaccess.Commit();
                this.GridBind();
                Methods.Write(this, "window.parent.document.getElementById('btnrefresh').click();window.parent.closeit();//关闭窗体 ");

            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ((SqlException)ex).ErrorCode == -2146232060)
                {
                    char[] cs = { '\r', '\n' };Methods.AjaxMessageBox(this, ex.Message.Split(cs)[0].Replace('\'', '"'));
                }
                LogManager.Write(this, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }
        }
    }
}
