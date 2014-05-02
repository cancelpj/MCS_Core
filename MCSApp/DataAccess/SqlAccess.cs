using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MCSApp.DataAccess.LogManage;

namespace MCSApp.DataAccess
{
	/// <summary>
	/// �ṩ��SQLserver����ز�����
	/// </summary>
	public class SqlAccess
	{
		#region ���ݿ��ʼ���ֶ�

		/// <summary>
		/// get the connection string from webconfig
		/// </summary>
		//private string connectionString = ConfigurationSettings.AppSettings["ConnString"].ToString();
        private string connectionString = ConfigurationManager.ConnectionStrings["MCSConntion"].ConnectionString;
        //private string connectionString = ConfigurationManager.ConnectionStrings["hbjlConntiontest"].ConnectionString;
		/// <summary>
		/// MS-SQL Connection
		/// </summary>
		private SqlConnection connection = null;

		/// <summary>
		/// MS-SQL SqlTransaction
		/// </summary>
		private SqlTransaction transaction = null;

		/// <summary>
		/// connection is closed when begin
		/// </summary>
		private bool isOpened = false;

		/// <summary>
		/// isBeginTransaction is false;
		/// </summary>
		private bool isBeginTransaction = false;

		#endregion

		#region ���ݿ⹹��
		/// <summary>
		/// SqlAccess����
		/// </summary>
		public SqlAccess()
		{
			this.connection = new SqlConnection(this.connectionString);
		}
		public SqlAccess(string connStr)
		{
            this.connection = new SqlConnection(connStr);
		}
		#endregion

		#region ���ݿ�Ĵ򿪡��رա�����ʼ��ִ�С��ع��Ȳ�����

		/// <summary>
		/// database open
		/// </summary>
		public void Open()
		{
			try
			{
				if(!this.isOpened)
				{
					this.connection.Open();
					this.isOpened = true;
				}
			}
			catch(SqlException ex)
			{
				LogManager.Write(this, ex.ToString());
				throw new Exception(ex.Message, ex);
			}
		}

		/// <summary>
		/// database close
		/// </summary>
		public void Close()
		{
			try
			{
				if(this.isOpened)
				{
					this.connection.Close();
					this.isOpened = false;
				}
			}
			catch(SqlException ex)
			{
				LogManager.Write(this, ex.ToString());
				throw new Exception(ex.Message, ex);
			}
		}

		/// <summary>
		/// transaction begin
		/// </summary>
		public void BeginTransaction()
		{
			try
			{
				if(this.isOpened)
				{
					this.transaction = this.connection.BeginTransaction();
					this.isBeginTransaction = true;
				}
			}
			catch(SqlException ex)
			{
				LogManager.Write(this, ex.ToString());
				throw new Exception(ex.Message, ex);
			}
		}

		/// <summary>
		/// commit
		/// </summary>
		public void Commit()
		{
			try
			{
                    
				if(this.isBeginTransaction)
				{
					this.transaction.Commit();
					this.isBeginTransaction = false;
				}
			}
			catch(SqlException ex)
			{
				LogManager.Write(this, ex.ToString());
				throw new Exception(ex.Message, ex);
			}
		}


        /// <summary>
        /// PDAcommit
        /// </summary>
        public void PDACommit()
        {
            try
            {
                if (this.isBeginTransaction)
                {
                    this.transaction.Commit();
                    this.isBeginTransaction = false;
                }
            }
            catch (SqlException ex)
            {
                LogManager.Write(this, ex.ToString());
                throw new Exception(ex.Message, ex);
            }
        }

		/// <summary>
		/// roll back
		/// </summary>
		public void Rollback()
		{
			try
			{
				if(this.isBeginTransaction)
				{
					this.transaction.Rollback();
					this.isBeginTransaction = false;
				}
			}
			catch(SqlException ex)
			{
				LogManager.Write(this, ex.ToString());
				throw new Exception(ex.Message, ex);
			}
		}

