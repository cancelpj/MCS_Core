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
    public partial class InputProductSN : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnSaveRcd.Attributes.Add("onclick", "return confirm('您确定要将记录保存到数据库中吗？')");

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
                this.lblCode.Text = "请输入产品序列号";
                this.txtSN.Visible = false;
                this.txtID.Focus();

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
                    TextBox box = (TextBox)this.GridView1.Rows[i].FindControl("txtSN");
                    ((DataSet)ViewState["tmpds"]).Tables[0].Rows[i]["SN"] = box.Text;
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
            try
            {
                int j = ((DataSet)ViewState["tmpds"]).Tables[0].Rows.Count;
                if (j == 0) { Methods.AjaxMessageBox(this, "没有记录要添加到数据库"); }
                sqlaccess.Open();
                sqlaccess.BeginTransaction();

                for (int i = 0; i < j; i++)
                {
                    SqlCommand cmd = new SqlCommand();
                    DataRow r = ((DataSet)ViewState["tmpds"]).Tables[0].Rows[i];
                    cmd.Parameters.AddWithValue("@p1", r["ID"]);
                    cmd.Parameters.AddWithValue("@p2", r["SN"]);

                    cmd.CommandText = "update TA_Product set SN=@p2 where ID=@p1";

                    sqlaccess.ExecuteQuerry(cmd);

                }
                Methods.AjaxMessageBox(this, "数据成功保存到数据库！");
                sqlaccess.Commit();
                ((DataSet)ViewState["tmpds"]).Tables[0].Rows.Clear();
                this.GridView1.DataSource = (DataSet)ViewState["tmpds"];
                this.GridView1.DataBind();

                this.btnAddRcd.Text = "验证产品序列号";
                this.lblCode.Text = "扫描产品序列号";
                this.txtID.Visible = true;
                this.txtID.Text = "";
                this.txtSN.Visible = false;

                this.txtID.Focus();
            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") && ((SqlException)ex).ErrorCode == -2146232060)
                {
                    char[] cs = { '\r', '\n' };Methods.AjaxMessageBox(this, ex.Message.Split(cs)[0].Replace('\'', '"'));
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
            if (this.btnAddRcd.Text == "验证产品序列号")
            {
                if (this.txtID.Text.Trim() == "")
                {
                    Methods.AjaxMessageBox(this, "产品序列号不能为空！"); return;
                }

                int rc = Methods.getModelIDCount(this.txtID.Text.Trim());
                if (rc == 0)
                {
                    Methods.AjaxMessageBox(this, "此产品序列号在数据库中不存在！"); return;
                }
                else if (rc < 0) { Methods.AjaxMessageBox(this, "查询产品序列号时出现异常，详情请查看日志！"); return; }

                //ds = (DataSet)ViewState["tmpds"];
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    if (this.txtID.Text.Trim() == ds.Tables[0].Rows[i]["ID"].ToString())
                //    {
                //        Methods.AjaxMessageBox(this, "此产品序列号的记录已经存在！"); return;
                //    }
                //}

                int Len = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MIDLenInSN"]);
                if (this.txtID.Text.Trim().Length <= Len)
                {
                    Methods.AjaxMessageBox(this, "此产品序列号长度必须" + Len + "位以上！"); return;
                }


                string PlanID = "";

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
                ViewState["PlanID"] = PlanID;
                ViewState["mid"]= mid; 

                this.btnAddRcd.Text = "添加产品条码";
                this.lblCode.Text = "扫描产品条码";
                this.txtID.Visible = false;
                this.txtSN.Visible = true;
                this.txtSN.Text = "";
                this.txtSN.Focus();
            }
            else
            {
                ds = (DataSet)ViewState["tmpds"];

                DataRow r = ds.Tables[0].NewRow();//将数据源添加一新行
                r["ID"] = this.txtID.Text.Trim();
                r["ModelID"] = ViewState["mid"];
                r["PlanID"] = ViewState["PlanID"];
                r["SN"] = this.txtSN.Text.Trim();

                ds.Tables[0].Rows.InsertAt(r, 0);
                ds.AcceptChanges();

                this.GridView1.DataSource = ds;
                this.GridView1.DataBind();

                this.btnAddRcd.Text = "验证产品序列号";
                this.lblCode.Text = "扫描产品序列号";
                this.txtID.Visible = true;
                this.txtID.Text = "";
                this.txtSN.Visible = false;

                this.txtID.Focus();
            }
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
