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
    public partial class BugPointStatistics : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        int mysum1 = 0;
        int mysum2 = 0;
        int mysum3 = 0;
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

                //string whstr = "";
                //if (this.RadioButton1.Checked)
                //{
                //    //string modelid = "";
                //    //modelid = Methods.getModelFromPlanIDInProduct(this.txtcondition.Text.Trim());//由计划单号取得产品品号 在产品表里
                //    //ModelIDSql = "('" + modelid + "')";
                //    //直接在产品表里按计划单号找
                //    //whstr = " planid='" + this.txtcondition.Text.Trim() + "' ";
                //    //ModelIDSql = whstr;
                //}
                if (this.RadioButton2.Checked)
                {
                    string modelid = this.txtcondition.Text.Trim();
                    ModelIDSql = "modelid in (select distinct id from ta_model where ModelType='1' and  (id ='" + modelid + "' or name='" + modelid + "' or code='" + modelid + "'))"; //品号，品名，代号均配 符合其一即可找到产品
                }

                string tmpSql = "";
                if (this.RadioButton1.Checked)
                {
                    tmpSql = "select modelid from ta_product where ModelType='1' and planid='" + this.txtcondition.Text.Trim() + "'";
                }

                if (this.RadioButton2.Checked)
                {
                    string modelid = this.txtcondition.Text.Trim();
                    tmpSql = " select distinct id from ta_model where  ModelType='1' and  (id ='" + modelid + "' or name='" + modelid + "' or code='" + modelid + "')"; //品号，品名，代号均配 符合其一即可找到产品
                }

                ds = sqlaccess.OpenQuerry(tmpSql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    ModelID = ds.Tables[0].Rows[0][0].ToString();
                }


                string pname;
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

                if (this.RadioButton2.Checked)
                {
                     tmpSql = ""
                            + " select rcd.BugPointCode bug,rcd.bugcount," 
                            +" case when pro.allpros=0 then 0 else ROUND(rcd.bugcount*10000/pro.allpros,4) end bugperc, "
                            +" case when bug.allbugs=0 then 0 else ROUND(rcd.bugcount*10000/bug.allbugs,4) end bugrate,bug.allbugs,pro.allpros from "
                            +" /*维记录表中关联到bugPoint表中查到bugPoint名称，分类bugPoint数量*/ "
                            + " (SELECT w.BugPointCode, w.bugcount "
                            +" FROM (SELECT  ModelID, BugPointCode, COUNT(BugPointCode) " //这里按modelid与bugpointcode唯一确一条记录 即这里分类是按类型与bugpoint统计
                            +" AS bugcount "
                            +"  FROM (SELECT t.ProductID, t.BugPointCode, tp.ModelID "
                            +" FROM TB_RepairRecord AS t INNER JOIN "
                            +"  TA_Product AS tp ON t.ProductID = tp.ID) AS tpp "
                            +"  GROUP BY  ModelID, BugPointCode) AS w "
                            + "  where w." + ModelIDSql //这里非常重要 w.要注意
                            +" ) rcd, "
                            +" /*总的bugPoint数量（符合条件）*/ "
                            + " (select count(*) allbugs from TB_RepairRecord where productid in ( select id from ta_product where " + ModelIDSql + "))bug, "
                            +" /*总的产品数量（符合条件）*/ "
                            + " (select count(*) allpros  from ta_product where " + ModelIDSql + ")pro";                    
                }

                if (this.RadioButton1.Checked)
                {
                    tmpSql = ""
                           + " select rcd.BugPointCode bug,rcd.bugcount,"
                           + " case when pro.allpros=0 then 0 else ROUND(rcd.bugcount*10000/pro.allpros,4) end bugperc, "
                           + " case when bug.allbugs=0 then 0 else ROUND(rcd.bugcount*10000/bug.allbugs,4) end bugrate,bug.allbugs,pro.allpros from "
                           + " /*维记录表中关联到bugPoint表中查到bugPoint名称，分类bugPoint数量*/ "
                           + " (SELECT w.BugPointCode, w.bugcount "
                           + " FROM (SELECT  ModelID, BugPointCode, COUNT(BugPointCode) " //这里按modelid与bugpointcode唯一确一条记录 即这里分类是按类型与bugpoint统计
                           + " AS bugcount "
                           + "  FROM (SELECT t.ProductID, t.BugPointCode, tp.ModelID "
                           + " FROM TB_RepairRecord AS t INNER JOIN "
                           + "  TA_Product AS tp ON t.ProductID = tp.ID where tp.planid='"+txtcondition.Text.Trim()+"') AS tpp "
                           + "  GROUP BY  ModelID, BugPointCode) AS w ) rcd, "
                           + " /*总的bugPoint数量（符合条件）*/ "
                           + " (select count(*) allbugs from TB_RepairRecord where productid in ( select id from ta_product where planid= '" + txtcondition.Text.Trim() + "'))bug, "
                           + " /*总的产品数量（符合条件）*/ "
                           + " (select count(*) allpros  from ta_product where planid= '" + txtcondition.Text.Trim() + "')pro";
                }

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

                e.Row.Cells[2].Text = Convert.ToString(Convert.ToDouble(myrows[2].ToString()) / 100 + "%");
                e.Row.Cells[3].Text = Convert.ToString(Convert.ToDouble(myrows[3].ToString()) / 100 + "%");
            }
            // 合计
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "合计";
                e.Row.Cells[1].Text = mysum1.ToString();
                e.Row.Cells[2].Text = mysum2.ToString();
                e.Row.Cells[3].Text = mysum2.ToString();

                //e.Row.Cells[2].Text = Convert.ToString(Convert.ToDouble(e.Row.Cells[2].Text) / Convert.ToDouble(e.Row.Cells[4].Text) / 100) + "%";
                //e.Row.Cells[3].Text = Convert.ToString(Convert.ToDouble(e.Row.Cells[3].Text) / Convert.ToDouble(e.Row.Cells[5].Text) / 100) + "%";
                e.Row.Cells[2].Text = Convert.ToString(Convert.ToDouble(mysum2) / 100) + "%";
                e.Row.Cells[3].Text = Convert.ToString(Convert.ToDouble(mysum3) / 100) + "%";
            }
            this.GridView1.ShowFooter = true;

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }
    }
}
