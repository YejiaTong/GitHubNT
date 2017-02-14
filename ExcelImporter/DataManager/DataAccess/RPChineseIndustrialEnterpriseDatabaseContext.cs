using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace DataManager.DataAccess
{
    public class RPChineseIndustrialEnterpriseDatabaseContext : DBWorker
    {
        public static void AddNewRecords(List<Dictionary<string, object>> list, string connectionString)
        {
            try
            {
                using (MySqlConnection connection = database.CreateConnection(connectionString))
                {
                    connection.Open();
                    MySqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        foreach (var item in list)
                        {
                            string commandText = "INSERT INTO RPChineseIndustrialEnterpriseDatabase "
                                + "(Column001, Column002, Column003, Column004, Column005, "
                                + "Column006, Column007, Column008, Column009, Column010) "
                                + "VALUES "
                                + "(@Column001, @Column002, @Column003, @Column004, @Column005, "
                                + "@Column006, @Column007, @Column008, @Column009, @Column010)";
                            using (MySqlCommand command = database.CreateCommand(commandText, connection))
                            {
                                command.Parameters.AddWithValue("@Column001", item["001"]);
                                command.Parameters.AddWithValue("@Column002", item["002"]);
                                command.Parameters.AddWithValue("@Column003", item["003"]);
                                command.Parameters.AddWithValue("@Column004", item["004"]);
                                command.Parameters.AddWithValue("@Column005", item["005"]);
                                command.Parameters.AddWithValue("@Column006", item["006"]);
                                command.Parameters.AddWithValue("@Column007", item["007"]);
                                command.Parameters.AddWithValue("@Column008", item["008"]);
                                command.Parameters.AddWithValue("@Column009", item["009"]);
                                command.Parameters.AddWithValue("@Column010", item["010"]);

                                command.Transaction = transaction;

                                int row = command.ExecuteNonQuery();

                                if (row == 0)
                                {
                                    throw new Exception("Failed to add records into Database table");
                                }
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
