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

namespace MCSApp.Application.ProductManage
{
    public partial class ProductTrace : PageBase
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
                //this.gridBind();
            }
            this.TreeView1.ExpandAll();

        }

        private void gridBind()
        {
            try
            {
                sqlaccess.Open();
                //SqlCommand cmd = new SqlCommand();
                //cmd.Parameters.AddWithValue("@p1","'%"+this.txtcondition.Text.Trim()+"%'");
                //cmd.CommandText = "select t.*,tm.name  from TA_Product t left outer join ta_model tm on t.ModelID=tm.ID " + ViewState["conStr"].ToString();
                string sql = "select t.*,tm.name  from TA_Product t left outer join ta_model tm on t.ModelID=tm.ID " + ViewState["conStr"].ToString();//从产品表里查产品，然后在界面上点击的时候结合结构表 构建产品列表
                ds = sqlaccess.OpenQuerry(sql + " and tm.modeltype=1");//表示只查看产品的信息
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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.GridView1.PageIndex = e.NewPageIndex;
            this.gridBind();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ID = ((Label)this.GridView1.SelectedRow.FindControl("lblid")).Text.Trim();//生产序列号
            string ModelID = ((Label)this.GridView1.SelectedRow.FindControl("lblModelid")).Text.Trim();//品号
            string Name = ((Label)this.GridView1.SelectedRow.FindControl("lblName")).Text.Trim();//品名 来自品号表
            this.generTree(ID, Name);//生产序列号生成树
            this.TreeView1.ExpandAll();
        }

        private void generTree(string ID, string Name)
        {
            try
            {
                this.TreeView1.Nodes.Clear();
                TreeNode root = new TreeNode("品名:" + Name + " 序列号" + ID, ID);
                root.NavigateUrl = "javascript:showDetail('品名: " + Name + " 序列号:" + ID + "','" + ID + "')";
                this.TreeView1.Nodes.Add(root);//产品根节点
                this.TreeView1.Visible = true;//配合查询按钮 每次查询的时候要清除树上的东东
                sqlaccess.Open();
                //到产品结构中取结构 

                ds = sqlaccess.OpenQuerry("select t.ID,t.itemID,tm.Name from TA_Relationship t left outer join ta_model tm on SUBSTRING(t.itemid,1," + Len + ")=tm.id  where t.ID='" + ID + "'");
                //部件 物料都要显示
                //string sqltmp = "select t.ID,t.itemID,tm.Name from TA_Relationship t inner join (select mdl.name,tp.id  from ta_model mdl,ta_product tp where tp.modelid=mdl.id and tp.modeltype=mdl.modeltype and mdl.modeltype='2') tm on t.itemid=tm.id where t.id='" + ID + "'"
                //                 + " union select t.ID,t.itemID,tm.Name from TA_Relationship t,( select tl.id,tl.modelid,tmm.name from ta_materiel tl,ta_model tmm where tl.modelid=tmm.id and tmm.modeltype=3 ) tm where t.itemid=tm.id and t.ID='" + ID + "'";
                //ds = sqlaccess.OpenQuerry(sqltmp);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        TreeNode node = new TreeNode("品名:" + r["Name"].ToString() + " 序列号：" + r["itemID"].ToString(), r["itemID"].ToString());
                        node.NavigateUrl = "javascript:showDetail('品名：" + r["Name"].ToString() + " 序列号：" + r["itemID"].ToString() + "','" + r["itemID"].ToString() + "')";
                        this.AddNode(node);
                        root.ChildNodes.Add(node);//添加物料
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

        private void AddNode(TreeNode node)
        {
            try
            {
                DataSet dstmp = null;
                sqlaccess.Open();
                dstmp = sqlaccess.OpenQuerry("select t.ID,t.itemID,tm.Name  from TA_Relationship t left outer join ta_model tm on SUBSTRING(t.itemid,1," + Len + ")=tm.id  where t.ID='" + node.Value + "'");
                /*下面代码将物料与品号表关联，便于取品名*/
                //dstmp = sqlaccess.OpenQuerry("select t.ID,t.itemID,tm.Name  from TA_Relationship t,( select tl.id,tl.modelid,tmm.name from ta_materiel tl,ta_model tmm where tl.modelid=tmm.id and tmm.modeltype=3 ) tm where t.itemid=tm.id and t.ID='" + node.Value + "'");


                if (dstmp != null && dstmp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dstmp.Tables[0].Rows)
                    {
                        TreeNode nodetmp = new TreeNode("品名:" + r["Name"].ToString() + " 序列号：" + r["itemID"].ToString(), r["itemID"].ToString());
                        nodetmp.NavigateUrl = "javascript:showDetail('品名：" + r["Name"].ToString() + " 序列号：" + r["itemID"].ToString() + "','" + r["itemID"].ToString() + "')";

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

        protected void btnquery_Click(object sender, EventArgs e)
        {
            //清理树上的结点
            this.TreeView1.Visible = false;
            this.GridView1.SelectedIndex = -1;

            if (this.rdoPID.Checked)
            {
                ViewState["conStr"] = " where t.id like '%" + this.txtcondition.Text.Trim() + "%'";
            }
            if (this.rdoPSN.Checked)
            {
                ViewState["conStr"] = " where t.sn like '%" + this.txtcondition.Text.Trim() + "%'";
            }
            if (this.rdoCID.Checked)
            {
                ViewState["conStr"] = " where t.id in (select id from TA_Relationship where itemID like '%" + this.txtcondition.Text.Trim() + "%')";
            }
            if (this.rdoMID.Checked)
            {
                ViewState["conStr"] = " where t.id in (select id from TA_Relationship where itemID like '%" + this.txtcondition.Text.Trim() + "%')" //产品下面的物料 或 产品下的部件下的物料
                + " or t.id in ( select ID from TA_Relationship where itemID in (select id from TA_Relationship where itemID like '%" + this.txtcondition.Text.Trim() + "%'))";
            }
            if (this.rdoPlanID.Checked)
            {
                ViewState["conStr"] = " where t.planid = '" + this.txtcondition.Text.Trim() + "'";
            }
            this.gridBind();
        }
    }
}
