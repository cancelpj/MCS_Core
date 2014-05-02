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
    public partial class SingleProductInfo : PageBase
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
                return;
            }
            if (!IsPostBack)
            {
                ViewState["conStr"] = "";//数据库where 部分用的字符串
                this.GridBind();
            }
        }

        protected void GridBind()
        {
            sqlaccess.Open();
            try
            {
                string tmpsql = "";

                if (this.RadioButton3.Checked)
                {
                    tmpsql = "select t.sn,tm.name,tm.code from ta_product t,ta_model tm where t.modelid = tm.id and t.id ='" + this.txtID.Text.Trim() + "'";
                }
                if (this.RadioButton4.Checked)
                {
                    tmpsql = "select t.sn,tm.name,tm.code from ta_product t,ta_model tm where t.modelid = tm.id and t.sn ='" + this.txtID.Text.Trim() + "'";
                }

                ds = sqlaccess.OpenQuerry(tmpsql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    //this.lblID.Text = ViewState["conStr"].ToString();
                    this.lblSN.Text = ds.Tables[0].Rows[0]["SN"].ToString();
                    this.lblModelName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    this.lblCode.Text = ds.Tables[0].Rows[0]["Code"].ToString();
                }
                else
                {
                    this.lblID.Text = "";
                    this.lblSN.Text = "";
                    this.lblModelName.Text = "";
                    this.lblCode.Text = "";
                    this.GridView1.Visible = false;
                    return;
                }
                this.GridView1.Visible = true;
                tmpsql = "select t.productid,t.Process,tdu.Name EmpName,t.BeginTime,t.EndTime"
                    + ",DATEDIFF(s, t.BeginTime, t.EndTime)/(60+0.00) usetime,t.Dispatch,t.result"
                    + ",case when t.result=0 then '通过' when t.result=1 then t.Exception end resultstr when t.result=2 then '通过',t.data,t.dataID"
                    + "  from TB_ProcedureHistory t,ta_employee tdu"
                    + "  where t.employeeid=tdu.id   " + ViewState["conStr"].ToString()
                    + "  order by t.begintime desc";

                ds = sqlaccess.OpenQuerry(tmpsql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    this.lblID.Text = ds.Tables[0].Rows[0]["productid"].ToString();
                }
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

        protected void btnQuery_Click(object sender, EventArgs e)
        {

            if (this.txtID.Text.Trim() != string.Empty)
            {

                if (this.RadioButton3.Checked)
                {
                    ViewState["conStr"] = " and productid='" + this.txtID.Text.Trim() + "'";
                }
                if (this.RadioButton4.Checked)
                {
                    ViewState["conStr"] = " and productid in (select id from ta_product where sn='" + this.txtID.Text.Trim() + "')";
                }
            }

            this.GridBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView myrows = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                double dt = myrows.Row.IsNull("usetime") ? -1 : Convert.ToDouble(myrows["usetime"]);
                e.Row.Cells[6].Text = dt < 0 ? "----" : dt.ToString("F02");
                if (myrows["Data"].ToString().StartsWith("<?xml") || (myrows["Data"].ToString().StartsWith("<xml")) || (myrows["Data"].ToString().StartsWith("< ?xml")) || (myrows["Data"].ToString().StartsWith("<NewDataSet>")))
                {
                    //这里设计一个javascript function
                    string xml = "<a href=# onclick='showDetailData(\"" + myrows["dataID"].ToString() + "\")'><font color=blue>查看</font></a>";
                    e.Row.Cells[8].Text = xml;
                }
            }
        }
    }
}

