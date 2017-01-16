using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace TTechIMCore.DBAccess
{
    public class ExpenseCategsContext : DBWorker
    {
        public static IEnumerable<ExpenseCateg> LoadUserExpenseCategs(User usr)
        {
            List<ExpenseCateg> ret = new List<ExpenseCateg>();
            try
            {
                using (MySqlConnection connection = database.CreateConnection(DBMapperFactory.DBMapper[usr.DBInstance]))
                {
                    connection.Open();
                    string commandText = "SELECT ExpenseCategId, ExpenseCategName, UserId, IsDefault, OrderVal "
                        + "FROM ExpenseCategs "
                        + "WHERE UserId = @usrUserId "
                        + "ORDER BY OrderVal, ExpenseCategName";
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@usrUserId", usr.UserId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ExpenseCateg item = new ExpenseCateg();
                                item.ExpenseCategId = !reader.IsDBNull(reader.GetOrdinal("ExpenseCategId")) ? reader.GetInt32("ExpenseCategId") : 0;
                                item.ExpenseCategName = !reader.IsDBNull(reader.GetOrdinal("ExpenseCategName")) ? reader.GetString("ExpenseCategName") : String.Empty;
                                item.UserId = !reader.IsDBNull(reader.GetOrdinal("UserId")) ? reader.GetInt32("UserId") : 0;
                                item.IsDefault = !reader.IsDBNull(reader.GetOrdinal("IsDefault")) ? reader.GetInt32("IsDefault") : 0;
                                item.OrderVal = !reader.IsDBNull(reader.GetOrdinal("OrderVal")) ? reader.GetInt32("OrderVal") : 0;

                                ret.Add(item);
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected failure");
            }

            return ret;
        }

        public static void AddNewExpenseCategs(List<ExpenseCateg> list, User usr)
        {
            try
            {
                using (MySqlConnection connection = database.CreateConnection(DBMapperFactory.DBMapper[usr.DBInstance]))
                {
                    connection.Open();
                    MySqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        foreach (var item in list)
                        {
                            string commandText = "INSERT INTO ExpenseCategs "
                                + "(ExpenseCategName, UserId, IsDefault, OrderVal) "
                                + "VALUES(@itemExpenseCategName, @usrUserId, 1, @itemOrderVal)";
                            using (MySqlCommand command = database.CreateCommand(commandText, connection))
                            {
                                command.Parameters.AddWithValue("@itemExpenseCategName", item.ExpenseCategName);
                                command.Parameters.AddWithValue("@usrUserId", usr.UserId);
                                command.Parameters.AddWithValue("@itemOrderVal", item.OrderVal);

                                command.Transaction = transaction;

                                int row = command.ExecuteNonQuery();

                                if (row == 0)
                                {
                                    throw new AddNewExpenseCategsException();
                                }
                            }
                        }
                        transaction.Commit();
                    }
                    catch (AddNewExpenseCategsException ex)
                    {
                        transaction.Rollback();
                        throw ex;
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
            catch (AddNewExpenseCategsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected failure");
            }
        }

        public static void UpdateExpenseCategs(List<ExpenseCateg> list, User usr)
        {
            try
            {
                using (MySqlConnection connection = database.CreateConnection(DBMapperFactory.DBMapper[usr.DBInstance]))
                {
                    connection.Open();
                    MySqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        string availableItemIds = String.Empty;
                        foreach (var item in list)
                        {
                            if (String.IsNullOrEmpty(availableItemIds))
                            {
                                availableItemIds = "" + item.ExpenseCategId + "";
                            }
                            else
                            {
                                availableItemIds += "," + item.ExpenseCategId + "";
                            }
                        }
                        string commandText = "DELETE FROM ExpenseCategs "
                                + "WHERE ExpenseCategId NOT IN "
                                + "(" + availableItemIds + ") "
                                + "AND UserId = " + usr.UserId;
                        using (MySqlCommand command = database.CreateCommand(commandText, connection))
                        {
                            command.Transaction = transaction;

                            int row = command.ExecuteNonQuery();
                        }

                        foreach (var item in list)
                        {
                            commandText = "UPDATE ExpenseCategs "
                                + "SET ExpenseCategName = @itemExpenseCategName, "
                                + "OrderVal = @itemOrderVal "
                                + "WHERE ExpenseCategId = @itemExpenseCategId";
                            using (MySqlCommand command = database.CreateCommand(commandText, connection))
                            {
                                command.Parameters.AddWithValue("@itemExpenseCategName", item.ExpenseCategName);
                                command.Parameters.AddWithValue("@itemOrderVal", item.OrderVal);
                                command.Parameters.AddWithValue("@itemExpenseCategId", item.ExpenseCategId);

                                command.Transaction = transaction;

                                int row = command.ExecuteNonQuery();

                                if (row == 0)
                                {
                                    throw new UpdateExpenseCategsException();
                                }
                            }
                        }
                        transaction.Commit();
                    }
                    catch (UpdateExpenseCategsException ex)
                    {
                        transaction.Rollback();
                        throw ex;
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
            catch (UpdateExpenseCategsException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected failure");
            }
        }
    }

    /// <summary>
    /// A basic class for an ExpenseCateg
    /// </summary>
    public class ExpenseCateg
    {
        public ExpenseCateg()
        { }

        public int ExpenseCategId { get; set; }

        public string ExpenseCategName { get; set; }

        public int UserId { get; set; }

        public int IsDefault { get; set; }

        public int OrderVal { get; set; }
    }

    public class AddNewExpenseCategsException : Exception
    {
        public AddNewExpenseCategsException()
            : base(String.Format("Failed to add new Expense Category")) { }
    }

    public class UpdateExpenseCategsException : Exception
    {
        public UpdateExpenseCategsException()
            : base(String.Format("Failed to update Expense Category")) { }
    }
}
