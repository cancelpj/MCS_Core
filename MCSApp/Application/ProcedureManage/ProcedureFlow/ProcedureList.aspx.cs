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

namespace MCSApp.Application.ProcedureManage.ProcedureFlow
{
    public partial class ProcedureList : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "制造工程师" };

            if (!IsPostBack)
            {
                ViewState["conStr"] = "";
                ViewState["tmpMID"] = "";//产品品号 用于记录上次输入的品号
                this.GridBind();

                ComboBox1_bind();

            }
        }

        private void GridBind()
        {
            sqlaccess.Open();
            try
            {
                //如果已经定义过流程，则不允许编辑
                string tmpsql = "SELECT ModelID,Name,ID,ProcessConfig,ProcessGraph,case when ProcessConfig is null then 'true' else 'false' end EditState,case when ProcessConfig is null then '不完整' else '完整' end State from TA_Procedure where 1=1 " + ViewState["conStr"].ToString() +" order by id desc";
                ds = sqlaccess.OpenQuerry(tmpsql);
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
            if (this.RadioButton1.Checked)
            {
                ViewState["conStr"] = " and modelid like '%" + this.txtcondition.Text.Trim() + "%'";
            }
            else
            {
                ViewState["conStr"] = " and Name like '%" + this.txtcondition.Text.Trim() + "%'";
            }
            this.GridBind();
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

            //DropDownList ddlstate = (DropDownList)this.GridView1.Rows[e.NewEditIndex].FindControl("ddlModelType");
            //ddlstate.SelectedValue = this.GridView1.DataKeys[e.NewEditIndex].Value.ToString();
            TextBox txtModelID = (TextBox)this.GridView1.Rows[e.NewEditIndex].FindControl("txtModelID");
            Coolite.Ext.Web.ComboBox ComboBox1 = (Coolite.Ext.Web.ComboBox)this.GridView1.Rows[e.NewEditIndex].FindControl("ComboBox1");

            txtModelID.Enabled = false;
            ComboBox1.Visible = false;

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



            try
            {
                string sql = "select ModelID from dbo.TC_PlanProcedure where ProcedureID=" + lbl.Text;
                sqlaccess.Open();
                string ModelId = sqlaccess.OpenQuerry(sql).Tables[0].Rows[0][0].ToString();
                sqlaccess.Close();
                string sql2 = "select * from dbo.TB_ProcedureHistory where ProductID like '" + ModelId + "%'";
                sqlaccess.Open();
                if (sqlaccess.OpenQuerry(sql2).Tables[0].Rows.Count > 0)
                {
                    Methods.AjaxMessageBox(this, "该流程已进行，无法删除!");
                    return;
                }

            }
            catch { }





            string delstr = "delete from TA_Procedure  where id='" + lbl.Text + "'";//取编号值
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                sqlaccess.ExecuteQuerry(delstr);
                //string logStr = delstr;
                //Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);

                sqlaccess.Commit();
                //LogManager.WriteOprationLog(SessionUser.DetailInfo, delstr);
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
            //ViewState["newMark"] = false;//新建清除

        }
        /// <summary>
        /// 更新时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //TextBox txtid = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtid");
            string txtid = this.GridView1.DataKeys[e.RowIndex].Value.ToString();
            TextBox txtName = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtName");
            //DropDownList ddlModelType = (DropDownList)this.GridView1.Rows[e.RowIndex].FindControl("ddlModelType");
            TextBox txtModelID = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtModelID");
            string tmpsql = "select id from ta_model where id='" + txtModelID.Text.Trim() + "'";
            ds = sqlaccess.OpenQuerry(tmpsql);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                this.lblMsg.Visible = false;
            }
            else
            {
                if (txtid != string.Empty)
                {
                    this.lblMsg.Text = "输入的品号在数据库中不存在!";
                    this.lblMsg.Visible = true;
                    return;
                }
            }


            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                if (txtid != string.Empty)//非新建状态
                {
                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Parameters.AddWithValue("@p1", txtid.Trim());
                    cmd.Parameters.AddWithValue("@p2", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@p3", txtModelID.Text.Trim());
                    cmd.CommandText = "update TA_Procedure set name=@p2,ModelID=@p3 where id=@p1";
                    sqlaccess.ExecuteQuerry(cmd);
                    //LogManager.WriteOprationLog(SessionUser.DetailInfo, updateStr);
                    //string logStr = "update TA_Model set id ='{0}',name='{1}',ModelType='{2}',Code='{3}' where id='{4}'";
                    //logStr = String.Format(logStr, txtid.Text.Trim(), txtName.Text.Trim(), ddlModelType.SelectedValue, txtCode.Text.Trim(), txtid.Text.Trim());
                    //Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);
                }
                else//新建状态
                {
                    Coolite.Ext.Web.ComboBox ComboBox1 = (Coolite.Ext.Web.ComboBox)this.GridView1.Rows[e.RowIndex].FindControl("ComboBox1");

                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                    //cmd.Parameters.AddWithValue("@p1", txtid.Text.Trim());
                    cmd.Parameters.AddWithValue("@p2", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@p3", ComboBox1.SelectedItem.Value.ToString().Trim());
                    cmd.CommandText = "insert into TA_Procedure  (name,ModelID) values(@p2,@p3)";
                    sqlaccess.ExecuteQuerry(cmd);
                    //string logStr = "insert into TA_Model  (id,name,ModelType,Code) values('{0}','{1}','{2}','{3}')";
                    //logStr = String.Format(logStr, txtid.Text.Trim(), txtName.Text.Trim(), ddlModelType.SelectedValue, txtCode.Text.Trim());
                    //LogManager.WriteOprationLog(SessionUser.DetailInfo, updateStr);
                    //Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);
                }

                sqlaccess.Commit();
                this.GridView1.EditIndex = -1;
                this.lblMsg.Visible = false;
                this.lblAccMsg.Visible = false;
                this.GridBind();
            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                //if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ((SqlException)ex).ErrorCode == -2146232060)
                //{
                //    char[] cs = { '\r', '\n' }; Methods.AjaxMessageBox(this, ex.Message.Split(cs)[0].Replace('\'', '"'));
                //}
                this.lblMsg.Text = "更新的记录在数据库中已存在！";
                this.lblMsg.Visible = true;
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
                ds = sqlaccess.OpenQuerry("select *,'false' EditState,case when ProcessConfig is null then '不完整' else '完整' end State  from TA_Procedure");

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
            r["EditState"] = "true";
            //r["EditState"] = "不完整";
            ds.Tables[0].Rows.InsertAt(r, 0);
            ds.AcceptChanges();
            this.GridView1.EditIndex = 0;
            this.GridView1.PageIndex = 0;




            this.GridView1.DataSource = ds;
            this.GridView1.DataBind();

            //TextBox txtModelID = (TextBox)this.GridView1.Rows[2].FindControl("txtModelID");



            //ViewState["newMark"] = true;//有效
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
                //DropDownList ddlstate = (DropDownList)e.Row.FindControl("ddlModelType");
                //Label lblp = (Label)e.Row.FindControl("lblp");
                //ddlstate.SelectedValue = lblp.Text;
                Label lblsta = (Label)e.Row.FindControl("lblstate");        //流程状态，State是用来显示，EditState是用来判断
                LinkButton lbBorrow = (LinkButton)e.Row.FindControl("lbBorrow");
                Label lblid = (Label)e.Row.FindControl("lblID");
                if (lblsta.Text == "不完整")
                {
                    lblsta.ForeColor = System.Drawing.Color.Red;
                    lbBorrow.Visible = true;
                    lbBorrow.Attributes.Add("onclick", "BorrowProcedure('" + lblid.Text + "')");

                }
                else
                {
                    if (lbBorrow != null)
                        lbBorrow.Visible = false;

                }
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


        protected void ComboBox1_bind()
        {
            Store1.DataSource = Database.DataTable("select ID from TA_Model");
            Store1.DataBind();



        }


    }
}
