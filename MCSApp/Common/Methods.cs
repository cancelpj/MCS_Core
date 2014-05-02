using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using MCSApp.DataAccess;
using MCSApp.DataAccess.LogManage;
using System.Collections.Generic;
using System.Text;
using System.IO;
using FINGU.MCS;
using System.Xml;
using System.Collections;

namespace MCSApp.Common
{
    public static class Methods
    {

        public static string setResult(int allCount, int pageSize, int pageIndex, int CountInPage)
        {
            int beginCount = 0;
            int endCount = 0;
            beginCount = pageIndex * pageSize + 1;
            endCount = beginCount + CountInPage - 1;
            string rtnStr = "共计：" + allCount.ToString() + "条,本页：" + beginCount + "－" + endCount + "条";
            return rtnStr;
        }

        /// <summary>
        /// 删除指定目录下的文件
        /// </summary>
        /// <param name="dirRoot">指定的目录</param>
        /// <param name="FileName">指定的文件名</param>
        /// <returns></returns>
        public static bool DeleteFile(string dirRoot, string FileName)//static
        {
            string deleteFileName = FileName;//要删除的文件名称
            bool ok = true; ;
            try
            {
                string[] rootDirs = Directory.GetDirectories(dirRoot); //当前目录的子目录：
                string[] rootFiles = Directory.GetFiles(dirRoot);      //当前目录下的文件：

                foreach (string fiel in rootFiles)
                {
                    if (fiel.Contains(deleteFileName))
                    {
                        File.Delete(fiel);
                        break;//删除文件
                    }
                }
                //foreach (string s1 in rootDirs)
                //{
                //    DeleteFile(s1);
                //}
            }
            catch (Exception ex)
            {
                ok = false;
                LogManager.Write("", ex.Message);
            }
            return ok;
        }
        /// <summary>
        /// 返回四舍五入的值
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="digit">小数点后的位数</param>
        /// <returns></returns>
        public static double RSRound(double value, int digit)
        {
            double vt = Math.Pow(10, digit);
            double vx = value * vt;

            vx += 0.5;
            return (Math.Floor(vx) / vt);
        }
        /// <summary>
        /// 设置细线表格
        /// </summary>
        /// <param name="tbl">首先,引用Common.js,将欲设置细线的HTML表格作为服务器端控件运行,然后调用此方法</param>
        public static void setThinLineTable(HtmlTable tbl)
        {
            string timestr = "t" + (DateTime.Now.ToBinary()).ToString();
            ScriptManager.RegisterStartupScript(tbl.Page, tbl.Page.GetType(), "t" + timestr, "<script language='javascript'>setThinLineTable('" + tbl.ClientID + "')</script>", false);
        }
        /// <summary>
        /// ajax消息框
        /// </summary>
        /// <param name="page">页面类</param>
        /// <param name="Message">弹出窗口显示的信息内容</param>
        public static void AjaxMessageBox(System.Web.UI.Page page, string Message)
        {
            string timestr = "t" + (DateTime.Now.ToBinary()).ToString();
            ScriptManager.RegisterStartupScript(page, page.GetType(), timestr, "alert('" + Message + "');", true);
        }
        /// <summary>
        /// 消息框
        /// </summary>
        /// <param name="page">页面类</param>
        /// <param name="Message">弹出窗口显示的信息内容</param>
        public static void MessageBox(System.Web.UI.Page page, string Message)
        {
            page.Response.Write("<script>alert('" + Message + "')</script>");
        }
        /// <summary>
        /// 取得页面参数
        /// </summary>
        /// <param name="page">页面类</param>
        /// <param name="Message">弹出窗口显示的信息内容</param>
        public static string GetParam(System.Web.UI.Page page, string Param)
        {
            return page.Request.QueryString[Param] == null ? "" : page.Request.QueryString[Param];
        }
        /// <summary>
        /// 书写器
        /// </summary>
        /// <param name="page">页面类</param>
        /// <param name="Message">写到页面的信息内容</param>
        public static void Write(System.Web.UI.Page page, string Message)
        {
            string timestr = "t" + (DateTime.Now.ToBinary()).ToString();
            ScriptManager.RegisterStartupScript(page, page.GetType(), timestr, Message, true);
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        /// <param name="page">页面类</param>
        /// <param name="Message">写到页面的信息内容</param>
        public static bool CheckUser(System.Web.UI.Page page, string ID, string password, string[] Roles, out string Msg, bool letShow)
        {
            if (SessionUser.IsSuperAdmin)
            {
                Msg = "超级管理员";
                return true;
            }
            bool hasScriptManager = false;
            foreach (System.Web.UI.Control control in page.Controls)
            {
                if (control is System.Web.UI.ScriptManager)
                {
                    hasScriptManager = true;
                    break;
                }
            }


            try
            {
                if (ID == "" || password == "")
                {
                    Msg = "帐号或密码都不能为空"; return false;
                }
                WSSecurity.Security scrty = new WSSecurity.Security();
                if (!scrty.CheckRight(ID, password, Roles, out Msg))
                {
                    if (letShow)
                    {
                        if (hasScriptManager == true)
                        {
                            Methods.AjaxMessageBox(page, Msg);
                        }
                        else
                        {
                            Methods.MessageBox(page, Msg);
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (hasScriptManager == true)
                {
                    Methods.AjaxMessageBox(page, ex.Message);
                }
                else
                {
                    Methods.MessageBox(page, ex.Message);
                }
                LogManager.Write(page, ex.Message);
                Msg = "";
                return false;

            }

            return true;
        }

        /// <summary>
        /// 弹出窗体导出excel
        /// </summary>
        /// <param name="control">页面中updatePanel对象</param>
        /// <param name="grid">要导出的GridView</param>
        public static void setGridToExcel(UpdatePanel control, GridView grid)
        {
            HttpContext.Current.Session["gridView"] = grid;
            ScriptManager.RegisterStartupScript(control, grid.Page.GetType(), "", "window.open('../PubUse/ToExcelPage.aspx')", true);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="response">Response对象</param>
        /// <param name="grid">gridView对象</param>
        public static void ToExcel(HttpResponse response, GridView grid)
        {
            if (grid.BottomPagerRow != null)
            {
                grid.BottomPagerRow.Visible = false;
            }

            for (int i = 0; i < grid.Rows.Count; i++)
            {
                if (grid.Rows[i].RowType == DataControlRowType.DataRow)
                {
                    for (int j = 0; j < grid.Rows[i].Cells.Count; j++)
                    {
                        grid.Rows[i].Cells[j].Attributes.Add("class", "text");
                    }
                }
            }

            string style = @"<style> .text { mso-number-format:\@; } </script> ";
            response.Clear();
            response.Buffer = true;
            response.Charset = "GB2312";
            response.AppendHeader("Content-Disposition", "attachment;filename=FileName.xls");

            response.ContentEncoding = System.Text.Encoding.UTF7;

            //设置输出文件类型为excel文件。 
            response.ContentType = "application/ms-excel";
            System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);

            grid.RenderControl(oHtmlTextWriter);
            response.Output.Write(style);
            string tmpStr = oStringWriter.ToString();
            tmpStr = tmpStr.Replace("href", "");
            response.Output.Write(tmpStr);
            response.Flush();
            response.End();
        }
        /**/
        /**/
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="originalImagePath">源图路径（物理路径）</param>
        /// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="mode">生成缩略图的方式</param>    
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height, string mode)
        {
            System.Drawing.Image originalImage = System.Drawing.Image.FromFile(originalImagePath);

            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            switch (mode)
            {
                case "HW"://指定高宽缩放（可能变形）                
                    break;
                case "W"://指定宽，高按比例                    
                    toheight = originalImage.Height * width / originalImage.Width;
                    break;
                case "H"://指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                    break;
                case "Cut"://指定高宽裁减（不变形）                
                    if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = originalImage.Height;
                        ow = originalImage.Height * towidth / toheight;
                        y = 0;
                        x = (originalImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = originalImage.Width;
                        oh = originalImage.Width * height / towidth;
                        x = 0;
                        y = (originalImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            //新建一个bmp图片
            System.Drawing.Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

            //新建一个画板
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);

            //设置高质量插值法
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            //清空画布并以透明背景色填充
            g.Clear(System.Drawing.Color.Transparent);

            //在指定位置并且按指定大小绘制原图片的指定部分
            g.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, towidth, toheight),
                new System.Drawing.Rectangle(x, y, ow, oh),
                System.Drawing.GraphicsUnit.Pixel);

            try
            {
                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
        }

        public static void ListBoxBind(ListBox listBox, string oqsql)
        {
            DataSet ds;
            SqlAccess sqlaccess = new SqlAccess();
            try
            {
                sqlaccess.Open();
                ds = sqlaccess.OpenQuerry(oqsql);

                listBox.DataSource = ds;
                listBox.DataValueField = "Value";
                listBox.DataTextField = "Text";

                listBox.DataBind();
            }
            catch (Exception ex)
            {
                if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") == false)
                    LogManager.Write(listBox, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        public static Boolean ModelIDIsInDB(string ModelID)
        {
            Boolean result = false;
            DataSet ds;
            SqlAccess sqlaccess = new SqlAccess();
            try
            {
                sqlaccess.Open();
                ds = sqlaccess.OpenQuerry("select case when count(ID) is null then 0 else count(id) end countid from ta_model where id='" + ModelID + "'");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                    {
                        result = true;
                    }

                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") == false)
                    LogManager.Write("", ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }
            return result;
        }


        public static void DDLBind(DropDownList ddl, string oqsql)
        {
            DataSet ds;
            SqlAccess sqlaccess = new SqlAccess();
            try
            {
                sqlaccess.Open();
                ds = sqlaccess.OpenQuerry(oqsql);

                ddl.DataSource = ds;
                ddl.DataValueField = "Value";
                ddl.DataTextField = "Text";

                ddl.DataBind();
            }
            catch (Exception ex)
            {
                if (ex.GetType().ToString().Equals("System.Data.SqlClient.SqlException") == false)
                    LogManager.Write(ddl, ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        public static bool setFocusDDL(DropDownList ddl, string value)
        {
            bool bfind = false;
            foreach (ListItem item in ddl.Items)
            {
                if (item.Value == value)
                {
                    //item.Selected = true;
                    ddl.SelectedIndex = ddl.Items.IndexOf(item);//设置下拉框选中项的索引
                    bfind = true;
                    break;
                }
            }
            return bfind;
        }
        public static bool setFocusDDLbyText(DropDownList ddl, string text)
        {
            bool bfind = false;
            foreach (ListItem item in ddl.Items)
            {
                if (item.Text == text)
                {
                    //item.Selected = true;
                    ddl.SelectedIndex = ddl.Items.IndexOf(item);//设置下拉框选中项的索引
                    bfind = true;
                    break;
                }
            }
            return bfind;
        }
        /// <summary>
        /// 设置新建的可用性
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="lbt"></param>

        public static void setNewEnable(DropDownList ddl, LinkButton lbt)
        {
            if (ddl.Items.Count > 0)
            {
                lbt.Enabled = true;
            }
            else
            {
                lbt.Enabled = false;
            }
        }
        public static string noPermissionStr = "对不起，您没有相应的权限!";


        /// <summary>
        /// 记录点击次数
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="FieldName">欲查询的字段名</param>
        /// <param name="keyField">主健字段</param>
        /// <param name="keyValue">主健值</param>
        /// <param name="addValue">增加值的步长</param>
        /// <returns>点击次数</returns>
        public static int autoAdd(string tableName, string FieldName, string keyField, string keyValue, int addValue)
        {
            string qu_sql = "select " + FieldName + " from " + tableName + " where " + keyField + "='" + keyValue + "'";

            SqlAccess sqlaccess = new SqlAccess();
            int c = 0;
            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                DataSet ds = sqlaccess.OpenQuerry(qu_sql);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    c = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                    int Add1 = c + addValue;
                    string up_sql = "update " + tableName + " set " + FieldName + "='" + Add1 + "' where " + keyField + "='" + keyValue + "'";
                    sqlaccess.ExecuteQuerry(up_sql);
                    sqlaccess.Commit();
                }
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                sqlaccess.Rollback();
                return 0;
            }
            finally
            {
                sqlaccess.Close();
            }
            return c;

        }
        /// <summary>
        /// 通过部件的条码查找对应部件的信息
        /// 其中包括部件的ID，部件的条码，部件的型号码，部件型号名称
        /// </summary>
        /// <param name="ProductSN">产品编码</param>
        /// <returns>返回部件信息的数据集</returns>
        public static DataSet getComponentFromComponentSN(string ComponentSN)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = "";
                tmpsql += " SELECT p.ID, p.SN, m.Code, m.Name";
                tmpsql += " FROM TA_Component AS p INNER JOIN";
                tmpsql += " TA_ProductModel AS m ON p.ModelID = m.ID";
                tmpsql += " WHERE (p.SN = '" + ComponentSN + "')";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                return tmpds;
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return null;
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        /// <summary>
        /// 通过产品的条码查找对应部件的信息
        /// 其中包括部件的ID，部件的条码，部件的型号码，部件型号名称
        /// </summary>
        /// <param name="ProductSN">产品编码</param>
        /// <returns>返回部件信息的数据集</returns>
        public static DataSet getComponentFromProductSN(string ProductSN)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = "";
                tmpsql += " SELECT trpc.ComponentID, c.SN, m.Code, m.Name";
                tmpsql += " FROM TRE_Product_Component AS trpc INNER JOIN";
                tmpsql += " TA_Component AS c ON trpc.ComponentID = c.ID INNER JOIN";
                tmpsql += " TA_ProductModel AS m ON c.ModelID = m.ID";
                tmpsql += " WHERE (trpc.ProductID IN";
                tmpsql += " (SELECT ID";
                tmpsql += " FROM TA_Product";
                tmpsql += " WHERE (SN = '" + ProductSN + "')))";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                return tmpds;
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return null;
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        /// <summary>
        /// 通过计划单号ID找新产品ID
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public static string getModelFromPlanID(string PlanID)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = ""
                        + " select ModelID from ta_plan t"
                        + " where t.ID='" + PlanID + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return tmpds.Tables[0].Rows[0][0].ToString();
                }
                else return "";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        /// <summary>
        /// 通过计划单号ID找新产品ID
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public static string getModelFromPlanIDInProduct(string PlanID)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = ""
                        + " select t.ModelID from ta_product t"
                        + " where t.PlanID='" + PlanID + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return tmpds.Tables[0].Rows[0][0].ToString();
                }
                else return "";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        /// <summary>
        /// 通过品号找代号
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public static string getProductCodeFromID(string ID, out string pname)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = ""
                        + " select t.Code,t.name from ta_model t"
                        + " where t.ID='" + ID + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    pname = tmpds.Tables[0].Rows[0][1].ToString();
                    return tmpds.Tables[0].Rows[0][0].ToString();
                }
                else { pname = ""; return ""; }
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                pname = "";
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        /// <summary>
        /// 通过品名找产品
        /// </summary>
        /// <param name="name">品名</param>
        /// <returns>品号</returns>
        internal static string getProductIDFromPName(string name)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = ""
                        + " select t.ID from ta_model t"
                        + " where t.Name='" + name + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return tmpds.Tables[0].Rows[0][0].ToString();
                }
                else return "";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        /// <summary>
        /// 通过代号取品号
        /// </summary>
        /// <param name="Daihao">代号</param>
        /// <returns>品号</returns>
        internal static string getProductIDFromDaiHao(string Daihao)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = ""
                        + " select t.id from ta_model t"
                        + " where t.code='" + Daihao + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return tmpds.Tables[0].Rows[0][0].ToString();
                }
                else return "";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        /// <summary>
        /// 通过ID找部件信息
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public static DataSet getComponentFromComponentID(string ID)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = ""
                        + " select * from ta_product t"
                        + " where t.ModelID='" + ID + "' and t.ModelID "
                        + " in ( select id from ta_Model where ModelType='2'"
                        + " and ID='" + ID + "')";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                return tmpds;
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return null;
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        /// <summary>
        /// 通过产品ID找产品信息
        /// </summary>
        /// <param name="ProductID"></param>
        /// <returns></returns>
        public static DataSet getProductFromProductID(string ProductID)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = ""
                        + " select * from ta_product t"
                        + " where t.ModelID='" + ProductID + "' and t.ModelID "
                        + " in ( select id from ta_Model where ModelType='1'"
                        + " and ID='" + ProductID + "')";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                return tmpds;
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return null;
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        /// <summary>
        /// 通过计划查找产品的信息
        /// 其中包括产品的ID，产品的条码，产品的型号码，产品型号名称
        /// </summary>
        /// <param name="ProductSN">产品编码</param>
        /// <returns>返回产品信息的数据集</returns>
        public static DataSet getProducFromProductSN(string ProductSN)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = "";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                return tmpds;
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return null;
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        public static void checkLogin(System.Web.UI.Page page)
        {
            if (!SessionUser.IsLogin)
            {
                Methods.MessageBox(page, "帐号没有通过验证，请验证后再进入本页面！"); return;
            }
        }
        /// <summary>
        /// 返回日期时间差，精确到秒
        /// </summary>
        /// <param name="DateTime1">开始日期</param>
        /// <param name="DateTime2">节束日期</param>
        /// <returns>返回X天X小时X分X秒格式的字符串</returns>
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {

            string dateDiff = null;
            try
            {
                TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                dateDiff = ts.Days.ToString() + "天"
                        + ts.Hours.ToString() + "小时"
                        + ts.Minutes.ToString() + "分钟"
                        + ts.Seconds.ToString() + "秒";

            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
            }
            return dateDiff;
        }
        /// <summary>
        ///本周是本年第几周
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int nowIsWitchWeekOfYear(System.DateTime dt)
        {
            System.Globalization.Calendar calendar = new System.Globalization.GregorianCalendar();

            //把指定日期转换成所在星期(第几个星期)
            int Week = calendar.GetWeekOfYear(dt, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
            return Week;
        }
        /// <summary>
        ///本周起止日期
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string WeekRange(System.DateTime dt, ref string dateBegin, ref string dateEnd)
        {
            int weeknow = Convert.ToInt32(dt.DayOfWeek);
            int daydiff = (-1) * weeknow;
            int dayadd = 6 - weeknow;
            dateBegin = dt.AddDays(daydiff).Date.ToString("yyyy-MM-dd");
            dateEnd = dt.AddDays(dayadd).Date.ToString("yyyy-MM-dd");
            if (Convert.ToDateTime(dateBegin).Year < dt.Year)
            {
                dateBegin = dt.Year.ToString() + "-01-01";
            }
            if (Convert.ToDateTime(dateEnd).Year > dt.Year)
            {
                dateEnd = dt.Year.ToString() + "-12-31";
            }
            return dateBegin + " 至 " + dateEnd;
        }

        internal static void WriteOprationLog(string EID, string DT, string logStr)
        {
            SqlAccess sqlaccess = new SqlAccess();
            //DataSet tmpds;
            try
            {
                SqlCommand cmd = new SqlCommand();

                sqlaccess.Open();
                sqlaccess.BeginTransaction();
                cmd.Parameters.AddWithValue("@p1", EID);
                cmd.Parameters.AddWithValue("@p2", DT);
                cmd.Parameters.AddWithValue("@p3", logStr);
                cmd.CommandText = "INSERT INTO TB_Event (EmplyeeID, EventTime, EventRecord) VALUES (@p1,@p2,@p3)";

                sqlaccess.ExecuteQuerry(cmd);
                sqlaccess.Commit();
            }
            catch (Exception ex)
            {
                sqlaccess.Rollback();
                LogManager.Write("", ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        /// <summary>
        /// 由员工号取得计划中的品号(不支持多计划）
        /// </summary>
        /// <param name="EID"></param>
        /// <returns></returns>
        internal static string GetPlanPIDByEID(string EID, out string PlanID)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                SqlCommand cmd = new SqlCommand();

                sqlaccess.Open();
                cmd.Parameters.AddWithValue("@p1", EID);
                cmd.CommandText = "SELECT ModelID,ID"
                                + " FROM TA_Plan"
                                + " WHERE (ID IN"
                                + "        (SELECT PlanID"
                                + "    FROM TA_Group"
                                + " WHERE (ID IN"
                                + "        (SELECT GroupID"
                                + "    FROM TRE_Group_Employee"
                                + " WHERE (EmployeeID = @p1)))))";
                tmpds = sqlaccess.OpenQuerry(cmd);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    PlanID = tmpds.Tables[0].Rows[0][1].ToString();
                    return tmpds.Tables[0].Rows[0][0].ToString();
                }
                PlanID = "";
                return "";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                PlanID = "";
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        /// <summary>
        /// 从计划中取得品号
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        internal static string GetModelIDByPlanID(string PlanID)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                SqlCommand cmd = new SqlCommand();

                sqlaccess.Open();
                cmd.Parameters.AddWithValue("@p1", PlanID);
                cmd.CommandText = "SELECT ModelID FROM TA_Plan WHERE ID = @p1";
                tmpds = sqlaccess.OpenQuerry(cmd);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    PlanID = tmpds.Tables[0].Rows[0][1].ToString();
                    return tmpds.Tables[0].Rows[0][0].ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        internal static object GetProcIDByMID(string mid, string planid)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                SqlCommand cmd = new SqlCommand();

                sqlaccess.Open();
                cmd.Parameters.AddWithValue("@p1", mid);
                cmd.Parameters.AddWithValue("@p2", planid);
                cmd.CommandText = "SELECT ProcedureID "
                                + " FROM TC_PlanProcedure where Modelid=@p1 and planid=@p2";
                tmpds = sqlaccess.OpenQuerry(cmd);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return tmpds.Tables[0].Rows[0][0];
                }
                return "-1";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return "-1";
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        internal static int getModelIDCount(string ID)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                SqlCommand cmd = new SqlCommand();

                sqlaccess.Open();
                cmd.Parameters.AddWithValue("@p1", ID);
                cmd.CommandText = "SELECT count(*) "
                                + " FROM TA_Product where id=@p1 or sn=@p1";//产品序列号或条码
                tmpds = sqlaccess.OpenQuerry(cmd);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(tmpds.Tables[0].Rows[0][0]);
                }
                return -1;
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return -1;
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        internal static int getMatierialIDCount(string ID)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                SqlCommand cmd = new SqlCommand();

                sqlaccess.Open();
                cmd.Parameters.AddWithValue("@p1", ID);
                cmd.CommandText = "SELECT count(*) "
                                + " FROM TA_Materiel where ID=@p1";
                tmpds = sqlaccess.OpenQuerry(cmd);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(tmpds.Tables[0].Rows[0][0]);
                }
                return -1;
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return -1;
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        internal static string getTypeOfModelID(string ModelID)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                SqlCommand cmd = new SqlCommand();

                sqlaccess.Open();
                cmd.Parameters.AddWithValue("@p1", ModelID);
                cmd.CommandText = "select t.ModelType from ta_Model t where t.id='" + ModelID + "' ";
                tmpds = sqlaccess.OpenQuerry(cmd);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return tmpds.Tables[0].Rows[0]["ModelType"].ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        /// <summary>
        /// 根据产品序号得到类型,注意这里可能在两个表中，一是product表 一是materiel表中
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        internal static string getTypeOfID(string ID)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds = null;
            try
            {
                SqlCommand cmd = new SqlCommand();

                sqlaccess.Open();
                cmd.Parameters.AddWithValue("@p1", ID);
                cmd.CommandText = "select t.ModelType from ta_Model t where t.id in ( select ModelID from ta_product where id='" + ID + "')  or t.id in ( select ModelID from ta_materiel where id='" + ID + "')";
                tmpds = sqlaccess.OpenQuerry(cmd);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return tmpds.Tables[0].Rows[0]["ModelType"].ToString();
                }
                return "";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        internal static bool PlanISActive(string planid)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds = null;
            try
            {
                SqlCommand cmd = new SqlCommand();

                sqlaccess.Open();
                cmd.Parameters.AddWithValue("@p1", planid);
                cmd.CommandText = "select state from TA_Plan where id='" + planid + "'";
                tmpds = sqlaccess.OpenQuerry(cmd);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    if (tmpds.Tables[0].Rows[0]["state"].ToString() == "2")
                    {
                        return true;
                    }
                    else
                        return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return false;
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        /// <summary>
        /// 根操操作处理xml数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        internal static string OperateXMLData(Page page, string op)
        {
            const string STATUS_NULL = "<Response status=\"-1\" ></Response> ";//未知错误标志 
            const string STATUS_SUCCESS = "<Response status=\"0\" ></Response> ";//操作成功标志 
            const string STATUS_FAIL = "<Response status=\"1\" ></Response> ";//操作失败标志
            const string STATUS_FILE_EXIST = "<Response status=\"3\" ></Response> ";//记录已存在标志
            const string STATUS_FILE_NOT_FOUND = "<Response status=\"5\" ></Response> ";//记录不存在标志
            const string STATUS_XML_PARSER_ERROR = "<Response status=\"7\" ></Response> ";//parse xml 出错标志
            const string STATUS_IO_ERROR = "<Response status=\"9\" ></Response> ";//文件写入失败标志 

            const string STATUS_MultiStartNode = "<Response status=\"20\" ></Response> ";//开始节点过多标志 
            const string STATUS_MultiEndNode = "<Response status=\"21\" ></Response> ";//结束节点过多标志 

            const string STATUS_NoProcess = "<Response status=\"22\" ></Response>";//不能只有开始与结束工序，应有中间工序
            const string STATUS_MultiToNode = "<Response status=\"23\" ></Response>";//目的工序节点数目超过20
            const string STATUS_MultiFromNode = "<Response status=\"24\" ></Response>";//源工序节点数目超过20
            const string STATUS_NoCloseProcess = "<Response status=\"25\" ></Response>";//有向图中存在孤立的工序节点


            const string STATUS_HASLOOP = "<Response status=\"26\" ></Response>";//有向图中存在闭合环路

            const string STATUS_MultiSamePName = "<Response status=\"27\" ></Response>";//工序名重复
            const string STATUS_MultiSameRange = "<Response status=\"28\" ></Response>";//工序类别重复

            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            string cnfgStrStart = "<?xml version=\"1.0\" ?>\n<NewDataSet>";
            string cnfgStrMiddelp = "";//中间Process部分
            string cnfgStrMiddelc = "";//中间Connection部分
            string cnfgStrEnd = "\n</NewDataSet>";
            string cnfgStr = "";
            bool isOK = true;//是否完整

            try
            {
                sqlaccess.Open();
                sqlaccess.BeginTransaction();

                System.IO.Stream instream = page.Request.InputStream;
                BinaryReader br = new BinaryReader(instream, System.Text.Encoding.UTF8);
                byte[] byt = br.ReadBytes((int)instream.Length);
                string sXml = System.Text.Encoding.UTF8.GetString(byt);
                if (op == "get")
                {
                    string idPartstr = sXml.Split('&')[0];
                    string id = idPartstr.Split('=')[1];

                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.AddWithValue("@p1", id);

                    cmd.CommandText = "select ProcessGraph from TA_Procedure where id=@p1";
                    tmpds = sqlaccess.OpenQuerry(cmd);

                    if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                    {
                        return tmpds.Tables[0].Rows[0]["ProcessGraph"].ToString();
                    }
                    else
                    {
                        return STATUS_FAIL;
                    }
                }
                else if (op == "update")
                {
                    string idPartstr = sXml.Split('&')[0];
                    string id = idPartstr.Split('=')[1];
                    string xmlPart = sXml.Split('&')[1];
                    string xml = xmlPart.Substring(4);//从第四个字符开始的子串

                    System.IO.StringReader xmlSR = new System.IO.StringReader(xml);

                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlSR);

                    //这里实现去除空点算法
                    XmlNodeList ACNodeOldList = doc.GetElementsByTagName("Activities");
                    XmlNodeList TRNodeOldList = doc.GetElementsByTagName("Transitions");

                    XmlNode ACNodeOld = null;
                    XmlNode TRNodeOld = null;

                    ArrayList RemoveACList = new ArrayList();
                    ArrayList RemoveTRList=new ArrayList();                    
                    ArrayList AddTRList=new ArrayList();

                    ArrayList l=new ArrayList(); 

                    if(ACNodeOldList.Count>0)
                    {
                        ACNodeOld = ACNodeOldList[0];
                        for (int i = 0; i < ACNodeOld.ChildNodes.Count; i++)
                        {
                            if (ACNodeOld.ChildNodes[i].Attributes["type"].Value == "EMPTY_NODE")
                            {
                                l.Add(ACNodeOld.ChildNodes[i].Attributes["id"].Value);//将空节点ID保存在列表中
                                RemoveACList.Add(ACNodeOld.ChildNodes[i]);            //将空节点保存在列表中
                            }
                        }
                    }
                    if (TRNodeOldList.Count > 0)
                    {
                        TRNodeOld = TRNodeOldList[0];
                        for (int i = 0; i < TRNodeOld.ChildNodes.Count; i++)//所有行为节点
                        {
                            for (int k = 0; k < l.Count;k++ )
                            {
                                if(TRNodeOld.ChildNodes[i].Attributes["to"].Value==l[k].ToString())//如果to 的值等于空值id
                                {                                           
                                    RemoveTRList.Add(TRNodeOld.ChildNodes[i]);
                                    for(int j = 0; j < TRNodeOld.ChildNodes.Count; j++)
                                    {
                                        if (TRNodeOld.ChildNodes[j].Attributes["from"].Value == l[k].ToString())//找到对应的from的值
                                        {
                                            XmlNode tempNode = null;
                                            tempNode = TRNodeOld.ChildNodes[i].Clone();
                                            tempNode.Attributes["to"].Value = TRNodeOld.ChildNodes[j].Attributes["to"].Value;
                                            AddTRList.Add(tempNode);
                                        }
                                    }
                                }
                                if (TRNodeOld.ChildNodes[i].Attributes["from"].Value == l[k].ToString())//如果from 的值等于空值id
                                {
                                    RemoveTRList.Add(TRNodeOld.ChildNodes[i]);
                                }
                            }
                        }
                    }

                    //重新构造 ACNodeOld TRNodeOld
                    for (int i = 0; i < RemoveACList.Count; i++)
                    {
                        ACNodeOld.RemoveChild((XmlNode)RemoveACList[i]);
                    }
                    for (int i = 0; i < RemoveTRList.Count; i++)
                    {
                         TRNodeOld.RemoveChild((XmlNode)RemoveTRList[i]);
                    }
                    for (int i = 0; i < AddTRList.Count; i++)
                    {
                        TRNodeOld.AppendChild((XmlNode)AddTRList[i]);
                    }


                    XmlNode ACNode = null;
                    XmlNode TRNode = null;

                    ACNode = ACNodeOld;//处理过的节点给ACNode
                        int bSNodeCount = 0;
                        int bENodeCount = 0;
                        for (int i = 0; i < ACNode.ChildNodes.Count; i++)
                        {
                            if (ACNode.ChildNodes[i].Attributes["type"].Value == "START_NODE")
                            {
                                bSNodeCount++;
                            }
                            if (ACNode.ChildNodes[i].Attributes["type"].Value == "END_NODE")
                            {
                                bENodeCount++;
                            }
                            //构造graph的xml格式的数据
                            cnfgStrMiddelp += "\n<process>\n<Name>" + ACNode.ChildNodes[i].Attributes["name"].Value + "</Name>\n<Range>" + ACNode.ChildNodes[i].Attributes["range"].Value + "</Range>\n</process>";

                        }
                        if (bSNodeCount == 0 || bENodeCount == 0)
                        {
                            isOK = false;
                        }
                        if (bSNodeCount > 1)
                        {
                            return STATUS_MultiStartNode;
                        }
                        if (bENodeCount > 1)
                        {
                            return STATUS_MultiEndNode;
                        }

                        TRNode = TRNodeOld;//处理过的节点给TRNode
                        for (int i = 0; i < TRNode.ChildNodes.Count; i++)
                        {
                            //构造config的xml格式的数据
                            cnfgStrMiddelc += "\n<Connection>\n<From>" + getProcessNameByFT(ACNode, TRNode.ChildNodes[i].Attributes["from"].Value) + "</From>\n<To>" + getProcessNameByFT(ACNode, TRNode.ChildNodes[i].Attributes["to"].Value) + "</To>\n</Connection>";

                        }

                    for (int i = 0; i < ACNode.ChildNodes.Count; i++)
                    {
                        string tmpid = ACNode.ChildNodes[i].Attributes["id"].Value;
                        string type = ACNode.ChildNodes[i].Attributes["type"].Value;
                        string name = ACNode.ChildNodes[i].Attributes["name"].Value;
                        int fromCount = 0;
                        int toCount = 0;
                        for (int j = 0; j < TRNode.ChildNodes.Count; j++)
                        {
                            if (TRNode.ChildNodes[j].Attributes["from"].Value == tmpid)
                            {
                                fromCount++;
                            }
                            if (TRNode.ChildNodes[j].Attributes["to"].Value == tmpid)
                            {
                                toCount++;
                            }
                        }

                        if (fromCount == 0 && toCount == 0)
                        {
                            //return STATUS_NoProcess + "<Response name=\"" + name + "\" ></Response> ";//不能只有开始或结束工序，应有中间工序
                            //return "<Response status=\"22\" name=\"" + name + "\" ></Response> ";//不能只有开始或结束工序，应有中间工序
                            isOK = false;
                        }
                        if (fromCount > 20)
                        {
                            //return STATUS_MultiToNode + "<Response name=\"" + name + "\" ></Response> "; ;//源工序节点数目超过20
                            //return "<Response status=\"23\" name=\"" + name + "\" ></Response> "; ;//源工序节点数目超过20
                            //isOK = false;
                        }
                        if (toCount > 20)
                        {
                            //return STATUS_MultiFromNode + "<Response name=\"" + name + "\" ></Response> ";//目的工序节点数目超过20
                            //return "<Response  status=\"24\" name=\"" + name + "\" ></Response> ";//目的工序节点数目超过20
                            //isOK = false;
                        }
                        if (fromCount == 0 && type != "END_NODE")
                        {
                            //return STATUS_NoCloseProcess + "<Response name=\"" + name + "\" ></Response> ";//有向图中存在没有闭合的路径
                            //return "<Response status=\"25\"  name=\"" + name + "\" ></Response> ";//有向图中存在没有闭合的路径
                            isOK = false;
                        }
                        if (toCount == 0 && type != "START_NODE")
                        {
                            //return STATUS_NoCloseProcess + "<Response name=\"" + name + "\" ></Response> ";//有向图中存在没有闭合的路径
                            //return "<Response status=\"25\"  name=\"" + name + "\" ></Response> ";//有向图中存在没有闭合的路径
                            isOK = false;
                        }

                    }

                    cnfgStr = cnfgStrStart + cnfgStrMiddelp + cnfgStrMiddelc + cnfgStrEnd;//整个config块数据

                    /****************************************检查环路开始************************************************/
                    StringReader stream = null;
                    XmlTextReader reader = null;
                    try
                    {
                        string xmlData = cnfgStr;

                        DataSet xmlDS = new DataSet();
                        stream = new StringReader(xmlData);
                        //从stream装载到XmlTextReader 
                        reader = new XmlTextReader(stream);
                        xmlDS.ReadXml(reader);

                        DataTable dtProcess = xmlDS.Tables["process"];
                        DataTable dtConnection = xmlDS.Tables["Connection"];

                        //DataRow[] dprows = dtProcess.Select("distinct Name");
                        DataTable dtp = SelectDistinct(dtProcess, "Name");
                        if (dtp.Rows.Count != dtProcess.Rows.Count)
                        {
                            return STATUS_MultiSamePName;//工序名称有重复
                        }

                        //DataRow[] drrows = dtProcess.Select("distinct Range");
                        //DataTable dtr = SelectDistinct(dtProcess,"Range");
                        //if(dtr.Rows.Count!=dtProcess.Rows.Count)
                        //{
                        //        return STATUS_MultiSameRange;//工序类别有重复
                        //}

                        for (int i = 0; i < dtProcess.Rows.Count; i++)
                        {
                            if (hasLoop(ref dtConnection, dtProcess.Rows[i]["Name"].ToString(), dtProcess.Rows[i]["Name"].ToString()))
                            {
                                return STATUS_HASLOOP;//发现环路
                            }
                        }

                    }
                    catch (System.Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (reader != null) reader.Close();
                    }
                    /*****************************************检查环路结束***********************************************/


                    object cnfgStrobj = isOK == true ? (object)cnfgStr : DBNull.Value;


                    SqlCommand cmd = new SqlCommand();
                    cmd.Parameters.AddWithValue("@p1", id);
                    cmd.Parameters.AddWithValue("@p2", xml);
                    cmd.Parameters.AddWithValue("@p3", cnfgStrobj);//如果完整 则添加配置字段

                    cmd.CommandText = " update TA_Procedure set  ProcessConfig=@p3,ProcessGraph=@p2 where id=@p1";

                    int c = sqlaccess.ExecuteQuerry(cmd);
                    sqlaccess.Commit();

                    if (c > 0)
                    {
                        return STATUS_SUCCESS;
                    }
                    else
                    {
                        return STATUS_FAIL;
                    }
                }
                //以下三种情况暂不实现 b
                else if (op == "list")
                {

                }
                else if (op == "add")
                {

                }
                else if (op == "delete")
                {

                }
                //以下三种情况暂不实现 e
                return STATUS_NULL;

            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return STATUS_NULL;
            }
            finally
            {
                sqlaccess.Close();
            }

        }
        private static DataTable SelectDistinct(DataTable SourceTable, params string[] FieldNames)
        {
            object[] lastValues;
            DataTable newTable;
            DataRow[] orderedRows;

            if (FieldNames == null || FieldNames.Length == 0)
                throw new ArgumentNullException("FieldNames");

            lastValues = new object[FieldNames.Length];
            newTable = new DataTable();

            foreach (string fieldName in FieldNames)
                newTable.Columns.Add(fieldName, SourceTable.Columns[fieldName].DataType);

            orderedRows = SourceTable.Select("", string.Join(", ", FieldNames));

            foreach (DataRow row in orderedRows)
            {
                if (!fieldValuesAreEqual(lastValues, row, FieldNames))
                {
                    newTable.Rows.Add(createRowClone(row, newTable.NewRow(), FieldNames));

                    setLastValues(lastValues, row, FieldNames);
                }
            }

            return newTable;
        }
        private static bool fieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames)
        {
            bool areEqual = true;

            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]]))
                {
                    areEqual = false;
                    break;
                }
            }

