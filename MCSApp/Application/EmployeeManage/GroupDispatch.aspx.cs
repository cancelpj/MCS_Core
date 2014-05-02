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
    public partial class GroupDispatch : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        public string clid = "";//用于界面上弹出窗体回传值时，找到控件的ID用
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string ID = SessionUser.ID;
                string password = SessionUser.Password;
                string[] Roles = { "班组长" };
                string outStr;

                if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
                {
                    return;
                }
                ViewState["newMark"] = false;//是否为新建标记
                GridBind();
            }
        }

        private void GridBind()
        {
            try
            {
                sqlaccess.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ID, Name,LeaderID,WorkDispatch FROM TA_Group where leaderid='"+SessionUser.ID+"'");

                ds = sqlaccess.OpenQuerry(sb.ToString());

                this.GridView1.DataSource = ds;
                this.GridView1.DataBind();
            }
            catch (Exception ex)
            {
                //Page.RegisterClientScriptBlock("错误", "<script language=Javascript>alert('当前登录用户不领导任何班组！" + ex.Message + "');< /script>");
                Methods.AjaxMessageBox(this,"当前登录用户不领导任何班组！" + ex.Message);
                Response.Redirect("/Main/index.aspx", true);
                LogManager.Write(this, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }
            this.GridView1.PageIndex = e.NewPageIndex;
            this.GridBind();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.GridView1.EditIndex = -1;
            this.GridBind();
            this.lblMsg.Visible = false;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes.Add("onclick", "changeStyle(this)");
                ImageButton bt = (ImageButton)e.Row.FindControl("ImageButton2");//删除图片按钮
                Label lbl = (Label)e.Row.FindControl("lblName");//用户名称标签
                if (bt.CommandName == "Delete")
                {
                    bt.Attributes.Add("onclick", "javascript:return confirm('你确认要删除\"" + lbl.Text + "\"吗?')");
                }
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }
            string id = this.GridView1.DataKeys[e.RowIndex].Value.ToString();
            string delstr = "delete from TA_Group  where id='" + id + "'";//取单位值
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                sqlaccess.ExecuteQuerry(delstr);
                string logStr = delstr;
                //Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);
                sqlaccess.Commit();
                //删除后重新绑定 
                this.GridBind();
            }
            catch (Exception ex)
            {
                if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ((SqlException)ex).ErrorCode == -2146232060)
                {
                    sqlaccess.Rollback();
                    this.lblMsg.Text = "尚有其它表的记录与它相关联，不能删除！";
                    this.lblMsg.Visible = true;
                }
                else
                    LogManager.Write(this, ex.ToString());
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }
            this.GridView1.EditIndex = e.NewEditIndex;
            this.GridBind();
            Label txtLeaderID = (Label)this.GridView1.Rows[e.NewEditIndex].FindControl("txtLeaderID");
            Label txtName = (Label)this.GridView1.Rows[e.NewEditIndex].FindControl("txtName");
            txtName.Enabled = false;
            txtLeaderID.Enabled = false;
            clid = txtLeaderID.ClientID;
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string id = this.GridView1.DataKeys[e.RowIndex].Value.ToString();
            Label txtName = (Label)this.GridView1.Rows[e.RowIndex].FindControl("txtName");//只作显示用，在更新的时候不作操作
            Label txtLeaderID = (Label)this.GridView1.Rows[e.RowIndex].FindControl("txtLeaderID");//只作显示用，在更新的时候不作操作
            TextBox txtWorkDispatch = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtWorkDispatch");

            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                if (id != string.Empty)
                {
                    string updateStr = "update TA_Group set Name ='" + txtName.Text.Trim() + "',WorkDispatch='"+txtWorkDispatch.Text.Trim()+"' where ID='" + id + "'";//, LeaderID ='" + txtLeaderID.Text.Trim() + "'
                    sqlaccess.ExecuteQuerry(updateStr);
                    string logStr = updateStr;
                    //Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);
                }
                else
                {
                    string insertStr = "insert into TA_Group (Name,LeaderID,WorkDispatch) values ('" + txtName.Text.Trim() + "','" + txtWorkDispatch.Text.Trim() + "')";//'" + txtLeaderID.Text.Trim() + "',
                    sqlaccess.ExecuteQuerry(insertStr);
                    string logStr = insertStr;
                    //Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);
                }

                sqlaccess.Commit();
                this.GridView1.EditIndex = -1;
                this.GridBind();
            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ((SqlException)ex).ErrorCode == -2146232060)
                {
                    char[] cs = { '\r', '\n' }; Methods.AjaxMessageBox(this, ex.Message.Split(cs)[0].Replace('\'', '"'));
                }
                LogManager.Write(this, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }

        }

        protected void lbtNew_Click(object sender, EventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }
            try
            {
                ds = sqlaccess.OpenQuerry("select * from ta_group order by id asc");
            }
            catch (Exception ex)
            {
                LogManager.Write(this, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }
            DataRow r = ds.Tables[0].NewRow();//将数据源添加一新行

            ds.Tables[0].Rows.InsertAt(r, 0);
            ds.AcceptChanges();
            this.GridView1.EditIndex = 0;
            this.GridView1.PageIndex = 0;

            this.GridView1.DataSource = ds;
            this.GridView1.DataBind();
            TextBox txtLeaderID = (TextBox)this.GridView1.Rows[0].FindControl("txtLeaderID");
            clid = txtLeaderID.ClientID;
        }

        protected void btnquery_Click(object sender, EventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }
            //ViewState["conStr"] = " where " + (this.rdioBugID.Checked == true ? "Name" : "LeaderID") + " like '%" + txtcondition.Text.Trim() + "%'";
            ViewState["conStr"] = " where 1=1 ";
            this.GridBind();
        }
    }
}
