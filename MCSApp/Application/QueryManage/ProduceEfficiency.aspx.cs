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


namespace MCSApp.Application.QueryManage
{
    public partial class ProduceEfficiency : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "一级数据查看者" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                //return;
            }
            if(!IsPostBack)
            {
                ViewState["conStr"] = "";//数据库where 部分用的字符串
                this.dt_begin_UseBox.Value = System.DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                this.dt_end_UseBox.Value = System.DateTime.Now.ToString("yyyy-MM-dd");
                //this.dt_begin_UseBox.Disabled = true;
                //this.dt_end_UseBox.Disabled = true; 
            }

        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string bstr = "";
            string estr = "";
            string whstr = "";
            string datestr = "";
            DateTime endTime;

            try
            {
                endTime = DateTime.Parse(this.dt_end_UseBox.Value);
            }
            catch (Exception)
            {
                Methods.AjaxMessageBox(this, "输入的日期格式不正确！"); return;
            }

            datestr = "endTime";

            if (!this.dt_begin_UseBox.Disabled)
            {
                bstr = " and " + datestr + ">='" + this.dt_begin_UseBox.Value + "'";
            }
            if (!this.dt_end_UseBox.Disabled)
            {
                estr = " and " + datestr + "<='" + endTime.AddDays(1).ToString("yyyy-MM-dd") + "'";
            }

            if (this.txtID.Text.Trim() != string.Empty)
            {
                if (this.RadioButton1.Checked)
                {
                    string modelid =this.txtID.Text.Trim();
                    ViewState["conStr"] = " and productid in (select id from ta_product where modelid='" + modelid + "')" + bstr + estr;
                }
                if (this.RadioButton2.Checked)
                {
                    ViewState["conStr"] = " and productid in (select id from ta_product where modelid in (select id from ta_model where name ='" + this.txtID.Text.Trim() + "'))" + bstr + estr;
                }
                if (this.RadioButton3.Checked)
                {
                    ViewState["conStr"] = " and productid in (select id from ta_product where modelid in (select id from ta_model where code ='" + this.txtID.Text.Trim() + "'))" + bstr + estr;
                }
            }
            else
                ViewState["conStr"] = whstr + bstr + estr;
            this.GridBind();
        }

        private void GridBind()
        {
            sqlaccess.Open();
            try
            {
                string tmpsql = "";

                if (this.RadioButton1.Checked)
                {
                    string modelid =this.txtID.Text.Trim();
                    tmpsql = "select * from ta_model where id='"+modelid+"'";
                }
                if (this.RadioButton2.Checked)
                {
                    tmpsql = "select * from ta_model where name='"+this.txtID.Text.Trim()+"'";
                }
                if (this.RadioButton3.Checked)
                {
                    tmpsql = "select * from ta_model where code='"+this.txtID.Text.Trim()+"'";
                }
                ds = sqlaccess.OpenQuerry(tmpsql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    this.lblModelID.Text = ds.Tables[0].Rows[0]["ID"].ToString();
                    this.lblModelName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    this.lblModelCode.Text = ds.Tables[0].Rows[0]["Code"].ToString();
                }
                else
                {
                    this.lblModelID.Text = "";
                    this.lblModelName.Text = "";
                    this.lblModelCode.Text = "";
                    this.lblDayAvgOutput.Text = "";
                    this.lblWholeOutput.Text = "";
                    this.GridView1.DataSource = null;
                    this.GridView1.DataBind();
                    return;
                }
                    

                int days = Methods.daysDiff(this.dt_begin_UseBox.Value.Trim(),this.dt_end_UseBox.Value.Trim());
                tmpsql = "select count(DISTINCT ProductID),count(DISTINCT ProductID)/(" + days + " + 0.0) from TB_ProcedureHistory where process='入库' and endtime is not null " + ViewState["conStr"].ToString();
                ds = sqlaccess.OpenQuerry(tmpsql);
                if(ds!=null&&ds.Tables[0].Rows.Count>0)
                {
                    this.lblWholeOutput.Text = ds.Tables[0].Rows[0][0].ToString();
                    this.lblDayAvgOutput.Text = string.Format("{0:F3}",ds.Tables[0].Rows[0][1]);
                }
                       tmpsql = " select Process+'平均耗时' as process,case when COUNT(*)=0 then 0 else SUM(DATEDIFF(s, BeginTime, EndTime))/((60*COUNT(*))+0.00) end avuseTime from TB_ProcedureHistory where 1=1 and endtime is not null " + ViewState["conStr"].ToString()
                              +" group by Process";//DATEDIFF(s, BeginTime, EndTime) s为秒级 先查出来 在界面上展示体现小数点后两位 

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

        protected void ckCancelTime_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ckCancelTime.Checked)
            {
                this.dt_begin_UseBox.Disabled = false;
                this.dt_end_UseBox.Disabled = false;
            }
            else
            {
                this.dt_begin_UseBox.Disabled = true;
                this.dt_end_UseBox.Disabled = true;
            }
        }
    }
}
