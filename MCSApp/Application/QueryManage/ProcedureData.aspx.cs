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
    public partial class ProcedureData : PageBase
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
                //this.GridBind();
            }
        }

        protected void GridBind()
        {
            sqlaccess.Open();
            try
            {
                string tmpsql = "";

                if (this.RadioButton1.Checked)
                {
                    tmpsql = "select t.id,t.sn,tm.name,tm.code from ta_product t,ta_model tm where t.modelid=tm.id and t.id ='" + this.txtcondition.Text.Trim() + "'";
                }
                if (this.RadioButton2.Checked)
                {
                    tmpsql = "select t.id,t.sn,tm.name,tm.code from ta_product t,ta_model tm where  t.modelid=tm.id and  t.sn ='" + this.txtcondition.Text.Trim() + "'";
                }

                ds = sqlaccess.OpenQuerry(tmpsql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    this.lblPID.Text = ds.Tables[0].Rows[0]["ID"].ToString();
                    this.lblSN.Text = ds.Tables[0].Rows[0]["SN"].ToString();
                    this.lblName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    this.lblDaiHao.Text = ds.Tables[0].Rows[0]["Code"].ToString();
                }
                else
                {
                    this.lblPID.Text = "";
                    this.lblSN.Text = "";
                    this.lblName.Text = "";
                    this.lblDaiHao.Text = "";
                    this.GridView1.Visible = false;
                    return;
                }
                this.GridView1.Visible = true;
                tmpsql = "  select t.productid,t.Process,t.DataID,th.Data,th.result,case when th.result=0 then '通过' when th.result=1 then th.exception end resultstr "
                                + "  from TB_ProcedureState t,TB_ProcedureHistory th"
                                + "  where t.productid=th.productid and t.process=th.process and t.dataid=th.dataid  " + ViewState["conStr"].ToString()
                                + " order by th.begintime ";

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

        protected void btnquery_Click(object sender, EventArgs e)
        {
            if (this.txtcondition.Text.Trim() != string.Empty)
            {

                if (this.RadioButton1.Checked)
                {
                    ViewState["conStr"] = " and t.productid='" + this.txtcondition.Text.Trim() + "'";
                }
                if (this.RadioButton2.Checked)
                {
                    ViewState["conStr"] = " and t.productid in (select id from ta_product where sn='" + this.txtcondition.Text.Trim() + "')";//产品条码需要唯一
                }
            }

            this.GridBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView myrows = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (myrows["Data"].ToString().StartsWith("<?xml") || (myrows["Data"].ToString().StartsWith("<xml")) || (myrows["Data"].ToString().StartsWith("< ?xml")) || (myrows["Data"].ToString().StartsWith("<NewDataSet>")))
                {
                    //这里设计一个javascript function
                    string xml = "<a href=# onclick='showDetailData(\"" + myrows["dataID"].ToString() + "\")'><font color=blue>查看</font></a>";
                    e.Row.Cells[3].Text = xml;
                }
            }
        }
    }
}
