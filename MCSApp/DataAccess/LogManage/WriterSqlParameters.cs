using System;
using System.Data;
using System.Data.SqlClient;

namespace MCSApp.DataAccess.LogManage
{
	/// <summary>
	/// WriterSqlParameters 的摘要说明。
	/// </summary>
	public class WriterSqlParameters
	{
		public WriterSqlParameters(SqlCommand command)
		{
			string sql=command.CommandText;
			SqlParameterCollection parmC=command.Parameters;
			LogManager.Write(this,"SqlParameter:");
			foreach(SqlParameter parm in  parmC)
			{
				int count=sql.IndexOf(parm.ParameterName);
				LogManager.Write(this,"DbType:"+parm.DbType.ToString());
				LogManager.Write(this,"Direction:"+parm.Direction.ToString());
				LogManager.Write(this,"ParameterName:"+parm.ParameterName.ToString());
				LogManager.Write(this,"Size:"+parm.Size.ToString());
				LogManager.Write(this,"SourceColumn:"+parm.SourceColumn.ToString());
				LogManager.Write(this,"SqlDbType:"+parm.SqlDbType.ToString());
				if(parm.SqlDbType==SqlDbType.Char || parm.SqlDbType==SqlDbType.DateTime ||
					parm.SqlDbType==SqlDbType.NChar || parm.SqlDbType==SqlDbType.NText ||
					parm.SqlDbType==SqlDbType.NVarChar || parm.SqlDbType==SqlDbType.SmallDateTime ||
					parm.SqlDbType==SqlDbType.Text || parm.SqlDbType==SqlDbType.VarChar )
				{
					sql=sql.Substring(0,count)+"'"+parm.Value+"'"+sql.Substring(count+parm.ParameterName.Length);
				}
				else
				{
					sql=sql.Substring(0,count)+parm.Value+sql.Substring(count+parm.ParameterName.Length);
				}
				LogManager.Write(this,"Value:"+parm.Value.ToString());
			}
			
			LogManager.Write(this,"SQL:");
			LogManager.Write(this,sql);
		}
	}
}