		/// <summary>
		/// Select
		/// </summary>
		/// <param name="selectCommandText">SQL</param>
		/// <returns></returns>
		public DataSet OpenQuerry(string selectCommandText)
		{
			try
			{
				DataSet dataSet = new DataSet();
				SqlCommand command = new SqlCommand(selectCommandText, this.connection);
				if(this.isBeginTransaction)
				{
					command.Transaction = this.transaction;
				}
				SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
				dataAdapter.Fill(dataSet, "OpenQuerry");

				if(!this.isOpened)
				{
					this.connection.Close();
				}
				return dataSet;
			}
			catch(SqlException ex)
			{
				LogManager.Write(this, ex.ToString());
				Rollback();
				throw new Exception(ex.Message, ex);
			}
		}

        public SqlDataReader OpenSqlDataReader(string selectCommandText)
        {
            try
            {
                SqlDataReader dr = null;
                SqlCommand command = new SqlCommand(selectCommandText, this.connection);
                if (this.isBeginTransaction)
                {
                    command.Transaction = this.transaction;
                }
                ///������
                this.connection.Open();
                ///��ȡ����
                dr = command.ExecuteReader(CommandBehavior.CloseConnection);

                if (!this.isOpened)
                {
                    this.connection.Close();
                }
                return dr;
            }
            catch (SqlException ex)
            {
                LogManager.Write(this, ex.ToString());
                Rollback();
                throw new Exception(ex.Message, ex);
            }
        }

		/// <summary>
		/// Select
		/// </summary>
		/// <param name="command"></param>
		/// <returns>DataSet</returns>
		public DataSet OpenQuerry(SqlCommand command)
		{
			try
			{
				DataSet dataSet = new DataSet();
				command.Connection=this.connection;
				if(this.isBeginTransaction)
				{
					command.Transaction = this.transaction;
				}
				SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
				dataAdapter.Fill(dataSet, "OpenQuerry");

				if(!this.isOpened)
				{
					this.connection.Close();
				}
				return dataSet;
			}
			catch(SqlException ex)
			{
				LogManager.Write(this, ex.ToString());
				Rollback();
				throw new Exception(ex.Message, ex);
			}
		}

		/// <summary>
		/// INSERT, UPDATE, DELETE
		/// </summary>
		/// <param name="sqlCommand">SQL</param>
		/// <returns></returns>
		public int ExecuteQuerry(string sqlCommand)
		{
			try
			{
				if(!this.isOpened)
				{
					return 0;
				}

				if(!this.isBeginTransaction)
				{
					return 0;
				}

				SqlCommand command = new SqlCommand(sqlCommand, this.connection);
				command.Transaction = this.transaction;
				int count = command.ExecuteNonQuery();
				return count;
			}
			catch(SqlException ex)
			{
				LogManager.Write(this, ex.ToString()+ex.ErrorCode);
				Rollback();
                //throw new DBException(ex.Message,ex.ErrorCode);
                //throw new Exception(ex.Message, ex);
                throw ex;
			}
		}

		/// <summary>
		/// INSERT, UPDATE, DELETE
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public int ExecuteQuerry(SqlCommand command)
		{
			try
			{
				if(!this.isOpened)
				{
					return 0;
				}

				if(!this.isBeginTransaction)
				{
					return 0;
				}

				command.Connection=this.connection;
				command.Transaction = this.transaction;
				int count = command.ExecuteNonQuery();
				return count;
			}
			catch(SqlException ex)
			{
				LogManager.Write(this, ex.ToString());
				Rollback();
				throw new Exception(ex.Message, ex);
			}
		}
		/// <summary>
		/// INSERT, UPDATE, DELETE
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
        public int ExecuteQuerryNOTransaction(SqlCommand command)
		{
			try
			{
				if(!this.isOpened)
				{
					return 0;
				}
				command.Connection=this.connection;
				int count = command.ExecuteNonQuery();
				return count;
			}
			catch(SqlException ex)
			{
				LogManager.Write(this, ex.ToString());
				Rollback();
				throw new Exception(ex.Message, ex);
			}
		}

