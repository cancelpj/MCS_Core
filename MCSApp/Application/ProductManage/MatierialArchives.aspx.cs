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
    public partial class MatierialArchives : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSaveRcd.Attributes.Add("onclick", "return confirm('您确定要将记录保存到数据库中吗？')");
            this.txtSN.Focus();
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "物料管理员" };
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
                int j = ((DataSet)ViewState["tmpds"]).Tables[0].Rows.Count;
                for (int i = 0; i < j; i++)
                {
                    TextBox txtremark = (TextBox)this.GridView1.Rows[i].FindControl("txtremark");
                    TextBox txtBatch = (TextBox)this.GridView1.Rows[i].FindControl("txtBatch");
                    TextBox txtVendor = (TextBox)this.GridView1.Rows[i].FindControl("txtVendor");
                    TextBox txtEmployeeID = (TextBox)this.GridView1.Rows[i].FindControl("txtEmployeeID");

                    ((DataSet)ViewState["tmpds"]).Tables[0].Rows[i]["Batch"] = txtBatch.Text;
                    ((DataSet)ViewState["tmpds"]).Tables[0].Rows[i]["remark"] = txtremark.Text;
                    ((DataSet)ViewState["tmpds"]).Tables[0].Rows[i]["Vendor"] = txtVendor.Text;
                    ((DataSet)ViewState["tmpds"]).Tables[0].Rows[i]["EmployeeID"] = txtEmployeeID.Text;
                }
                return;

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
                sb.Append("SELECT ID,ModelID,batch,vendor,EmployeeID,remark,FoundTime from TA_Materiel where 1<>1 " + ViewState["conStr"].ToString());//由此取得结构不显示记录

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
            this.txtSN.Focus();
            try
            {
                int j = ((DataSet)ViewState["tmpds"]).Tables[0].Rows.Count;
                if (j == 0) { Methods.AjaxMessageBox(this, "没有记录要添加到数据库"); return; }
                for (int i = 0; i < j; i++)
                {
                    DataRow r = ((DataSet)ViewState["tmpds"]).Tables[0].Rows[i];
                    if (r["Batch"].ToString() == "" || r["Vendor"].ToString() == "")
                    {
                        Methods.AjaxMessageBox(this, "批次号及供应商信息不能为空！"); return;
                    }
                }

                sqlaccess.Open();
                sqlaccess.BeginTransaction();

                for (int i = 0; i < j; i++)
                {
                    SqlCommand cmd = new SqlCommand();
                    DataRow r = ((DataSet)ViewState["tmpds"]).Tables[0].Rows[i];
                    cmd.Parameters.AddWithValue("@p1", r["ID"]);
                    cmd.Parameters.AddWithValue("@p2", r["ModelID"]);
                    cmd.Parameters.AddWithValue("@p3", r["Batch"]);
                    cmd.Parameters.AddWithValue("@p4", r["Vendor"]);
                    cmd.Parameters.AddWithValue("@p5", r["EmployeeID"]);
                    cmd.Parameters.AddWithValue("@p6", r["FoundTime"]);
                    cmd.Parameters.AddWithValue("@p7", r["Remark"]);

                    cmd.CommandText = "INSERT INTO TA_Materiel(ID, ModelID, Batch, Vendor, EmployeeID, FoundTime, Remark) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7)";

                    sqlaccess.ExecuteQuerry(cmd);

                }
                Methods.AjaxMessageBox(this, "数据成功保存到数据库！");
                sqlaccess.Commit();
                ((DataSet)ViewState["tmpds"]).Tables[0].Rows.Clear();
                this.GridView1.DataSource = (DataSet)ViewState["tmpds"];
                this.GridView1.DataBind();
            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ((SqlException)ex).ErrorCode == -2146232060)
                {
                    char[] cs = { '\r', '\n' }; Methods.AjaxMessageBox(this, ex.Message.Split(cs)[0].Replace('\'', '"'));
                }
                LogManager.Write(this, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }


        }

        protected void btnAddRcd_Click(object sender, EventArgs e)
        {
            this.txtSN.Focus();
            if (this.txtSN.Text.Trim() == "")
            {
                Methods.AjaxMessageBox(this, "物料序列号不能为空！"); return;
            }

            int rc = Methods.getMatierialIDCount(this.txtSN.Text.Trim());
            if (rc > 0)
            {
                Methods.AjaxMessageBox(this, "此物料序列号已经存在！"); return;
            }
            else if (rc < 0) { Methods.AjaxMessageBox(this, "查询部件序列号时出现异常，详情请查看日志！"); return; }

            ds = (DataSet)ViewState["tmpds"];
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (this.txtSN.Text.Trim() == ds.Tables[0].Rows[i]["ID"].ToString())
                {
                    Methods.AjaxMessageBox(this, "此物料序列号已经存在！"); return;
                }
            }

            int Len = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MIDLenInSN"]);
            if (this.txtSN.Text.Trim().Length <= Len)
            {
                Methods.AjaxMessageBox(this, "此部件序列号长度必须" + Len + "位以上！"); return;
            }

            DataRow r = ds.Tables[0].NewRow();//将数据源添加一新行

            string mid = this.txtSN.Text.Trim().Substring(0, Len);
            //if(mid!=ModelID)
            //{
            //        Methods.AjaxMessageBox(this,"序列号对应的品号与计划中的品号不一致！"); return;
            //}
            //WSProcedureCtrl.ProcedureCtrl pctrl = new ProcedureCtrl();
            //if (!pctrl.CheckModel(mid, SessionUser.ID))
            //{
            //    Methods.AjaxMessageBox(this, "序列号对应的品号与计划中的品号不一致！"); return;
            //}


            r["ID"] = this.txtSN.Text.Trim();
            r["ModelID"] = mid;
            r["Batch"] = "";
            r["Vendor"] = "";
            r["EmployeeID"] = "";
            r["FoundTime"] = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd");
            r["Remark"] = "";

            ds.Tables[0].Rows.InsertAt(r, 0);
            ds.AcceptChanges();

            this.GridView1.DataSource = ds;
            this.GridView1.DataBind();

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
