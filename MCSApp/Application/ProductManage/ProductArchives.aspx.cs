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
    public partial class ProductArchives : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSaveRcd.Attributes.Add("onclick", "return confirm('您确定要将记录保存到数据库中吗？')");
            this.txtID.Focus();
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "作业员" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
            if (!IsPostBack)
            {
                ViewState["conStr"] = "";
                ds = this.getDS();
                ViewState["tmpds"] = ds;
                this.gridBind(ds);
            }
            else
            {
                int j=((DataSet)ViewState["tmpds"]).Tables[0].Rows.Count;
                for (int i = 0; i < j;i++ )
                {
                    TextBox box = (TextBox)this.GridView1.Rows[i].FindControl("txtremark");
                    ((DataSet)ViewState["tmpds"]).Tables[0].Rows[i]["remark"] = box.Text;
                }

            }
        }

        private void gridBind(DataSet ds)
        {
            if (ds != null)
            {
                this.GridView1.DataSource = ds;
                this.DataBind();
            }
        }

        private DataSet getDS()
        {
            try
            {
                sqlaccess.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ID,SN,ModelID,PlanID,remark,FoundTime,ProcedureID from TA_Product where 1<>1 " + ViewState["conStr"].ToString());//由此取得结构不显示记录

                ds = sqlaccess.OpenQuerry(sb.ToString());
                return ds;
            }
            catch (Exception ex)
            {
                LogManager.Write(this, ex.Message);
                return null;
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        protected void btnSaveRcd_Click(object sender, EventArgs e)
        {
            this.txtID.Focus();
            try 
            {               
                int j = ((DataSet)ViewState["tmpds"]).Tables[0].Rows.Count;
                if (j == 0) { Methods.AjaxMessageBox(this,"没有记录要添加到数据库");}
                sqlaccess.Open();
                sqlaccess.BeginTransaction();            

                for (int i = 0; i < j; i++)
                {
                    SqlCommand cmd = new SqlCommand();
                    DataRow r=((DataSet)ViewState["tmpds"]).Tables[0].Rows[i];
                    cmd.Parameters.AddWithValue("@p1",r["ID"]);
                    cmd.Parameters.AddWithValue("@p2", r["SN"]);
                    cmd.Parameters.AddWithValue("@p3", r["ModelID"]);
                    cmd.Parameters.AddWithValue("@p4", r["PlanID"]);
                    cmd.Parameters.AddWithValue("@p5", r["ProcedureID"]);
                    cmd.Parameters.AddWithValue("@p6", r["FoundTime"]);
                    cmd.Parameters.AddWithValue("@p7", r["Remark"]);
                    cmd.Parameters.AddWithValue("@p8", "1");//这里1表示产品


                    cmd.CommandText = "INSERT INTO TA_Product(ID, SN, ModelID, PlanID, ProcedureID, FoundTime, Remark,ModelType) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8)";

                    sqlaccess.ExecuteQuerry(cmd);

                }                   
                Methods.AjaxMessageBox(this,"数据成功保存到数据库！");
                sqlaccess.Commit();
                this.txtID.Text = ""; 

               ((DataSet)ViewState["tmpds"]).Tables[0].Rows.Clear();
               this.GridView1.DataSource = (DataSet)ViewState["tmpds"];
               this.GridView1.DataBind();
            }catch(Exception ex)
            {
                sqlaccess.Rollback();
                if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ((SqlException)ex).ErrorCode == -2146232060)
                {
                    char[] cs = { '\r', '\n' };Methods.AjaxMessageBox(this, ex.Message.Split(cs)[0].Replace('\'', '"'));
                }
                LogManager.Write(this,ex.Message);
            }finally
            {
                sqlaccess.Close();
            }


        }

        protected void btnAddRcd_Click(object sender, EventArgs e)
        {
            this.txtID.Focus();
            if (this.txtID.Text.Trim() == "")
            {
                Methods.AjaxMessageBox(this, "产品序列号不能为空！"); return;
            }

            int rc = Methods.getModelIDCount(this.txtID.Text.Trim());
            if(rc>0)
            {
                Methods.AjaxMessageBox(this, "此产品序列号已经存在！"); return;
            }
            else if (rc < 0) { Methods.AjaxMessageBox(this, "查询产品序列号时出现异常，详情请查看日志！"); return; }

            ds = (DataSet)ViewState["tmpds"];
            for (int i = 0; i < ds.Tables[0].Rows.Count;i++ )
            {
                if (this.txtID.Text.Trim() == ds.Tables[0].Rows[i]["ID"].ToString())
                {
                    Methods.AjaxMessageBox(this,"此产品序列号已经存在！"); return;
                }
            }            
            
            int Len = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MIDLenInSN"]);
            if (this.txtID.Text.Trim().Length <= Len)
            {
                    Methods.AjaxMessageBox(this,"此产品序列号长度必须"+Len+"位以上！"); return;
            }

            DataRow r = ds.Tables[0].NewRow();//将数据源添加一新行
            Guid gd = Guid.NewGuid();
            string sn = gd.ToString();
            string PlanID = "";
            int ProcedureID = -1;
            string ModelID = Methods.GetPlanPIDByEID(SessionUser.ID, out PlanID);



            string mid = this.txtID.Text.Trim().Substring(0, Len);
            //if(mid!=ModelID)
            //{
            //        Methods.AjaxMessageBox(this,"序列号对应的品号与计划中的品号不一致！"); return;
            //}
            WSProcedureCtrl.ProcedureCtrl pctrl = new ProcedureCtrl();
            if (!pctrl.CheckModel(mid, SessionUser.ID, 0))
            {
                Methods.AjaxMessageBox(this, "序列号对应的品号与计划中的品号不一致！"); return;
            }


            ProcedureID = Convert.ToInt32(Methods.GetProcIDByMID(mid, PlanID));//
            if (ProcedureID < 0)
            {
                Methods.AjaxMessageBox(this, "提示：没有对应的产品流程号！");
            }

            r["ID"] = this.txtID.Text.Trim();
            r["SN"] = sn;//GUID
            r["ModelID"] = mid;
            r["PlanID"] = PlanID;
            r["ProcedureID"] = ProcedureID;
            r["FoundTime"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd");
            r["Remark"] = "";

            ds.Tables[0].Rows.InsertAt(r, 0);
            ds.AcceptChanges();

            this.GridView1.DataSource = ds;
            this.GridView1.DataBind();

            this.txtID.Text = "";
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            ds = (DataSet)ViewState["tmpds"];
            ds.Tables[0].Rows[e.RowIndex].Delete();
            ds.Tables[0].AcceptChanges();
            this.GridView1.DataSource = ds;
            this.GridView1.DataBind();
        }
    }
}
