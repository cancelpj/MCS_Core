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

namespace MCSApp.Application.ProductManage
{
    public partial class ModelList : PageBase
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

            DropDownList ddlstate = (DropDownList)this.GridView1.Rows[e.NewEditIndex].FindControl("ddlModelType");
            ddlstate.SelectedValue = this.GridView1.DataKeys[e.NewEditIndex].Value.ToString();
            TextBox txtid = (TextBox)this.GridView1.Rows[e.NewEditIndex].FindControl("txtid");
            txtid.Enabled = false; txtid.ReadOnly = true;


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
            string delstr = "delete from TA_Model  where id='" + lbl.Text + "'";//取编号值
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                sqlaccess.ExecuteQuerry(delstr);
                string logStr = "删除品号[" + lbl.Text + "] ## " + delstr;
                Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);

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
            DropDownList ddlModelType = (DropDownList)this.GridView1.Rows[e.RowIndex].FindControl("ddlModelType");
            TextBox txtCode = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtcode");
            TextBox txtCustomerID = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtCustomerID");

            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                if (!(bool)ViewState["newMark"])//非新建状态
                {
                    if (!isAccountExist(txtid.Text.Trim(), txtCode.Text.Trim()))
                    {
                        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                        cmd.Parameters.AddWithValue("@p1", txtid.Text.Trim());
                        cmd.Parameters.AddWithValue("@p2", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@p3", txtCustomerID.Text.Trim());
                        cmd.Parameters.AddWithValue("@p5", ddlModelType.SelectedValue);
                        cmd.Parameters.AddWithValue("@p6", txtCode.Text.Trim());
                        cmd.CommandText = "update TA_Model set id =@p1,name=@p2,CustomerID=@p3,ModelType=@p5,Code=@p6 where id=@p1";
                        sqlaccess.ExecuteQuerry(cmd);
                        //LogManager.WriteOprationLog(SessionUser.DetailInfo, updateStr);
                        string logStr = "update TA_Model set id ='{0}',name='{1}',ModelType='{2}',Code='{3}' where id='{4}'";
                        logStr = String.Format(logStr, txtid.Text.Trim(), txtName.Text.Trim(), ddlModelType.SelectedValue, txtCode.Text.Trim(),txtid.Text.Trim());
                        logStr = "修改品号[" + txtid.Text.Trim() + "] ## " + logStr;
                        Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);

                    }
                    else
                    {
                        this.lblAccMsg.Visible = true;
                        return;
                    }

                }
                else//新建状态
                {
                    if (!isAccountExist(txtid.Text.Trim(), txtCode.Text.Trim()))
                    {
                        System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                        cmd.Parameters.AddWithValue("@p1", txtid.Text.Trim());
                        cmd.Parameters.AddWithValue("@p2", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@p3", txtCustomerID.Text.Trim());
                        cmd.Parameters.AddWithValue("@p5", ddlModelType.SelectedValue);
                        cmd.Parameters.AddWithValue("@p6", txtCode.Text.Trim());
                        cmd.CommandText = "insert into TA_Model  (id,name,CustomerID,ModelType,Code) values(@p1,@p2,@p3,@p5,@p6)";
                        sqlaccess.ExecuteQuerry(cmd);
                        string logStr = "insert into TA_Model  (id,name,ModelType,Code,CustomerID) values('{0}','{1}','{2}','{3}','{4}')";
                        logStr = String.Format(logStr, txtid.Text.Trim(), txtName.Text.Trim(), ddlModelType.SelectedValue, txtCode.Text.Trim(), txtCustomerID.Text.Trim());
                        //LogManager.WriteOprationLog(SessionUser.DetailInfo, updateStr);
                        logStr = "新建品号[" + txtid.Text.Trim() + "] ## " + logStr;
                        Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);
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
                    char[] cs = { '\r', '\n' };Methods.AjaxMessageBox(this, ex.Message.Split(cs)[0].Replace('\'', '"'));
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
                ds = sqlaccess.OpenQuerry("select *  from TA_Model");

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
                DropDownList ddlstate = (DropDownList)e.Row.FindControl("ddlModelType");
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

        /// <summary>
        /// 分页时处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }
            this.GridView1.PageIndex = e.NewPageIndex;//页面索引重新给定
            this.GridBind();
        }


        /// <summary>
        /// 绑定grid
        /// </summary>
        private void GridBind()
        {
            try
            {
                if (this.RadioButton1.Checked)
                {
                    ViewState["conStr"] = " where id like '%" + txtcondition.Text.Trim() + "%'";
                }
                if (this.RadioButton2.Checked)
                {
                    ViewState["conStr"] = " where name like '%" + txtcondition.Text.Trim() + "%'";
                }
                if (this.RadioButton3.Checked)
                {
                    ViewState["conStr"] = " where Code like '%" + txtcondition.Text.Trim() + "%'";
                }

                sqlaccess.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT * from TA_Model " + ViewState["conStr"].ToString());

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


        private bool isAccountExist(string id, string Code)
        {
            bool isExist = false;
            SqlAccess accesstmp = new SqlAccess();
            string queryStr = "";

            try
            {
                if ((bool)ViewState["newMark"])//新建态
                {
                    //queryStr = "select name from TA_Model where id='" + id + "' or Code='" + Code + "'";
                    queryStr = "select name from TA_Model where id='" + id  + "'";
                }
                else //更新态
                {
                    //queryStr = "select name from TA_Model where Code='" + Code + "' and id<>'" + id + "'";
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
            //ViewState["conStr"] = " where " + (RadioButton1.Checked == true ? "id" : "name") + " like '%" + txtcondition.Text.Trim() + "%'";
            this.GridBind();
        }

        protected void GridView1_PageIndexChanging1(object sender, GridViewPageEventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }
            this.GridView1.PageIndex = e.NewPageIndex;
            //if(this.RadioButton1.Checked)
            //{
            //    ViewState["conStr"] = " where id like '%" + txtcondition.Text.Trim() + "%'";
            //}
            //if(this.RadioButton2.Checked)
            //{
            //    ViewState["conStr"] = " where name like '%" + txtcondition.Text.Trim() + "%'";
            //}
            //if(this.RadioButton3.Checked)
            //{
            //    ViewState["conStr"] = " where Code like '%" + txtcondition.Text.Trim() + "%'";
            //}
            this.GridBind();
        }
    }
}
