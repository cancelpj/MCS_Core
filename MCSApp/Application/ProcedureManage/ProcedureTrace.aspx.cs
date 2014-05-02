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
using MCSApp.WSProcedureCtrl;

namespace MCSApp.Application.ProcedureManage
{
    public partial class ProcedureTrace : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        int Len = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MIDLenInSN"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "一级数据查看者" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
            if (!IsPostBack)
            {
                ViewState["conStr"] = "";
            }

        }
        private void generRSTree(string ID, string Name)
        {
            try
            {
                this.TreeStructure.Nodes.Clear();
                //TreeNode root = new TreeNode(ID, ID);
                TreeNode root = new TreeNode("品名:" + Name + " 序列号" + ID, ID);
                //root.NavigateUrl = "javascript:showDetail('" + ID + Name + "','" + ID + "')";
                this.TreeStructure.Nodes.Add(root);//产品根节点
                sqlaccess.Open();
                //到产品结构中取结构 
                
                ds = sqlaccess.OpenQuerry("select t.ID,t.itemID,tm.Name from TA_Relationship t left outer join ta_model tm on SUBSTRING(t.itemid,1,"+Len+")=tm.id  where t.ID='" + ID + "'");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        //TreeNode node = new TreeNode(r["itemID"].ToString(), r["itemID"].ToString());
                        TreeNode node = new TreeNode("品名:" + r["Name"].ToString() + " 序列号：" + r["itemID"].ToString(), r["itemID"].ToString());
                        //node.NavigateUrl = "javascript:showDetail('" + r["itemID"].ToString() + r["Name"].ToString() + "','" + r["itemID"].ToString() + "')";
                        this.AddRSNode(node);
                        root.ChildNodes.Add(node);//添加部件或物料
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

        private void AddRSNode(TreeNode node)
        {
            try
            {
                DataSet dstmp = null;
                sqlaccess.Open();
                
                dstmp = sqlaccess.OpenQuerry("select t.ID,t.itemID,tm.Name  from TA_Relationship t left outer join ta_model tm on SUBSTRING(t.itemid,1,"+Len+")=tm.id  where t.ID='" + node.Value + "'");
                if (dstmp != null && dstmp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dstmp.Tables[0].Rows)
                    {
                        //TreeNode nodetmp = new TreeNode(r["itemID"].ToString(), r["itemID"].ToString());
                        TreeNode nodetmp = new TreeNode("品名:" + r["Name"].ToString() + " 序列号：" + r["itemID"].ToString(), r["itemID"].ToString());
                        //nodetmp.NavigateUrl = "javascript:showDetail('" + r["itemID"].ToString() + r["Name"].ToString() + "','" + r["itemID"].ToString() + "')";
                        node.ChildNodes.Add(nodetmp);//添加物料
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Write(this, ex.Message);
                node = new TreeNode();
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        protected void btnAddRcd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtID.Text.Trim() == "")
                {
                    Methods.AjaxMessageBox(this, "序列号或条码不能为空！"); return;
                }
                
                string mid = "";
                string productid = "";
                //int rc = Methods.getModelIDCount(this.txtID.Text.Trim());
                //if (rc == 0)
                //{
                //    Methods.AjaxMessageBox(this, "此序列号或条码在数据库中不存在！"); return;
                //}
                //else if (rc < 0) { Methods.AjaxMessageBox(this, "查询产品序列号时出现异常，详情请查看日志！"); return; }

                DataSet dstmp = Methods.getInforBySql("select modelid,id from ta_product where id ='"+this.txtID.Text.Trim()+"' or sn='"+this.txtID.Text.Trim()+"'");

                if (dstmp != null && dstmp.Tables[0].Rows.Count > 0)
                {
                    mid = dstmp.Tables[0].Rows[0][0].ToString();
                    productid = dstmp.Tables[0].Rows[0][1].ToString();
                }
                else
                {
                    this.TreeStructure.Nodes.Clear();
                    this.Panel1.Visible = this.Panel2.Visible = false;
                    this.GridView1.DataSource = null;
                    this.GridView1.DataBind();
                    Methods.AjaxMessageBox(this, "此序列号或条码在数据库中不存在！"); return;
                }
                string name = "";
                sqlaccess.Open();
                
                ds = sqlaccess.OpenQuerry("select tm.Name,t.id  from ta_structure t,ta_model tm where t.id=tm.id and t.id='" + mid + "'");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    name = ds.Tables[0].Rows[0][0].ToString();
                    this.generRSTree(productid, name);
                    this.TreeStructure.ExpandAll();//展开所有节点
                }
                this.GridView1.DataSource = null;
                this.GridView1.DataBind();
                this.Panel1.Visible = this.Panel2.Visible = false;

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

        protected void TreeStructure_SelectedNodeChanged(object sender, EventArgs e)
        {
            string sID = this.TreeStructure.SelectedNode.Value;
            string type = Methods.getTypeOfID(sID);//
            this.showDetailInfo(sID, type);//显示产品档案相关信息
            this.showDetailHisData(sID);//显示产品详细的历史信息
        }

        private void showDetailHisData(string sID)
        {
            sqlaccess.Open();
            try
            {
                string tmpsql = " SELECT t.ProductID, t.Process, t.EmployeeID, te.Name,t.Dispatch,t.result,"
                                + " CASE WHEN t.result = 0 THEN '通过' WHEN t.result = 1 then t.Exception END AS resultstr,"
                                + " t.Exception, t.Data, t.DataID, t.BeginTime, t.EndTime"
                                +" FROM TB_ProcedureHistory AS t LEFT OUTER JOIN"
                                +" TA_Employee AS te ON t.EmployeeID = te.ID"
                                +" LEFT OUTER JOIN"
                                +" (SELECT t.EmployeeID, t.GroupID, tg.WorkDispatch"
                                +" FROM TRE_Group_Employee AS t INNER JOIN"
                                +" TA_Group AS tg ON t.GroupID = tg.ID) AS tr ON" 
                                +" t.EmployeeID = tr.EmployeeID"
                                + " WHERE (t.ProductID = '" + sID + "') order by t.productid,t.BeginTime,t.process";

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
        private void showDetailInfo(string sID, string type)
        {
            string sql = "";
            if (type != "3")
            {
                this.Panel2.Visible = false;
                this.Panel1.Visible = true;
                sql = " select t.id,t.sn,t.Modelid,t.FoundTime,case when t.ManufactureState is  null then '初始态' else t.ManufactureState end ManufactureState ,t.Remark,tm.Name from ta_product t,ta_model tm where t.modelid=tm.id and t.id='" + sID + "'";
                try
                {
                    sqlaccess.Open();
                    ds = sqlaccess.OpenQuerry(sql);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow r = ds.Tables[0].Rows[0];
                        this.lblID.Text = r["ID"].ToString();
                        this.lblModelID.Text = r["ModelID"].ToString();
                        this.lblReMark.Text = r["Remark"].ToString();
                        this.lblSN.Text = type == "1" ? r["SN"].ToString() : "--";//只显示产品的
                        this.lblName.Text = r["Name"].ToString();
                        this.lblFoundTime.Text = r["FoundTime"].ToString();
                        this.lblState.Text = type == "1" ? r["ManufactureState"].ToString() : "--";//只显示产品的 

                    }
                    else
                    {
                        this.lblID.Text = "";
                        this.lblModelID.Text = "";
                        this.lblReMark.Text = "";
                        this.lblSN.Text = "";
                        this.lblName.Text = "";
                        this.lblFoundTime.Text = "";
                        this.lblState.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Write(this, ex.Message);
                    Methods.AjaxMessageBox(this, "在查询的过程中出现异常,请再次尝试或与管理员联系");

                }
                finally
                {
                    sqlaccess.Close();
                }
            }
            else
            {
                this.Panel2.Visible = true;
                this.Panel1.Visible = false;
                sql = " select t.id,t.batch,t.Modelid,t.FoundTime,t.Remark,t.vendor,tm.Name from ta_materiel t,ta_model tm where t.modelid=tm.id and t.id='" + sID + "'";
                try
                {
                    sqlaccess.Open();
                    ds = sqlaccess.OpenQuerry(sql);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow r = ds.Tables[0].Rows[0];
                        this.lblMID.Text = r["ID"].ToString();
                        this.lblMMID.Text = r["ModelID"].ToString();
                        this.lblMName.Text = r["Name"].ToString();
                        this.lblBatch.Text = r["batch"].ToString();
                        this.lblVendor.Text = r["vendor"].ToString();
                        this.lblMFoundTime.Text = r["FoundTime"].ToString();
                        this.lblMremark.Text = r["Remark"].ToString();
                    }
                    else
                    {
                        this.lblMID.Text = "";
                        this.lblMMID.Text = "";
                        this.lblMName.Text = "";
                        this.lblBatch.Text = "";
                        this.lblVendor.Text = "";
                        this.lblMFoundTime.Text = "";
                        this.lblMremark.Text = "";
                   }
                }
                catch (Exception ex)
                {
                    LogManager.Write(this, ex.Message);
                    Methods.AjaxMessageBox(this, "在查询的过程中出现异常,请再次尝试或与管理员联系");

                }
                finally
                {
                    sqlaccess.Close();
                }
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView myrows = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (myrows["Data"].ToString().StartsWith("<?xml") || (myrows["Data"].ToString().StartsWith("<xml")) || (myrows["Data"].ToString().StartsWith("< ?xml")))
                {
                    //这里设计一个javascript function
                    string xml = "<a href=# onclick='showDetailData(\"" + myrows["dataID"].ToString() + "\")'><font color=blue>查看</font></a>";
                    e.Row.Cells[7].Text = xml;
                }
            }

        }

    }
}
