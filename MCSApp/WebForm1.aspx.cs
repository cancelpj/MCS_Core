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
using System.Collections.Generic;
using MCSApp.Common;
using System.Xml;

namespace MCSApp
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string tmp = Database.DataCode("select ProcessConfig from dbo.TA_Procedure,TC_PlanProcedure where ProcedureID=TA_Procedure.ID AND TC_PlanProcedure.PlanID='2'");

            XmlTextReader reader = new XmlTextReader(tmp, XmlNodeType.Document, null);
            ds.ReadXml(reader);
            GridView1.DataSource = ds.Tables[0];
            GridView1.DataBind();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //WSProcedureCtrl.ProcedureCtrl c = new MCSApp.WSProcedureCtrl.ProcedureCtrl();
            //WSProductTrace.ProductTrace P = new MCSApp.WSProductTrace.ProductTrace();

           // Response.Write(c.CheckModel("029650583a0110040030".Substring(0,9), "00001"));
           // Response.Write(c.CheckModelByBarCode("029650583a0110040031", "00001"));
            //string msg;


            //Response.Write(c.CheckProcedure("029650583a0110040033", string.Empty, 1, out msg) + "<br>" + msg);


            //Response.Write(P.SaveProduct("00001", "029650583a0110040030", "029650583", "", out msg));

            //Response.Write(c.SaveProcessBegin("029650583a0110040030", "装配", "00001", DateTime.Today, out msg) + "<br>" + msg);

            //Response.Write(c.SaveProcessEnd("029650583a0110040030", "装配", 0,"","", DateTime.Today, out msg) + "<br>" + msg);

            Response.Write(FINGU.MCS.Encrypt.DecryptString("Z+zGd9P/aExq2MJZsL4kFA=="));
        }
    }
}
