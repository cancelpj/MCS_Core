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

namespace MCSApp.Application.ProcedureManage.ProcedureFlow
{
    public partial class TestSoft : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1_Init();

            string id = "";
            if (Request.QueryString["id"] != null) id = Request.QueryString["id"].ToString();
            Label1.Text = id;
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text.Trim() == string.Empty || TextBox2.Text.Trim() == string.Empty)
            {
                MCSApp.Common.Methods.AjaxMessageBox(Page, "输入信息不完整！");
                return;
            }

            string Name = Label1.Text + "_" + TextBox1.Text;
            string Version = TextBox2.Text;

            string sql = "insert into TB_TestConfigVer (Name,Version) values ('" + Name + "','" + Version + "')";
            Database.Execute(sql);
            GridView1_Init();
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            string Name = (sender as LinkButton).CommandName;
            string sql = "delete from TB_TestConfigVer where Name='" + Name + "'";
            Database.Execute(sql);
            GridView1_Init();
        }
        

        protected void GridView1_Init()
        {
            string id="~";
            if (Request.QueryString["id"] != null) id = Request.QueryString["id"].ToString();
            
            GridView1.DataSource = Database.DataTable("select * from TB_TestConfigVer where Name like '" + id + "%'");
            //GridView1.DataKeyNames={"Name"};
            GridView1.DataBind();

        }
    }
}
