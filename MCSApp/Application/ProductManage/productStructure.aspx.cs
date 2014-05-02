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
    public partial class productStructure : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "制造工程师" };
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
            this.btnDelete.Attributes.Add("onclick", "return confirm('你确定要删除品号'+document.getElementById('txtID').value+'吗?');");
            this.TreeView1.ExpandAll();
        }

        protected void btnquery_Click(object sender, EventArgs e)
        {

            if (this.RadioButton1.Checked)
            {
                ViewState["conStr"] = " and id like '%" + txtcondition.Text.Trim() + "%'";
            }
            if (this.RadioButton2.Checked)
            {
                ViewState["conStr"] = " and name like '%" + txtcondition.Text.Trim() + "%'";
            }
            if (this.RadioButton3.Checked)
            {
                ViewState["conStr"] = " and code like '%" + txtcondition.Text.Trim() + "%'";
            }
            this.gridBind();

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            sqlaccess.Open();
            sqlaccess.BeginTransaction();
            try
            {
                TreeNode node = this.TreeView1.SelectedNode;
                if (node == null)
                {
                    Methods.AjaxMessageBox(this, "请先选择一个产品或部件节点！"); return;
                }
                string logStr = "";
                string ModelID = node.Value;
                SqlCommand cmd = new SqlCommand();

                //不许添加本级节点
                if (ModelID == this.txtID.Text.Trim())
                {
                    Methods.AjaxMessageBox(this, "请点放大镜图标选择可用于添加的下级节点!"); return;
                }
                //数量必须大于0
                int i = 0;
                try
                {
                    i = Convert.ToInt32(this.txtOutput.Text.Trim());
                }
                catch
                {
                    i = 0;
                }
                if (i <= 0)
                {
                    Methods.AjaxMessageBox(this, "数量必须大于0!"); return;
                }
                //不准重复添加

                for (i = 0; i < node.ChildNodes.Count; i++)
                {
                    if (this.txtID.Text.Trim() == node.ChildNodes[i].Value)
                    {
                        Methods.AjaxMessageBox(this, "已经包含了此节点!"); return;
                    }
                }


                cmd.Parameters.AddWithValue("@p1", ModelID);
                cmd.Parameters.AddWithValue("@p2", this.txtID.Text.Trim());
                cmd.Parameters.AddWithValue("@p3", this.txtOutput.Text.Trim());
                cmd.CommandText = "insert into ta_structure (ID,ItemID,Amount) values (@p1,@p2,@p3)";

                sqlaccess.ExecuteQuerry(cmd);
                sqlaccess.Commit();

                logStr = "insert into ta_structure (ID,ItemID,Amount) values ('{0}','{1}','{2}')";
                logStr = String.Format(logStr, ModelID, this.txtID.Text.Trim(), this.txtOutput.Text.Trim());
                logStr = "新增产品结构定义[" + ModelID + "] ## " + logStr;
                Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);
                GridView1_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                Methods.AjaxMessageBox(this, "添加操作出错！");
                LogManager.Write(this, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            sqlaccess.Open();
            sqlaccess.BeginTransaction();
            try
            {
                TreeNode node = this.TreeView1.SelectedNode;
                if (node == null || node.Depth == 0)
                {
                    Methods.AjaxMessageBox(this, "请先选择一个想要删除的部件或物料！"); return;
                }
                string logStr = "";
                string ModelID = node.Value;
                SqlCommand cmd = new SqlCommand();

                if (node.Depth == 1)//删除中间层节点 即部件节点
                {
                    bool isSingleUsed = Methods.isSingleUsed(ModelID);

                    //删除此节点，并且循环删除其子节点
                    cmd.Parameters.AddWithValue("@p1", ModelID);
                    cmd.Parameters.AddWithValue("@p2", node.Parent.Value);
                    cmd.CommandText = "delete ta_structure where ItemID=@p1 and id=@p2 ";
                    sqlaccess.ExecuteQuerry(cmd);//这里要执行一下，否则在最后执行会漏掉

                    logStr = "delete ta_structure where ItemID='{0}' and id='{1}'";
                    logStr = String.Format(logStr, ModelID, node.Parent.Value);

                    if (isSingleUsed)//如果其下有子节点 即有物料 并且此节点没有包含在别的产品下
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@p1", ModelID);
                        cmd.CommandText = "delete ta_structure where id=@p1";
                        sqlaccess.ExecuteQuerry(cmd);

                        logStr += "delete ta_structure where id='{1}'";
                        logStr = String.Format(logStr, ModelID, node.Parent.Value);
                    }

                }
                else if (node.Depth == 2)//删除最底层节点时
                {
                    cmd.Parameters.AddWithValue("@p1", ModelID);
                    cmd.Parameters.AddWithValue("@p2", node.Parent.Value);
                    cmd.CommandText = "delete ta_structure where ItemID=@p1 and id=@p2";
                    sqlaccess.ExecuteQuerry(cmd);

                    logStr = "delete ta_structure where ItemID='{0}' and id='{1}'";
                    logStr = String.Format(logStr, ModelID, node.Parent.Value);
                }

                sqlaccess.Commit();
                logStr = "删除产品结构定义[" + ModelID + "] ## " + logStr;
                Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);
                GridView1_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ((SqlException)ex).ErrorCode == -2146232060)
                {
                    Methods.AjaxMessageBox(this, "尚有其他记录与此记录关联，暂不能删除！"); return;
                }
                LogManager.Write(this, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            sqlaccess.Open();
            sqlaccess.BeginTransaction();
            try
            {
                TreeNode node = this.TreeView1.SelectedNode;
                if (node == null || node.Depth == 0)
                {
                    Methods.AjaxMessageBox(this, "请先选择一个想要修改的部件或物料！"); return;
                }
                string ModelID = node.Value;
                SqlCommand cmd = new SqlCommand();

                int i = 0;
                try
                {
                    i = Convert.ToInt32(this.txtOutput.Text.Trim());
                }
                catch
                {
                    i = 0;
                }
                if (i <= 0)
                {
                    Methods.AjaxMessageBox(this, "数量必须大于0!"); return;
                }
                cmd.Parameters.AddWithValue("@p1", node.Parent.Value);
                cmd.Parameters.AddWithValue("@p2", this.txtID.Text.Trim());
                cmd.Parameters.AddWithValue("@p3", this.txtOutput.Text.Trim());
                cmd.CommandText = "update ta_structure set Amount=@p3 where ID=@p1 and itemID=@p2";

                sqlaccess.ExecuteQuerry(cmd);
                sqlaccess.Commit();

                string logStr = "update ta_structure set Amount='{0}' where ID='{1}' and itemID='{2}'";
                logStr = String.Format(logStr, this.txtOutput.Text.Trim(), node.Parent.Value, this.txtID.Text.Trim());
                logStr = "修改产品结构定义[" + ModelID + "] ## " + logStr;
                Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), logStr);
                GridView1_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                Methods.AjaxMessageBox(this, "修改操作出错！");
                LogManager.Write(this, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }

        }
        private void gridBind()
        {
            try
            {
                sqlaccess.Open();
                ds = sqlaccess.OpenQuerry("select *  from TA_Model where ModelType=1" + ViewState["conStr"].ToString());
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
            string ModelID = ((Label)this.GridView1.SelectedRow.FindControl("lblid")).Text.Trim();
            string Name = ((Label)this.GridView1.SelectedRow.FindControl("lblName")).Text.Trim();
            this.generTree(ModelID, Name);
            this.TreeView1.ExpandAll();
            ShowButton("");
        }

        private void generTree(string ModelID, string Name)
        {
            try
            {
                this.TreeView1.Nodes.Clear();
                TreeNode root = new TreeNode(Name + "(" + ModelID + ")", ModelID);
                root.ToolTip = "产品";
                this.TreeView1.Nodes.Add(root);//产品根节点
                sqlaccess.Open();
                ds = sqlaccess.OpenQuerry("select t.ID,t.itemID,tm.Name,tm.ModelType,t.Amount from TA_Structure t left outer join ta_model tm on t.itemid=tm.id  where t.ID='" + ModelID + "'");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        TreeNode node = new TreeNode(r["Name"].ToString() + "(" + r["itemID"].ToString() + ")--数量:" + r["Amount"].ToString(), r["itemID"].ToString());
                        switch ((int)r["ModelType"])
                        {
                            case 1:
                                node.ToolTip = "产品";
                                break;
                            case 2:
                                node.ToolTip = "部件";
                                break;
                            case 3:
                                node.ToolTip = "物料";
                                break;
                            default:
                                node.ToolTip = "";
                                break;

                        }
                        this.AddNode(node);
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

        private void AddNode(TreeNode node)
        {
            try
            {
                DataSet dstmp = null;
                sqlaccess.Open();
                dstmp = sqlaccess.OpenQuerry("select t.ID,t.itemID,tm.Name,tm.ModelType,t.Amount from TA_Structure t left outer join ta_model tm on t.itemid=tm.id  where t.ID='" + node.Value + "'");
                if (dstmp != null && dstmp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dstmp.Tables[0].Rows)
                    {
                        TreeNode nodetmp = new TreeNode(r["Name"].ToString() + "(" + r["itemID"].ToString() + ")--数量:" + r["Amount"].ToString(), r["itemID"].ToString());
                        switch ((int)r["ModelType"])
                        {
                            case 1:
                                nodetmp.ToolTip = "产品";
                                break;
                            case 2:
                                nodetmp.ToolTip = "部件";
                                break;
                            case 3:
                                nodetmp.ToolTip = "物料";
                                break;
                            default:
                                nodetmp.ToolTip = "";
                                break;

                        }
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

        private void ShowButton(string ModelType)
        {
            switch (ModelType)
            {
                case "产品":
                    btnAdd.Visible = true;
                    btnDelete.Visible = false;
                    btnEdit.Visible = false;
                    Session["__ModelType"] = 2;//用于选择部件用
                    this.img1.Visible = true;
                    this.txtOutput.Text = "1";//根节点选中 数量置1
                    this.txtOutput.Enabled = false;
                    break;
                case "部件":
                    btnAdd.Visible = true;
                    btnDelete.Visible = true;
                    btnEdit.Visible = true;
                    Session["__ModelType"] = 3;//用于选择物料用
                    this.img1.Visible = true;
                    this.txtOutput.Enabled = true;
                    break;
                case "物料":
                    btnAdd.Visible = false;
                    btnDelete.Visible = true;
                    btnEdit.Visible = true;
                    this.img1.Visible = false;
                    this.txtOutput.Enabled = true;
                    break;
                default:
                    btnAdd.Visible = false;
                    btnDelete.Visible = false;
                    btnEdit.Visible = false;
                    this.img1.Visible = false;
                    this.txtOutput.Enabled = false;
                    break;
            }
        }

        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            this.txtID.Text = "";
            this.txtOutput.Text = "";

            ShowButton(this.TreeView1.SelectedNode.ToolTip);

            if (this.TreeView1.SelectedNode.Text.IndexOf(':') > 0)
            {
                this.txtOutput.Text = this.TreeView1.SelectedNode.Text.Split(':')[1].ToString();
            }
            this.txtID.Text = this.TreeView1.SelectedNode.Value;

        }

    }
}
