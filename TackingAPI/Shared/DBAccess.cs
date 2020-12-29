using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TackingAPI.Shared
{
    public class DBAccess
    {
        private IDbCommand cmd = new SqlCommand();
        private string strConnectionString = "";
        private bool handleErrors = false;
        private string strLastError = "";

        public DBAccess()
        {
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"];
            this.strConnectionString = settings.ConnectionString;
            SqlConnection connection = new SqlConnection
            {
                ConnectionString = this.strConnectionString
            };
            this.cmd.Connection = connection;
            this.cmd.CommandType = CommandType.StoredProcedure;
        }

        public void AddParameter(IDataParameter param)
        {
            this.cmd.Parameters.Add(param);
        }

        public void AddParameter(string paramname, object paramvalue)
        {
            SqlParameter parameter = new SqlParameter(paramname, paramvalue);
            this.cmd.Parameters.Add(parameter);
        }

        private void Close()
        {
            this.cmd.Connection.Close();
        }

        public void Dispose()
        {
            this.cmd.Dispose();
        }

        public DataSet ExecuteDataSet()
        {
            SqlDataAdapter adapter = null;
            DataSet dataSet = null;
            try
            {
                adapter = new SqlDataAdapter
                {
                    SelectCommand = (SqlCommand)this.cmd
                };
                dataSet = new DataSet();
                adapter.Fill(dataSet);
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return dataSet;
        }

        public DataSet ExecuteDataSet(string commandtext)
        {
            DataSet set = null;
            try
            {
                this.cmd.CommandText = commandtext;
                set = this.ExecuteDataSet();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return set;
        }

        public int ExecuteNonQuery()
        {
            int num = -1;
            try
            {
                this.Open();
                num = this.cmd.ExecuteNonQuery();
                this.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return num;
        }

        public int ExecuteNonQuery(string commandtext)
        {
            int num = -1;
            try
            {
                this.cmd.CommandText = commandtext;
                this.cmd.CommandType = CommandType.StoredProcedure;
                num = this.ExecuteNonQuery();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return num;
        }

        public SqlDataReader executeReader(string str)
        {
            SqlCommand command = new SqlCommand();
            this.ConnectionString = new Connection().getconnection();
            command.CommandText = str;
            this.Open();
            return command.ExecuteReader();
        }

        public IDataReader ExecuteReader()
        {
            IDataReader reader = null;
            try
            {
                this.Open();
                reader = this.cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return reader;
        }

        public IDataReader ExecuteReader(string commandtext)
        {
            IDataReader reader = null;
            try
            {
                this.cmd.CommandText = commandtext;
                reader = this.ExecuteReader();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return reader;
        }

        public object ExecuteScalar()
        {
            object obj2 = null;
            try
            {
                this.Open();
                obj2 = this.cmd.ExecuteScalar();
                this.Close();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return obj2;
        }

        public object ExecuteScalar(string commandtext)
        {
            object obj2 = null;
            try
            {
                this.cmd.CommandText = commandtext;
                obj2 = this.ExecuteScalar();
            }
            catch (Exception exception1)
            {
                throw exception1;
            }
            return obj2;
        }

        private void Open()
        {
            this.cmd.Connection.Open();
        }

        public string CommandText
        {
            get
            {
                return this.cmd.CommandText;
            }
            set
            {
                this.cmd.CommandText = value;
                this.cmd.Parameters.Clear();
            }
        }

        public IDataParameterCollection Parameters
        {
            get
            {
                return this.cmd.Parameters;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.strConnectionString;
            }
            set
            {
                this.strConnectionString = value;
            }
        }

        public bool HandleExceptions
        {
            get
            {
                return this.handleErrors;
            }
            set
            {
                this.handleErrors = value;
            }
        }

        public string LastError
        {
            get
            {
                return this.strLastError;
            }
        }
    }
}