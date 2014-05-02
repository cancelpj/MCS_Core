using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using MCSApp.Common;

namespace MCSApp.Common
{
    public partial class ToExcelPage : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             if(!IsPostBack)
             {
                 Methods.ToExcel(this.Response, (GridView)Session["gridView"]);
                 Session["gridView"] = null;
             }
        }
    }
}
