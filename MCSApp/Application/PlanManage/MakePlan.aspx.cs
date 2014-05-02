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

namespace MCSApp.Application.PlanManage
{
    public partial class MakePlan : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "计划员" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }

            if (!IsPostBack)
            {
                ViewState["conStr"] = "";//是否为新建标记
                this.GridBind();

                ComboBox1_bind();
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

            TextBox txtid = (TextBox)this.GridView1.Rows[e.NewEditIndex].FindControl("txtid");
            txtid.Enabled = false; txtid.Enabled = false;


            Coolite.Ext.Web.ComboBox ComboBox1 = (Coolite.Ext.Web.ComboBox)this.GridView1.Rows[e.NewEditIndex].FindControl("ComboBox1");
            ComboBox1.Visible = false;


            DropDownList ddlPlanType = (DropDownList)this.GridView1.Rows[e.NewEditIndex].FindControl("ddlPlanType");//用户名称标签
            Label lblPlanType = (Label)this.GridView1.Rows[e.NewEditIndex].FindControl("lblPlanType");

            ddlPlanType.SelectedIndex = ddlPlanType.Items.IndexOf(ddlPlanType.Items.FindByValue(lblPlanType.Text));


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
            string delstr = "delete from TA_Plan  where id='" + lbl.Text + "'";//取编号值
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

        }
        /// <summary>
        /// 更新时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string id = this.GridView1.DataKeys[e.RowIndex].Value.ToString();
            TextBox txtid = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtid");
            TextBox txtModelID = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtModelID");
            TextBox txtOutput = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtOutput");
            TextBox txtOrderID = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtOrderID");
            //TextBox txtPlanType = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtPlanType");
            DropDownList ddlPlanType = (DropDownList)this.GridView1.Rows[e.RowIndex].FindControl("ddlPlanType");
            TextBox txtremark = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtremark");

            if (id != null && id != string.Empty)
            {
                if (!Methods.ModelIDIsInDB(txtModelID.Text.Trim()))
                {
                    Methods.AjaxMessageBox(this, "输入的品号无效！"); return;
                }
            }
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                if (id!=null && id!= string.Empty)
                {
                    SqlCommand cmd = new SqlCommand();

                    cmd.Parameters.AddWithValue("@p1",txtid.Text.Trim());
                    cmd.Parameters.AddWithValue("@p2",txtModelID.Text.Trim());
                    cmd.Parameters.AddWithValue("@p3",txtOutput.Text.Trim());
                    cmd.Parameters.AddWithValue("@p4",txtOrderID.Text.Trim());
                    //cmd.Parameters.AddWithValue("@p5",txtPlanType.Text.Trim());
                    cmd.Parameters.AddWithValue("@p5", ddlPlanType.SelectedValue);
                    cmd.Parameters.AddWithValue("@p6",txtremark.Text.Trim());
                    //cmd.Parameters.AddWithValue("@p7", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd"));
                    //cmd.Parameters.AddWithValue("@p8",1);



                    cmd.CommandText = " UPDATE TA_Plan SET  ModelID = @p2, Output = @p3, OrderID = @p4, PlanType = @p5,Remark = @p6 where ID = @p1";

                    sqlaccess.ExecuteQuerry(cmd);
                }
                else
                {

                    Coolite.Ext.Web.ComboBox ComboBox1 = (Coolite.Ext.Web.ComboBox)this.GridView1.Rows[e.RowIndex].FindControl("ComboBox1");


                    SqlCommand cmd = new SqlCommand();                    
                    cmd.Parameters.AddWithValue("@p1",txtid.Text.Trim());
                    cmd.Parameters.AddWithValue("@p2",ComboBox1.SelectedItem.Value.Trim());
                    cmd.Parameters.AddWithValue("@p3",txtOutput.Text.Trim());
                    cmd.Parameters.AddWithValue("@p4",txtOrderID.Text.Trim());
                    //cmd.Parameters.AddWithValue("@p5",txtPlanType.Text.Trim());
                    cmd.Parameters.AddWithValue("@p5", ddlPlanType.SelectedValue);
                    cmd.Parameters.AddWithValue("@p6",txtremark.Text.Trim());
                    cmd.Parameters.AddWithValue("@p7", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd"));
                    cmd.Parameters.AddWithValue("@p8",1);

                    cmd.CommandText = "INSERT INTO TA_Plan (ID, ModelID, Output, OrderID, PlanType, Remark, FoundTime, State) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)";
                    sqlaccess.ExecuteQuerry(cmd);
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
                ds = sqlaccess.OpenQuerry("select *,case when plantype=1 then '常规' when plantype=2 then '客退' end plantypestr  from TA_Plan where state=1");

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
            r["output"] = 1;
            r["PlanType"] = 1;

            ds.Tables[0].Rows.InsertAt(r, 0);
            ds.AcceptChanges();
            this.GridView1.EditIndex = 0;
            this.GridView1.PageIndex = 0;

            this.GridView1.DataSource = ds;
            this.GridView1.DataBind();
            ViewState["newMark"] = true;//有效

            (this.GridView1.Rows[0].FindControl("txtModelID") as TextBox).Text = "0";
            (this.GridView1.Rows[0].FindControl("txtModelID") as TextBox).Visible = false;

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

                ImageButton bt = (ImageButton)e.Row.FindControl("ImageButton2");//删除图片按钮
                Label lbl = (Label)e.Row.FindControl("lblid");//用户名称标签
                if (bt.CommandName == "Delete")
                {
                    bt.Attributes.Add("onclick", "javascript:return confirm('你确认要删除计划\"" + lbl.Text + "\"吗?')");
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
                sb.Append("SELECT *,case when plantype=1 then '常规' when plantype=2 then '客退' end plantypestr from TA_Plan where  state=1 " + ViewState["conStr"].ToString());

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
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }
            ViewState["conStr"] = " and id like '%" + txtcondition.Text.Trim() + "%'";
            this.GridBind();
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }
            ViewState["conStr"] = " and  id like '%" + txtcondition.Text.Trim() + "%'";
            this.GridView1.PageIndex = e.NewPageIndex;
            this.GridBind();
        }


        protected void ComboBox1_bind()
        {
            Store1.DataSource = Database.DataTable("select ID,ID+'['+Name+']' AS IDName from TA_Model");
            Store1.DataBind();
        }

    }
}
