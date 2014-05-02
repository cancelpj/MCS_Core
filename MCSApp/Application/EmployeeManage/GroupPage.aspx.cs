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
    public partial class GroupPage : PageBase
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
                string[] Roles = { "系统管理员" };
                string outStr;

                if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
                {
                    return;
                }
                ViewState["newMark"] = false;//是否为新建标记
                ViewState["conStr"] = "";//是否为新建标记
                GridBind();
            }
        }

        private void GridBind()
        {
            try
            {
                sqlaccess.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ID, Name,LeaderID,WorkDispatch FROM TA_Group" + ViewState["conStr"].ToString());

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
            Label lblName = (Label)this.GridView1.Rows[e.RowIndex].FindControl("lblName");
            string delstr = "delete from TA_Group  where id='" + id + "'";//取单位值
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                sqlaccess.ExecuteQuerry(delstr);
                string logStr = "删除班组[" + lblName.Text + "] ## " + delstr;
                Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);
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
            TextBox txtLeaderID = (TextBox)this.GridView1.Rows[e.NewEditIndex].FindControl("txtLeaderID");
            //TextBox txtName = (TextBox)this.GridView1.Rows[e.NewEditIndex].FindControl("txtName");
            //txtName.Enabled = false;
            clid=txtLeaderID.ClientID;
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string id = this.GridView1.DataKeys[e.RowIndex].Value.ToString();
            TextBox txtName = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtName");
            TextBox txtLeaderID = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtLeaderID");
            Label lblLeaderID = (Label)this.GridView1.Rows[e.RowIndex].FindControl("lblLeaderID");
            if(lblLeaderID.Text!=txtLeaderID.Text.Trim())//新旧值不相等
            {
                if (!Methods.passLeader(txtLeaderID.Text.Trim()))
                {
                    this.lblMsg.Visible = true;
                    this.lblMsg.Text = "输入的班组长工号没有通过验证，可能是此工号已经分配了班组或此工号不存在或此工号不具有班长权限!"; return;
                }
                else
                {
                    this.lblMsg.Visible = false;
                }
            }

            //Label txtWorkDispatch = (Label)this.GridView1.Rows[e.RowIndex].FindControl("txtWorkDispatch");//只作显示用，在更新的时候不作操作

            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                if (id != string.Empty)
                {
                    string updateStr = "update TA_Group set Name ='" + txtName.Text.Trim() + "', LeaderID ='" + txtLeaderID.Text.Trim() + "' where ID='" + id + "'";//,WorkDispatch='"+txtWorkDispatch.Text.Trim()+"'
                    sqlaccess.ExecuteQuerry(updateStr);
                    string logStr = "修改班组[" + txtName.Text.Trim() + "] ## " + updateStr;
                    Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);
                }
                else
                {
                    string insertStr = "insert into TA_Group (Name,LeaderID) values ('" + txtName.Text.Trim() + "','" + txtLeaderID.Text.Trim() + "')";//,'" + txtWorkDispatch.Text.Trim() + "'
                    sqlaccess.ExecuteQuerry(insertStr);
                    string logStr = "新建班组[" + txtName.Text.Trim() + "] ## " + insertStr;
                    Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);
                }

                sqlaccess.Commit();
                this.GridView1.EditIndex = -1;
                this.GridBind();
                this.lblMsg.Visible = false;
            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                //if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ((SqlException)ex).ErrorCode == -2146232060)
                //{
                //    char[] cs = { '\r', '\n' };Methods.AjaxMessageBox(this, ex.Message.Split(cs)[0].Replace('\'', '"'));
                //}
                this.lblMsg.Visible = true;
                this.lblMsg.Text = "班组名称已经存在!";
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
            ViewState["conStr"] = " where " + (this.rdioBugID.Checked == true ? "Name" : "LeaderID") + " like '%" + txtcondition.Text.Trim() + "%'";
            this.GridBind();
        }
    }
}
