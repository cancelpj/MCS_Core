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
    public partial class GroupMaterial : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        int Len = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MIDLenInSN"]);
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
            if (!IsPostBack)
            {
                ViewState["conStr"] = "";
                GridBind();
                if (this.GridView1.Rows.Count > 0)
                {
                    this.GridView1.SelectedIndex = 0;
                }

            }
            this.btnSave.Attributes.Add("onclick", "return confirm('你确定要保存操作结果到数据库吗?');");
            this.btnDelete.Attributes.Add("onclick", "return confirm('你确定要删除选定的物料吗?');");
        }
        /// <summary>
        /// gridview1绑定用
        /// </summary>
        private void GridBind()
        {
            try
            {
                sqlaccess.Open();
                StringBuilder sb = new StringBuilder();
                //sb.Append("SELECT ID, Name,LeaderID,planID FROM TA_Group" + ViewState["conStr"].ToString());//在html中隐了查找，这里因为没地方设置viewstate 所以注掉了
                //string tmpgid = "";
                //if (this.GridView1.Rows.Count != 0 && this.GridView1.SelectedIndex != -1)
                //{
                //    tmpgid = this.GridView1.DataKeys[this.GridView1.SelectedIndex].Value.ToString();
                //}

                sb.Append("SELECT A.ID, A.Name, A.LeaderID, R.PlanID FROM TA_Group A, TRE_Group_Plan R where A.ID=R.GroupID and A.LeaderID='" + SessionUser.ID + "'");
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
        protected void btnOK_Click(object sender, EventArgs e)
        {
            this.dealFirstInput();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            TreeNode node = this.TreeView1.SelectedNode;

            if (node != null && node.Depth == 1)
            {
                if (node.Text.IndexOf('[') != -1)
                {
                    node.Text = node.Text.Split('[')[0].ToString();
                    node.Target = "";
                }
            }
            else
            {
                Methods.AjaxMessageBox(this, "没有选中要删除的物料!"); return;
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.TreeView1.Nodes.Count == 0)
            {
                Methods.AjaxMessageBox(this, "无数据要保存!"); return;
            }
            sqlaccess.Open();
            sqlaccess.BeginTransaction();
            SqlCommand cmd = new SqlCommand();
            try
            {
                string Groupid = "";
                //DataSet tmpds= Methods.getInforBySql("select id from ta_group where LeaderID='"+SessionUser.ID+"'");//?会不会有计划不同时 出现重复记录呢

                //if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                //{
                //    Groupid=tmpds.Tables[0].Rows[0][0].ToString();
                //}
                if (this.GridView1.Rows.Count != 0 && this.GridView1.SelectedIndex != -1)
                {
                    Groupid = this.GridView1.DataKeys[this.GridView1.SelectedIndex].Value.ToString();
                }
                else
                {
                    Methods.AjaxMessageBox(this, "请选择相应的班组!"); return;
                }

                string ModelID = "";
                if (this.TreeView1.Nodes.Count > 0)
                {
                    ModelID = this.TreeView1.Nodes[0].Value;
                }
                if (this.TreeView1.Nodes.Count > 0)
                {
                    cmd.Parameters.AddWithValue("@p1", Groupid);
                    cmd.Parameters.AddWithValue("@p2", ModelID);

                    cmd.CommandText = "delete TB_GroupMateriel where GroupID=@p1 and ModelID=@p2";
                    sqlaccess.ExecuteQuerry(cmd);

                    TreeNode root = this.TreeView1.Nodes[0];
                    foreach (TreeNode node in root.ChildNodes)
                    {
                        if (node.Target != "")
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@p1", Groupid);
                            cmd.Parameters.AddWithValue("@p2", ModelID);
                            cmd.Parameters.AddWithValue("@p3", node.Target);//取值

                            cmd.CommandText = "insert into TB_GroupMateriel (GroupID,ModelID,MaterielID) values (@p1,@p2,@p3)";
                            sqlaccess.ExecuteQuerry(cmd);
                        }
                    }
                }

                sqlaccess.Commit();
                //btnOK_Click(null,null);//在树上体现变化
                //让树消失更合理些 能体现变化
                this.TreeView1.Nodes.Clear();
                this.txtMaterialCode.Text = "";
                this.txtModelID.Text = "";

            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                Methods.AjaxMessageBox(this, "在操作数据时出现异常!");
                LogManager.Write(this, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        private void generTree(string ModelID, string Name)
        {
            try
            {
                this.TreeView1.Nodes.Clear();
                TreeNode root = new TreeNode(Name + "(" + ModelID + ")", ModelID);
                this.TreeView1.Nodes.Add(root);//产品根节点
                sqlaccess.Open();

                //ds = sqlaccess.OpenQuerry("select t.ID,t.itemID,tm.Name from TA_Structure t left outer join ta_model tm on t.itemid=tm.id  where tm.ModelType=3 and t.ID='" + ModelID + "'");//3为物料
                ds = sqlaccess.OpenQuerry("select t.ID,t.itemID,tm.Name from TA_Structure t left outer join ta_model tm on t.itemid=tm.id  where t.ID='" + ModelID + "'");//3为物料
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        TreeNode node = new TreeNode(r["Name"].ToString() + "(" + r["itemID"].ToString() + ")", r["itemID"].ToString());
                        this.AddNode(node, ModelID);
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

        protected void btnOKforM_Click(object sender, EventArgs e)
        {
            this.dealSecondInput();
        }

        private void dealSecondInput()
        {
            if (this.TreeView1.Nodes.Count == 0)
            {
                Methods.AjaxMessageBox(this, "请先输入产品品号"); return;
            }
            //string sql = "select t.Modelid from ta_materiel t where t.id='" + txtMaterialCode.Text.Trim() + "'";
            //string sql = "select t.id from ta_model t where t.id=SUBSTRING('" + txtMaterialCode.Text.Trim() + "',1," + Len + ") and ModelType = 3";
            string sql = "select t.id from ta_model t where t.id=SUBSTRING('" + txtMaterialCode.Text.Trim() + "',1," + Len + ")";
            DataSet myds = Methods.getInforBySql(sql);

            if (myds != null && myds.Tables[0].Rows.Count > 0)
            {
                DataRow myrow = myds.Tables[0].Rows[0];
                foreach (TreeNode node in this.TreeView1.Nodes[0].ChildNodes)
                {
                    if (node.Value == myrow["id"].ToString())
                    {
                        //for (int j = 0; j < node.ChildNodes.Count; j++)
                        //{
                        //if (node.ChildNodes[j].Value == txtMaterialCode.Text.Trim())
                        //{
                        //Methods.AjaxMessageBox(this, "已经包含此节点"); return;
                        //}
                        //}

                        //node.ChildNodes.Add(new TreeNode(txtMaterialCode.Text.Trim(), txtMaterialCode.Text.Trim()));
                        if (node.Text.IndexOf('[') != -1)
                        {
                            node.Text = node.Text.Split('[')[0].ToString() + "[" + txtMaterialCode.Text.Trim() + "]";
                        }
                        else
                        {
                            node.Text = node.Text + "[" + txtMaterialCode.Text.Trim() + "]";
                        }
                        node.Target = txtMaterialCode.Text.Trim();//为了取值方便 这里当序列号用 
                        break;

                    }
                }
            }
            else
            {
                Methods.AjaxMessageBox(this, "通过此物料条码无法查到对应的物料品号!"); return;
            }
        }

        private void AddNode(TreeNode node, string Modelid)
        {
            try
            {
                string Groupid = "";
                if (this.GridView1.Rows.Count != 0 && this.GridView1.SelectedIndex != -1)
                {
                    Groupid = this.GridView1.DataKeys[this.GridView1.SelectedIndex].Value.ToString();
                }

                DataSet dstmp = null;
                sqlaccess.Open();
                //dstmp = sqlaccess.OpenQuerry("select t.MaterielID,t.ModelID from TB_GroupMateriel t, TA_Materiel tm where t.MaterielID=tm.id and tm.ModelID='" + node.Value + "' and t.groupid='" + Groupid + "' and t.modelid='" + Modelid + "'");//班组长所在的组的，查找品号与物料号
                dstmp = sqlaccess.OpenQuerry("select t.MaterielID,t.ModelID from TB_GroupMateriel t where SUBSTRING(t.MaterielID,1," + Len + ")='" + node.Value + "' and t.groupid='" + Groupid + "' and t.modelid='" + Modelid + "'");//班组长所在的组的，查找品号与物料号
                if (dstmp != null && dstmp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dstmp.Tables[0].Rows)
                    {
                        //TreeNode nodetmp = new TreeNode(r["MaterielID"].ToString());
                        //node.ChildNodes.Add(nodetmp);//添加物料
                        node.Text = node.Text + "[" + r["MaterielID"].ToString() + "]";
                        node.Target = r["MaterielID"].ToString(); //为了取值方便 这里当序列号用                       
                    }
                }

            }
            catch (Exception ex)
            {
                LogManager.Write(this, ex.Message);
                //node = new TreeNode();
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        protected void txtModelID_TextChanged(object sender, EventArgs e)
        {
            this.dealFirstInput();
        }

        private void dealFirstInput()
        {
            string sql = "select t.id,t.name from ta_model t where t.id='" + this.txtModelID.Text.Trim() + "'";//这里没有考虑是否为此班组的品号
            DataSet myds = Methods.getInforBySql(sql);

            if (myds != null && myds.Tables[0].Rows.Count > 0)
            {
                DataRow myrow = myds.Tables[0].Rows[0];
                this.generTree(myrow["id"].ToString(), myrow["name"].ToString());
            }
            else
            {
                Methods.AjaxMessageBox(this, "此品号未定义!"); this.TreeView1.Nodes.Clear(); return;
            }
            this.TreeView1.ExpandAll();
        }

        protected void txtMaterialCode_TextChanged(object sender, EventArgs e)
        {
            dealSecondInput();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtMaterialCode.Text = this.txtModelID.Text = "";
            this.TreeView1.Nodes.Clear();
        }
    }
}
