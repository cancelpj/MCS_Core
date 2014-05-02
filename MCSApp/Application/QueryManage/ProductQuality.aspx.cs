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
    public partial class ProductQuality : PageBase
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
                this.dt_begin_UseBox.Value = System.DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                this.dt_end_UseBox.Value = System.DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            sqlaccess.Open();
            try
            {
                string tmpsql = " select t.ID,t.ModelID,t.output,t.orderid,t.plantype,t.remark,t.FoundTime,t.CloseTime,t.state,tm.name,tm.code from ta_plan t left outer join ta_model tm on t.modelid=tm.id where tm.modeltype=1 and t.ID='"+this.txtPlanHao.Text.Trim()+"'";

                ds = sqlaccess.OpenQuerry(tmpsql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow r = ds.Tables[0].Rows[0];
                    this.lblPlanID.Text = r["ID"].ToString();
                    this.lblOutput.Text = r["output"].ToString();
                    this.lblName.Text = r["name"].ToString();
                    this.lblModelID.Text = r["ModelID"].ToString();
                    this.lblDaiHao.Text = r["code"].ToString();
                    this.lblBeginTime.Text = r["FoundTime"].ToString();
                    this.lblCloseTime.Text = r["closetime"].ToString();
                    //返修的记录数
                    tmpsql = "select count(t.productid) from tb_procedureHistory t where t.Process='返修' and t.begintime>='" + this.dt_begin_UseBox.Value + "' and t.endtime<='" + this.dt_end_UseBox.Value + " 23:59:59 ' and t.productid in (select id from ta_product where modelid in (select Modelid from ta_plan where id ='" + this.txtPlanHao.Text.Trim() + "'))";
                    ds = sqlaccess.OpenQuerry(tmpsql);
                    int repairCount = 0;
                    if(ds!=null&&ds.Tables[0].Rows.Count>0)
                    {
                        repairCount=Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    }
                    this.lblRepairCount.Text = repairCount.ToString();
                    //返修的台数
                    tmpsql = "select count( distinct(t.productid)) from tb_procedureHistory t where t.Process='返修' and t.begintime>='" + this.dt_begin_UseBox.Value + "' and t.endtime<='" + this.dt_end_UseBox.Value + " 23:59:59 ' and t.productid in (select id from ta_product where modelid in (select Modelid from ta_plan where id ='" + this.txtPlanHao.Text.Trim() + "'))";
                    ds = sqlaccess.OpenQuerry(tmpsql);
                    int repairTaiShu = 0;
                    if(ds!=null&&ds.Tables[0].Rows.Count>0)
                    {
                        repairTaiShu = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    }
                    this.lblRepairTaiShu.Text = repairTaiShu.ToString();

                    //合格数
                    tmpsql = "select count(t.productid) from tb_procedureHistory t where t.Process='入库' and t.endtime is not null and t.begintime>='" + this.dt_begin_UseBox.Value + "' and t.endtime<='" + this.dt_end_UseBox.Value + " 23:59:59 ' and t.productid in (select id from ta_product where planid = '" + this.txtPlanHao.Text.Trim() + "')";
                    ds = sqlaccess.OpenQuerry(tmpsql);
                    int okCount = 0;
                    if(ds!=null&&ds.Tables[0].Rows.Count>0)
                    {
                        okCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    }
                    this.lblOKCount.Text = okCount.ToString();

                    //合格率
                    if (this.lblOutput.Text.Trim() != "" && this.lblOutput.Text.Trim() != "0")
                    {
                        double okRate = Convert.ToDouble(this.lblOKCount.Text) / Convert.ToDouble(this.lblOutput.Text);
                        this.lblOKRate.Text = okRate.ToString();//保有两位小数
                    }
                    else
                    {
                        this.lblOKRate.Text = "0";
                    }


                    //一次合格数
                    tmpsql = "select count(t.productid) from tb_procedureHistory t where t.Process='入库' and t.endtime is not null and t.begintime>='" + this.dt_begin_UseBox.Value + "' and t.endtime<='" + this.dt_end_UseBox.Value + " 23:59:59 ' and t.productid in  (select id from ta_product where planid ='" + this.txtPlanHao.Text.Trim() + "')"
                             + " and t.productid not in ( select tt.productid from tb_procedureHistory tt where tt.Process='返修' and tt.begintime>='" + this.dt_begin_UseBox.Value + "' and tt.endtime<='" + this.dt_end_UseBox.Value + " 23:59:59 ' and tt.productid in (select id from ta_product where planid ='" + this.txtPlanHao.Text.Trim() + "'))";
                    ds = sqlaccess.OpenQuerry(tmpsql);
                    int OnceokCount = 0;
                    if(ds!=null&&ds.Tables[0].Rows.Count>0)
                    {
                        OnceokCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    }
                    this.lblOnceOKCount.Text = OnceokCount.ToString();

                    //一次合格率
                    if (this.lblOutput.Text.Trim() != "" && this.lblOutput.Text.Trim() != "0")
                    {
                        double OnceokRate = Convert.ToDouble(this.lblOnceOKCount.Text) / Convert.ToDouble(this.lblOutput.Text);
                        this.lblOnceOKRate.Text = OnceokRate.ToString();//保有两位小数
                    }
                    else
                    {
                        this.lblOnceOKRate.Text = "0";
                    }


                }
                else
                {
                    this.lblPlanID.Text = "--";
                    this.lblOutput.Text = "--";
                    this.lblName.Text = "--";
                    this.lblModelID.Text = "--";
                    this.lblDaiHao.Text = "--";
                    this.lblBeginTime.Text = "--";
                    this.lblCloseTime.Text = "--";

                    this.lblOKCount.Text= "--";
                    this.lblOKRate.Text = "--";
                    this.lblOnceOKCount.Text = "--";
                    this.lblRepairCount.Text = "--";
                    this.lblRepairTaiShu.Text = "--";
                    this.lblOnceOKRate.Text = "--";

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
    }
}
