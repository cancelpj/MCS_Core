using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace MCSApp.Common
{
    public static class SessionUser
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public static string ID
        {
            get 
            { return HttpContext.Current.Session["ID"] == null ? "" : HttpContext.Current.Session["ID"].ToString();}
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public static string Name
        {
            get { return HttpContext.Current.Session["UserName"]==null?"":HttpContext.Current.Session["UserName"].ToString(); }
        }
        /// <summary>
        /// 帐号
        /// </summary>
        public static string Account
        {
            get { return HttpContext.Current.Session["Account"]==null?"":HttpContext.Current.Session["Account"].ToString(); }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public static string Password
        {
            get { return HttpContext.Current.Session["Password"]==null?"":HttpContext.Current.Session["Password"].ToString(); }
        }
        /// <summary>
        /// 地区
        /// </summary>
        public static string State
        {
            get { return HttpContext.Current.Session["state"]==null?"":HttpContext.Current.Session["state"].ToString(); }
        }
        /// <summary>
        /// 单位
        /// </summary>
        public static string Company
        {
            get { return HttpContext.Current.Session["Company"]==null?"":HttpContext.Current.Session["Company"].ToString(); }
        }
        /// <summary>
        /// 部门
        /// </summary>
        public static string Department
        {
            get { return HttpContext.Current.Session["Department"]==null?"":HttpContext.Current.Session["Department"].ToString(); }
        }
        /// <summary>
        /// 权限码
        /// </summary>
        public static string[] Roles
        {
            get { return HttpContext.Current.Session["Roles"] == null ? null : (string[])HttpContext.Current.Session["Roles"]; }
        }
        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        public static bool IsSuperAdmin
        {
            get { return (bool)(HttpContext.Current.Session["IsSuperAdmin"]==null?false:true); }
        }
        /// <summary>
        /// 权限码
        /// </summary>
        public static bool IsLogin
        {
            get { return (bool)(HttpContext.Current.Session["Login"] == null ? false : true); }
        }

        /// <summary>
        /// 任务码
        /// </summary>
        public static string Task
        {
            get { return (HttpContext.Current.Session["Task"] == null ? "N" : "Y"); }
        }
        /// <summary>
        /// 用户IP地址
        /// </summary>
        public static string IP
        {
            get { return (HttpContext.Current.Session["IP"] == null ? "" : HttpContext.Current.Session["IP"].ToString()); }
        }
        /// <summary>
        /// 用户电脑主机名
        /// </summary>
        public static string UserHostName
        {
            get { return (HttpContext.Current.Session["UserHostName"] == null ? "" : HttpContext.Current.Session["UserHostName"].ToString()); }
        }
       //判断需保存的文件名和路径是否在session中存在。
        public static bool GetFileUrl(string fileUrl)
        {
            try 
            {
               //List<string> dinosaurs = new List<string>();
               bool bfind = false;
               List<string> urls = ( List<string>) HttpContext.Current.Session["fileUrlList"];
               foreach (string url in urls)
               {
                   if (url.Equals(fileUrl))
                   {
                       bfind = true;
                       break;
                   }
               }
               return bfind;
            }
            catch
            {
                return false;
            }
        }
       //保存需要编辑的文件名和路径在session中。
        public static bool PutFileUrl(string fileUrl)
        {
            List<string> urls = ( List<string>) HttpContext.Current.Session["fileUrlList"];
            if (urls == null)
            {
                urls = new List<string>();
                HttpContext.Current.Session["fileUrlList"] = urls;
            }
            bool bfind = false;
            foreach (string url in urls)
            {
                if (url.Equals(fileUrl))
                {
                    bfind = true;
                    break;
                }
            }
            if (!bfind)
                urls.Add(fileUrl);
            return true;
        }





        /// <summary>
        /// 用户的详细信息
        /// </summary>
        public static string DetailInfo
        {
            get { return SessionUser.UserHostName
                +SessionUser.IP
                + SessionUser.Company 
                + SessionUser.Department 
                + SessionUser.Name 
                + "(用户ID:" + SessionUser.ID 
                + ";帐号:" + SessionUser.Account 
                +")";
            }
        }

    }
}
