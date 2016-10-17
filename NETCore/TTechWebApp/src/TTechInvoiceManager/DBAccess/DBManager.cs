using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using MySql.Data.MySqlClient;

namespace NTWebApp.DBAccess
{
    public sealed class DBManager
    {
        protected static string ConnectionString = String.Empty;

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

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public MySqlConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
        }

        public MySqlCommand CreateCommand()
        {
            return new MySqlConnection(ConnectionString).CreateCommand();
        }

        public MySqlCommand CreateCommand(string commandText, MySqlConnection connection)
        {
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = commandText;
            return cmd;
        }

        public MySqlParameter CreateParameter(string parameterName, object parameterValue)
        {
            return new MySqlParameter(parameterName, parameterValue);
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
