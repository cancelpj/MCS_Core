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
using MCSApp.Common;
using MCSApp.DataAccess.LogManage;
using MCSApp.DataAccess;
using FINGU.MCS;
using MCSApp.WSSecurity;

namespace MCSApp.Application.SystemManage
{
    public partial class BugType : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string ID = SessionUser.ID;
                string password = SessionUser.Password;
                string[] Roles = { "制造工程师" };
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
                sb.Append("SELECT ID, Bug FROM TA_BugType" + ViewState["conStr"].ToString());

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
                this.lblMsg.Text = "表格正处在编辑状态，请先保存或取消!";
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
                Label lbl = (Label)e.Row.FindControl("Label2");//用户名称标签
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
                this.lblMsg.Text = "表格正处在编辑状态，请先保存或取消!";
                return;
            }
            string id = this.GridView1.DataKeys[e.RowIndex].Value.ToString();
            string delstr = "delete from TA_BugType  where id='" + id + "'";//取单位值
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                sqlaccess.ExecuteQuerry(delstr);
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
                this.lblMsg.Text = "表格正处在编辑状态，请先保存或取消!";
                return;
            }
            this.GridView1.EditIndex = e.NewEditIndex;
            this.GridBind();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string id = this.GridView1.DataKeys[e.RowIndex].Value.ToString();
            TextBox txtID = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtID");
            TextBox txtBug = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtBug");

            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                if (id != string.Empty)
                {
                    string updateStr = "update TA_BugType set ID ='" + txtID.Text.Trim() + "', Bug ='" + txtBug.Text.Trim() + "' where ID='" + id + "'";
                    sqlaccess.ExecuteQuerry(updateStr);
                }
                else
                {
                    string insertStr = "insert into TA_BugType (ID,Bug) values ('" + txtID.Text.Trim() + "','" + txtBug.Text.Trim() + "')";
                    sqlaccess.ExecuteQuerry(insertStr);
                }

                sqlaccess.Commit();
                this.GridView1.EditIndex = -1;
                this.GridBind();
                this.lblMsg.Visible = false ;
            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                //if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ((SqlException)ex).ErrorCode == -2146232060)
                //{
                //    char[] cs = { '\r', '\n' };Methods.AjaxMessageBox(this, ex.Message.Split(cs)[0].Replace('\'', '"')); 
                //}
                this.lblMsg.Text = "缺陷条码重复或缺陷名称重复！";
                this.lblMsg.Visible = true;
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
                this.lblMsg.Text = "表格正处在编辑状态，请先保存或取消!";
                return;
            }
            try
            {
                ds = sqlaccess.OpenQuerry("select * from ta_bugType");
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
        }

        protected void btnquery_Click(object sender, EventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                this.lblMsg.Text = "表格正处在编辑状态，请先保存或取消!";
                return;
            }
            ViewState["conStr"] = " where " + (this.rdioBugID.Checked == true ? "id" : "bug") + " like '%" + txtcondition.Text.Trim() + "%'";
            this.GridBind();
        }
    }
}
