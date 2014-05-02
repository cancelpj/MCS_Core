using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Configuration;

namespace MCSApp.DataAccess.LogManage
{
    /// <summary>
    /// 采用lock加Monitor机制,解决了多线程写日志文件出现在文件被占用的问题
    /// </summary>
    public static class LogWriter
    {
        static string strContents; // 信息对象里边的内容
        static bool writeFileFlag = false; // 状态标志，为true时可以读取，为false则正在写入
        static object PLACE;
        static string MSG = "";
        static public string prestr = "";//扩展名前面的前缀　如:2005-1-1_user.log
        static public void WriteLogFile()
        {
            lock (typeof(LogWriter)) // Lock 在写的时候不能添加新的字符到内容字段
            {
                if (!writeFileFlag)//如果现在不可写
                {
                    try
                    {
                        //等待调用Monitor.Pulse()方法
                        Monitor.Wait(typeof(LogWriter));
                    }
                    catch (SynchronizationLockException e)
                    {
                        throw new Exception(e.Message, e);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        throw new Exception(e.Message, e);
                    }
                }

                try
                {
                    string sDate = DateTime.Now.ToShortDateString();
                    sDate = sDate.Replace("/", "");

                    string logPath = "";

                    logPath = ConfigurationManager.AppSettings["LogPath"].ToString();

                    logPath = logPath + sDate + prestr + ".log";
                    if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["LogPath"].ToString()))//文件夹不存在创建
                    {
                        System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["LogPath"].ToString());
                    }
                    StreamWriter writer = null;
                    writer = new StreamWriter(logPath, true, System.Text.Encoding.GetEncoding("GB2312"));//直接生成文件

                    writer.WriteLine(strContents);
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                writeFileFlag = false;//重置writeFileFlag标志，表示消费行为已经完成
                Monitor.Pulse(typeof(LogWriter));//通知AddMessage()方法（该方法在另外一个线程中执行，等待中）
            }
        }
        /// <summary>
        /// 添加信息
        /// </summary>
        static public void AddMessage()
        {
            lock (typeof(LogWriter))
            {
                if (writeFileFlag)
                {
                    try
                    {
                        Monitor.Wait(typeof(LogWriter));
                    }
                    catch (SynchronizationLockException e)
                    {
                        //当同步方法（指Monitor类除Enter之外的方法）在非同步的代码区被调用
                        Console.WriteLine(e);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        //当线程在等待状态的时候中止 
                        Console.WriteLine(e);
                    }
                }
                string message = "";
                message += "[" + DateTime.Now.ToString() + "]";	//
                message += "[" + PLACE.ToString() + "]";
                message += MSG;
                strContents = message;
                writeFileFlag = true;
                Monitor.Pulse(typeof(LogWriter)); //通知另外一个线程中正在等待的WriteLogFile()方法
            }
        }
        /// <summary>
        /// 设定消息
        /// </summary>
        /// <param name="place">信息源的位置</param>
        /// <param name="msg">消息字符串</param>
        static public void SetMessage(object place, string msg)
        {
            PLACE = place;
            MSG = msg;
        }
    }
}