            return areEqual;
        }

        private static DataRow createRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames)
        {
            foreach (string field in fieldNames)
                newRow[field] = sourceRow[field];

            return newRow;
        }

        private static void setLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames)
        {
            for (int i = 0; i < fieldNames.Length; i++)
                lastValues[i] = sourceRow[fieldNames[i]];
        }

        /// <summary>
        /// 发现环路
        /// </summary>
        /// <param name="xmlNodeList"></param>
        /// <param name="tostr"></param>
        /// <param name="ddl"></param>
        internal static bool hasLoop(ref DataTable dt, string lstr, string beginstr)
        {
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DataRow r = dt.Rows[j];
                if (r["from"].ToString() == lstr)//添加到下拉框中
                {
                    if (r["to"].ToString() == beginstr)
                    {
                        return true;
                    }
                    else
                    {
                        lstr = r["to"].ToString();
                        hasLoop(ref dt, lstr, beginstr);
                    }
                }
            }
            return false;

        }
        private static string getProcessNameByFT(XmlNode ACNode, string FromOrToValue)
        {
            string name = "";
            for (int i = 0; i < ACNode.ChildNodes.Count; i++)
            {

                if (ACNode.ChildNodes[i].Attributes["id"].Value == FromOrToValue)
                {
                    name = ACNode.ChildNodes[i].Attributes["name"].Value;
                    break;
                }
            }
            return name;
        }

        internal static int daysDiff(string bStr, string eStr)
        {
            return Convert.ToInt32((DateTime.Parse(eStr) - DateTime.Parse(bStr)).TotalDays.ToString());
        }

        internal static string getProductIDFromPSN(string sn)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = ""
                        + " select t.ID from TA_Product t"
                        + " where t.SN='" + sn + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return tmpds.Tables[0].Rows[0][0].ToString();
                }
                else return "";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }

        }

        internal static bool HasExceptonInHistory(string tmpPid)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = ""
                        + " select top 1 t.productID, t.result from TB_ProcedureHistory t"
                        + " where t.productID='" + tmpPid + "' order by t.begintime desc";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    DataRow r = tmpds.Tables[0].Rows[0];
                    int result = r.IsNull("result") ? 0 : (int)r["result"];

                    if (result == 1)
                    {
                        return true;
                    }
                    else return false;

                }
                else return false;
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return false; ;
            }
            finally
            {
                sqlaccess.Close();
            }

        }

        internal static string getModelIDByProductID(string tmpPid)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = ""
                        + " select t.ModelID from TA_Product t"
                        + " where t.ID='" + tmpPid + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return tmpds.Tables[0].Rows[0][0].ToString();
                }
                else return "";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        internal static int getProcedureIDByProductID(string tmpPid)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = ""
                        + " select t.ProcedureID from TA_Product t"
                        + " where t.ID='" + tmpPid + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return (int)tmpds.Tables[0].Rows[0][0];
                }
                else return -1;
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return -1;
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        internal static void DDLBugPointBind(DropDownList dropDownList, string tmpModelID)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = ""
                        + " select t.BugPointCode,t.BugPointDsc from TA_BugPoint t where t.ModelID='" + tmpModelID + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < tmpds.Tables[0].Rows.Count; i++)
                    {
                        DataRow r = tmpds.Tables[0].Rows[i];
                        dropDownList.Items.Add(new ListItem(r["BugPointDsc"].ToString(), r["BugPointCode"].ToString()));
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        /// <summary>
        /// 取得上一步流程名字列表
        /// </summary>
        /// <param name="ProcedureID">工艺流程ID</param>
        /// <param name="PName">工序名称</param>
        internal static void DDLLastProcess(DropDownList ddlrein, int ProcedureID, string PName)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = ""
                        + " select ProcessGraph from TA_Procedure t where t.ID=" + ProcedureID.ToString() + " ";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    DataRow r = tmpds.Tables[0].Rows[0];
                    string tmpxmlstr = r["ProcessGraph"].ToString();
                    string tmpid = "";
                    System.IO.StringReader xmlSR = new System.IO.StringReader(tmpxmlstr);
                    XmlDocument doc = new XmlDocument();
                    doc.Load(xmlSR);

                    XmlNodeList ACNodeList = doc.GetElementsByTagName("Activities");
                    XmlNodeList TRNodeList = doc.GetElementsByTagName("Transitions");

                    for (int i = 0; i < ACNodeList.Count; i++)
                    {
                        for (int j = 0; j < ACNodeList[0].ChildNodes.Count; j++)
                        {
                            if (ACNodeList[0].ChildNodes[j].Attributes["name"].Value == PName)
                            {
                                tmpid = ACNodeList[0].ChildNodes[j].Attributes["id"].Value;
                                break;
                            }
                        }
                        break;

                    }

                    //for (int i = 0; i < TRNodeList.Count; i++)
                    //{
                    //    for (int j = 0; j < TRNodeList[0].ChildNodes.Count;j++ )
                    //    {
                    //        if (TRNodeList[0].ChildNodes[j].Attributes["from"].Value == tmpid)//将其子节点全部移除掉 
                    //        {

                    //            TRNodeList[0].RemoveChild(TRNodeList[0].ChildNodes[j]);
                    //            removeNode(ref TRNodeList, tmpid);
                    //        }
                    //    }

                    //} 
                    ddlrein.Items.Add(new ListItem("", tmpid));//先添加自己作为重入工序
                    addNodetoDdl(ref TRNodeList, tmpid, ref ddlrein);//将祖节点依次添加

                    for (int i = 0; i < ddlrein.Items.Count; i++)
                    {

                        for (int j = 0; j < ACNodeList[0].ChildNodes.Count; j++)
                        {
                            if (ACNodeList[0].ChildNodes[j].Attributes["id"].Value == ddlrein.Items[i].Value)
                            {
                                ddlrein.Items[i].Text = ACNodeList[0].ChildNodes[j].Attributes["name"].Value;
                                break;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }

        }
        /// <summary>
        /// 递归添加下拉框
        /// </summary>
        /// <param name="xmlNodeList"></param>
        /// <param name="tostr"></param>
        /// <param name="ddl"></param>
        internal static void addNodetoDdl(ref XmlNodeList xmlNodeList, string tostr, ref DropDownList ddl)
        {
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                for (int j = 0; j < xmlNodeList[0].ChildNodes.Count; j++)
                {
                    if (xmlNodeList[0].ChildNodes[j].Attributes["to"].Value == tostr)//添加到下拉框中
                    {

                        if (!ddl.Items.Contains(ddl.Items.FindByValue(xmlNodeList[0].ChildNodes[j].Attributes["from"].Value)))
                        {
                            ddl.Items.Add(new ListItem("", xmlNodeList[0].ChildNodes[j].Attributes["from"].Value));//添加项目
                        }

                        addNodetoDdl(ref xmlNodeList, xmlNodeList[0].ChildNodes[j].Attributes["from"].Value, ref ddl);
                        //xmlNodeList[0].RemoveChild(xmlNodeList[0].ChildNodes[j]);
                    }
                }
            }
        }
        internal static string getProcedureIDByPlanID(string planid, string ModelID)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = " select t.ProcedureID from TC_PlanProcedure t where t.Planid='" + planid + "' and t.ModelID='" + ModelID + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return tmpds.Tables[0].Rows[0][0].ToString();
                }
                else return "";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        internal static string getProcedureIDByModelID(string modelid)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = " select t.id from ta_procedure t where modelid='" + modelid + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return tmpds.Tables[0].Rows[0][0].ToString();
                }
                else return "";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        /// <summary>
        /// 部件的品号被唯一应用于一个产品下，不在别的产品下
        /// </summary>
        /// <param name="ModelID"></param>
        /// <returns></returns>
        internal static bool isSingleUsed(string ModelID)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = " select case when count(id) is null then 0 else count(id) end idcount from ta_structure where itemid='" + ModelID + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(tmpds.Tables[0].Rows[0][0]) == 1)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else return false;
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return false; ;
            }
            finally
            {
                sqlaccess.Close();
            }
        }



        internal static string getBugNameByID(string id)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = " select bug from ta_bugtype where id ='" + id + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return tmpds.Tables[0].Rows[0][0].ToString();
                }
                else return "";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        //internal static string getPointDescByID(string modelid, string bpointID)
        //{
        //    SqlAccess sqlaccess = new SqlAccess();
        //    DataSet tmpds;
        //    try
        //    {
        //        sqlaccess.Open();
        //        string tmpsql = " select BugPointDsc from TA_BugPoint where modelid ='" + modelid + "' and bugPointCode='" + bpointID + "'";
        //        tmpds = sqlaccess.OpenQuerry(tmpsql);
        //        if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
        //        {
        //            return tmpds.Tables[0].Rows[0][0].ToString();
        //        }
        //        else return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManager.Write("", ex.Message);
        //        return "";
        //    }
        //    finally
        //    {
        //        sqlaccess.Close();
        //    }
        //}

        //获取某个产品的组成项的品名
        internal static string getItem(string modelid, string ItemID)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            try
            {
                sqlaccess.Open();
                string tmpsql = "SELECT TA_Model.Name " +
                                "FROM (SELECT '" + modelid + "' AS ItemID UNION SELECT ItemID FROM TA_Structure WHERE (ID = '" + modelid + "')) tmp " +
                                "INNER JOIN TA_Model ON tmp.ItemID = TA_Model.ID WHERE (TA_Model.ID = '" + ItemID + "')";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    return tmpds.Tables[0].Rows[0][0].ToString();
                }
                else return "";
            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return "";
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        
        internal static void findNextProcedure(string process, int ProcedureID, DropDownList ddlDel)
        {

            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds = new DataSet();
            try
            {
                sqlaccess.Open();
                string tmpsql = " select ProcessConfig from TA_Procedure t where t.ID=" + ProcedureID.ToString() + "";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    StringReader stream = null;
                    XmlTextReader reader = null;
                    try
                    {
                        string xmlData = tmpds.Tables[0].Rows[0][0].ToString();

                        DataSet xmlDS = new DataSet();
                        stream = new StringReader(xmlData);
                        //从stream装载到XmlTextReader 
                        reader = new XmlTextReader(stream);
                        xmlDS.ReadXml(reader);

                        ddlDel.Items.Add(process);
                        DataTable dt = xmlDS.Tables["Connection"];
                        addNodetoDdlProcedure(ref dt, process, ref ddlDel);

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

            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
            }
            finally
            {
                sqlaccess.Close();
            }

        }

        /// <summary>
        /// 递归添加下拉框 用于删除流程状态用
        /// </summary>
        /// <param name="xmlNodeList"></param>
        /// <param name="tostr"></param>
        /// <param name="ddl"></param>
        internal static void addNodetoDdlProcedure(ref DataTable dt, string fstr, ref DropDownList ddl)
        {
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                DataRow r = dt.Rows[j];
                if (r["from"].ToString() == fstr)//添加到下拉框中
                {
                    if (!ddl.Items.Contains(ddl.Items.FindByValue(r["to"].ToString())))
                    {
                        ddl.Items.Add(r["to"].ToString());//添加项目
                    }

                    addNodetoDdlProcedure(ref dt, r["to"].ToString(), ref ddl);
                    //xmlNodeList[0].RemoveChild(xmlNodeList[0].ChildNodes[j]);
                }
            }

        }


        internal static bool passLeader(string pid)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            bool ispass = false;
            try
            {
                sqlaccess.Open();
                string tmpsql = " SELECT * from TA_Employee,TRE_Employee_Role  where TRE_Employee_Role.EmployeeID=TA_Employee.id and TRE_Employee_Role.role='班组长' and state<>2 and id='" + pid + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    ispass = true;
                }
                return ispass;

            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return false;
            }
            finally
            {
                sqlaccess.Close();
            }
        }
        /// <summary>
        /// 是否在品号表中存在mid的记录
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        internal static bool HasModel(string mid)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds;
            bool ispass = false;
            try
            {
                sqlaccess.Open();
                string tmpsql = " select * from ta_model where id='" + mid + "'";
                tmpds = sqlaccess.OpenQuerry(tmpsql);
                if (tmpds != null && tmpds.Tables[0].Rows.Count > 0)
                {
                    ispass = true;
                }
                return ispass;

            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return false;
            }
            finally
            {
                sqlaccess.Close();
            }
        }

        internal static DataSet getInforBySql(string sql)
        {
            SqlAccess sqlaccess = new SqlAccess();
            DataSet tmpds = null;

            try
            {
                sqlaccess.Open();

                tmpds = sqlaccess.OpenQuerry(sql);
                return tmpds;

            }
            catch (Exception ex)
            {
                LogManager.Write("", ex.Message);
                return null;
            }
            finally
            {
                sqlaccess.Close();
            }
        }
    }
}
