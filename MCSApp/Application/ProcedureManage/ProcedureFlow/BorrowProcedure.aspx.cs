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
    public partial class BorrowProcedure : PageBase
    {
        SqlAccess sqlaccess = new SqlAccess();
        int Len = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["MIDLenInSN"]);
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "制造工程师" };
            string outStr;
            this.btnquery.Attributes.Add("onclick", "return confirm('确定要借用选中的流程吗？')");

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
            if (!IsPostBack)
            {
                ViewState["ProcedureID"] = Methods.GetParam(this, "ProcedureID");
            }

        }

        protected void btnquery_Click(object sender, EventArgs e)
        {

            string sql = "SELECT ProcessGraph FROM TA_Procedure WHERE Name = '" + this.txtcondition.Text.Trim() + "'";

            DataSet myds = Methods.getInforBySql(sql);

            if (myds != null && myds.Tables[0].Rows.Count > 0)
            {
                DataRow myrow = myds.Tables[0].Rows[0];
                string Procedure = myrow["ProcessGraph"].ToString();

                SqlCommand cmd = new SqlCommand();
                cmd.Parameters.AddWithValue("@p1", Procedure);
                cmd.Parameters.AddWithValue("@p2", ViewState["ProcedureID"]);
                cmd.CommandText = "UPDATE TA_Procedure SET ProcessGraph = @p1 WHERE (ID = @p2)";

                sqlaccess.Open();
                sqlaccess.ExecuteQuerryNOTransaction(cmd);
                sqlaccess.Close();
            }
            else
            {
                Methods.AjaxMessageBox(this, "未找到此工艺流程!"); 
                return;
            }
            Methods.Write(this, "window.parent.closeit();");

        }

    }
}
