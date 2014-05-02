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

namespace MCSApp
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //调用WebService举例
            WSSecurity.Security sec = new WSSecurity.Security();
            string a = sec.HelloWorld();
            
            WSProcedureCtrl.ProcedureCtrl ctrl = new WSProcedureCtrl.ProcedureCtrl();
            string oMsg = "";
            bool ret = ctrl.CheckProcedure("8032000101", "装配后", 0, out oMsg);
            Response.Write(oMsg);
        }
    }
}
