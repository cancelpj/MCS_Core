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

namespace MCSApp.Application.ProcedureManage
{
    public partial class CustomerReturn : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "作业员" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //没有实现
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                SqlCommand cmd = new SqlCommand();

                cmd.Parameters.AddWithValue("@p1",this.lblProductID.Text);
                cmd.Parameters.AddWithValue("@p2", txtPlanIDNew.Text);
                cmd.Parameters.AddWithValue("@p3", ViewState["procedureid"].ToString());//取得流程号 由品号
                cmd.Parameters.AddWithValue("@p4", this.txtRPInfo.Text.Trim() + System.DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒") + "，客退登记，原计划单号：" + this.lblOldPlanID.Text);
                cmd.CommandText = "update ta_product set planid=@p2,ProcedureID=@p3,remark=remark+@p4 where id=@p1";

                sqlaccess.ExecuteQuerry(cmd);

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@p1", this.lblProductID.Text);
                //cmd.Parameters.AddWithValue("@p2",this.lblModelID.Text);
                cmd.CommandText = "delete TB_ProcedureState where ProductID=@p1";

                sqlaccess.ExecuteQuerry(cmd);

                sqlaccess.Commit();   
         
                this.lblModelID.Text = "----";
                this.lblOldPlanID.Text = "----";
                this.lblProductID.Text = "----";
                this.lblSN.Text = "----";
                this.txtRPInfo.Text = "";
                this.txtID.Text = "";

                ctrlState(false);


            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                LogManager.Write("", ex.Message);
                Methods.AjaxMessageBox(this,"向数据库中插入记录的时候出现异常!");
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        private void ctrlState(bool b)
        {
             this.txtRPInfo.Enabled = b;
             this.btnSave.Enabled = b;

        }

        protected void btnAddRcd_Click(object sender, EventArgs e)
        {
            string tmpPid = this.txtID.Text.Trim();
            
            if (!Methods.PlanISActive(tmpPid))
            {
                Methods.AjaxMessageBox(this, "请先选择当前生产计划！"); return;
            }

            if (this.RadioButton1.Checked)
            {
                tmpPid = Methods.getProductIDFromPSN(this.txtID.Text.Trim());//由SN找产品序列号 
            }
            try
            {
                sqlaccess.Open();
                ds = sqlaccess.OpenQuerry("select * from ta_product where id='"+tmpPid+"'");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    //取得当前生产品号
                    string thisModelid = Methods.GetModelIDByPlanID(txtPlanIDNew.Text);
                    
                    DataRow r = ds.Tables[0].Rows[0];
                    if(r["ModelID"].ToString()!=thisModelid)
                    {
                        Methods.AjaxMessageBox(this, "产品档案中的品号与计划的品号不一致！"); return;
                    }
                    ViewState["procedureid"] = Methods.getProcedureIDByPlanID(txtPlanIDNew.Text, r["ModelID"].ToString());//新的计划号取得新的流程号

                    
                    this.lblProductID.Text = r["ID"].ToString();
                    this.lblOldPlanID.Text = r["PlanID"].ToString();
                    this.lblModelID.Text = r["ModelID"].ToString();
                    this.lblSN.Text = r["SN"].ToString();
                    this.txtRPInfo.Text = r["Remark"].ToString();
                    this.ctrlState(true);
                }
                else
                {
                    this.ctrlState(false);
                    this.lblModelID.Text = "----";
                    this.lblOldPlanID.Text = "----";
                    this.lblProductID.Text = "----";
                    this.lblSN.Text = "----";
                    this.txtRPInfo.Text = "----";
                }
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }

        }
    }
}
