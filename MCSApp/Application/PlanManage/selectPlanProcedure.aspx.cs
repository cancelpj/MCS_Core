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
    public partial class selectPlanProcedure : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string pModelID = Methods.GetParam(this, "productModel");
                ViewState["planid"] = Methods.GetParam(this, "planid");
                ViewState["rblEnable"] = Methods.GetParam(this, "rblEnable");

                ViewState["pModelID"] = pModelID;

                this.btnOK.Visible = ViewState["rblEnable"].ToString().Equals("true");
                this.ddlop.Enabled = ViewState["rblEnable"].ToString().Equals("true");
                this.ddlop.Visible = false;

                if (pModelID != string.Empty)
                {
                    string sql = "select name from ta_model where id='" + pModelID + "'";
                    DataSet tmpds = Methods.getInforBySql(sql);
                    if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                    {
                        this.lblProductModel.Text = pModelID;
                        this.lblProductModelName.Text = tmpds.Tables[0].Rows[0][0].ToString();
                        //由品号查找流程
                        sql = "select id,name from ta_procedure where modelid='" + pModelID + "'";
                        ds = Methods.getInforBySql(sql);
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            this.rblProductModel.DataValueField = "id";
                            this.rblProductModel.DataTextField = "name";
                            this.rblProductModel.DataSource = ds;
                            this.rblProductModel.DataBind();
                        }

                        //将原有的值设置有效状态
                        this.setChecked(this.rblProductModel, pModelID);
                        this.rblProductModel.Enabled = ViewState["rblEnable"].ToString().Equals("true");
                        
                        //将原有的值保存下来用于后面判断流程是否发生改变
                        if (rblProductModel.SelectedIndex > -1) this.rblProductModelOld.Text = rblProductModel.Items[rblProductModel.SelectedIndex].Value;
                        
                        //查找部件品号及流程
                        this.gridBind(pModelID);
                    }
                    else
                    {
                        Methods.AjaxMessageBox(this, "此品号未定义！"); return;
                    }
                }
            }
        }

        private void gridBind(string pModelID)
        {
            string sql = " select t.id,t.itemid,tm.name from ta_structure t inner join ta_model tm on t.itemid=tm.id where t.id='" + pModelID + "' and tm.modeltype=2";//只找出部件
            ds = Methods.getInforBySql(sql);

            this.GridView1.DataSource = ds;
            this.GridView1.DataBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblitemid = (Label)e.Row.FindControl("lblitemid");//部件品号id
                RadioButtonList rblComModel = (RadioButtonList)e.Row.FindControl("rblComModel");//部件流程列表
                //由品号查找流程
                string sql = "select id,name from ta_procedure where modelid='" + lblitemid.Text + "'";
                ds = Methods.getInforBySql(sql);
                rblComModel.DataValueField = "id";
                rblComModel.DataTextField = "name";
                rblComModel.DataSource = ds;
                rblComModel.DataBind();
                //将原有的值设置有效状态
                this.setChecked(rblComModel, lblitemid.Text);
                rblComModel.Enabled = ViewState["rblEnable"].ToString().Equals("true");

                //将原有的值保存下来用于后面判断流程是否发生改变
                Label rblComModelOld = (Label)e.Row.FindControl("rblComModelOld"); //临时保存之前的部件流程选项
                if (rblComModel.SelectedIndex > -1) rblComModelOld.Text = rblComModel.Items[rblComModel.SelectedIndex].Value;
            }
        }

        private void setChecked(RadioButtonList rblComModel, string itemid)
        {
            string sql = "select procedureid from tc_planProcedure where planid='" + ViewState["planid"].ToString() + "' and modelid='" + itemid + "'";
            ds = Methods.getInforBySql(sql);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                rblComModel.SelectedIndex = rblComModel.Items.IndexOf(rblComModel.Items.FindByValue(ds.Tables[0].Rows[0][0].ToString()));
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["pModelID"] != null && ViewState["pModelID"].ToString() != string.Empty)
                {
                    sqlaccess.Open();
                    sqlaccess.BeginTransaction();
                    SqlCommand cmd = new SqlCommand();

                    if (this.ddlop.SelectedValue == "3")//删除时
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p1", ViewState["planid"]);
                        cmd.Parameters.AddWithValue("@p2", this.ddlop.SelectedValue);
                        cmd.Parameters.AddWithValue("@p3", System.DateTime.Now);
                        cmd.CommandText = "update ta_plan set State=@p2, CloseTime=@p3 where id=@p1";
                        sqlaccess.ExecuteQuerry(cmd);

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p1", ViewState["planid"]);
                        cmd.CommandText = "delete TC_PlanProcedure where planid=@p1";
                        sqlaccess.ExecuteQuerry(cmd);

                        sqlaccess.Commit();
                        string strLog = "关闭计划单[" + ViewState["planid"].ToString() + "] ## ";
                        Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), strLog + "update ta_plan set State=3 where id=" + ViewState["planid"].ToString() + ";delete TC_PlanProcedure where planid='" + ViewState["planid"].ToString() + "'");
                        Methods.Write(this, "window.parent.closeit();window.parent.query()"); return;

                    }
                    string logstr = "";
                    if (this.rblProductModel.SelectedIndex != -1)
                    {
                        //更新计划激活状态
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p1", ViewState["planid"]);
                        cmd.Parameters.AddWithValue("@p2", this.ddlop.SelectedValue);
                        cmd.CommandText = "update ta_plan set State=@p2 where id=@p1";
                        sqlaccess.ExecuteQuerry(cmd);

                        //产品工艺流程发生变化
                        if (rblProductModelOld.Text != rblProductModel.Items[rblProductModel.SelectedIndex].Value)
                        {
                            string sql = "SELECT ID FROM TA_Product"
                                + " WHERE PlanID = '" + ViewState["planid"].ToString() 
                                + "' AND ModelID='" + this.lblProductModel.Text +"'";
                            DataSet tmpDS = Methods.getInforBySql(sql);
                            if (tmpDS != null && tmpDS.Tables[0].Rows.Count > 0)
                            {
                                string tmpProductID;
                                string oMsg;
                                WSProcedureCtrl.ProcedureCtrl c = new MCSApp.WSProcedureCtrl.ProcedureCtrl();
                                for (int i = 0; i < tmpDS.Tables[0].Rows.Count; i++)
                                {
                                    //发生了流程变更的产品序列号
                                    tmpProductID = tmpDS.Tables[0].Rows[i][0].ToString();
                                    //创建一条流程变更的工序历史记录
                                    c.SaveProcessPurely(tmpProductID, "变更工艺流程", SessionUser.ID, 2, "", "", DateTime.Now, DateTime.Now, out oMsg);
                                }
                                c.Dispose();
                            }
                            tmpDS.Dispose();
                        }
                        
                        //更新计划选定的产品工艺流程
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p1", ViewState["planid"]);
                        cmd.Parameters.AddWithValue("@p2", ViewState["pModelID"]);
                        cmd.CommandText = "delete TC_PlanProcedure where planid = @p1 AND ModelID = @p2";
                        sqlaccess.ExecuteQuerry(cmd);

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p1", ViewState["planid"]);
                        cmd.Parameters.AddWithValue("@p2", ViewState["pModelID"]);
                        cmd.Parameters.AddWithValue("@p3", this.rblProductModel.SelectedValue);
                        cmd.CommandText = "insert into TC_PlanProcedure (planid,modelid,procedureid) values (@p1,@p2,@p3)";
                        sqlaccess.ExecuteQuerry(cmd);

                        logstr = "delete TC_PlanProcedure where planid='" + ViewState["planid"].ToString() 
                            + "';insert into TC_PlanProcedure (planid,modelid,procedureid) values ('" + ViewState["planid"].ToString() + "','" 
                            + ViewState["pModelID"].ToString() + "','" + this.rblProductModel.SelectedValue + "');";
                        
                        //逐项处理部件工艺流程
                        for (int i = 0; i < this.GridView1.Rows.Count; i++)
                        {
                            RadioButtonList rbl = (RadioButtonList)this.GridView1.Rows[i].FindControl("rblComModel");
                            Label lblitemid = (Label)this.GridView1.Rows[i].FindControl("lblitemid");
                            Label rblOld = (Label)this.GridView1.Rows[i].FindControl("rblComModelOld");

                            if (rbl != null && rbl.SelectedIndex != -1)
                            {
                                //部件工艺流程发生变化
                                if (rblOld.Text != rbl.Items[rbl.SelectedIndex].Value)
                                {
                                    string sql = "SELECT ID FROM TA_Product"
                                        + " WHERE PlanID = '" + ViewState["planid"].ToString()
                                        + "' AND ModelID='" + lblitemid.Text + "'";
                                    DataSet tmpDS = Methods.getInforBySql(sql);
                                    if (tmpDS != null && tmpDS.Tables[0].Rows.Count > 0)
                                    {
                                        string tmpProductID;
                                        string oMsg;
                                        WSProcedureCtrl.ProcedureCtrl c = new MCSApp.WSProcedureCtrl.ProcedureCtrl();
                                        for (int j = 0; j < tmpDS.Tables[0].Rows.Count; j++)
                                        {
                                            //发生了流程变更的产品序列号
                                            tmpProductID = tmpDS.Tables[0].Rows[j][0].ToString();
                                            //创建一条流程变更的工序历史记录
                                            c.SaveProcessPurely(tmpProductID, "变更工艺流程", SessionUser.ID, 2, "", "", DateTime.Now, DateTime.Now, out oMsg);
                                        }
                                        c.Dispose();
                                    }
                                    tmpDS.Dispose();
                                }

                                //更新计划选定的部件工艺流程
                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@p1", ViewState["planid"]);
                                cmd.Parameters.AddWithValue("@p2", lblitemid.Text);
                                cmd.CommandText = "delete TC_PlanProcedure where planid = @p1 AND ModelID = @p2";
                                sqlaccess.ExecuteQuerry(cmd);

                                cmd.Parameters.Clear();
                                cmd.Parameters.AddWithValue("@p1", ViewState["planid"]);
                                cmd.Parameters.AddWithValue("@p2", lblitemid.Text);
                                cmd.Parameters.AddWithValue("@p3", rbl.SelectedValue);
                                cmd.CommandText = "insert into TC_PlanProcedure (planid,modelid,procedureid) values (@p1,@p2,@p3)";
                                sqlaccess.ExecuteQuerry(cmd);

                                logstr = logstr + "insert into TC_PlanProcedure (planid,modelid,procedureid) values ('" + ViewState["planid"].ToString() 
                                    + "','" + lblitemid.Text + "','" + rbl.SelectedValue + "')";
                            }
                        }
                    }
                    else
                    {
                        Methods.AjaxMessageBox(this, "请选择产品的流程!");
                    }
                    sqlaccess.Commit();
                    if (logstr != "")
                    {
                        string strLog = "激活计划单[" + ViewState["planid"].ToString() + "] ## ";
                        Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), strLog + logstr);
                    }
                    Methods.Write(this, "window.parent.closeit();window.parent.query()");
                }


            }
            catch (Exception ex)
            {
                LogManager.Write(this, ex.Message);
                Methods.AjaxMessageBox(this, "保存流程到计划流程表时出现异常！可能是由于添加重复记录！");
            }
            finally
            {
                sqlaccess.Close();
                
            }
        }
    }
}
