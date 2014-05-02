using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using MCSApp.Common;

namespace MCSApp
{
    public class PageBase : System.Web.UI.Page
    {
        protected override void Render(HtmlTextWriter writer)
        {
            if (!SessionUser.IsLogin)
            {
                this.Response.Write("<div align=center><font size=2 color=red>您的账号没有通过认证，您不能查看页面内容！</font>"
                    + "<a href='javascript:top.window.location.href = location.protocol+ \"//\"+ location.host+ \"/MCSApp/Default.aspx\";'>重新登录</a></div>"); 
                return;
            }
            base.Render(writer);
        }
    }
}
