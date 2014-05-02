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

namespace MCSApp.EmployeeManage
{
    public partial class UserInfor : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "人事管理员" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }

            if (!IsPostBack)
            {

                ViewState["newMark"] = false;//是否为新建标记
                ViewState["conStr"] = "";//是否为新建标记
                this.GridBind();
            }
            if (this.Request.QueryString["refresh"] != null) this.GridBind();

        }
        /// <summary>
        /// 编辑时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }

            this.GridView1.EditIndex = e.NewEditIndex;
            this.GridBind();

            DropDownList ddlstate = (DropDownList)this.GridView1.Rows[e.NewEditIndex].FindControl("ddlstate");
            ddlstate.SelectedValue = this.GridView1.DataKeys[e.NewEditIndex].Value.ToString();
            TextBox txtid = (TextBox)this.GridView1.Rows[e.NewEditIndex].FindControl("txtid");
            txtid.Enabled = false; txtid.Enabled = false;


        }

        /// <summary>
        /// 删除时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }
            Label lbl = (Label)this.GridView1.Rows[e.RowIndex].Cells[0].FindControl("lblid");//强行转换为lable控件
            string delstr = "delete from TA_Employee  where id='" + lbl.Text + "'";//取编号值
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                sqlaccess.ExecuteQuerry(delstr);
                sqlaccess.Commit();
                LogManager.WriteOprationLog(SessionUser.DetailInfo, delstr);
                this.GridView1.EditIndex = -1;
                this.lblMsg.Visible = false;
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

        /// <summary>
        /// 取消时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.GridView1.EditIndex = -1;
            this.lblMsg.Visible = false;
            this.lblAccMsg.Visible = false;
            this.GridBind();
            ViewState["newMark"] = false;//新建清除

        }
        /// <summary>
        /// 更新时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            TextBox txtid = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtid");
            TextBox txtName = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtName");
            DropDownList ddlstate = (DropDownList)this.GridView1.Rows[e.RowIndex].FindControl("ddlstate");
            TextBox txtremark = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtremark");

            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                if (!(bool)ViewState["newMark"])//非新建状态
                {
                    if (!isAccountExist(txtid.Text.Trim(), txtName.Text.Trim()))
                    {
                        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                        cmd.Parameters.AddWithValue("@p1", txtid.Text.Trim());
                        cmd.Parameters.AddWithValue("@p2", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@p5", ddlstate.SelectedValue);
                        cmd.Parameters.AddWithValue("@p6", txtremark.Text.Trim());
                        cmd.CommandText = "update TA_Employee set id =@p1,name=@p2,state=@p5,remark=@p6 where id=@p1";
                        sqlaccess.ExecuteQuerry(cmd);
                        //LogManager.WriteOprationLog(SessionUser.DetailInfo, updateStr);
                    }
                    else
                    {
                        this.lblAccMsg.Visible = true;
                        return;
                    }

                }
                else//新建状态
                {
                    if (!isAccountExist(txtid.Text.Trim(), txtName.Text.Trim()))
                    {
                        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                        cmd.Parameters.AddWithValue("@p1", txtid.Text.Trim());
                        cmd.Parameters.AddWithValue("@p2", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@p4", FINGU.MCS.Encrypt.EncryptString("888888"));
                        cmd.Parameters.AddWithValue("@p5", ddlstate.SelectedValue);
                        cmd.Parameters.AddWithValue("@p6", txtremark.Text.Trim());
                        cmd.CommandText = "insert into TA_Employee  (id,name,password,state,remark) values(@p1,@p2,@p4,@p5,@p6)";
                        sqlaccess.ExecuteQuerry(cmd);
                        //LogManager.WriteOprationLog(SessionUser.DetailInfo, updateStr);
                    }
                    else
                    {
                        this.lblAccMsg.Visible = true;
                        return;
                    }

                }

                sqlaccess.Commit();
                this.GridView1.EditIndex = -1;
                this.lblMsg.Visible = false;
                this.lblAccMsg.Visible = false;
                this.GridBind();
                ViewState["newMark"] = false;//新建清除
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
        /// <summary>
        /// 新增时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtNew_Click(object sender, EventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }

            try
            {
                sqlaccess.Open();
                ds = sqlaccess.OpenQuerry("select *  from TA_Employee");

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
            ViewState["newMark"] = true;//有效

            //iniDdl(0);//先绑定数据源再设定控件
        }
        /// <summary>
        /// 生成行的时候设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //e.Row.Attributes.Add("onclick", "return sortColumn(event)");
                //e.Row.Cells[0].Attributes.Add("Type","Number");
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes.Add("onclick", "changeStyle(this)");
                DropDownList ddlstate = (DropDownList)e.Row.FindControl("ddlstate");
                Label lblp = (Label)e.Row.FindControl("lblp");
                ddlstate.SelectedValue = lblp.Text;
                ImageButton bt = (ImageButton)e.Row.FindControl("ImageButton2");//删除图片按钮
                Label lbl = (Label)e.Row.FindControl("lblname");//用户名称标签
                if (bt.CommandName == "Delete")
                {
                    bt.Attributes.Add("onclick", "javascript:return confirm('你确认要删除\"" + lbl.Text + "\"吗?')");
                }
            }
        }

        ///// <summary>
        ///// 分页时处理
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    if (this.GridView1.EditIndex != -1)
        //    {
        //        this.lblMsg.Visible = true;
        //        return;
        //    }
        //    this.GridView1.PageIndex = e.NewPageIndex;//页面索引重新给定
        //    this.GridBind();
        //}


        /// <summary>
        /// 绑定grid
        /// </summary>
        private void GridBind()
        {
            try
            {
                sqlaccess.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * from TA_Employee " + ViewState["conStr"].ToString());

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


        private bool isAccountExist(string id, string Name)
        {
            bool isExist = false;
            SqlAccess accesstmp = new SqlAccess();
            string queryStr = "";

            try
            {
                if ((bool)ViewState["newMark"])//新建态
                {
                    queryStr = "select name from TA_Employee where id='" + id + "'";
                }
                else //更新态
                {
                    //queryStr = "select name from TA_Employee where id<>'" + id + "'";
                    return false;
                }
                accesstmp.Open();
                ds = accesstmp.OpenQuerry(queryStr);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    isExist = true;
                }
                return isExist;
            }
            catch (Exception ex)
            {
                LogManager.Write(this, ex.Message);
                return true;
            }
            finally
            {
                accesstmp.Close();
            }

        }

        protected void btnquery_Click(object sender, EventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }
            ViewState["conStr"] = " where " + (RadioButton1.Checked == true ? "id" : "name") + " like '%" + txtcondition.Text.Trim() + "%'";
            this.GridBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }
            ViewState["conStr"] = " where " + (RadioButton1.Checked == true ? "id" : "name") + " like '%" + txtcondition.Text.Trim() + "%'";
            this.GridView1.PageIndex = e.NewPageIndex;
            this.GridBind();
        }

    }
}
