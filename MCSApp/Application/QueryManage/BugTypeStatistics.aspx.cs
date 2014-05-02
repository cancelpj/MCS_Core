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
    public partial class BugTypeStatistics : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        int mysum1 = 0;
        int mysum2 = 0;
        int mysum3 = 0;
        //int mysum4 = 0;
        //int mysum5 = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "二级数据查看者" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
        }

        protected void btnquery_Click(object sender, EventArgs e)
        {

            try
            {
                sqlaccess.Open();
                string ModelID = "";
                string ModelIDSql = "";
                string whstr = "";
                if (this.RadioButton1.Checked)
                {
                    //string modelid = "";
                    //modelid = Methods.getModelFromPlanIDInProduct(this.txtcondition.Text.Trim());//由计划单号取得产品品号 在产品表里
                    //ModelIDSql = "('" + modelid + "')";
                    //直接在产品表里按计划单号找
                    whstr = " planid='"+this.txtcondition.Text.Trim()+"' ";
                    ModelIDSql = whstr;
                }
                if (this.RadioButton2.Checked)
                {
                    string modelid = this.txtcondition.Text.Trim();
                    ModelIDSql = " modelid in (select distinct id from ta_model where ModelType='1' and (id ='" + modelid + "' or name='" + modelid + "' or code='" + modelid + "'))"; //品号，品名，代号均配 符合其一即可找到产品
                }

                string tmpSql = "";
                if(this.RadioButton1.Checked)
                {
                    tmpSql = "select modelid from ta_product where ModelType='1' and planid='" + this.txtcondition.Text.Trim() + "'";
                }

                if(this.RadioButton2.Checked)
                {
                    string modelid = this.txtcondition.Text.Trim();
                    tmpSql = " select distinct id from ta_model where ModelType='1' and  (id ='" + modelid + "' or name='" + modelid + "' or code='" + modelid + "')"; //品号，品名，代号均配 符合其一即可找到产品
                }

                ds = sqlaccess.OpenQuerry(tmpSql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ModelID = ds.Tables[0].Rows[0][0].ToString();
                }

                string pname="";
                this.lblDaiHao.Text = Methods.getProductCodeFromID(ModelID,out pname);//由产品品号取得代号
                this.lblName.Text = pname;
                if (this.lblDaiHao.Text.Trim() != string.Empty)
                {
                    this.lblModelID.Text = ModelID;
                }
                else
                {
                    this.lblModelID.Text = "    ----";
                    this.lblDaiHao.Text = "    ----";
                    this.lblName.Text = "    ----";
                    this.GridView1.DataSource = null;
                    this.GridView1.DataBind();
                    return;
                }



                    tmpSql = ""
                            + " select rcd.Bug bug,rcd.bugcount,"
                            + " case when pro.allpros=0 then 0 else ROUND(rcd.bugcount*10000/pro.allpros,4) end bugperc, "
                            + " case when bug.allbugs=0 then 0 else ROUND(rcd.bugcount*10000/bug.allbugs,4) end bugrate,bug.allbugs,pro.allpros from "
                            + " /*维记录表中关联到bugPoint表中查到bugPoint名称，分类bugPoint数量*/ "
                            + " (select bt.Bug,w.bugcount from "
                            + " (select rr.BugID,count(rr.BugID) bugcount "
                            + " from TB_RepairRecord rr where productid in ( select id from ta_product where " + ModelIDSql + ") "
                            + " group by BugID) w "
                            + " left outer join ta_bugType bt on w.BugID=bt.ID"
                            + " ) rcd, "
                            + " /*总的bugID数量（符合条件）*/ "
                            + " (select count(*) allbugs from TB_RepairRecord where productid in ( select id from ta_product where " + ModelIDSql + "))bug, "
                            + " /*总的产品数量（符合条件）*/ "
                            + " (select count(*) allpros  from ta_product where " + ModelIDSql + ")pro";
                ds = sqlaccess.OpenQuerry(tmpSql);
                this.GridView1.DataSource = ds;
                this.GridView1.DataBind();
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            DataRowView myrows = (DataRowView)e.Row.DataItem;            
            if (e.Row.RowType == DataControlRowType.DataRow)
            { 
                mysum1 += Convert.ToInt32(myrows[1].ToString());
                mysum2 += Convert.ToInt32(myrows[2].ToString());
                mysum3 += Convert.ToInt32(myrows[3].ToString());
                //mysum4 = Convert.ToInt32(myrows[4].ToString());
                //mysum5 = Convert.ToInt32(myrows[5].ToString());

                e.Row.Cells[2].Text = Convert.ToString(Convert.ToDouble(myrows[2].ToString()) / 100 + "%");
                e.Row.Cells[3].Text = Convert.ToString(Convert.ToDouble(myrows[3].ToString()) / 100 + "%");
            }
            // 合计
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "合计";
                e.Row.Cells[1].Text = mysum1.ToString();
                e.Row.Cells[2].Text = mysum2.ToString();
                e.Row.Cells[3].Text = mysum3.ToString();

                e.Row.Cells[2].Text = Convert.ToString(Convert.ToDouble(mysum2) / 100) + "%";
                e.Row.Cells[3].Text = Convert.ToString(Convert.ToDouble(mysum3) / 100) + "%";
            }
            this.GridView1.ShowFooter = true;


        }
    }
}
