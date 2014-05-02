using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Configuration;

namespace FINGU.MCS
{
    /// <summary>
    /// 安全性接口：此接口函数接收调用方输入的帐号、密码及期望角色，经过认证后向调用方返回一个bool值指示权限验证是否成功，如不成功还将在输出参数中说明权限验证失败的原因。
    /// 可能的权限验证失败的原因包括：帐号不存在、密码不正确、帐号已停用、不具有期望权限。
    /// </summary>
    [WebService(Namespace = "http://fingu.com/mcs")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    public class Security : System.Web.Services.WebService
    {
        protected SqlConnection sqlConnectionMain;
        protected SqlCommand sqlCommandMain;
        protected SqlDataReader DataReaderMain;

        public Security()
        {
            string strConn = ConfigurationManager.ConnectionStrings["mcsdbConnectionString"].ConnectionString;
            this.sqlConnectionMain = new SqlConnection();
            this.sqlConnectionMain.ConnectionString = strConn;

            this.sqlCommandMain = new SqlCommand();
            this.sqlCommandMain.Connection = sqlConnectionMain;

        }

        [WebMethod(Description = "欢迎，欢迎，热烈欢迎！")]
        public string HelloWorld()
        {
            string oMsg = "Hello World";
            //string[] Roles = { "物料管理员", "作业员" };
            //bool ret = CheckRight("07033", "test", Roles, out oMsg);

            //bool ret = ChangePassword("00000", "newtest", "test", out oMsg);
            //try
            //{
            //    string original = "test";

            //    string aaaaaa = Encrypt.EncryptString(original);
            //    string bbbbbb = Encrypt.DecryptString(aaaaaa);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Error: {0}", e.Message);
            //}

            //ProductTrace iPT = new ProductTrace();
            //iPT.GetProduct("123", out oMsg);

            //ProcedureCtrl iPC = new ProcedureCtrl();
            //bool ret = iPC.CheckModelByBarCode("029070041A1088008800 ", "456", 1);
            //ret = iPC.RepairProduct("8032000008160012");

            //string Data = "<?xml version=\"1.0\"?><NewDataSet><Info><Content>IP3测试</Content><ver>1</ver><Description>这是IP3测试数据</Description></Info>";
            ////string Data = "<?xml version=\"1.0\"?><NewDataSet><Info><Content>IP3测试</Content><Content>1</Content><Content>这是IP3测试数据</Content></Info>";
            //Data += "<fieldname><field1>字段1</field1><field2>字段2</field2><field3>字段3</field3><field4>字段4</field4></fieldname>";
            //Data += "<Data><field1>数据1</field1><field2>数据2</field2><field3>数据3</field3><field4>数据4</field4></Data>";
            //Data += "</NewDataSet>";
            //try
            //{
            //    DataFormatChecker.CheckFormat(Data);
            //}
            //catch (Exception e)
            //{
            //    oMsg = e.Message;
            //    return "false";
            //}

            ProcedureCtrl iPC = new ProcedureCtrl();
            //bool ret = iPC.CheckProcedure("029070041A1088008800", "VSWR告警测试", 0, out oMsg);
            oMsg = iPC.GetRange("029070041A1088008800", "VSWR告警测试");
            ////bool ret = iPC.SaveProcess("8032000008160019", "装配", "00001", 1, "1", null, DateTime.Now, DateTime.Now.AddHours(2.3), out oMsg);
            ////bool ret = iPC.SaveProcessEnd("8032000008160019", "装配", 0, "1", "1", DateTime.Now, out oMsg);
            //DataSet ds = iPC.GetProcedure("8032000008160011");
            //if (ds != null)
            //{
            //    ds.WriteXml(@"c:\123.xml");
            //}

            //string tmp = iPC.GetProcessGraph("8032000008160011");
            oMsg = "Hello World";
            return oMsg;
            //return tmp;
        }

        [WebMethod(Description = "审核权限")]
        public bool CheckRight(string UID, string PSW, string[] Roles, out string oMsg)
        {
            try
            {
                sqlConnectionMain.Open();

                string sql;

                sql = "SELECT ID, Password, State FROM TA_Employee WHERE (ID = @ID)";
                sqlCommandMain.CommandText = sql;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.VarChar, 50, "ID"));
                sqlCommandMain.Parameters["@ID"].Value = UID;

                DataReaderMain = sqlCommandMain.ExecuteReader();
                if (DataReaderMain.Read())
                {
                    string ID = DataReaderMain.GetString(DataReaderMain.GetOrdinal("ID"));
                    string Password = DataReaderMain.GetString(DataReaderMain.GetOrdinal("Password"));
                    int State = DataReaderMain.GetInt32(DataReaderMain.GetOrdinal("State"));

                    try
                    {
                        Password = Encrypt.DecryptString(Password);

                        if (Password == PSW)
                        {
                            if (State != 1)
                            {
                                oMsg = "帐号已停用";
                                return false;
                            }
                        }
                        else
                        {
                            oMsg = "密码不正确";
                            return false;
                        }
                    }
                    catch
                    {
                        oMsg = "密码不正确";
                        return false;
                    }
                }
                else
                {
                    oMsg = "帐号不存在";
                    return false;
                }

                //判断角色
                sql = "SELECT EmployeeID, Role FROM TRE_Employee_Role WHERE (EmployeeID = @EmployeeID)";
                sqlCommandMain.CommandText = sql;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@EmployeeID", System.Data.SqlDbType.VarChar, 50, "EmployeeID"));
                sqlCommandMain.Parameters["@EmployeeID"].Value = UID;

                DataReaderMain.Close();
                DataReaderMain = sqlCommandMain.ExecuteReader();
                while (DataReaderMain.Read())
                {
                    foreach (string Role in Roles)
                    {
                        if (Role == DataReaderMain.GetString(DataReaderMain.GetOrdinal("Role")))
                        {
                            oMsg = string.Empty;
                            DataReaderMain.Close();
                            return true;
                        }
                    }
                }

                oMsg = "不具有期望权限";
                DataReaderMain.Close();
                return false;
            }
            finally
            {
                sqlConnectionMain.Close();
            }
        }

