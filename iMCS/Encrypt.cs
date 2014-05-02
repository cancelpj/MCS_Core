using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FINGU.MCS
{
    public class Encrypt
    {
        static Byte[] Key = { 0x88, 0x77, 0x66, 0x55, 0x44, 0x33, 0x22, 0x11 };
        static Byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        //加密函数
        static public String EncryptString(String strText)
        {
            return EncryptString(strText, Key, IV);
        }

        //解密函数
        static public String DecryptString(String strText)
        {
            return DecryptString(strText, Key, IV);
        }

        //加密函数
        static public String EncryptString(String strText, Byte[] Key, Byte[] IV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            Byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        //解密函数
        static public String DecryptString(String strText, Byte[] Key, Byte[] IV)
        {
            Byte[] inputByteArray = new byte[strText.Length];

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(strText);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(Key, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }
    }
}
