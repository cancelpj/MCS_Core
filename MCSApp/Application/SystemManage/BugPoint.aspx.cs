using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data.SqlClient;
using MCSApp.DataAccess;
using MCSApp.DataAccess.LogManage;
using MCSApp.Common;

namespace MCSApp.Application.SystemManage
{
    public partial class BugPoint : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "制造工程师" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
            if (!IsPostBack)
            {
                ViewState["conStr"] = "";//是否为新建标记
                ViewState["tmpMID"] = "";//产品品号 用于记录上次输入的品号
                this.grid2Bind();

            }
        }

        private void grid2Bind()
        {
            sqlaccess.Open();
            try
            {
                string selstr = "";
                if (this.txtcondition.Text.Trim() == string.Empty)
                {
                    selstr = "";
                }
                else
                {
                    selstr = " and " + (this.RadioButton1.Checked ? "ModelID like'%" + this.txtcondition.Text.Trim() + "%' " : "BugPointCode like '%" + this.txtcondition.Text.Trim() + "%'");
                }
                string tmpsql = "SELECT ModelID,BugPointCode,BugPointDsc from TA_BugPoint where 1=1 " + selstr;
                ds = sqlaccess.OpenQuerry(tmpsql);
                this.GridView2.DataSource = ds;
                this.GridView2.DataBind();

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

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (this.GridView2.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                this.lblMsg.Text = "表格正处在编辑状态，请先保存或取消!";
                return;
            }
            this.GridView2.PageIndex = e.NewPageIndex;
            this.grid2Bind();
        }

        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.GridView2.EditIndex = -1;
            this.grid2Bind();
            this.lblMsg.Visible = false;
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes.Add("onclick", "changeStyle(this)");
                ImageButton bt = (ImageButton)e.Row.FindControl("ImageButton2");//删除图片按钮
                Label lbl = (Label)e.Row.FindControl("lblBugPointCode");//用户名称标签
                if (bt.CommandName == "Delete")
                {
                    bt.Attributes.Add("onclick", "javascript:return confirm('你确认要删除\"" + lbl.Text + "\"吗?')");
                }
            }
        }

        protected void GridView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (this.GridView2.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                this.lblMsg.Text = "表格正处在编辑状态，请先保存或取消!";
                return;
            }
            string id = this.GridView2.DataKeys[e.RowIndex].Value.ToString();
            string delstr = "delete from TA_BugPoint  where ModelID='" + ((Label)this.GridView2.Rows[e.RowIndex].FindControl("lblModelID")).Text + "' and BugPointCode='"+id+"'";//取单位值
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                sqlaccess.ExecuteQuerry(delstr);
                sqlaccess.Commit();
                //删除后重新绑定 
                //this.grid2Bind(((Label)this.GridView2.Rows[e.RowIndex].FindControl("lblModelID")).Text);
                this.grid2Bind();
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

        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (this.GridView2.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                this.lblMsg.Text = "表格正处在编辑状态，请先保存或取消!";
                return;
            }
            this.GridView2.EditIndex = e.NewEditIndex;
            this.grid2Bind();
            TextBox box = (TextBox)this.GridView2.Rows[e.NewEditIndex].FindControl("txtModelID");
            box.Enabled = false;

        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string ModelID = ((TextBox)this.GridView2.Rows[e.RowIndex].FindControl("txtModelID")).Text; //this.GridView1.SelectedRow.Cells[1].Text;//第一个view1的ModelID
            string id = this.GridView2.DataKeys[e.RowIndex].Value.ToString();
            TextBox txtBugPointCode = (TextBox)this.GridView2.Rows[e.RowIndex].FindControl("txtBugPointCode");
            TextBox txtBugPointDsc = (TextBox)this.GridView2.Rows[e.RowIndex].FindControl("txtBugPointDsc");

            if (!Methods.HasModel(ModelID))
            {
                this.lblMsg.Visible = true;
                this.lblMsg.Text = "输入的品号在品号表中没定义!"; return;
            }
            else
            {
                this.lblMsg.Visible = false ; 

            }

            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                if (id != string.Empty)
                {
                    string updateStr = "update TA_BugPoint set BugPointDsc ='" + txtBugPointDsc.Text.Trim() + "', BugPointCode ='" + txtBugPointCode.Text.Trim() + "' where ModelID='" + ModelID + "' and BugPointCode ='"+id+"'";
                    sqlaccess.ExecuteQuerry(updateStr);
                }
                else
                {
                    string insertStr = "insert into TA_BugPoint (ModelID,BugPointCode,BugPointDsc) values ('" + ModelID.Trim() + "','" + txtBugPointCode.Text.Trim() + "','" + txtBugPointDsc.Text.Trim()+ "')";
                    sqlaccess.ExecuteQuerry(insertStr);
                }
                ViewState["tmpMID"]=ModelID.Trim();
                sqlaccess.Commit();

                //this.grid2Bind(((Label)this.GridView2.Rows[e.RowIndex].FindControl("lblModelID")).Text);
                               
                this.GridView2.EditIndex = -1;
                this.grid2Bind();
                this.lblMsg.Visible = false;
            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                //if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ((SqlException)ex).ErrorCode == -2146232060)
                //{
                //    char[] cs = { '\r', '\n' };Methods.AjaxMessageBox(this, ex.Message.Split(cs)[0].Replace('\'', '"')); 
                //}
                this.lblMsg.Text = "缺陷定位点代码重复！";
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

            if (this.GridView2.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                this.lblMsg.Text = "表格正处在编辑状态，请先保存或取消!";
                return;
            }
            try
            {
                //string tmpsql = "SELECT ModelID,BugPointCode,BugPointDsc from TA_BugPoint where ModelID='" + this.GridView1.SelectedRow.Cells[1].Text + "'";
                string tmpsql = "SELECT ModelID,BugPointCode,BugPointDsc from TA_BugPoint ";//将gv1隐起来 080816
                ds = sqlaccess.OpenQuerry(tmpsql);
            }
            catch (Exception ex)
            {
                LogManager.Write(this, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }
            this.GridView2.EditIndex = 0;
            this.GridView2.PageIndex = 0;

            DataRow r = ds.Tables[0].NewRow();//将数据源添加一新行
            r["ModelID"] = ViewState["tmpMID"].ToString(); 
            
            ds.Tables[0].Rows.InsertAt(r, 0);
            ds.AcceptChanges();


            this.GridView2.DataSource = ds;
            this.GridView2.DataBind();

        }

        protected void btnquery_Click(object sender, EventArgs e)
        {           
            if (this.GridView2.EditIndex != -1)
            {
                this.lblMsg.Visible = true;                
                this.lblMsg.Text = "表格正处在编辑状态，请先保存或取消!";
                return;
            }
            this.grid2Bind();
        }

    }
}
