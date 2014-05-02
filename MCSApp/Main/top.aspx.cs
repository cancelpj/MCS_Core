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
using MCSApp.Common;

namespace MCSApp.Main
{
    public partial class top : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lbtQuit.Attributes.Add("onclick", "return confirm('你确定要安全退出吗?')");
            this.lblUserName.Text = "[ 欢迎"+SessionUser.Name + " ]    ";
        }

        protected void lbtQuit_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Methods.Write(this, "top.window.location='../Default.aspx'\n ");
        }
    }
}
