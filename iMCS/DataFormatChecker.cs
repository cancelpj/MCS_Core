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
using System.Xml;

namespace FINGU.MCS
{
    public class DataFormatChecker
    {
        static public bool CheckInfo(DataSet ds, string[] field)
        {
            DataTable dt = ds.Tables["Info"];

            if (dt == null)
                return false;

            if (dt.Columns.Count != field.Length)
                return false;

            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName != field[col.Ordinal])
                    return false;
            }

            return true;
        }

        static public bool CheckFieldName(DataSet ds)
        {
            DataTable dtFieldName = ds.Tables["FieldName"];
            DataTable dtData = ds.Tables["Data"];

            if (dtFieldName == null || dtData == null)
                return false;

            if (dtFieldName.Columns.Count != dtData.Columns.Count)
                return false;

            foreach (DataColumn col in dtFieldName.Columns)
            {
                if (col.ColumnName != dtData.Columns[col.Ordinal].ColumnName)
                    return false;
            }

            return true;
        }

        static public bool CheckFormat(String Data)
        {
            if (Data.Contains("<?xml"))
            {
                DataSet ds = new DataSet();
                try
                {
                    XmlTextReader reader = new XmlTextReader(Data, XmlNodeType.Document, null);
                    ds.ReadXml(reader);
                }
                catch (Exception e)
                {
                    throw new Exception("数据[Data]格式不正确：" + e.Message);
                }

                string[] field = { "Content", "Ver", "Description" };
                if (!CheckInfo(ds, field))
                {
                    throw new Exception("数据[Data]格式不正确：" + "[Info]段出错");
                }

                if (!CheckFieldName(ds))
                {
                    throw new Exception("数据[Data]格式不正确：" + "[FieldName]段出错");
                }
            }

            return true;
        }

        static public bool CheckCalFormat(String Data)
        {
            DataSet ds = new DataSet();
            try
            {
                XmlTextReader reader = new XmlTextReader(Data, XmlNodeType.Document, null);
                ds.ReadXml(reader);

            }
            catch (Exception e)
            {
                throw new Exception("数据[Data]格式不正确" + e.Message);
            }

            string[] field = { "InstrumentModel", "TestItem", "Ver", "Description" };
            if (!CheckInfo(ds, field))
            {
                throw new Exception("数据[Data]格式不正确：" + "[Info]段出错");
            }

            if (!CheckFieldName(ds))
            {
                throw new Exception("数据[Data]格式不正确：" + "[FieldName]段出错");
            }

            return true;
        }
    }
}
