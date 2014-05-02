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
using MCSApp.WSSecurity;
using FINGU.MCS;

namespace MCSApp
{
    public partial class _Default : System.Web.UI.Page
    {           
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtname.Focus();
                //Hashtable ht = (Hashtable)ConfigurationManager.GetSection("superAdminSection");
                //Response.Write(ht.Count.ToString() + "<br/>");
                //foreach (DictionaryEntry key in ht)
                //{
                //    Response.Write(key.Key.ToString() + ":" + key.Value.ToString() + "<br/>");
                //}
            }
        }

        protected void IBtn_Click(object sender, ImageClickEventArgs e)
        {
            SqlAccess sqlAccess = null;

            try
            {
                DataSet userData = new DataSet();
                sqlAccess = new SqlAccess();
                sqlAccess.Open();

                string ID = this.txtname.Text.Trim();
                string password = this.txtpassword.Text.Trim();
                string[] Roles = null;

                string outStr;
                string queryString = "select role from ta_role ";//这里保证所有权限的用户均能进入此页面
                DataSet dataSet = sqlAccess.OpenQuerry(queryString);
                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    Roles = new string[dataSet.Tables[0].Rows.Count];
                    for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                    {
                        Roles[i] = dataSet.Tables[0].Rows[i][0].ToString();
                    }
                }

                Byte[] Key = { 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88 };
                Byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                Hashtable ht = (Hashtable)ConfigurationManager.GetSection("superAdminSection");//从超级管理权限配置中取
                foreach (DictionaryEntry key in ht)
                {
                    if (ID == key.Key.ToString() && password == Encrypt.DecryptString(key.Value.ToString(), Key, IV))
                    {
                        Session["ID"] = ID;
                        Session["UserName"] = " 超级管理员："+ID;
                        Session["Password"] = password;
                        Session["IsSuperAdmin"] = true;
                        Session["Login"] = true;
                        Session["Roles"] = Roles;
                        break;
                    }
                }
                if (SessionUser.IsSuperAdmin)
                {
                    Response.Redirect("Main/index.aspx",false); return;
                }

                if (!Methods.CheckUser(this, ID, password, Roles, out outStr, false))//验证用户的存在与合法性
                {
                    this.lblMsg.Text = outStr;
                    return;    
                }


                //验证权限
                queryString = "SELECT ID, Name,  Password,state FROM TA_Employee  where ID='" + ID + "' and Password= '" + Encrypt.EncryptString(password) + "' and state !='2'";

                dataSet = sqlAccess.OpenQuerry(queryString);

                if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow row = dataSet.Tables[0].Rows[0];

                    Session["ID"] = row["id"].ToString();
                    Session["UserName"] = row["Name"].ToString();
                    Session["Account"] = row["ID"].ToString();
                    Session["Password"] = Encrypt.DecryptString(row["Password"].ToString());
                    Session["Login"] = true;
                    Session["UserHostName"] = this.Request.UserHostName;
                    Session["IP"] = this.Request.UserHostAddress;
                    Session["State"] = row["State"].ToString();
                    queryString = " select role from TRE_Employee_Role where EmployeeID='" + ID + "'";//找其角色并保存session变量中

                    dataSet = sqlAccess.OpenQuerry(queryString);
                    Roles = null;

                    if (dataSet != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        Roles = new string[dataSet.Tables[0].Rows.Count];
                        for (int i = 0; i < dataSet.Tables[0].Rows.Count; i++)
                        {
                            Roles[i] = dataSet.Tables[0].Rows[i]["role"].ToString();
                        }
                    }
                    Session["Roles"] = Roles;
                    //FormsAuthentication.SetAuthCookie(TextBoxUserName.Text, false);
                    Response.Redirect("Main/index.aspx");
                }
                else
                {
                    Response.Write("<script>alert('工号密码不匹配！请重试！');</script>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('服务器错误！');</script>");
                LogManager.Write(this, ex.ToString());
            }
            finally
            {
                sqlAccess.Close();
            }

        }
    }
}