        [WebMethod(Description = "修改密码")]
        public bool ChangePassword(string UID, string OldPSW, string NewPSW, out string oMsg)
        {
            if (NewPSW.Length > 20)
            {
                oMsg = "新密码超长（密码长度限定在20个字符以内）";
                return false;
            }

            try
            {
                sqlConnectionMain.Open();

                string sql;

                sql = "SELECT ID, Password FROM TA_Employee WHERE (ID = @ID)";
                sqlCommandMain.CommandText = sql;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.VarChar, 50, "ID"));
                sqlCommandMain.Parameters["@ID"].Value = UID;

                DataReaderMain = sqlCommandMain.ExecuteReader();
                if (DataReaderMain.Read())
                {
                    string ID = DataReaderMain.GetString(DataReaderMain.GetOrdinal("ID"));
                    string Password = DataReaderMain.GetString(DataReaderMain.GetOrdinal("Password"));

                    try
                    {
                        Password = Encrypt.DecryptString(Password);

                        if (Password != OldPSW)
                        {
                            oMsg = "原密码不正确";
                            return false;
                        }
                    }
                    catch
                    {
                        oMsg = "原密码不正确";
                        return false;
                    }
                }
                else
                {
                    oMsg = "帐号不存在";
                    return false;
                }

                //改密码
                sql = "UPDATE TA_Employee SET Password = @Password  WHERE (ID = @ID)";
                sqlCommandMain.CommandText = sql;
                sqlCommandMain.Parameters.Clear();
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@ID", System.Data.SqlDbType.VarChar, 50, "ID"));
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Password", System.Data.SqlDbType.VarChar, 50, "Password"));
                sqlCommandMain.Parameters["@ID"].Value = UID;
                sqlCommandMain.Parameters["@Password"].Value = Encrypt.EncryptString(NewPSW);

                DataReaderMain.Close();
                if (sqlCommandMain.ExecuteNonQuery() == 1)
                {
                    oMsg = string.Empty;
                    return true;
                }

                oMsg = "修改密码时出现未知错误";
                return false;
            }
            finally
            {
                sqlConnectionMain.Close();
            }
        }

        [WebMethod(Description = "验证版本")]
        public bool CheckVersion(string Name, string Version, out string oMsg)
        {
            try
            {
                sqlConnectionMain.Open();

                string sql;

                sql = "SELECT Name, Version FROM TB_SoftwareVersion WHERE (Name = @Name)";
                sqlCommandMain.CommandText = sql;
                sqlCommandMain.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Name", System.Data.SqlDbType.VarChar, 50, "Name"));
                sqlCommandMain.Parameters["@Name"].Value = Name;

                DataReaderMain = sqlCommandMain.ExecuteReader();
                if (DataReaderMain.Read())
                {
                    string Ver = DataReaderMain.GetString(DataReaderMain.GetOrdinal("Version"));

                    if (Ver == Version)
                    {
                        oMsg = string.Empty;
                        return true;
                    }
                    else
                    {
                        oMsg = "版本不正确，当前使用版本：" + Version + "，应使用版本：" + Ver;
                        return false;
                    }
                }
                else
                {
                    oMsg = "程序名不存在";
                    return false;
                }

            }
            finally
            {
                DataReaderMain.Close();
                sqlConnectionMain.Close();
            }
        }

        [WebMethod(Description = "加密字符串")]
        public string EncryptString(String strText)
        {
            Byte[] Key = { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88 };
            Byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            return Encrypt.EncryptString(strText, Key, IV);
        }
    }
}
