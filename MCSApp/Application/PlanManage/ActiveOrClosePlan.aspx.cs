﻿using System;
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
    public partial class ActiveOrClosePlan : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "生产主管" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }

            if (!IsPostBack)
            {
                ViewState["conStr"] = "";//是否为新建标记
                this.GridBind();
                this.dt_begin_UseBox.Value = System.DateTime.Now.AddMonths(-1).ToShortDateString();
                this.dt_end_UseBox.Value = System.DateTime.Now.ToShortDateString();
                this.dt_begin_UseBox.Disabled = true;
                this.dt_end_UseBox.Disabled = true;
            }

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
            string state = this.GridView1.DataKeys[e.NewEditIndex].Values["state"].ToString();
            if (state == "1")
            {
                ddlstate.Items.Add(new ListItem("初始", "1"));
                ddlstate.Items.Add(new ListItem("激活", "2"));
            }
            if (state == "2")
            {
                ddlstate.Items.Add(new ListItem("激活", "2"));
                ddlstate.Items.Add(new ListItem("关闭", "3"));
            }
            if (state == "3")
            {
                ddlstate.Items.Add(new ListItem("关闭", "3"));
            }
            ddlstate.Items[0].Selected = true;

            Label lblid = (Label)this.GridView1.Rows[e.NewEditIndex].FindControl("lblid");
            lblid.Enabled = false;
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

            this.GridBind();

        }

        /// <summary>
        /// 生成行的时候设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblid = (Label)e.Row.FindControl("lblid");
                Label lblModelID = (Label)e.Row.FindControl("lblModelID");
                Label lblstatestr = (Label)e.Row.FindControl("lblstatestr");
                LinkButton lbActive = (LinkButton)e.Row.FindControl("lbActive");
                LinkButton lbEdit = (LinkButton)e.Row.FindControl("lbEdit");
                LinkButton lbClose = (LinkButton)e.Row.FindControl("lbClose");
                lbActive.Attributes.Add("onclick", "getPlanProcedure('" + lblModelID.Text + "','" + lblid.Text + "','true')");
                lbEdit.Attributes.Add("onclick", "getPlanProcedure('" + lblModelID.Text + "','" + lblid.Text + "','true')");
                lbClose.Attributes.Add("onclick", "javascript:return confirm('你确认要关闭计划\"" + lblid.Text + "\"吗?')");

                switch (lblstatestr.Text)
                {
                    case "初始":
                        lbActive.Visible = true;
                        lbEdit.Visible = false;
                        lbClose.Visible = false;
                        break;
                    case "激活":
                        lbActive.Visible = false;
                        lbEdit.Visible = true;
                        lbClose.Visible = true;
                        break;
                    case "关闭":
                        lbActive.Visible = false;
                        lbEdit.Visible = false;
                        lbClose.Visible = false;
                        break;
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
                sb.Append("SELECT t.*,case when plantype=1 then '常规' when plantype=2 then '客退' end plantypestr,case when t.state=1 then '初始' when t.state=2 then '激活' else '关闭' end statestr ,tp.Procedureid pid from TA_Plan t left outer join tc_planProcedure tp on t.id=tp.planid and t.modelid =tp.modelid  where  1=1 " + ViewState["conStr"].ToString() + " order by FoundTime desc");

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

            string bstr = "";
            string estr = "";
            if (!this.dt_begin_UseBox.Disabled)
            {
                bstr = " and t.FoundTime>='" + this.dt_begin_UseBox.Value + "'";
            }
            if (!this.dt_end_UseBox.Disabled)
            {
                estr = " and t.FoundTime<='" + this.dt_end_UseBox.Value + "'";
            }

            if (this.RadioButton1.Checked) ViewState["conStr"] = bstr + estr + " and t.id like '%" + txtcondition.Text.Trim() + "%'";
            if (this.RadioButton2.Checked) ViewState["conStr"] = bstr + estr + " and t.modelid like '%" + txtcondition.Text.Trim() + "%'";
            if (this.RadioButton3.Checked) ViewState["conStr"] = bstr + estr + " and t.orderid like '%" + txtcondition.Text.Trim() + "%'";
            //if (this.RadioButton4.Checked) ViewState["conStr"] = bstr + estr + " and t.state like '%" + txtcondition.Text.Trim() + "%'";
            if (this.ddlPlanState.SelectedIndex != 0)
            {
                ViewState["conStr"] = ViewState["conStr"].ToString() + " and t.state='" + this.ddlPlanState.SelectedIndex + "'";
            }

            //ViewState["conStr"] = " and id like '%" + txtcondition.Text.Trim() + "%'";
            this.GridBind();
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

        protected void ckCancelTime_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckCancelTime.Checked)
            {
                this.dt_begin_UseBox.Disabled = false; ;
                this.dt_end_UseBox.Disabled = false; ;
            }
            else
            {
                this.dt_begin_UseBox.Disabled = true;
                this.dt_end_UseBox.Disabled = true;
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Close":
                    try
                    {
                        int index = Convert.ToInt32(e.CommandArgument);
                        GridViewRow row = GridView1.Rows[index];
                        Label lbl = (Label)row.FindControl("lblid");//强行转换为lable控件

                        sqlaccess.Open();
                        sqlaccess.BeginTransaction();
                        SqlCommand cmd = new SqlCommand();

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p1", lbl.Text);
                        cmd.Parameters.AddWithValue("@p2", 3);
                        cmd.Parameters.AddWithValue("@p3", System.DateTime.Now);
                        cmd.CommandText = "update ta_plan set State=@p2, CloseTime=@p3 where id=@p1";
                        sqlaccess.ExecuteQuerry(cmd);

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p1", lbl.Text);
                        cmd.CommandText = "delete TC_PlanProcedure where planid=@p1";
                        sqlaccess.ExecuteQuerry(cmd);

                        sqlaccess.Commit();
                        string strLog = "关闭计划单[" + lbl.Text + "] ## ";
                        Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), strLog + "update ta_plan set State=3 where id=" + lbl.Text + ";delete TC_PlanProcedure where planid='" + lbl.Text + "'");
                        //Methods.Write(this, "window.parent.closeit();window.parent.query()"); return;
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
                    break;
            }

        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbActive = (LinkButton)e.Row.FindControl("lbActive");
                LinkButton lbEdit = (LinkButton)e.Row.FindControl("lbEdit");
                LinkButton lbClose = (LinkButton)e.Row.FindControl("lbClose");

                lbActive.CommandArgument = e.Row.RowIndex.ToString();
                lbEdit.CommandArgument = e.Row.RowIndex.ToString();
                lbClose.CommandArgument = e.Row.RowIndex.ToString();
            }

        }


        protected string getNameByModelID(string modelID)
        {
            return Database.DataCode("select name from TA_Model where ID='" + modelID + "'");
        }


    }
}

