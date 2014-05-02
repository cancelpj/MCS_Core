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
using MCSApp.DataAccess.LogManage;
using MCSApp.DataAccess;
using FINGU.MCS;
using MCSApp.WSSecurity;

namespace MCSApp.Application.EmployeeManage
{
    public partial class ChangePassword :PageBase
    {
        WSSecurity.Security sc = new MCSApp.WSSecurity.Security();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                this.lblName.Text = SessionUser.Name;
                this.lblID.Text = SessionUser.ID;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {        

            try
            {
                string Msg="";

                if (sc.ChangePassword(SessionUser.ID, this.txtOldPsw.Text, this.txtNewPsw.Text, out Msg))
                    Methods.AjaxMessageBox(this, "密码修改成功，请牢记新密码！");
                else
                    Methods.AjaxMessageBox(this, Msg);
            }
            catch(Exception ex)
            {
                LogManager.Write(this, ex.ToString());
            }
        }
    }
}
