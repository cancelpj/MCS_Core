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
    public partial class ModelDetail : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            string sID = Methods.GetParam(this,"id");
            string type=Methods.getTypeOfID(sID);
            this.showDetailInfo(sID,type);
        }
        private void showDetailInfo(string sID,string type)
        {
            string sql = "";
            if (type != "3")
            {
                this.Panel2.Visible = false;
                this.Panel1.Visible = true;
                sql = " select t.id,t.sn,t.Modelid,t.FoundTime,case when t.ManufactureState is  null then '初始态' else t.ManufactureState end ManufactureState ,t.Remark,tm.Name from ta_product t,ta_model tm where t.modelid=tm.id and t.id='" + sID + "'";
                try
                {
                    sqlaccess.Open();
                    ds = sqlaccess.OpenQuerry(sql);
                    if(ds!=null&&ds.Tables[0].Rows.Count>0)
                    {
                        DataRow r = ds.Tables[0].Rows[0];
                        this.lblID.Text=r["ID"].ToString();
                        this.lblModelID.Text=r["ModelID"].ToString();

                        this.lblSN.Text=type=="1"?r["SN"].ToString():"--";//只显示产品的


                        this.lblName.Text=r["Name"].ToString();
                        this.lblFoundTime.Text = r["FoundTime"].ToString();
                        this.lblState.Text = type == "1" ? r["ManufactureState"].ToString() : "--";//只显示产品的 
                        this.lblReMark.Text=r["Remark"].ToString();

                    }
                }
                catch (Exception ex)
                {
                    LogManager.Write(this,ex.Message);
                    Methods.AjaxMessageBox(this, "在查询的过程中出现异常,请再次尝试或与管理员联系");

                }
                finally
                {
                    sqlaccess.Close();
                }
            }
            else
            {
                this.Panel2.Visible = true;
                this.Panel1.Visible = false;
                sql = " select t.id,t.batch,t.Modelid,t.FoundTime,t.Remark,t.vendor,tm.Name from ta_materiel t,ta_model tm where t.modelid=tm.id and t.id='" + sID + "'";                
                try
                {
                    sqlaccess.Open();
                    ds = sqlaccess.OpenQuerry(sql);
                    if(ds!=null&&ds.Tables[0].Rows.Count>0)
                    {
                        DataRow r = ds.Tables[0].Rows[0];
                        this.lblMID.Text=r["ID"].ToString();
                        this.lblMMID.Text=r["ModelID"].ToString();
                        this.lblMName.Text=r["Name"].ToString();
                        this.lblBatch.Text = r["batch"].ToString();
                        this.lblVendor.Text = r["vendor"].ToString();
                        this.lblMFoundTime.Text = r["FoundTime"].ToString();
                        this.lblMremark.Text=r["Remark"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    LogManager.Write(this,ex.Message);
                    Methods.AjaxMessageBox(this, "在查询的过程中出现异常,请再次尝试或与管理员联系");

                }
                finally
                {
                    sqlaccess.Close();
                }
            }
        }
    }
}
