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

namespace MCSApp.Application.ProductManage
{
    public partial class BindProduct : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        int Len = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MIDLenInSN"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "制造工程师" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                this.Page.Visible = false;
                return;
            }
            if(!IsPostBack)
            {
                ViewState["conStr"] = "";
            }

            this.TreeRShip.ExpandAll();
            this.TreeStructure.ExpandAll();

        }

        protected void btnAddRcd_Click(object sender, EventArgs e)
        {
            if (this.txtID.Text.Trim() == "")
            {
                Methods.AjaxMessageBox(this, "序列号不能为空！"); return;
            }

            int rc = Methods.getModelIDCount(this.txtID.Text.Trim());
            if (rc == 0)
            {
                Methods.AjaxMessageBox(this, "此序列号在数据库中不存在！"); return;
            }
            else if (rc < 0) { Methods.AjaxMessageBox(this, "查询产品序列号时出现异常，详情请查看日志！"); return; }

            int Len = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MIDLenInSN"]);
            if (this.txtID.Text.Trim().Length <= Len)
            {
                Methods.AjaxMessageBox(this, "此序列号长度必须" + Len + "位以上！"); return;
            }

            string mid = this.txtID.Text.Trim().Substring(0, Len);
            WSProcedureCtrl.ProcedureCtrl pctrl = new ProcedureCtrl();
            if (!pctrl.CheckModel(mid, SessionUser.ID, 0))
            {
                Methods.AjaxMessageBox(this, "序列号对应的品号与计划中的品号不一致！"); return;
            }

            if (Methods.getTypeOfModelID(mid) == "1")//如果是产品
            {
                if(this.TreeRShip.Nodes.Count>0)
                {
                    foreach (TreeNode rnode in this.TreeRShip.Nodes[0].ChildNodes)
                    {
                        if(rnode.Value.Substring(0, Len)==mid)
                        {
                            Methods.AjaxMessageBox(this,"在关联树状列表中已存在！"); break;
                        }
                    }                    
                }

                try
                {
                    string name = "";
                    sqlaccess.Open();
                    ds = sqlaccess.OpenQuerry("select tm.Name,t.id  from ta_structure t,ta_model tm where t.id=tm.id and t.id='" + mid + "'");
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        name = ds.Tables[0].Rows[0][0].ToString();
                        this.generTree(mid, name);
                        this.generRSTree(this.txtID.Text.Trim(), name);
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
            else
            {
                if(this.TreeStructure.Nodes.Count<0)
                {
                    Methods.AjaxMessageBox(this, "请输入产品序列号"); return;
                }
                foreach (TreeNode node in this.TreeStructure.Nodes[0].ChildNodes)
                {
                    if(node.Value==mid)
                    {
                        if (this.TreeRShip.Nodes[0].ChildNodes.Count > 0)
                        {
                            foreach (TreeNode rnode in this.TreeRShip.Nodes[0].ChildNodes)
                            {
                                if (rnode.Value.Substring(0, Len) == mid)
                                {
                                    Methods.AjaxMessageBox(this, "在关联树状列表中已存在！"); break;
                                }
                                this.TreeRShip.Nodes[0].ChildNodes.Add(new TreeNode(this.txtID.Text.Trim(), this.txtID.Text.Trim()));
                                break;
                            }
                        }
                        else
                        {
                                this.TreeRShip.Nodes[0].ChildNodes.Add(new TreeNode(this.txtID.Text.Trim(), this.txtID.Text.Trim()));
                        }

                    }
                }
            }


            this.TreeRShip.ExpandAll();
            this.TreeStructure.ExpandAll();
        }
        private void generTree(string ModelID, string Name)
        {
            try
            {
                this.TreeStructure.Nodes.Clear();
                TreeNode root = new TreeNode(Name, ModelID);
                this.TreeStructure.Nodes.Add(root);//产品根节点
                sqlaccess.Open();
                ds = sqlaccess.OpenQuerry("select t.ID,t.itemID,tm.Name,t.Amount from TA_Structure t left outer join ta_model tm on t.itemid=tm.id  where t.ID=" + ModelID);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        TreeNode node = new TreeNode(r["Name"].ToString() + "--数量:" + r["Amount"].ToString(), r["itemID"].ToString());
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
                dstmp = sqlaccess.OpenQuerry("select t.ID,t.itemID,tm.Name,t.Amount  from TA_Structure t left outer join ta_model tm on t.itemid=tm.id  where t.ID=" + node.Value);
                if (dstmp != null && dstmp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dstmp.Tables[0].Rows)
                    {
                        TreeNode nodetmp = new TreeNode(r["Name"].ToString() + "--数量:" + r["Amount"].ToString(), r["itemID"].ToString());
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

        private void generRSTree(string ID,string Name)
        {
            try
            {
                this.TreeRShip.Nodes.Clear();
                TreeNode root = new TreeNode(ID, ID);
                //root.NavigateUrl = "javascript:showDetail('" + ID + Name + "','" + ID + "')";
                this.TreeRShip.Nodes.Add(root);//产品根节点
                sqlaccess.Open();
                //到产品结构中取结构 

                ds = sqlaccess.OpenQuerry("select t.ID,t.itemID,tm.Name from TA_Relationship t left outer join ta_model tm on SUBSTRING(t.itemid,1,"+Len+")=tm.id  where t.ID='" + ID + "'");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        TreeNode node = new TreeNode( r["itemID"].ToString(), r["itemID"].ToString());
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
                        TreeNode nodetmp = new TreeNode(r["itemID"].ToString(), r["itemID"].ToString());
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

        protected void btnSaveRcd_Click(object sender, EventArgs e)
        {
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                
                if(this.TreeRShip.Nodes.Count>0)
                {
                    string delstr="delete ta_relationship where id='"+this.TreeRShip.Nodes[0].Value+"'";
                    sqlaccess.ExecuteQuerry(delstr);
                    if(this.TreeRShip.Nodes[0].ChildNodes.Count>0)
                    {
                        for (int i = 0; i < this.TreeRShip.Nodes[0].ChildNodes.Count; i++)
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Parameters.AddWithValue("@p1", this.TreeRShip.Nodes[0].Value);
                            cmd.Parameters.AddWithValue("@p2", this.TreeRShip.Nodes[0].ChildNodes[i].Value);
                            cmd.CommandText = "insert into ta_relationship (id,itemid) values (@p1,@p2)";
                            sqlaccess.ExecuteQuerry(cmd);
                        }

                    }
                }
                sqlaccess.Commit();
                Methods.AjaxMessageBox(this,"保存成功!");

            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                LogManager.Write(this, ex.Message);
                if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ((SqlException)ex).ErrorCode == -2146232060)
                {
                    char[] cs = { '\r', '\n' }; Methods.AjaxMessageBox(this, ex.Message.Split(cs)[0].Replace('\'', '"'));
                }
            }
            finally
            {
                sqlaccess.Close();
            }
        }
    }
}
