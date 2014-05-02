using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Database 数据库操作类
/// </summary>
public class Database
{
    #region 数据成员： 默认的连接字符串

    /// <summary>
    /// 默认的数据库连接字符串
    /// </summary>
    private static string conntext = ConfigurationManager.ConnectionStrings["MCSConntion"].ConnectionString;

    #endregion

    #region 函数： 获取满足查询条件的记录总数

    /// <summary>
    /// 获取满足查询条件的记录总数
    /// </summary>
    /// <param name="sqltext">查询的SQL语句</param>
    /// <returns>满足条件的记录总数</returns>
    public static int Count(string sqltext)
    {
        SqlConnection conn = new SqlConnection(conntext);
        DataSet ds = new DataSet();
        SqlDataAdapter sqld = new SqlDataAdapter(sqltext, conn);
        sqld.Fill(ds, "tab");
        int count = ds.Tables["tab"].Rows.Count;
        return count;
    }
    /// <summary>
    /// 获取满足查询条件的记录总数(可设置数据库)
    /// </summary>
    /// <param name="conntext">数据库连接字符串</param>
    /// <param name="sqltext">查询的SQL语句</param>
    /// <returns>满足条件的记录总数</returns>
    public static int new_Count(string conntext, string sqltext)
    {
        SqlConnection conn = new SqlConnection(conntext);
        DataSet ds = new DataSet();
        SqlDataAdapter sqld = new SqlDataAdapter(sqltext, conn);
        sqld.Fill(ds, "tab");
        int count = ds.Tables["tab"].Rows.Count;
        return count;
    }

    #endregion

    #region 函数： 通过查询返回DataTable

    /// <summary>
    /// 通过查询返回DataTable
    /// </summary>
    /// <param name="sqltext">传入的sql语句</param>
    /// <returns>返回DataTable</returns>
    public static DataTable DataTable(string sqltext)
    {
        SqlConnection conn = new SqlConnection(conntext);
        DataSet ds = new DataSet();
        SqlDataAdapter sqld = new SqlDataAdapter(sqltext, conn);
        sqld.Fill(ds, "tab");
        return ds.Tables["tab"];
    }
    /// <summary>
    /// 通过查询返回DataTable(可设置数据库)
    /// </summary>
    /// <param name="sqltext">传入的sql语句</param>
    /// <param name="conntext">数据库连接字符串</param>
    /// <returns>返回DataTable</returns>
    public static DataTable new_DataTable(string conntext, string sqltext)
    {
        SqlConnection conn = new SqlConnection(conntext);
        DataSet ds = new DataSet();
        SqlDataAdapter sqld = new SqlDataAdapter(sqltext, conn);
        sqld.Fill(ds, "tab");
        return ds.Tables["tab"];
    }

    #endregion

    #region 函数： 通过查询返回0行的DataRow

    /// <summary>
    /// 通过查询返回0行的DataRow
    /// </summary>
    /// <param name="sqltext">传入的sql语句</param>
    /// <returns>返回DataRow</returns>
    public static DataRow DataRow(string sqltext)
    {
        SqlConnection conn = new SqlConnection(conntext);
        DataSet ds = new DataSet();
        SqlDataAdapter sqld = new SqlDataAdapter(sqltext, conn);
        sqld.Fill(ds, "tab");
        DataRow drow = ds.Tables["tab"].Rows[0];
        return drow;
    }
    /// <summary>
    /// 通过查询返回0行的DataRow(可设置数据库)
    /// </summary>
    /// <param name="sqltext">传入的sql语句</param>
    /// <param name="conntext">数据库连接字符串</param>
    /// <returns>返回DataRow</returns>
    public static DataRow new_DataRow(string conntext, string sqltext)
    {
        SqlConnection conn = new SqlConnection(conntext);
        DataSet ds = new DataSet();
        SqlDataAdapter sqld = new SqlDataAdapter(sqltext, conn);
        sqld.Fill(ds, "tab");
        DataRow drow = ds.Tables["tab"].Rows[0];
        return drow;
    }

    #endregion

    #region 函数： 通过查询返回指定的数据项

    /// <summary>
    /// 通过查询返回指定的数据项
    /// </summary>
    /// <param name="sqltext">传入的sql语句</param>
    /// <returns>返回0行0列的数据项</returns>
    public static string DataCode(string sqltext)
    {
        SqlConnection conn = new SqlConnection(conntext);
        DataSet ds = new DataSet();
        SqlDataAdapter sqld = new SqlDataAdapter(sqltext, conn);
        sqld.Fill(ds, "tab");
        string drow = "";
        if (ds.Tables["tab"].Rows.Count != 0)
        {
            drow = ds.Tables["tab"].Rows[0][0].ToString();
        }
        return drow;
    }
    /// <summary>
    /// 通过查询返回指定的数据项(可设置数据库)
    /// </summary>
    /// <param name="conntext">数据库连接字符串</param>
    /// <param name="sqltext">传入的sql语句</param>
    /// <returns>返回0行0列的数据项</returns>
    public static string new_DataCode(string conntext, string sqltext)
    {
        SqlConnection conn = new SqlConnection(conntext);
        DataSet ds = new DataSet();
        SqlDataAdapter sqld = new SqlDataAdapter(sqltext, conn);
        sqld.Fill(ds, "tab");
        string drow = "";
        if (ds.Tables["tab"].Rows.Count != 0)
        {
            drow = ds.Tables["tab"].Rows[0][0].ToString();
        }
        return drow;
    }