		public string ExecuteScalar(string sqlCommand)
		{
			try
			{
				SqlCommand command = new SqlCommand(sqlCommand, this.connection);
				command.Transaction = this.transaction;
				object result = command.ExecuteScalar();
				if(result == null)
				{
					return "";
				}
				else
				{
					return result.ToString();
				}
				
			}
			catch(SqlException ex)
			{
				LogManager.Write(this, ex.ToString());
				Rollback();
				throw new Exception(ex.Message, ex);
			}
		}



		#endregion
        /// <summary>
        /// ȡ����ص�������
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        public SqlDataAdapter getAdapter(string sqlQuery)
        {
            try
            {                
                if (!this.isOpened)
                {
                    return null;
                }

                SqlCommand cmd = new SqlCommand(sqlQuery,this.connection);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                SqlCommandBuilder cb = new SqlCommandBuilder(da);                
                da.UpdateCommand = cb.GetUpdateCommand();
                da.DeleteCommand = cb.GetDeleteCommand();
                da.InsertCommand = cb.GetInsertCommand();  

                return da;

            }
			catch(SqlException ex)
			{
				LogManager.Write(this, ex.ToString());
				Rollback();
				throw new Exception(ex.Message, ex);
			}
        }
        /// <summary>
        /// ��ѯ�ַ���������ȷ����Ľṹ
        /// </summary>
        /// <param name="sqlQuery">��ѯSQL</param>
        /// <param name="srcTable">Դ��</param>
        /// <param name="op">����</param>
        /// <returns>��Ӱ�������</returns>
        public int AutoUpdate(string sqlQuery,DataTable srcTable)
        {
            if (!this.isOpened)
            {
                return 0;
            }
            SqlDataAdapter da = getAdapter(sqlQuery);
            int i=commitToDB(da,srcTable);
            return i;
        }


        /// <summary>
        /// ��ѯ�ַ���������ȷ����Ľṹ
        /// </summary>
        /// <param name="sqlQuery">��Դ�����Ӧ������������</param>
        /// <param name="srcTable">Դ��</param>
        /// <param name="op">����</param>
        /// <returns>��Ӱ�������</returns>
        public int AutoUpdate(SqlDataAdapter daSrc, DataTable srcTable)
        {
            if (!this.isOpened)
            {
                return 0;
            }
            SqlDataAdapter da = getAdapter(daSrc.SelectCommand.CommandText);
            int i = commitToDB(da, srcTable);
            return i;
        }


        private int commitToDB(SqlDataAdapter da,DataTable dt)
        {
            int i = 0;
            try
            {
                i = da.Update(dt);
                dt.AcceptChanges();
                return i;
            }
            catch(Exception ex)
            {
                LogManager.Write(this,ex.Message);
                Rollback();
            }
            return i;
        }

		#region Procedure�洢����
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="storedProcName"></param>
		/// <param name="parameters"></param>
		/// <param name="rowAffected"></param>
		public int RunProcedure( string storedProcName, IDataParameter[] parameters) 
		{ 
			 
			try 
			{   
				int rowAffected;
				this.connection.Open();
				SqlCommand command = BuildQueryCommand( storedProcName, parameters); 
				return rowAffected = command.ExecuteNonQuery(); 
			} 
			catch(SqlException ex)
			{
				LogManager.Write(this,ex.ToString());
				throw new Exception(ex.Message, ex);
			}
			finally 
			{ 
				this.connection.Close();
			} 
		}   

		/// <summary>
		/// 
		/// </summary>
		/// <param name="storedProcName"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public SqlCommand BuildQueryCommand( string storedProcName, IDataParameter[] parameters) 
		{ 
			try
			{
				SqlCommand result = connection.CreateCommand();
				result.CommandText = storedProcName; 
				result.CommandType = CommandType.StoredProcedure;
				if(parameters==null)
				{
					return result;
				}
				else
				{
					foreach ( IDataParameter parameter in parameters ) 
					{ 
						result.Parameters.Add( parameter ); 
					}
					return result;  
				}
			}
			catch(SqlException ex)
			{
				LogManager.Write(this,ex.ToString());
				throw new Exception(ex.Message, ex);
			}
		} 

		#endregion
	}
}
