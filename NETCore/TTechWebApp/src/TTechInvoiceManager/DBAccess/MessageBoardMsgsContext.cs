using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using NTWebApp.Messaging;

namespace NTWebApp.DBAccess
{
    public class MessageBoardMsgsContext : DBWorker
    {
        public static void AddNewMessageBoardMessage(MessageBoardMsg msg, User usr)
        {
            string userFullName = String.Empty;
            if(usr == null)
            {
                usr = new User();
                usr.UserId = msg.UserId;
                usr.Email = msg.Email;
                userFullName = msg.Name;
            }
            else
            {
                userFullName = String.IsNullOrEmpty((usr.FirstName + " " + usr.LastName).Trim()) ? usr.UserName : (usr.FirstName + " " + usr.LastName).Trim();
            }

            try
            {
                using (MySqlConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    MySqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        string commandText = "INSERT INTO MessageBoard "
                            + "(UserId, Message, Time, Email, Name) "
                            + "VALUES "
                            + "(" + usr.UserId + ", '" + msg.Message + "', STR_TO_DATE('" + msg.Time.ToString("MM/dd/yyyy") + "','%m/%d/%Y'), '" + usr.Email + "', '" + userFullName + "')";
                        using (MySqlCommand command = database.CreateCommand(commandText, connection))
                        {
                            command.Transaction = transaction;

                            int row = command.ExecuteNonQuery();

                            if (row == 0)
                            {
                                throw new AddNewMessageBoardMessageException();
                            }
                        }

                        transaction.Commit();
                    }
                    catch (AddNewMessageBoardMessageException ex)
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

                string subject = "New Message from " + usr.Email + " through Invoice Manager Message Board";
                string body = "--- Message ---"
                    + Environment.NewLine
                    + Environment.NewLine
                    + Environment.NewLine
                    + msg.Message
                    + Environment.NewLine
                    + Environment.NewLine
                    + "[User - " + userFullName + "]"
                    + Environment.NewLine
                    + Environment.NewLine
                    + Environment.NewLine
                    + "--- End ---"
                    + Environment.NewLine + "Invoice Manager";

                try
                {
                    Email.SendEmail(subject, body, usr.Email, userFullName, MessageBoardMsg.HostEmail, MessageBoardMsg.HostName, null, null);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (AddNewMessageBoardMessageException ex)
            {
                throw ex;
            }
            catch (EmailHandlingException ex)
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
    /// A basic class for an MessageBoard Message
    /// </summary>
    public class MessageBoardMsg
    {
        public static string HostName = "Noah Tong";
        public static string HostEmail = "noah089736@gmail.com";

        public MessageBoardMsg()
        { }

        public int MessageId { get; set; }

        public int UserId { get; set; }

        public string Message { get; set; }

        public DateTime Time { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
    }

    public class AddNewMessageBoardMessageException : Exception
    {
        public AddNewMessageBoardMessageException()
            : base(String.Format("Failed to send message through system")) { }
    }
}