    /// <summary>
    /// 通过查询返回指定的数据项
    /// </summary>
    /// <param name="sqltext">传入的sql语句</param>
    /// <param name="index">指定列的索引</param>
    /// <returns>返回0行索引列的数据项</returns>
    public static string DataCode(string sqltext, int index)
    {
        SqlConnection conn = new SqlConnection(conntext);
        DataSet ds = new DataSet();
        SqlDataAdapter sqld = new SqlDataAdapter(sqltext, conn);
        sqld.Fill(ds, "tab");
        string drow = "";
        if (ds.Tables["tab"].Rows.Count != 0)
        {
            drow = ds.Tables["tab"].Rows[0][index].ToString();
        }
        return drow;
    }
    /// <summary>
    /// 通过查询返回指定的数据项(可设置数据库)
    /// </summary>
    /// <param name="conntext">数据库连接字符串</param>
    /// <param name="sqltext">传入的sql语句</param>
    /// <param name="index">指定列的索引</param>
    /// <returns>返回0行索引列的数据项</returns>
    public static string new_DataCode(string conntext, string sqltext, int index)
    {
        SqlConnection conn = new SqlConnection(conntext);
        DataSet ds = new DataSet();
        SqlDataAdapter sqld = new SqlDataAdapter(sqltext, conn);
        sqld.Fill(ds, "tab");
        string drow = "";
        if (ds.Tables["tab"].Rows.Count != 0)
        {
            drow = ds.Tables["tab"].Rows[0][index].ToString();
        }
        return drow;
    }

    /// <summary>
    /// 通过查询返回指定的数据项
    /// </summary>
    /// <param name="sqltext">传入的sql语句</param>
    /// <param name="colname">指定列的列名</param>
    /// <returns>返回0行指定列的数据项</returns>
    public static string DataCode(string sqltext, string colname)
    {
        SqlConnection conn = new SqlConnection(conntext);
        DataSet ds = new DataSet();
        SqlDataAdapter sqld = new SqlDataAdapter(sqltext, conn);
        sqld.Fill(ds, "tab");
        string drow = "";
        if (ds.Tables["tab"].Rows.Count != 0)
        {
            drow = ds.Tables["tab"].Rows[0][colname].ToString();
        }
        return drow;
    }
    /// <summary>
    /// 通过查询返回指定的数据项(可设置数据库)
    /// </summary>
    /// <param name="conntext">数据库连接字符串</param>
    /// <param name="sqltext">传入的sql语句</param>
    /// <param name="colname">指定列的列名</param>
    /// <returns>返回0行指定列的数据项</returns>
    public static string new_DataCode(string conntext, string sqltext, string colname)
    {
        SqlConnection conn = new SqlConnection(conntext);
        DataSet ds = new DataSet();
        SqlDataAdapter sqld = new SqlDataAdapter(sqltext, conn);
        sqld.Fill(ds, "tab");
        string drow = "";
        if (ds.Tables["tab"].Rows.Count != 0)
        {
            drow = ds.Tables["tab"].Rows[0][colname].ToString();
        }
        return drow;
    }

    #endregion

    #region 函数： 执行SQL语句

    /// <summary>
    /// 执行SQL语句
    /// </summary>
    /// <param name="sqltext">需要执行的SQL语句</param>
    /// <returns>指示SQL语句是否执行成功</returns>
    public static bool Execute(string sqltext)
    {
        SqlConnection conn = new SqlConnection(conntext);
        SqlCommand cmd = new SqlCommand();
        try
        {
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = sqltext;
            cmd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            conn.Close();
        }
    }
    /// <summary>
    /// 执行SQL语句
    /// </summary>
    /// <param name="conntext">数据库连接字符串</param>
    /// <param name="sqltext">需要执行的SQL语句</param>
    /// <returns>指示SQL语句是否执行成功</returns>
    public static bool new_Execute(string conntext, string sqltext)
    {
        SqlConnection conn = new SqlConnection(conntext);
        SqlCommand cmd = new SqlCommand();
        try
        {
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = sqltext;
            cmd.ExecuteNonQuery();
            return true;
        }
        catch
        {
            return false;
        }
        finally
        {
            conn.Close();
        }
    }

    #endregion

}
