using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Configuration;

namespace MCSApp.DataAccess.LogManage
{
	/// <summary>
	/// 多线程写操作日志
    /// 完成日:2007-05-24
    /// 完成人:门永奎
	/// </summary>
	public class LogManager
	{   
		public LogManager()
		{
		}
        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="place">源对象</param>
        /// <param name="msg">写的信息字符串</param>
		private static void WriteErrorLog(object place, string msg)
		{
            //LogWriter logWriter = new LogWriter();
            LogWriter.SetMessage(place, msg);
            Thread threadAdd = new Thread(new ThreadStart(LogWriter.AddMessage));
            threadAdd.Start();
            Thread threadWrite = new Thread(new ThreadStart(LogWriter.WriteLogFile));
            threadWrite.Start();  
		}
        /// <summary>
        /// 写操作日志
        /// </summary>
        /// <param name="place">源对象</param>
        /// <param name="msg">写的信息字符串</param>
        public static void WriteOprationLog(object place, string msg)
        {
            //LogWriter logmanage = new LogWriter();
            LogWriter.SetMessage(place, msg);
            LogWriter.prestr = "_UserOP";
            Thread threadAdd = new Thread(new ThreadStart(LogWriter.AddMessage));
            threadAdd.Start();
            Thread threadWrite = new Thread(new ThreadStart(LogWriter.WriteLogFile));
            threadWrite.Start();
            //多线操操作同步测试通过
            //Thread[] athreads=new Thread[100];
            //Thread[] wthreads=new Thread[100];
            //for (int i = 0; i < 100;i++ )
            //{
            //    LogManage logmanage = new LogManage();
            //    logmanage.SetMessage(place, msg);

            //    athreads[i] = new Thread(new ThreadStart(logmanage.AddMessage));
            //    athreads[i].Start();

            //    wthreads[i] = new Thread(new ThreadStart(logmanage.WriteLogFile));
            //    wthreads[i].Start();
            //}
        }
        /// <summary>
        /// 写错误日志
        /// </summary>
        /// <param name="place">源对象</param>
        /// <param name="msg">写的信息字符串</param>
        public static void Write(object place, string msg)
        {
             WriteErrorLog(place,msg);
        }
        /// <summary>
        /// 写操作日志
        /// </summary>
        /// <param name="place">源对象</param>
        /// <param name="msg">写的信息字符串</param>
        private static void WriteOPLog(object place, string msg)
        {
             WriteOprationLog(place,msg);
        }
	}
}
