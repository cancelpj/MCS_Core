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
using System.IO;

namespace MCSApp.Application.ProcedureManage.ProcedureFlow.processes
{
    public partial class DealWithXMLData : PageBase
    {
        //DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            sqlaccess.Open();
            try
            {
                string op = Methods.GetParam(this,"op");
                string xmlToCallser=Methods.OperateXMLData(this,op);

                this.Response.ContentType = "text/xml";
                this.Response.Write(xmlToCallser);

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
