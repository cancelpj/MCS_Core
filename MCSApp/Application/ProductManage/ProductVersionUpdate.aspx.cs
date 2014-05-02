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
    public partial class ProductVersionUpdate : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "维修员" };
            //
            this.btnSaveRcd.Attributes.Add("onclick", "return confirm('你确定要升级产品版本吗?')");

            if (!IsPostBack)
            {

            }
        }

        protected void btnSaveRcd_Click(object sender, EventArgs e)
        {
            int Len = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MIDLenInSN"]);
            if (this.txtOldModelID.Text.Trim().Length != Len || this.txtNewModelID.Text.Trim().Length != Len)
            {
                Methods.AjaxMessageBox(this, "输入的产品品号不合法");
                return;
            }

            if (this.txtOldModelID.Text.Trim() == this.txtNewModelID.Text.Trim())
            {
                Methods.AjaxMessageBox(this, "新品号不应与旧品号相同！"); return;
            }

            string mid1 = this.txtOldModelID.Text.Trim().Substring(0, Len - 3);
            string mid2 = this.txtNewModelID.Text.Trim().Substring(0, Len - 3);
            if (mid1 != mid2)
            {
                Methods.AjaxMessageBox(this, "新品号与旧品号不一致！");
                return;
            }

            try
            {
                DataTable dt = (DataTable)ViewState["dt"];
                sqlaccess.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT ID FROM TA_Model where ID ='" + this.txtOldModelID.Text.Trim() + "' AND (ModelType = 1)");

                ds = sqlaccess.OpenQuerry(sb.ToString());
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {

                }
                else
                {
                    Methods.AjaxMessageBox(this, "输入的旧产品品号在数据库中不存在！");
                    return;
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

            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO TA_Model(ID, Name, Code, CustomerID, ModelType) " +
                             "SELECT @NewModelID AS id, Name, Code, CustomerID, ModelType FROM TA_Model " +
                             "WHERE (ID = @OldModelID) AND (ModelType = 1)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@NewModelID", this.txtNewModelID.Text.Trim());
                cmd.Parameters.AddWithValue("@OldModelID", this.txtOldModelID.Text.Trim());

                sqlaccess.ExecuteQuerry(cmd);

                cmd.CommandText = "INSERT INTO TA_Structure(ID, ItemID, Amount) " +
                        "SELECT @NewModelID AS id, ItemID, Amount FROM TA_Structure WHERE (ID = @OldModelID)";
                sqlaccess.ExecuteQuerry(cmd);

                sqlaccess.Commit();
                this.txtNewModelID.Text = "";

                Methods.AjaxMessageBox(this, "已创建新产品品号，并继承了旧产品的结构表，可以在“产品结构定义”中做相应修改");
            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                Methods.AjaxMessageBox(this, "保存的过程中出现异常!");
                LogManager.Write(this, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }
        }


        protected void btnAddRecord_Click(object sender, EventArgs e)
        {

        }

    }
}
