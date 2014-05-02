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
using FINGU.MCS;

namespace MCSApp
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //调用WebService举例
            ProcedureCtrl ctrl = new ProcedureCtrl();
            ProductTrace trace = new ProductTrace();
            string oMsg = "";
            bool ret = ctrl.SaveProcessEnd("029666666A0111080029", "包装", 0,"","",System.DateTime.Now, out oMsg);
            Response.Write(oMsg);
        }
    }
}
