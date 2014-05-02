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
using System.Xml;

namespace MCSApp.Application.QueryManage
{
    public class ProcessData
    {
        public string Process;
        public int LineNum;
        public int FinishNum;
    }

    public partial class ProduceBoard : PageBase
    {
        DataSet ds = null;
        SqlAccess sqlaccess = new SqlAccess();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "一级数据查看者" };
            string outStr;

            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }
            if (!IsPostBack)
            {
                this.GridBind();
            }

        }

        private void GridBind()
        {
            sqlaccess.Open();
            try
            {
                string tmpsql = "";
                tmpsql = " select t.id planid,t.modelid,tm.name modelName,tm.code from ta_plan t,ta_model tm where t.modelid = tm.id and t.state= 2 order by t.FoundTime desc ";

                ds = sqlaccess.OpenQuerry(tmpsql);
                this.GridView1.DataSource = ds;
                this.GridView1.DataBind();

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

        protected DataSet GetProcedure(string PlanID)
        {
            DataSet ds = new DataSet();
            string sql;

            sql = "SELECT TA_Procedure.ProcessConfig FROM TA_Procedure INNER JOIN TC_PlanProcedure ON TA_Procedure.ID = TC_PlanProcedure.ProcedureID " +
                  "INNER JOIN TA_Plan ON TC_PlanProcedure.PlanID = TA_Plan.ID AND TC_PlanProcedure.ModelID = TA_Plan.ModelID " +
                  "WHERE (TA_Plan.ID = '" + PlanID + "')";
            DataSet dataSet = sqlaccess.OpenQuerry(sql);

            if (dataSet.Tables[0].Rows.Count > 0)
            {
                string tmp;

                DataRow row = dataSet.Tables[0].Rows[0];
                if (!row.IsNull("ProcessConfig"))
                {
                    tmp = row["ProcessConfig"].ToString();
                }
                else
                {
                    return null;
                }

                try
                {
                    XmlTextReader reader = new XmlTextReader(tmp, XmlNodeType.Document, null);
                    ds.ReadXml(reader);

                    return ds;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }


        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbl = (Label)e.Row.FindControl("lblPlanID");
                Table tbl = (Table)e.Row.FindControl("tblDetail");
                int planOutPut = 0;//计划数
                int baofei = 0; //报废数
                int repairCount = 0;//返修数

                sqlaccess.Open();
                try
                {
                    string tmpsql = "";
                    //先算出计划产量 与 报废数、 返修数 
                    tmpsql = " SELECT tp.Output, ISNULL(tbf.报废数,0), ISNULL(tbf.返修数,0)"
                            + " FROM (SELECT PlanID, SUM(CASE WHEN manufacturestate = '报废' THEN 1 END) "
                            + " AS 报废数, SUM(CASE WHEN manufacturestate = '返修' THEN 1 END) "
                            + " AS 返修数"
                            + " FROM TA_Product"
                            + " WHERE (PlanID = '" + lbl.Text.Trim() + "')"
                            + " GROUP BY PlanID) AS tbf INNER JOIN"
                            + " TA_Plan AS tp ON tbf.PlanID = tp.ID";
                    ds = sqlaccess.OpenQuerry(tmpsql);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        planOutPut = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                        baofei = Convert.ToInt32(ds.Tables[0].Rows[0][1]);
                        repairCount = Convert.ToInt32(ds.Tables[0].Rows[0][2]);
                    }

                    // 获取所有的工序类型
                    ds = GetProcedure(lbl.Text.Trim());
                    if (ds == null)
                    {
                        sqlaccess.Close();
                        return;
                    }

                    DataView vProcess = new DataView(ds.Tables["Process"]);
                    //vProcess.RowFilter = "GROUP BY Range";
                    if (vProcess.Count == 0)
                    {
                        sqlaccess.Close();
                        return;
                    }

                    string strClass = "SELECT '000Null' AS Class ";
                    for (int i = 0; i < vProcess.Count; i++)
                    {
                        strClass += "UNION SELECT '" + vProcess[i]["Range"] + "' AS Class ";
                    }

                    //先按manufactureState分类统计出 各个状态的数目
                    //tmpsql = " select cls.class,isnull(dtl.ccount,0) ccount from (SELECT '0Null' AS Class UNION SELECT '1装配' AS Class UNION SELECT '2总装' AS Class UNION SELECT '3调试' AS Class UNION SELECT '4检验' AS Class UNION SELECT '5包装' AS Class UNION SELECT '6入库' AS Class) cls left outer join "
                    tmpsql = " select cls.class,isnull(dtl.ccount,0) ccount from (" + strClass + ") cls left outer join "
                            + " (SELECT class, ccount"
                            + " FROM (SELECT TOP 100 PERCENT CASE WHEN ManufactureState IS NULL"
                            + " THEN '000Null' ELSE ManufactureState END AS class, COUNT(*) AS ccount"
                            + " FROM TA_Product"
                            + " WHERE (PlanID = '" + lbl.Text.Trim() + "') and ModelType = 1"
                            + " GROUP BY ManufactureState"
                            + " ORDER BY ManufactureState) AS derivedtbl_1"
                            + " WHERE (class <> '报废') AND (class <> '返修')) dtl on cls.class=dtl.class";
                   // Response.Write(tmpsql + "<br><br><br><br>");
                    ds = sqlaccess.OpenQuerry(tmpsql);

                    TableRow row0 = new TableRow();
                    TableCell cellr0 = new TableCell();
                    cellr0.RowSpan = 2;
                    cellr0.Text = "计划";
                    row0.Cells.Add(cellr0);

                    cellr0 = new TableCell();
                    cellr0.RowSpan = 2;
                    cellr0.Text = "返修";
                    row0.Cells.Add(cellr0);

                    cellr0 = new TableCell();
                    cellr0.RowSpan = 2;
                    cellr0.Text = "报废";
                    row0.Cells.Add(cellr0);


                    TableRow row1 = new TableRow();
                    TableCell cellr1 = null;

                    TableRow row2 = new TableRow();
                    TableCell cellr2 = null;
                    cellr2 = new TableCell();
                    cellr2.Text = planOutPut.ToString();//计划数
                    row2.Cells.Add(cellr2);

                    cellr2 = new TableCell();
                    cellr2.Text = repairCount.ToString();//返修数
                    row2.Cells.Add(cellr2);

                    cellr2 = new TableCell();
                    cellr2.Text = baofei.ToString(); //报废数
                    row2.Cells.Add(cellr2);

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ProcessData[] pData = new ProcessData[ds.Tables[0].Rows.Count - 1];

                        //计算在线数
                        for (int i = 1; i < ds.Tables[0].Rows.Count; i++)//从1开始 表示第一道工序
                        {
                            pData[i - 1] = new ProcessData();

                            DataRow lastr = ds.Tables[0].Rows[i - 1];//上一行
                            DataRow thisr = ds.Tables[0].Rows[i];    //本行

                            pData[i - 1].Process = thisr[0].ToString();//工序类别名称
                            pData[i - 1].LineNum = Convert.ToInt32(lastr[1]); //在线数
                        }
                        //计算工序类型N完成数
                        DataRow row = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1];    //最后一行
                        pData[ds.Tables[0].Rows.Count - 2].FinishNum = Convert.ToInt32(row[1]); //工序类型N完成数
                        //计算工序类型n（1≤n≤N-1）完成数
                        for (int i = ds.Tables[0].Rows.Count - 3; i >= 0; i--)
                        {
                            pData[i].FinishNum = pData[i + 1].LineNum + pData[i + 1].FinishNum;
                        }


                        for (int i = 1; i < ds.Tables[0].Rows.Count; i++)//从1开始 表示第一道工序
                        {
                            cellr0 = new TableCell();
                            cellr0.ColumnSpan = 2;
                            cellr0.Text = pData[i - 1].Process;//工序类别名称
                            row0.Cells.Add(cellr0);

                            cellr1 = new TableCell();
                            cellr1.Text = "在线";
                            row1.Cells.Add(cellr1);
                            cellr1 = new TableCell();
                            cellr1.Text = "完成";
                            row1.Cells.Add(cellr1);

                            cellr2 = new TableCell();
                            cellr2.Text = pData[i - 1].LineNum.ToString(); //在线数
                            row2.Cells.Add(cellr2);

                            cellr2 = new TableCell();
                            cellr2.Text = pData[i - 1].FinishNum.ToString(); //完成数=本行完成数+下行完成数
                            row2.Cells.Add(cellr2);
                        }

                        //for (int i = 1; i < ds.Tables[0].Rows.Count; i++)//从1开始 表示第一道工序
                        //{
                        //    DataRow thisr = ds.Tables[0].Rows[i];    //本行
                        //    DataRow lastr = ds.Tables[0].Rows[i - 1];//上一行
                        //    DataRow nextr = null;//下一行

                        //    int thecount = Convert.ToInt32(thisr[1]);
                        //    int lstcount = Convert.ToInt32(lastr[1]);
                        //    int nxtcount = 0;

                        //    if (i < ds.Tables[0].Rows.Count - 1)
                        //    {
                        //        nextr = ds.Tables[0].Rows[i + 1];//下一行
                        //        nxtcount = Convert.ToInt32(nextr[1]);
                        //    }


                        //    cellr0 = new TableCell();
                        //    cellr0.ColumnSpan = 2;
                        //    cellr0.Text = thisr[0].ToString();//工序类别名称
                        //    row0.Cells.Add(cellr0);

                        //    cellr1 = new TableCell();
                        //    cellr1.Text = "在线";
                        //    row1.Cells.Add(cellr1);
                        //    cellr1 = new TableCell();
                        //    cellr1.Text = "完成";
                        //    row1.Cells.Add(cellr1);

                        //    cellr2 = new TableCell();
                        //    cellr2.Text = lstcount.ToString(); //在线数
                        //    row2.Cells.Add(cellr2);

                        //    cellr2 = new TableCell();
                        //    cellr2.Text = (thecount + nxtcount).ToString(); //完成数=本行完成数+下行完成数
                        //    row2.Cells.Add(cellr2);
                        //}
                    }
                    row0.HorizontalAlign = HorizontalAlign.Center;
                    row1.HorizontalAlign = HorizontalAlign.Center;
                    row2.HorizontalAlign = HorizontalAlign.Center;

                    tbl.Rows.Add(row0);
                    tbl.Rows.Add(row1);
                    tbl.Rows.Add(row2);

                }
                catch (Exception ex)
                {
                    LogManager.Write(this, ex.Message);
                }
                finally
                {
                    sqlaccess.Close();
                }

                string tblid = tbl.ID;
            }
        }

    }
}
