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
    public class WeekExpensesContext : DBWorker
    {
        public static IEnumerable<WeekExpense> LoadUserWeekExpenses(User usr, DateTime startTs, DateTime endTs)
        {
            List<WeekExpense> ret = new List<WeekExpense>();
            try
            {
                using (MySqlConnection connection = database.CreateConnection(DBMapperFactory.DBMapper[usr.DBInstance]))
                {
                    connection.Open();
                    string commandText = "SELECT expCat.ExpenseCategId, expCat.ExpenseCategName, expCat.OrderVal, ROUND(SUM(exp.Cost), 2) AS TotalCost, exp.UserId "
                        + "FROM Expenses exp "
                        + "INNER JOIN CA001_IM.ExpenseCategs expCat ON exp.ExpenseCategId = expCat.ExpenseCategId "
                        + "AND exp.UserId = expCat.UserId "
                        + "WHERE exp.UserId = @usrUserId "
                        + "AND exp.Time > DATE_SUB(STR_TO_DATE(@startTs,'%m/%d/%Y'), INTERVAL 1 SECOND) "
                        + "AND exp.Time < DATE_ADD(STR_TO_DATE(@endTs,'%m/%d/%Y'), INTERVAL 1 DAY) "
                        + "GROUP BY expCat.ExpenseCategId, expCat.ExpenseCategName, expCat.OrderVal, exp.UserId "
                        + "ORDER BY expCat.OrderVal";
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@usrUserId", usr.UserId);
                        command.Parameters.AddWithValue("@startTs", startTs.ToString("MM/dd/yyyy"));
                        command.Parameters.AddWithValue("@endTs", endTs.ToString("MM/dd/yyyy"));

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                WeekExpense item = new WeekExpense();
                                item.UserId = usr.UserId;
                                item.ExpenseCategId = !reader.IsDBNull(reader.GetOrdinal("ExpenseCategId")) ? reader.GetInt32("ExpenseCategId") : 0;
                                item.ExpenseCategName = !reader.IsDBNull(reader.GetOrdinal("ExpenseCategName")) ? reader.GetString("ExpenseCategName") : String.Empty;
                                item.OrderVal = !reader.IsDBNull(reader.GetOrdinal("OrderVal")) ? reader.GetInt32("OrderVal") : 0;
                                item.TotalCost = !reader.IsDBNull(reader.GetOrdinal("TotalCost")) ? reader.GetDouble("TotalCost") : 0.0;

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
    }

    /// <summary>
    /// A basic class for an Weekly Expense
    /// </summary>
    public class WeekExpense
    {
        public int UserId { get; set; }

        public int ExpenseCategId { get; set; }

        public string ExpenseCategName { get; set; }

        public int OrderVal { get; set; }

        public double TotalCost { get; set; }
    }
}
