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
using MCSApp.Common;
using MCSApp.DataAccess.LogManage;
using MCSApp.DataAccess;
using FINGU.MCS;
using MCSApp.WSSecurity;

namespace MCSApp.Application.EmployeeManage
{
    public partial class RoleDispatch : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string ID = SessionUser.ID;
                string password = SessionUser.Password;
                string[] Roles = { "系统管理员" };
                string outStr;
                ViewState["conStr"] = "";//是否为新建标记

                if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
                {
                    return;
                }

                //GridBind();
            }
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.GridView1.EditIndex = -1;
            this.lblMsg.Visible = false;
            this.GridBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //e.Row.Attributes.Add("onclick", "changeStyle(this)");
                string id = this.GridView1.DataKeys[e.Row.RowIndex].Value.ToString();
                CheckBoxList ckl = (CheckBoxList)e.Row.FindControl("ckblstRight");//
                iniCheckList(ckl, id);//初始化多选框
                ckl.Enabled = false;
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
            CheckBoxList ckl = (CheckBoxList)this.GridView1.Rows[e.NewEditIndex].FindControl("ckblstRight");
            ckl.Enabled = true;
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label txtID = (Label)this.GridView1.Rows[e.RowIndex].FindControl("txtID");
            CheckBoxList ckl = (CheckBoxList)this.GridView1.Rows[e.RowIndex].FindControl("ckblstRight");
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                string strdel = "delete TRE_Employee_Role where EmployeeID='" + txtID.Text + "'";
                sqlaccess.ExecuteQuerry(strdel);
                LogManager.WriteOprationLog(SessionUser.DetailInfo, strdel);

                foreach (ListItem item in ckl.Items)
                {
                    if (item.Selected)
                    {
                        string insertStr = "insert into TRE_Employee_Role (EmployeeID,Role) values ('" + txtID.Text + "','" + item.Value + "')";
                        sqlaccess.ExecuteQuerry(insertStr);
                        LogManager.WriteOprationLog(SessionUser.DetailInfo, insertStr);
                    }
                }

                sqlaccess.Commit();
                this.GridView1.EditIndex = -1;
                this.lblMsg.Visible = false;
                this.GridBind();
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
        /// 初始化chklist
        /// </summary>
        private void iniCheckList(CheckBoxList ckblst, string EID)
        {
            string superStr = (SessionUser.IsSuperAdmin == true) ? "" : "where role <> '超级管理员' and role <> '系统管理员' ";
            string queryStr = "select role from ta_role "+superStr;//是否为超级管理员
            try
            {
                sqlaccess.Open();
                DataSet ds = null;
                ds = sqlaccess.OpenQuerry(queryStr);
                //删除后重新绑定 
                ckblst.DataSource = ds;
                ckblst.DataTextField = "role";
                ckblst.DataValueField = "role";
                ckblst.DataBind();

                string strForUser = "select ter.EmployeeID,ter.Role from TRE_Employee_Role ter where ter.EmployeeID='" + EID + "'";
                DataSet dstmp = null;
                dstmp = sqlaccess.OpenQuerry(strForUser);
                if (dstmp != null && dstmp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in dstmp.Tables[0].Rows)
                    {
                        ckblst.Items.FindByValue(row["Role"].ToString()).Selected = true;
                    }
                }

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
            this.GridView1.PageIndex = e.NewPageIndex;//页面索引重新给定
            this.GridBind();
        }

        private void GridBind()
        {
            try
            {
                sqlaccess.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ID, Name FROM TA_Employee" + ViewState["conStr"].ToString());

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
            string preStr = "";
            if(RadioButton1.Checked)
            {
                preStr = "id";
            }
            if(RadioButton2.Checked)
            {
                preStr = "name";                
            }
            ViewState["conStr"] = " where " + preStr+ " like '%" + txtcondition.Text.Trim() + "%' and id <>'"+SessionUser.ID+"'";

            if (RadioButton3.Checked)
            {
                ViewState["conStr"] = " WHERE (ID IN (SELECT EmployeeID FROM TRE_Employee_Role AS er WHERE (Role like'%"+txtcondition.Text.Trim()+"%')) and id <>'"+SessionUser.ID+"')";
            } 

            this.GridBind();
        }
    }
}
