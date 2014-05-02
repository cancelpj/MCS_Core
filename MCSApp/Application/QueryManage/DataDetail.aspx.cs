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
using System.Xml;

namespace MCSApp.Application.QueryManage
{
    public partial class DataDetail : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string DataID = Methods.GetParam(this, "Dataid");
                //string Process = Methods.GetParam(this,"process");
                //string result = Methods.GetParam(this, "result");
                if (DataID == "")
                {
                    Methods.AjaxMessageBox(this, "无法查看异常数据"); return;
                }
                this.gridBind(DataID);
            }
        }

        private void gridBind(string DataID)
        {
            sqlaccess.Open();
            try
            {

                string tmpsql = " select data from TB_ProcedureHistory where dataid='" + DataID + "'";
                //                string tmpsql = " select data from TB_ProcedureHistory where productid='" + Productid + "' and process='" + Process + "' and result='" + result + "'";

                ds = sqlaccess.OpenQuerry(tmpsql);

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    StringReader stream = null;
                    XmlTextReader reader = null;
                    try
                    {
                        string xmlData = ds.Tables[0].Rows[0][0].ToString();

                        DataSet xmlDS = new DataSet();
                        stream = new StringReader(xmlData);
                        //从stream装载到XmlTextReader 
                        reader = new XmlTextReader(stream);
                        xmlDS.ReadXml(reader);

                        DataTable dtfieldname = xmlDS.Tables["fieldname"];
                        DataTable dt = xmlDS.Tables["data"];
                        foreach (DataColumn column in dt.Columns)
                        {
                            string aaa = column.ColumnName;
                            try
                            {
                                column.ColumnName = dtfieldname.Rows[0][column.ColumnName].ToString();
                            }
                            catch (Exception ex)
                            {
                                LogManager.Write(this, ex.Message);

                            }
                        }
                        this.GridView1.DataSource = dt;
                        this.GridView1.DataBind();
                        this.GridView1.Visible = true;

                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (reader != null) reader.Close();
                    }

                }
                else this.GridView1.Visible = false;



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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int j = 0; j < e.Row.Cells.Count; j++)
            {
                e.Row.Cells[j].Wrap = false;//不拐行
                e.Row.Cells[j].Style.Add("word-break", "keep-all");//保持整体
            }
        }

    }
}
