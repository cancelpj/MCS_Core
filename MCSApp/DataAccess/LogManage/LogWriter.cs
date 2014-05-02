using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Configuration;

namespace MCSApp.DataAccess.LogManage
{
    /// <summary>
    /// ����lock��Monitor����,����˶��߳�д��־�ļ��������ļ���ռ�õ�����
    /// </summary>
    public static class LogWriter
    {
        static string strContents; // ��Ϣ������ߵ�����
        static bool writeFileFlag = false; // ״̬��־��Ϊtrueʱ���Զ�ȡ��Ϊfalse������д��
        static object PLACE;
        static string MSG = "";
        static public string prestr = "";//��չ��ǰ���ǰ׺����:2005-1-1_user.log
        static public void WriteLogFile()
        {
            lock (typeof(LogWriter)) // Lock ��д��ʱ��������µ��ַ��������ֶ�
            {
                if (!writeFileFlag)//������ڲ���д
                {
                    try
                    {
                        //�ȴ�����Monitor.Pulse()����
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
                    if (!System.IO.Directory.Exists(ConfigurationManager.AppSettings["LogPath"].ToString()))//�ļ��в����ڴ���
                    {
                        System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings["LogPath"].ToString());
                    }
                    StreamWriter writer = null;
                    writer = new StreamWriter(logPath, true, System.Text.Encoding.GetEncoding("GB2312"));//ֱ�������ļ�

                    writer.WriteLine(strContents);
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                writeFileFlag = false;//����writeFileFlag��־����ʾ������Ϊ�Ѿ����
                Monitor.Pulse(typeof(LogWriter));//֪ͨAddMessage()�������÷���������һ���߳���ִ�У��ȴ��У�
            }
        }
        /// <summary>
        /// �����Ϣ
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
                        //��ͬ��������ָMonitor���Enter֮��ķ������ڷ�ͬ���Ĵ�����������
                        Console.WriteLine(e);
                    }
                    catch (ThreadInterruptedException e)
                    {
                        //���߳��ڵȴ�״̬��ʱ����ֹ 
                        Console.WriteLine(e);
                    }
                }
                string message = "";
                message += "[" + DateTime.Now.ToString() + "]";	//
                message += "[" + PLACE.ToString() + "]";
                message += MSG;
                strContents = message;
                writeFileFlag = true;
                Monitor.Pulse(typeof(LogWriter)); //֪ͨ����һ���߳������ڵȴ���WriteLogFile()����
            }
        }
        /// <summary>
        /// �趨��Ϣ
        /// </summary>
        /// <param name="place">��ϢԴ��λ��</param>
        /// <param name="msg">��Ϣ�ַ���</param>
        static public void SetMessage(object place, string msg)
        {
            PLACE = place;
            MSG = msg;
        }
    }
}
