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
    public partial class ManageGroup : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        public string clid = "";//用于界面上弹出窗体回传值时，找到控件的ID用
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "班组长" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
            this.btnAdd.Attributes.Add("onclick", " return patchAddEmployee();");
            this.btndelete.Attributes.Add("onclick", " return confirm('你确定要删除勾选的班组成员?');");
            if (!IsPostBack)
            {
                ViewState["conStr"] = "";//是否为新建标记
                GridBind();
                sltBzBind(SessionUser.ID);
                if(this.GridView1.Rows.Count>0)
                {
                    this.GridView1.SelectedIndex = 0;
                    this.grid2Bind(this.GridView1.DataKeys[this.GridView1.SelectedIndex].Value.ToString());
                    Session["Groupid"] = this.GridView1.DataKeys[this.GridView1.SelectedIndex].Value.ToString(); //组ID 用于第二页批量添加员工用
                }
            }

        }
        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //这里不作任何事，界面弹出窗体作
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                foreach(GridViewRow row in this.GridView2.Rows)
                {
                    CheckBox ck = (CheckBox)row.FindControl("ckID");
                    if (ck.Checked)
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Parameters.AddWithValue("@p1",this.GridView2.DataKeys[row.RowIndex].Value.ToString());
                        cmd.Parameters.AddWithValue("@p2", this.GridView1.DataKeys[this.GridView1.SelectedIndex].Value.ToString());
                        cmd.CommandText = "delete TRE_Group_Employee where groupid=@p2 and employeeid=@p1";
                        sqlaccess.ExecuteQuerry(cmd);
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p1",this.GridView2.DataKeys[row.RowIndex].Value.ToString());
                        cmd.Parameters.AddWithValue("@p2", "作业员");
                        cmd.CommandText = "delete TRE_Employee_Role where Role=@p2 and employeeid=@p1";
                        sqlaccess.ExecuteQuerry(cmd);
                    }

                }
                sqlaccess.Commit();
                this.grid2Bind(this.GridView1.DataKeys[this.GridView1.SelectedIndex].Value.ToString());

            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ((SqlException)ex).ErrorCode == -2146232060)
                {
                    Methods.AjaxMessageBox(this, "尚有其它数据与此数据关联，暂不能删除！"); return;
                }
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
            ViewState["conStr"] = " where " + (this.RadioButton1.Checked == true ? "Name" : "PlanID") + " like '%" + txtcondition.Text.Trim() + "%'";
            this.GridBind();
        }
        /// <summary>
        /// gridview1绑定用
        /// </summary>
        private void GridBind()
        {
            try
            {
                sqlaccess.Open();

                SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Parameters.AddWithValue("@p1", SessionUser.ID);
                cmd.CommandText = "select ID, Name from TA_Group where id not in (select GroupID from TRE_Group_Plan) and LeaderID=@p1";
                ds = sqlaccess.OpenQuerry(cmd);
                if (ds.Tables[0].Rows.Count > 0) Methods.AjaxMessageBox(this,"有的班组还没分配生产计划！");
                ds.Clear();
                cmd.Dispose();

                StringBuilder sb = new StringBuilder();
                //sb.Append("SELECT ID, Name,LeaderID,planID FROM TA_Group" + ViewState["conStr"].ToString());//在html中隐了查找，这里因为没地方设置viewstate 所以注掉了
                sb.Append("SELECT A.ID, A.Name, A.LeaderID, R.PlanID FROM TA_Group A, TRE_Group_Plan R where A.ID=R.GroupID and A.LeaderID='"+SessionUser.ID+"'");
                ds = sqlaccess.OpenQuerry(sb.ToString());

                this.GridView1.DataSource = ds;//没有记录时，添加 删除按钮均不显示
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    this.btnAdd.Visible = true;
                    this.btndelete.Visible = true;
                }
                else
                {
                    this.btnAdd.Visible = false; 
                    this.btndelete.Visible = false ;                     
                }
                this.GridView1.DataBind();
                this.lblMsg.Visible = false ;
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
            //ViewState["groupid"] = this.GridView1.DataKeys[this.GridView1.SelectedIndex].Value.ToString();
            
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.GridView1.EditIndex = -1;
            this.GridBind();
            this.lblMsg.Visible = false;
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
            TextBox txtPlanID = (TextBox)this.GridView1.Rows[e.NewEditIndex].FindControl("txtPlanID");
            clid = txtPlanID.ClientID;
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string GroupID = this.GridView1.DataKeys[e.RowIndex].Value.ToString();
            Label PlanID = (Label)this.GridView1.Rows[e.RowIndex].FindControl("lblPlanID");
            
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Parameters.AddWithValue("@p1", GroupID);
            cmd.Parameters.AddWithValue("@p2", PlanID.Text);

            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                if (GroupID != string.Empty && PlanID.Text != string.Empty)
                {
                    cmd.CommandText = "delete from TRE_Group_Plan where GroupID=@p1 and planID =@p2";
                    sqlaccess.ExecuteQuerry(cmd);
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

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string id = this.GridView1.DataKeys[e.RowIndex].Value.ToString();
            TextBox txtPlanID = (TextBox)this.GridView1.Rows[e.RowIndex].FindControl("txtPlanID");
            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.Parameters.AddWithValue("@p1", id);
            cmd.Parameters.AddWithValue("@p2", txtPlanID.Text.Trim());

            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                if (!Methods.PlanISActive(txtPlanID.Text.Trim()))
                {
                    Methods.AjaxMessageBox(this, "无效的计划单，不能被工作组执行！"); return;
                }
                if (id != string.Empty)
                {
                    cmd.CommandText= "update TA_Group set planID =@p2 where ID=@p1";
                    sqlaccess.ExecuteQuerry(cmd);
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
                    char[] cs = { '\r', '\n' };Methods.AjaxMessageBox(this, ex.Message.Split(cs)[0].Replace('\'', '"'));
                }
                LogManager.Write(this, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        private void grid2Bind(string GroupID)
        {
                try
                {
                    SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                    cmd.Parameters.AddWithValue("@p1", GroupID);
                    cmd.CommandText = "select t.employeeid,p.name from TRE_Group_Employee t,TA_Employee p where t.employeeid=p.ID and t.groupid=@p1";
                    ds = sqlaccess.OpenQuerry(cmd);
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
        /// <summary>
        /// 刷新窗体以展示批量添加员工用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnrefresh_Click(object sender, EventArgs e)
        {
            this.GridBind();
            this.grid2Bind(this.GridView1.DataKeys[this.GridView1.SelectedIndex].Value.ToString());
            Session["Groupid"] = this.GridView1.DataKeys[this.GridView1.SelectedIndex].Value.ToString(); //组ID 用于第二页批量添加员工用
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.GridView1.EditIndex != -1)
            {
                this.lblMsg.Visible = true;
                return;
            }
            //this.GridBind();
            string GroupID = "";
            GroupID = this.GridView1.DataKeys[this.GridView1.SelectedIndex].Value.ToString();
            this.grid2Bind(GroupID);
            Session["Groupid"] = this.GridView1.DataKeys[this.GridView1.SelectedIndex].Value.ToString(); //组ID 用于第二页批量添加员工用
        }

        private void sltBzBind(string UserID)
        {
            try
            {
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
                cmd.Parameters.AddWithValue("@p1", UserID);
                cmd.CommandText = "select ID, Name from TA_Group where LeaderID = @p1";
                ds = sqlaccess.OpenQuerry(cmd);
                this.sltBz.DataSource = ds;
                this.sltBz.DataValueField = ds.Tables[0].Columns["ID"].ToString();
                this.sltBz.DataTextField = ds.Tables[0].Columns["Name"].ToString();
                this.sltBz.DataBind();
                ds.Dispose();
                //this.btnInsertPlan.Attributes.Add("onclick", " return AddPlan(" + sltBz.SelectedValue.ToString() + ");");
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

        protected void btnInsertPlan_Click(object sender, EventArgs e)
        {
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();

                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.AddWithValue("@p1", sltBz.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@p2", txtPlanIDNew.Text);
                cmd.CommandText = "insert into TRE_Group_Plan (GroupID,PlanID) values(@p1,@p2)";
                sqlaccess.ExecuteQuerry(cmd);
                sqlaccess.Commit();

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
    }
}
