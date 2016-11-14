using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace NTWebApp.DBAccess
{
    public class ExpensesContext : DBWorker
    {
        public static IEnumerable<Expense> LoadUserExpenses(User usr)
        {
            List<Expense> ret = new List<Expense>();
            try
            {
                using (MySqlConnection connection = database.CreateConnection(DBMapperFactory.DBMapper[usr.DBInstance]))
                {
                    connection.Open();
                    string commandText = "SELECT exp.ExpenseId, exp.ExpenseName, expCat.ExpenseCategId, expCat.ExpenseCategName, exp.Cost, exp.Time, exp.Address, exp.Description, exp.UserId "
                        + "FROM Expenses exp "
                        + "INNER JOIN ExpenseCategs expCat ON exp.ExpenseCategId = expCat.ExpenseCategId "
                        + "AND exp.UserId = expCat.UserId "
                        + "WHERE exp.UserId = @usrUserId "
                        + "ORDER BY exp.Time";
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@usrUserId", usr.UserId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Expense item = new Expense();
                                item.ExpenseId = !reader.IsDBNull(reader.GetOrdinal("ExpenseId")) ? reader.GetInt32("ExpenseId") : 0;
                                item.Name = !reader.IsDBNull(reader.GetOrdinal("ExpenseName")) ? reader.GetString("ExpenseName") : String.Empty;
                                item.ExpenseCategId = !reader.IsDBNull(reader.GetOrdinal("ExpenseCategId")) ? reader.GetInt32("ExpenseCategId") : 0;
                                item.Cost = !reader.IsDBNull(reader.GetOrdinal("Cost")) ? reader.GetDouble("Cost") : 0.0;
                                item.Time = !reader.IsDBNull(reader.GetOrdinal("Time")) ? reader.GetDateTime("Time") : DateTime.MinValue;
                                item.Address = !reader.IsDBNull(reader.GetOrdinal("Address")) ? reader.GetString("Address") : String.Empty;
                                item.Description = !reader.IsDBNull(reader.GetOrdinal("Description")) ? reader.GetString("Description") : String.Empty;
                                item.UserId = !reader.IsDBNull(reader.GetOrdinal("UserId")) ? reader.GetInt32("UserId") : 0;

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

        public static IEnumerable<Expense> LoadUserExpensesPaged(User usr, DateTime startTs, DateTime endTs, long lastItemId, int pageSize)
        {
            List<Expense> ret = new List<Expense>();
            try
            {
                using (MySqlConnection connection = database.CreateConnection(DBMapperFactory.DBMapper[usr.DBInstance]))
                {
                    connection.Open();
                    string commandText = "SELECT exp.ExpenseId, exp.ExpenseName, expCat.ExpenseCategId, expCat.ExpenseCategName, exp.Cost, exp.Time, exp.Address, exp.Description, exp.UserId "
                        + "FROM Expenses exp "
                        + "INNER JOIN ExpenseCategs expCat ON exp.ExpenseCategId = expCat.ExpenseCategId "
                        + "AND exp.UserId = expCat.UserId "
                        + "WHERE exp.UserId = @usrUserId "
                        + "AND exp.Time > DATE_SUB(STR_TO_DATE(@startTs,'%m/%d/%Y'), INTERVAL 1 SECOND) "
                        + "AND exp.Time < DATE_ADD(STR_TO_DATE(@endTs,'%m/%d/%Y'), INTERVAL 1 DAY) "
                        + "ORDER BY exp.Time "
                        + "LIMIT " + pageSize;
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@usrUserId", usr.UserId);
                        command.Parameters.AddWithValue("@startTs", startTs.ToString("MM/dd/yyyy"));
                        command.Parameters.AddWithValue("@endTs", endTs.ToString("MM/dd/yyyy"));

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Expense item = new Expense();
                                item.ExpenseId = !reader.IsDBNull(reader.GetOrdinal("ExpenseId")) ? reader.GetInt32("ExpenseId") : 0;
                                item.Name = !reader.IsDBNull(reader.GetOrdinal("ExpenseName")) ? reader.GetString("ExpenseName") : String.Empty;
                                item.ExpenseCategId = !reader.IsDBNull(reader.GetOrdinal("ExpenseCategId")) ? reader.GetInt32("ExpenseCategId") : 0;
                                item.Cost = !reader.IsDBNull(reader.GetOrdinal("Cost")) ? reader.GetDouble("Cost") : 0.0;
                                item.Time = !reader.IsDBNull(reader.GetOrdinal("Time")) ? reader.GetDateTime("Time") : DateTime.MinValue;
                                item.Address = !reader.IsDBNull(reader.GetOrdinal("Address")) ? reader.GetString("Address") : String.Empty;
                                item.Description = !reader.IsDBNull(reader.GetOrdinal("Description")) ? reader.GetString("Description") : String.Empty;
                                item.UserId = !reader.IsDBNull(reader.GetOrdinal("UserId")) ? reader.GetInt32("UserId") : 0;

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

        public static IEnumerable<Expense> LoadUserExpensesPagedNav(User usr, DateTime startTs, DateTime endTs, int pageSize, int pageIndex)
        {
            List<Expense> ret = new List<Expense>();
            try
            {
                using (MySqlConnection connection = database.CreateConnection(DBMapperFactory.DBMapper[usr.DBInstance]))
                {
                    int startIndex = pageSize * (pageIndex - 1);

                    connection.Open();
                    string commandText = "SELECT exp.ExpenseId, exp.ExpenseName, expCat.ExpenseCategId, expCat.ExpenseCategName, exp.Cost, exp.Time, exp.Address, exp.Description, exp.UserId "
                        + "FROM Expenses exp "
                        + "INNER JOIN ExpenseCategs expCat ON exp.ExpenseCategId = expCat.ExpenseCategId "
                        + "AND exp.UserId = expCat.UserId "
                        + "WHERE exp.UserId = @usrUserId "
                        + "AND exp.Time > DATE_SUB(STR_TO_DATE(@startTs,'%m/%d/%Y'), INTERVAL 1 SECOND) "
                        + "AND exp.Time < DATE_ADD(STR_TO_DATE(@endTs,'%m/%d/%Y'), INTERVAL 1 DAY) "
                        + "ORDER BY exp.Time "
                        + "LIMIT " + startIndex + ", " + pageSize;
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@usrUserId", usr.UserId);
                        command.Parameters.AddWithValue("@startTs", startTs.ToString("MM/dd/yyyy"));
                        command.Parameters.AddWithValue("@endTs", endTs.ToString("MM/dd/yyyy"));

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Expense item = new Expense();
                                item.ExpenseId = !reader.IsDBNull(reader.GetOrdinal("ExpenseId")) ? reader.GetInt32("ExpenseId") : 0;
                                item.Name = !reader.IsDBNull(reader.GetOrdinal("ExpenseName")) ? reader.GetString("ExpenseName") : String.Empty;
                                item.ExpenseCategId = !reader.IsDBNull(reader.GetOrdinal("ExpenseCategId")) ? reader.GetInt32("ExpenseCategId") : 0;
                                item.Cost = !reader.IsDBNull(reader.GetOrdinal("Cost")) ? reader.GetDouble("Cost") : 0.0;
                                item.Time = !reader.IsDBNull(reader.GetOrdinal("Time")) ? reader.GetDateTime("Time") : DateTime.MinValue;
                                item.Address = !reader.IsDBNull(reader.GetOrdinal("Address")) ? reader.GetString("Address") : String.Empty;
                                item.Description = !reader.IsDBNull(reader.GetOrdinal("Description")) ? reader.GetString("Description") : String.Empty;
                                item.UserId = !reader.IsDBNull(reader.GetOrdinal("UserId")) ? reader.GetInt32("UserId") : 0;

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

        public static void AddNewExpenses(List<Expense> list, User usr)
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
                            string commandText = "INSERT INTO Expenses "
                                + "(ExpenseName, ExpenseCategId, Cost, Time, Address, Description, UserId) "
                                + "VALUES "
                                + "(@itemName, @itemExpenseCategId, @itemCost, STR_TO_DATE(@itemTime,'%m/%d/%Y'), @itemAddress, @itemDescription, @usrUserId)";
                            using (MySqlCommand command = database.CreateCommand(commandText, connection))
                            {
                                command.Parameters.AddWithValue("@itemName", item.Name);
                                command.Parameters.AddWithValue("@itemExpenseCategId", item.ExpenseCategId);
                                command.Parameters.AddWithValue("@itemCost", item.Cost);
                                command.Parameters.AddWithValue("@itemTime", item.Time.ToString("MM/dd/yyyy"));
                                command.Parameters.AddWithValue("@itemAddress", item.Address);
                                command.Parameters.AddWithValue("@itemDescription", item.Description);
                                command.Parameters.AddWithValue("@usrUserId", usr.UserId);

                                command.Transaction = transaction;

                                int row = command.ExecuteNonQuery();

                                if (row == 0)
                                {
                                    throw new AddNewExpensesException();
                                }
                            }
                        }
                        transaction.Commit();
                    }
                    catch (AddNewExpensesException ex)
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
            catch (AddNewExpensesException ex)
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
    /// A basic class for an Expense
    /// </summary>
    public class Expense
    {
        public Expense()
        { }

        public int ExpenseId { get; set; }

        public int ExpenseCategId { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public double Cost { get; set; }

        public string Address { get; set; }

        public DateTime Time { get; set; }

        public string Description { get; set; }
    }

    public class AddNewExpensesException : Exception
    {
        public AddNewExpensesException()
            : base(String.Format("Failed to add new Expenses")) { }
    }
}
