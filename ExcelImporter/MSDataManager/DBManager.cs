using System;

using System.Data.SqlClient;

namespace MSDataManager
{
    public sealed class DBManager
    {
        public static string ConnectionString = String.Empty;

        private DBManager() { }

        public static void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public static DB CreateDatabase()
        {
            return new DB(ConnectionString);
        }
    }

    public class DB
    {
        public string ConnectionString;

        public DB(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public SqlConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        public SqlCommand CreateCommand()
        {
            return new SqlConnection(ConnectionString).CreateCommand();
        }

        public SqlCommand CreateCommand(string commandText, SqlConnection connection)
        {
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = commandText;
            return cmd;
        }

        public SqlParameter CreateParameter(string parameterName, object parameterValue)
        {
            return new SqlParameter(parameterName, parameterValue);
        }
    }

    public class DBWorker
    {
        private static DB _database = null;
        static DBWorker()
        {
            try
            {
                _database = DBManager.CreateDatabase();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DB database
        {
            get { return _database; }
        }
    }
}
