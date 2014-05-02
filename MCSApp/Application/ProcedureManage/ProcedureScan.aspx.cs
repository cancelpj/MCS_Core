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
using Coolite.Ext.Web;
using System.Collections.Generic;
using System.Xml;
//029650583A0110040030
namespace MCSApp.Application.ProcedureManage
{
    public partial class ProcedureScan:PageBase//:System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = SessionUser.ID;
            string password = SessionUser.Password;
            string[] Roles = { "作业员" };
            string outStr;
            if (!Methods.CheckUser(this, ID, password, Roles, out outStr, true))
            {
                return;
            }


            if (!Ext.IsAjaxRequest)
            {
                Cache.Remove("data");
                if (SessionUser.IsSuperAdmin)
                {
                    Ext.Msg.Alert("提示", "超级管理员虽然具备权限，但无需扫描条码。<br>它不属于任何班组，因此并不会成功执行扫描转序操作。").Show();
                    return;
                }
                Msg_init();
            }
        }


        /// <summary>
        /// 根据用户信息获取部分信息
        /// </summary>
        protected void Msg_init()
        {

            string Id = SessionUser.ID;//"00002";//
            SqlAccess sqlaccess = new SqlAccess();

            string sql = @"select TA_Group.Name,TA_Group.PlanID
                        from dbo.TRE_Group_Employee,dbo.TA_Group,dbo.TA_Employee
                        where TA_Employee.ID='" + Id + @"'
                          and TA_Employee.ID=TRE_Group_Employee.EmployeeID 
                          and TRE_Group_Employee.GroupID=TA_Group.ID";
            DataRow row = null;
            try
            {
                row = sqlaccess.OpenQuerry(sql).Tables[0].Rows[0];
            }
            catch { }

            if (row != null)
            {
                Label1.Text = row["Name"].ToString();
                Hidden1.Text = row["Name"].ToString();

                Label2.Text = row["PlanID"].ToString();
                Hidden2.Text = row["PlanID"].ToString();
            }
            string[,] groups = { { "预制", "装配", "调试", "电检", "包装" }, { "预制", "装配.总装", "调试.老化", "电检", "包装" } };
            int groupIndex = 0;
            for (int i = 0; i < 5; i++)
            {
                if (row["Name"].ToString().Contains(groups[0,i])) groupIndex = i;
            }

            foreach (string p in groups[1, groupIndex].Split('.'))
            {
                ComboBox1.Items.Add(new Coolite.Ext.Web.ListItem(p, p));
            }



            //寻找新定义的工序名称
            DataSet ds = new DataSet();
            string tmp = Database.DataCode("select ProcessConfig from dbo.TA_Procedure,TC_PlanProcedure where ProcedureID=TA_Procedure.ID AND TC_PlanProcedure.PlanID='" + Hidden2.Text.ToString() + "'");
            XmlTextReader reader = new XmlTextReader(tmp, XmlNodeType.Document, null);
            ds.ReadXml(reader);
            foreach (DataRow row1 in ds.Tables[0].Rows)
            {
                if (row1["Range"].ToString().Contains(groups[0, groupIndex]))
                {
                    groups[1, groupIndex] = groups[1, groupIndex] + "." + row1["Name"].ToString();
                    bool mark=false;
                    foreach (var a in ComboBox1.Items)
                    {
                        if (a.Value == row1["Name"].ToString()) mark = true;
                    }
                    if (!mark) ComboBox1.Items.Add(new Coolite.Ext.Web.ListItem(row1["Name"].ToString(), row1["Name"].ToString()));
                }
            }


            //Label3.Text = groups[1, groupIndex].Replace(".", "或");
            //Hidden3.Text = groups[1, groupIndex].Replace(".", "或");
        }


        /// <summary>
        /// 刷新列表时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MyData_Refresh(object sender, StoreRefreshDataEventArgs e)
        {
            if ((List<object[]>)Cache["data"] == null) return;
            this.Store2.DataSource = (List<object[]>)Cache["data"];
            this.Store2.DataBind();
        }



        /// <summary>
        /// 扫描时触发
        /// </summary>
        /// <param name="Barcode"></param>
        [AjaxMethod]
        public void test1(string Barcode)
        {

            if (ComboBox1.SelectedItem.Value == "待定" || ComboBox2.SelectedItem.Value == "待定")
            {
                Ext.Msg.Alert("提示", "您必须选择工序和流向信息！").Show();
                return;
            }

            //用户登陆名
            string Id = SessionUser.ID;//"00002";//
            //获取工序
            //string[] process_probable = Hidden3.Text.Split('或');
            string msg;

            WSProcedureCtrl.ProcedureCtrl c = new MCSApp.WSProcedureCtrl.ProcedureCtrl();
            WSProductTrace.ProductTrace P = new MCSApp.WSProductTrace.ProductTrace();

            //如果品号未通过
            if (!c.CheckModel(Barcode.Substring(0, 9), Id))
            {
                Ext.Msg.Alert("错误", "当前产品不是正在生产的产品。").Show();
                return;
            }




            ////这时还需要收集工艺信息
            //if (Hidden4.Text == "待定")
            //{


            //    //第一步：判断是否为新入工序的产品

            //    if (!c.CheckProcedure(Barcode, string.Empty, 0, out msg) && msg == "根据输入的条形码未查询到对应的产品")
            //    {
            //        P.SaveProduct(Id, Barcode, Barcode.Substring(0, 9), "", out msg);
            //    }

            //    //确定可能正确的工序
            //    for (int i = 0; i < process_probable.Length; i++)
            //    {
            //        if (c.CheckProcedure(Barcode, process_probable[i], 0, out msg))
            //        {
            //            Label3.Text = process_probable[i];
            //            Hidden3.Text = process_probable[i];

            //            Label4.Text = "入";
            //            Hidden4.Text = "入";
            //            break;
            //        }
            //        if (c.CheckProcedure(Barcode, process_probable[i], 1, out msg))
            //        {
            //            Label3.Text = process_probable[i];
            //            Hidden3.Text = process_probable[i];

            //            Label4.Text = "出";
            //            Hidden4.Text = "出";
            //            break;
            //        }
            //    }

            //    if (Hidden4.Text == "待定")
            //    {
            //        Ext.Msg.Alert("错误", msg).Show();
            //        return;
            //    }


            if (Hidden_s.Value.ToString() != "")
            {
                Hidden_s.Value = string.Empty;
                //自动提取已完工的数据清单
                {
                    List<object[]> data = (List<object[]>)Cache["data"];
                    if (data == null) data = new List<object[]>();


                    string sql1 = "select ProductID, Name from dbo.TB_ProcedureHistory,dbo.TA_Employee where ProductID like '" + Barcode.Substring(0, 9) + "A%' and Process ='" + ComboBox1.SelectedItem.Value + "'";
                    if (ComboBox2.SelectedItem.Value == "出") sql1 += " and Result=0";
                    sql1 += " and TA_Employee.ID=EmployeeID";

                    SqlAccess sqlAccess = new SqlAccess();
                    DataTable mytab = sqlAccess.OpenQuerry(sql1).Tables[0];
                    foreach (DataRow row in mytab.Rows)
                    {
                        int id = (data.Count + 1);
                        string Barcode1 = row["ProductID"].ToString();
                        string ModelId = row["ProductID"].ToString().Substring(0, 9);
                        string PlanId = string.Empty;
                        string Process = ComboBox1.SelectedItem.Value;
                        string User = row["Name"].ToString();
                        {
                            SqlAccess sqlAccess1 = new SqlAccess();
                            DataRow myrow;
                            try
                            {
                                myrow = sqlAccess.OpenQuerry("select * from TC_PlanProcedure where ModelId= '" + ModelId + "'").Tables[0].Rows[0];
                                PlanId = myrow["PlanId"].ToString();
                            }
                            catch { }
                        }
                        data.Add(new object[] { id, Barcode1, PlanId, ModelId, Process, User });
                    }

                    Cache["data"] = data;
                }
            }


            //}



            //如果是新产品
            if (!c.CheckProcedure(Barcode, string.Empty, 0, out msg) && msg == "根据输入的条形码未查询到对应的产品")
            {
                P.SaveProduct(Id, Barcode, Barcode.Substring(0, 9), "", out msg);
            }



            if (!c.CheckProcedure(Barcode, ComboBox1.SelectedItem.Value, ComboBox2.SelectedItem.Value == "入" ? 0 : 1, out msg))
            {
                Ext.Msg.Alert("错误", msg).Show();
                return;
            }

            if (ComboBox2.SelectedItem.Value == "入")
            {
                if (!c.SaveProcessBegin(Barcode, ComboBox1.SelectedItem.Value, Id, DateTime.Now, out msg)) Ext.Msg.Alert("工序保存错误", msg);
            }
            if (ComboBox2.SelectedItem.Value == "出")
            {
                if (!c.SaveProcessEnd(Barcode, ComboBox1.SelectedItem.Value, 0, string.Empty, string.Empty, DateTime.Now, out msg)) Ext.Msg.Alert("工序保存错误", msg);
            }




            //下面是填充已扫描的列表数据--------------------------------------------------------------------------------------------------------------
            {
                List<object[]> data = (List<object[]>)Cache["data"];
                if (data == null) data = new List<object[]>();

                int id = (data.Count + 1);
                string ModelId = Barcode.Substring(0, 9);
                string PlanId = string.Empty;
                string Process = ComboBox1.SelectedItem.Value;
                string User = SessionUser.Name;

                SqlAccess sqlAccess = new SqlAccess();
                DataRow myrow;
                try
                {
                    myrow = sqlAccess.OpenQuerry("select * from TC_PlanProcedure where ModelId= '" + ModelId + "'").Tables[0].Rows[0];
                }
                catch { myrow = null; }
                if (myrow != null)
                {
                    PlanId = myrow["PlanId"].ToString();
                    data.Add(new object[] { id, Barcode, PlanId, ModelId, Process, User });
                    Cache["data"] = data;
                }
                else
                {
                    Ext.Msg.Alert("警告", Barcode + "并不是正在生产的产品。").Show();
                }
                Cache["data"] = data;
                this.Store2.DataSource = data;
                this.Store2.DataBind();
            }
        }


        


    }
}
