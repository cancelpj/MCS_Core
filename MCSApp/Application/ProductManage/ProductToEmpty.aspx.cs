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
    public partial class ProductToEmpty : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        int Len = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MIDLenInSN"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "维修员" };
            string outStr;
            this.btnChaiChu.Attributes.Add("onclick","return confirm('确定要拆空选中的产品吗？')");

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
            if (!IsPostBack)
            {
                ViewState["conStr"] = "";
            }
            this.TreeView1.ExpandAll();

        }

        private void generTree(string ID, string SN,string ModelName )
        {
            try
            {
                this.TreeView1.Nodes.Clear();
                TreeNode root = new TreeNode("生产序列号:"+ID+" 条码:"+SN+" 品名:"+ModelName , ID);
                root.NavigateUrl="javascript:showDetail('"+ID+"--"+SN+"','"+ID+"')";
                this.TreeView1.Nodes.Add(root);//产品根节点
                sqlaccess.Open();
                //到产品结构中取结构 

                ds = sqlaccess.OpenQuerry("select t.ID,t.itemID,tm.Name from TA_Relationship t inner join (select mdl.name,tp.id  from ta_model mdl,ta_product tp where tp.modelid=mdl.id and tp.modeltype=mdl.modeltype and mdl.modeltype='2') tm on t.itemid=tm.id where t.id='"+ID+"'");//Modeltype=2 只显示部件
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in ds.Tables[0].Rows)
                    {
                        TreeNode node = new TreeNode("生产序列号:" + r["itemID"].ToString() + " 品名：" + r["Name"].ToString(), r["itemID"].ToString());//这里控制了只显示了部件的序列号
                        node.NavigateUrl = "javascript:showDetail('" +r["itemID"].ToString()+r["Name"].ToString() + "','" + r["itemID"].ToString() + "')";
                        this.AddNode(node);//由于这里控制了只能添加部件，故这addnode方法只添加物料
                        root.ChildNodes.Add(node);//添加部件
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
                                                                                                   /*下面代码将物料与品号表关联，便于取品名*/
                dstmp = sqlaccess.OpenQuerry("select t.ID,t.itemID,tm.Name  from TA_Relationship t,( select tl.id,tl.modelid,tmm.name from ta_materiel tl,ta_model tmm where tl.modelid=tmm.id and tmm.modeltype=3 ) tm where t.itemid=tm.id and t.ID='" + node.Value + "'");
                if (dstmp != null && dstmp.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow r in dstmp.Tables[0].Rows)
                    {
                        TreeNode nodetmp = new TreeNode(r["Name"].ToString(), r["itemID"].ToString());
                        nodetmp.NavigateUrl = "javascript:showDetail('" + r["itemID"].ToString()+r["Name"].ToString() + "','" + r["itemID"].ToString() + "')";
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

            string sql = "select t.id,t.sn,tm.name modelName from ta_product t,ta_model tm where t.modelid=tm.id and tm.modeltype='1' and t.id='" + this.txtcondition.Text.Trim() + "'";//序列号 品号 新的改进 只要求显示产品 这里要显示品名，所以还是无法不关联ta_model

            DataSet myds = Methods.getInforBySql(sql);

            if (myds != null && myds.Tables[0].Rows.Count > 0)
            {
                DataRow myrow = myds.Tables[0].Rows[0];
                this.generTree(myrow["id"].ToString(), myrow["sn"].ToString(), myrow["modelName"].ToString());
            }
            else
            {
                Methods.AjaxMessageBox(this, "未找到此产品!"); this.TreeView1.Nodes.Clear(); return;
            }
            this.TreeView1.ExpandAll(); 
        }

        protected void btnChaiChu_Click(object sender, EventArgs e)
        {
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                Guid gid = Guid.NewGuid();
                if (this.TreeView1.Nodes==null||this.TreeView1.Nodes.Count == 0)
                {
                    Methods.AjaxMessageBox(this, "数据库中不存在您要拆除的产品!"); return;
                }
                string ID = this.TreeView1.Nodes[0].Value ;// ((Label)this.GridView1.SelectedRow.FindControl("lblid")).Text.Trim();//生产序列号
                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.AddWithValue("@p1",ID);
                cmd.Parameters.AddWithValue("@p2",gid.ToString());
                cmd.CommandText = " update ta_product set ManufactureState='报废' /*报废*/ ,sn=@p2 where ID=@p1 ";
                sqlaccess.ExecuteQuerry(cmd);

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@p1",ID);

                cmd.CommandText = " delete from TA_Relationship where ID=@p1 ";
                sqlaccess.ExecuteQuerry(cmd);
                sqlaccess.Commit();

                string logstr = string.Format(" update ta_product where ManufactureState='报废' /*报废*/ ,sn={1} where ID='{0}' ", ID, gid.ToString());
                logstr = logstr + string.Format("delete from TA_Relationship where ID='{0}' ", ID);
                string strLog = "腔体拆空[" + ID.ToString() + "] ";
                Methods.WriteOprationLog(SessionUser.ID, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), strLog + logstr);
                Methods.AjaxMessageBox(this,"拆除成功！");
                this.TreeView1.Nodes.Clear();

            }
            catch (Exception ex)
            {
                LogManager.Write(this, ex.Message);
                sqlaccess.Rollback();
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
