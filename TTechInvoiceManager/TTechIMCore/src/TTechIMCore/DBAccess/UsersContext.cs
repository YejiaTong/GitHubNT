using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

using MySql.Data.MySqlClient;

using TTechIMCore.Messaging;

namespace TTechIMCore.DBAccess
{
    public class UsersContext : DBWorker
    {
        public static IEnumerable<User> LoadAllUsers()
        {
            List<User> ret = new List<User>();
            try
            {
                using (MySqlConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    string commandText = "SELECT usr.UserId, usr.UserName, usr.Email, usr.FirstName, usr.LastName, usr.Address, "
                        + "usr.PostalCode, usr.Gender, usr.IsActive, usr.Password, usr.SecurityToken, usr.Description, "
                        + "usr.ProfilePhotoUrl, usr.DBInstance, sm.SiteMapController, sm.SiteMapView "
                        + "FROM InvoiceManager.Users usr "
                        + "LEFT JOIN InvoiceManager.SiteMap sm ON usr.DefaultView = sm.SiteMapId";
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User item = new User();
                                item.UserId = !reader.IsDBNull(reader.GetOrdinal("UserId")) ? reader.GetInt32("UserId") : 0;
                                item.UserName = !reader.IsDBNull(reader.GetOrdinal("UserName")) ? reader.GetString("UserName") : String.Empty;
                                item.Email = !reader.IsDBNull(reader.GetOrdinal("Email")) ? reader.GetString("Email") : String.Empty;
                                item.FirstName = !reader.IsDBNull(reader.GetOrdinal("FirstName")) ? reader.GetString("FirstName") : String.Empty;
                                item.LastName = !reader.IsDBNull(reader.GetOrdinal("LastName")) ? reader.GetString("LastName") : String.Empty;
                                item.Address = !reader.IsDBNull(reader.GetOrdinal("Address")) ? reader.GetString("Address") : String.Empty;
                                item.PostalCode = !reader.IsDBNull(reader.GetOrdinal("PostalCode")) ? reader.GetString("PostalCode") : String.Empty;
                                item.Gender = !reader.IsDBNull(reader.GetOrdinal("Gender")) ? reader.GetString("Gender") : String.Empty;
                                item.IsActive = !reader.IsDBNull(reader.GetOrdinal("IsActive")) ? reader.GetInt32("IsActive") : 0;
                                item.Description = !reader.IsDBNull(reader.GetOrdinal("Description")) ? reader.GetString("Description") : String.Empty;
                                item.ProfilePhotoUrl = !reader.IsDBNull(reader.GetOrdinal("ProfilePhotoUrl")) ? reader.GetString("ProfilePhotoUrl") : String.Empty;
                                item.DBInstance = !reader.IsDBNull(reader.GetOrdinal("DBInstance")) ? reader.GetString("DBInstance") : String.Empty;
                                item.DefaultController = !reader.IsDBNull(reader.GetOrdinal("SiteMapController")) ? reader.GetString("SiteMapController") : String.Empty;
                                item.DefaultView = !reader.IsDBNull(reader.GetOrdinal("SiteMapView")) ? reader.GetString("SiteMapView") : String.Empty;

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

        public static User ValidateUser(User usr)
        {
            User ret = new User();
            bool existed = false;
            try
            {
                using (MySqlConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    string commandText = "SELECT usr.UserId, usr.UserName, usr.Email, usr.FirstName, usr.LastName, usr.Address, "
                        + "usr.PostalCode, usr.Gender, usr.IsActive, usr.Password, usr.SecurityToken, usr.Description, "
                        + "usr.ProfilePhotoUrl, usr.DBInstance, sm.SiteMapController, sm.SiteMapView "
                        + "FROM InvoiceManager.Users usr "
                        + "LEFT JOIN InvoiceManager.SiteMap sm ON usr.DefaultView = sm.SiteMapId ";
                    if (!String.IsNullOrEmpty(usr.UserName))
                    {
                        commandText += "WHERE usr.UserName = '" + usr.UserName + "' OR usr.Email = '" + usr.UserName + "'";
                    }
                    else if (!String.IsNullOrEmpty(usr.Email))
                    {
                        commandText += "WHERE usr.UserName = '" + usr.Email + "' OR usr.Email = '" + usr.Email + "'";
                    }
                    else
                    {
                        throw new UserNotFoundException();
                    }

                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ret.UserId = !reader.IsDBNull(reader.GetOrdinal("UserId")) ? reader.GetInt32("UserId") : 0;
                                ret.UserName = !reader.IsDBNull(reader.GetOrdinal("UserName")) ? reader.GetString("UserName") : String.Empty;
                                ret.Email = !reader.IsDBNull(reader.GetOrdinal("Email")) ? reader.GetString("Email") : String.Empty;
                                ret.FirstName = !reader.IsDBNull(reader.GetOrdinal("FirstName")) ? reader.GetString("FirstName") : String.Empty;
                                ret.LastName = !reader.IsDBNull(reader.GetOrdinal("LastName")) ? reader.GetString("LastName") : String.Empty;
                                ret.Address = !reader.IsDBNull(reader.GetOrdinal("Address")) ? reader.GetString("Address") : String.Empty;
                                ret.PostalCode = !reader.IsDBNull(reader.GetOrdinal("PostalCode")) ? reader.GetString("PostalCode") : String.Empty;
                                ret.Gender = !reader.IsDBNull(reader.GetOrdinal("Gender")) ? reader.GetString("Gender") : String.Empty;
                                ret.IsActive = !reader.IsDBNull(reader.GetOrdinal("IsActive")) ? reader.GetInt32("IsActive") : 0;
                                ret.Password = !reader.IsDBNull(reader.GetOrdinal("Password")) ? reader.GetString("Password") : String.Empty;
                                ret.SecurityToken = !reader.IsDBNull(reader.GetOrdinal("SecurityToken")) ? reader.GetString("SecurityToken") : String.Empty;
                                ret.Description = !reader.IsDBNull(reader.GetOrdinal("Description")) ? reader.GetString("Description") : String.Empty;
                                ret.ProfilePhotoUrl = !reader.IsDBNull(reader.GetOrdinal("ProfilePhotoUrl")) ? reader.GetString("ProfilePhotoUrl") : String.Empty;
                                ret.DBInstance = !reader.IsDBNull(reader.GetOrdinal("DBInstance")) ? reader.GetString("DBInstance") : String.Empty;
                                ret.DefaultController = !reader.IsDBNull(reader.GetOrdinal("SiteMapController")) ? reader.GetString("SiteMapController") : String.Empty;
                                ret.DefaultView = !reader.IsDBNull(reader.GetOrdinal("SiteMapView")) ? reader.GetString("SiteMapView") : String.Empty;
                            }
                        }
                    }
                    connection.Close();
                }

                using (MD5 md5Hash = MD5.Create())
                {
                    string source = usr.Password + ret.SecurityToken;

                    if (!Md5Hash.VerifyMd5Hash(md5Hash, source, ret.Password))
                    {
                        throw new UserValidationException();
                    }
                }
            }
            catch (UserNotFoundException ex)
            {
                throw ex;
            }
            catch (UserValidationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected failure");
            }

            return ret;
        }

        public static User ValidateExternalUser(User usr, string externalSource)
        {
            User ret = new User();
            bool existed = false;
            try
            {
                using (MySqlConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    string commandText = "SELECT usr.UserId, usr.UserName, usr.Email, usr.FirstName, usr.LastName, usr.Address, "
                        + "usr.PostalCode, usr.Gender, usr.IsActive, usr.Password, usr.SecurityToken, usr.Description, "
                        + "usr.ProfilePhotoUrl, usr.DBInstance, sm.SiteMapController, sm.SiteMapView "
                        + "FROM InvoiceManager.Users usr "
                        + "LEFT JOIN InvoiceManager.SiteMap sm ON usr.DefaultView = sm.SiteMapId ";
                    if (String.IsNullOrEmpty(externalSource))
                    {
                        throw new UserNotFoundException();
                    }
                    /*if (!String.IsNullOrEmpty(usr.UserName))
                    {
                        commandText += "WHERE (usr.UserName = '" + usr.UserName + "' OR usr.Email = '" + usr.UserName + "') ";
                        //commandText += "AND usr.externalSource = '" + externalId + "'";
                    }
                    else*/
                    if (!String.IsNullOrEmpty(usr.Email))
                    {
                        commandText += "WHERE usr.UserName = '" + usr.Email + "' OR usr.Email = '" + usr.Email + "'";
                        //commandText += "AND usr.externalSource = '" + externalId + "'";
                    }
                    else
                    {
                        throw new UserNotFoundException();
                    }

                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                existed = true;
                                ret.UserId = !reader.IsDBNull(reader.GetOrdinal("UserId")) ? reader.GetInt32("UserId") : 0;
                                ret.UserName = !reader.IsDBNull(reader.GetOrdinal("UserName")) ? reader.GetString("UserName") : String.Empty;
                                ret.Email = !reader.IsDBNull(reader.GetOrdinal("Email")) ? reader.GetString("Email") : String.Empty;
                                ret.FirstName = !reader.IsDBNull(reader.GetOrdinal("FirstName")) ? reader.GetString("FirstName") : String.Empty;
                                ret.LastName = !reader.IsDBNull(reader.GetOrdinal("LastName")) ? reader.GetString("LastName") : String.Empty;
                                ret.Address = !reader.IsDBNull(reader.GetOrdinal("Address")) ? reader.GetString("Address") : String.Empty;
                                ret.PostalCode = !reader.IsDBNull(reader.GetOrdinal("PostalCode")) ? reader.GetString("PostalCode") : String.Empty;
                                ret.Gender = !reader.IsDBNull(reader.GetOrdinal("Gender")) ? reader.GetString("Gender") : String.Empty;
                                ret.IsActive = !reader.IsDBNull(reader.GetOrdinal("IsActive")) ? reader.GetInt32("IsActive") : 0;
                                ret.Password = !reader.IsDBNull(reader.GetOrdinal("Password")) ? reader.GetString("Password") : String.Empty;
                                ret.SecurityToken = !reader.IsDBNull(reader.GetOrdinal("SecurityToken")) ? reader.GetString("SecurityToken") : String.Empty;
                                ret.Description = !reader.IsDBNull(reader.GetOrdinal("Description")) ? reader.GetString("Description") : String.Empty;
                                ret.ProfilePhotoUrl = !reader.IsDBNull(reader.GetOrdinal("ProfilePhotoUrl")) ? reader.GetString("ProfilePhotoUrl") : String.Empty;
                                ret.DBInstance = !reader.IsDBNull(reader.GetOrdinal("DBInstance")) ? reader.GetString("DBInstance") : String.Empty;
                                ret.DefaultController = !reader.IsDBNull(reader.GetOrdinal("SiteMapController")) ? reader.GetString("SiteMapController") : String.Empty;
                                ret.DefaultView = !reader.IsDBNull(reader.GetOrdinal("SiteMapView")) ? reader.GetString("SiteMapView") : String.Empty;
                            }
                        }
                    }
                    connection.Close();
                }

                if (!existed)
                {
                    ret.UserId = 0;
                    return ret;
                }

                /*using (MD5 md5Hash = MD5.Create())
                {
                    string source = usr.Password + ret.SecurityToken;

                    if (!Md5Hash.VerifyMd5Hash(md5Hash, source, ret.Password))
                    {
                        throw new UserValidationException();
                    }
                }*/
            }
            catch (UserNotFoundException ex)
            {
                throw ex;
            }
            catch (UserValidationException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected failure");
            }

            return ret;
        }

        public static User GetUserById(int userId)
        {
            User ret = new User();
            try
            {
                using (MySqlConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    string commandText = "SELECT usr.UserId, usr.UserName, usr.Email, usr.FirstName, usr.LastName, usr.Address, "
                        + "usr.PostalCode, usr.Gender, usr.IsActive, usr.Password, usr.SecurityToken, usr.Description, "
                        + "usr.ProfilePhotoUrl, usr.DBInstance, sm.SiteMapController, sm.SiteMapView "
                        + "FROM InvoiceManager.Users usr "
                        + "LEFT JOIN InvoiceManager.SiteMap sm ON usr.DefaultView = sm.SiteMapId "
                        + "WHERE usr.UserId = @userId";
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ret.UserId = !reader.IsDBNull(reader.GetOrdinal("UserId")) ? reader.GetInt32("UserId") : 0;
                                ret.UserName = !reader.IsDBNull(reader.GetOrdinal("UserName")) ? reader.GetString("UserName") : String.Empty;
                                ret.Email = !reader.IsDBNull(reader.GetOrdinal("Email")) ? reader.GetString("Email") : String.Empty;
                                ret.FirstName = !reader.IsDBNull(reader.GetOrdinal("FirstName")) ? reader.GetString("FirstName") : String.Empty;
                                ret.LastName = !reader.IsDBNull(reader.GetOrdinal("LastName")) ? reader.GetString("LastName") : String.Empty;
                                ret.Address = !reader.IsDBNull(reader.GetOrdinal("Address")) ? reader.GetString("Address") : String.Empty;
                                ret.PostalCode = !reader.IsDBNull(reader.GetOrdinal("PostalCode")) ? reader.GetString("PostalCode") : String.Empty;
                                ret.Gender = !reader.IsDBNull(reader.GetOrdinal("Gender")) ? reader.GetString("Gender") : String.Empty;
                                ret.IsActive = !reader.IsDBNull(reader.GetOrdinal("IsActive")) ? reader.GetInt32("IsActive") : 0;
                                ret.Password = !reader.IsDBNull(reader.GetOrdinal("Password")) ? reader.GetString("Password") : String.Empty;
                                ret.SecurityToken = !reader.IsDBNull(reader.GetOrdinal("SecurityToken")) ? reader.GetString("SecurityToken") : String.Empty;
                                ret.Description = !reader.IsDBNull(reader.GetOrdinal("Description")) ? reader.GetString("Description") : String.Empty;
                                ret.ProfilePhotoUrl = !reader.IsDBNull(reader.GetOrdinal("ProfilePhotoUrl")) ? reader.GetString("ProfilePhotoUrl") : String.Empty;
                                ret.DBInstance = !reader.IsDBNull(reader.GetOrdinal("DBInstance")) ? reader.GetString("DBInstance") : String.Empty;
                                ret.DefaultController = !reader.IsDBNull(reader.GetOrdinal("SiteMapController")) ? reader.GetString("SiteMapController") : String.Empty;
                                ret.DefaultView = !reader.IsDBNull(reader.GetOrdinal("SiteMapView")) ? reader.GetString("SiteMapView") : String.Empty;
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

        public static void RegisterUser(User usr)
        {
            try
            {
                using (MySqlConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    int existed = 0;
                    string commandText = "SELECT COUNT(UserId) AS Num "
                        + "FROM Users "
                        + "WHERE UserName = @usrUserName "
                        + "OR Email = @usrEmail";
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@usrUserName", usr.UserName);
                        command.Parameters.AddWithValue("@usrEmail", usr.Email);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                existed = !reader.IsDBNull(reader.GetOrdinal("Num")) ? reader.GetInt32("Num") : 0;
                            }
                        }
                    }

                    if (existed != 0)
                    {
                        throw new UserAlreadyExistedException();
                    }
                    else
                    {
                        string passCode = Md5Hash.GetRandomString(12);

                        /*int counter = 0;
                        while (!Regex.IsMatch(passCode, UIClasses.UIUser.PasswordRegexString))
                        {
                            if(counter > 10)
                            {
                                throw new PassCodeException();
                            }
                            passCode = Md5Hash.GetRandomString(12);
                            counter++;
                        }*/

                        string token = Md5Hash.GetRandomString(6);
                        string hash = String.Empty;
                        using (MD5 md5Hash = MD5.Create())
                        {
                            hash = Md5Hash.GetMd5Hash(md5Hash, passCode + token);
                        }

                        commandText = "INSERT INTO Users "
                            + "(UserName, Email, FirstName, LastName, Address, "
                            + "PostalCode, Gender, IsActive, Password, SecurityToken, Description) "
                            + "VALUES "
                            + "(@usrUserName, @usrEmail, NULL, NULL, NULL, "
                            + "NULL, NULL, 0, @hash, @token, NULL)";
                        using (MySqlCommand command = database.CreateCommand(commandText, connection))
                        {
                            command.Parameters.AddWithValue("@usrUserName", usr.UserName);
                            command.Parameters.AddWithValue("@usrEmail", usr.Email);
                            command.Parameters.AddWithValue("@hash", hash);
                            command.Parameters.AddWithValue("@token", token);

                            int row = command.ExecuteNonQuery();

                            if (row == 0)
                            {
                                throw new AddNewUserException();
                            }
                        }

                        string subject = "T Tech Invoice Manger account receipt for " + usr.Email;
                        string body = "Welcome! " + usr.UserName
                            + Environment.NewLine + "Your account has been created and please use the following pass code for your Invoice Manger login"
                            + Environment.NewLine
                            + Environment.NewLine + "Code: " + passCode
                            + Environment.NewLine
                            + Environment.NewLine
                            + Environment.NewLine
                            + Environment.NewLine + "Thank you,"
                            + Environment.NewLine + "Invoice Manager";

                        try
                        {
                            Email.SendEmail(subject, body, "non-reply@gmail.com", "Invoice Manager", usr.Email, usr.UserName, "noah089736@hotmail.com", "Noah Tong");
                        }
                        catch (Exception ex)
                        {
                            commandText = "DELETE FROM Users "
                                + "WHERE Userid IN "
                                + "( "
                                + "SELECT UserId "
                                + "FROM "
                                + "( "
                                + "SELECT UserId "
                                + "FROM Users "
                                + "WHERE UserName = @usrUserName "
                                + "OR Email = @usrEmail "
                                + ") t "
                                + ")";
                            using (MySqlCommand command = database.CreateCommand(commandText, connection))
                            {
                                command.Parameters.AddWithValue("@usrUserName", usr.UserName);
                                command.Parameters.AddWithValue("@usrEmail", usr.Email);

                                command.ExecuteNonQuery();
                            }

                            throw ex;
                        }
                    }

                    connection.Close();
                }
            }
            catch (UserAlreadyExistedException ex)
            {
                throw ex;
            }
            catch (PassCodeException ex)
            {
                throw ex;
            }
            catch (AddNewUserException ex)
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

        public static void RegisterExternalUser(User usr, string externalSource)
        {
            try
            {
                using (MySqlConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    int existed = 0;
                    string commandText = "SELECT COUNT(UserId) AS Num "
                        + "FROM Users "
                        + "WHERE UserName = @usrUserName "
                        + "OR Email = @usrEmail";
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@usrUserName", usr.UserName);
                        command.Parameters.AddWithValue("@usrEmail", usr.Email);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                existed = !reader.IsDBNull(reader.GetOrdinal("Num")) ? reader.GetInt32("Num") : 0;
                            }
                        }
                    }

                    if (existed != 0)
                    {
                        throw new UserAlreadyExistedException();
                    }
                    else
                    {
                        string passCode = Md5Hash.GetRandomString(12);

                        /*int counter = 0;
                        while (!Regex.IsMatch(passCode, UIClasses.UIUser.PasswordRegexString))
                        {
                            if(counter > 10)
                            {
                                throw new PassCodeException();
                            }
                            passCode = Md5Hash.GetRandomString(12);
                            counter++;
                        }*/

                        string token = Md5Hash.GetRandomString(6);
                        string hash = String.Empty;
                        using (MD5 md5Hash = MD5.Create())
                        {
                            hash = Md5Hash.GetMd5Hash(md5Hash, passCode + token);
                        }

                        commandText = "INSERT INTO Users "
                            + "(UserName, Email, FirstName, LastName, Address, "
                            + "PostalCode, Gender, IsActive, Password, SecurityToken, Description, ProfilePhotoUrl) "
                            + "VALUES "
                            + "(@usrUserName, @usrEmail, NULL, NULL, NULL, "
                            + "NULL, NULL, 0, @hash, @token, NULL, @profilePic)";
                        using (MySqlCommand command = database.CreateCommand(commandText, connection))
                        {
                            command.Parameters.AddWithValue("@usrUserName", usr.UserName);
                            command.Parameters.AddWithValue("@usrEmail", usr.Email);
                            command.Parameters.AddWithValue("@hash", hash);
                            command.Parameters.AddWithValue("@token", token);
                            command.Parameters.AddWithValue("@profilePic", usr.ProfilePhotoUrl);

                            int row = command.ExecuteNonQuery();

                            if (row == 0)
                            {
                                throw new AddNewUserException();
                            }
                        }

                        string subject = "T Tech Invoice Manger account receipt for " + usr.Email;
                        string body = "Welcome! " + usr.UserName
                            + Environment.NewLine + "Your account has been created and please use the following pass code for your Invoice Manger login"
                            + Environment.NewLine
                            + Environment.NewLine + "Code: " + passCode
                            + Environment.NewLine
                            + Environment.NewLine
                            + Environment.NewLine
                            + Environment.NewLine + "Thank you,"
                            + Environment.NewLine + "Invoice Manager";

                        try
                        {
                            Email.SendEmail(subject, body, "non-reply@ttech.com", "Invoice Manager", usr.Email, usr.UserName, "noah089736@hotmail.com", "Noah Tong");
                        }
                        catch (Exception ex)
                        {
                            commandText = "DELETE FROM Users "
                                + "WHERE Userid IN "
                                + "( "
                                + "SELECT UserId "
                                + "FROM "
                                + "( "
                                + "SELECT UserId "
                                + "FROM Users "
                                + "WHERE UserName = @usrUserName "
                                + "OR Email = @usrEmail "
                                + ") t "
                                + ")";
                            using (MySqlCommand command = database.CreateCommand(commandText, connection))
                            {
                                command.Parameters.AddWithValue("@usrUserName", usr.UserName);
                                command.Parameters.AddWithValue("@usrEmail", usr.Email);

                                command.ExecuteNonQuery();
                            }

                            throw ex;
                        }
                    }

                    connection.Close();
                }
            }
            catch (UserAlreadyExistedException ex)
            {
                throw ex;
            }
            catch (PassCodeException ex)
            {
                throw ex;
            }
            catch (AddNewUserException ex)
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

        public static void ForgotPassword(User usr)
        {
            try
            {
                using (MySqlConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    int existed = 0;
                    string commandText = "SELECT COUNT(UserId) AS Num "
                        + "FROM Users "
                        + "WHERE UserName = @usrUserName "
                        + "AND Email = @usrEmail";
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@usrUserName", usr.UserName);
                        command.Parameters.AddWithValue("@usrEmail", usr.Email);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                existed = !reader.IsDBNull(reader.GetOrdinal("Num")) ? reader.GetInt32("Num") : 0;
                            }
                        }
                    }

                    if (existed == 0)
                    {
                        throw new UserNotFoundException();
                    }
                    else
                    {
                        string passCode = Md5Hash.GetRandomString(12);

                        /*int counter = 0;
                        while (!Regex.IsMatch(passCode, UIClasses.UIUser.PasswordRegexString))
                        {
                            if(counter > 10)
                            {
                                throw new PassCodeException();
                            }
                            passCode = Md5Hash.GetRandomString(12);
                            counter++;
                        }*/

                        string token = Md5Hash.GetRandomString(6);
                        string hash = String.Empty;
                        using (MD5 md5Hash = MD5.Create())
                        {
                            hash = Md5Hash.GetMd5Hash(md5Hash, passCode + token);
                        }

                        commandText = "UPDATE Users "
                            + "SET Password = @hash, "
                            + "SecurityToken = @token "
                            + "WHERE Userid IN "
                            + "( "
                            + "SELECT UserId "
                            + "FROM "
                            + "( "
                            + "SELECT UserId "
                            + "FROM Users "
                            + "WHERE UserName = @usrUserName "
                            + "OR Email = @usrEmail "
                            + ") t "
                            + ")";
                        using (MySqlCommand command = database.CreateCommand(commandText, connection))
                        {
                            command.Parameters.AddWithValue("@hash", hash);
                            command.Parameters.AddWithValue("@token", token);
                            command.Parameters.AddWithValue("@usrUserName", usr.UserName);
                            command.Parameters.AddWithValue("@usrEmail", usr.Email);

                            int row = command.ExecuteNonQuery();

                            if (row == 0)
                            {
                                throw new PasswordResetException();
                            }
                        }

                        string subject = "T Tech Invoice Manger password reset for " + usr.Email;
                        string body = "Your password has been reset and please use the following pass code for your Invoice Manger login"
                            + Environment.NewLine
                            + Environment.NewLine + "Code: " + passCode
                            + Environment.NewLine
                            + Environment.NewLine
                            + Environment.NewLine
                            + Environment.NewLine + "Thank you,"
                            + Environment.NewLine + "Invoice Manager";
                        /*string content = "Your password has been reset and please use the following pass code for your Invoice Manger login"
                        + Environment.NewLine
                        + Environment.NewLine + "Code: " + passCode
                        + Environment.NewLine
                        + Environment.NewLine
                        + Environment.NewLine
                        + Environment.NewLine + "Thank you,"
                        + Environment.NewLine + "Invoice Manager";
                        string body = "MIME - Version: 1.0" + Environment.NewLine
                            + "X - Mailer: MailBee.NET 8.0.4.428" + Environment.NewLine
                            + "Subject: " + subject + Environment.NewLine
                            + "To: " + usr.Email + Environment.NewLine
                            + "Content - Type: multipart / alternative;" + usr.Email + Environment.NewLine
                            + "boundary = \"----=_NextPart_000_AE6B_725E09AF.88B7F934\"" + Environment.NewLine
                            + "------ = _NextPart_000_AE6B_725E09AF.88B7F934" + Environment.NewLine
                            + "Content - Type: text / plain;" + Environment.NewLine
                            + "charset = \"utf-8\"" + Environment.NewLine
                            + "Content - Transfer - Encoding: quoted - printable" + Environment.NewLine
                            + content + Environment.NewLine
                            + "------= _NextPart_000_AE6B_725E09AF.88B7F934" + Environment.NewLine
                            + "Content - Type: text / html;" + Environment.NewLine
                            + "charset = \"utf-8\"" + Environment.NewLine
                            + "Content - Transfer - Encoding: quoted - printable" + Environment.NewLine
                            + "< pre > " + content + " </ pre >" + Environment.NewLine
                            + "------= _NextPart_000_AE6B_725E09AF.88B7F934--";*/

                        try
                        {
                            Email.SendEmail(subject, body, "non-reply@gmail.com", "Invoice Manager", usr.Email, usr.UserName, "noah089736@hotmail.com", "Noah Tong");
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    connection.Close();
                }
            }
            catch (UserNotFoundException ex)
            {
                throw ex;
            }
            catch (PassCodeException ex)
            {
                throw ex;
            }
            catch (PasswordResetException ex)
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

        public static User UpdateUserProfile(User usr)
        {
            try
            {
                using (MySqlConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    string commandText = "UPDATE Users "
                        + "SET FirstName = @usrFirstName, "
                        + "LastName = @usrLastName, "
                        + "Address = @usrAddress, "
                        + "PostalCode = @usrPostalCode, "
                        + "Gender = @usrGender, "
                        + "Description = @usrDescription "
                        + "WHERE Userid IN "
                        + "( "
                        + "SELECT UserId "
                        + "FROM "
                        + "( "
                        + "SELECT UserId "
                        + "FROM Users "
                        + "WHERE UserName = @usrUserName "
                        + "OR Email = @usrEmail "
                        + ") t "
                        + ")";
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@usrFirstName", usr.FirstName);
                        command.Parameters.AddWithValue("@usrLastName", usr.LastName);
                        command.Parameters.AddWithValue("@usrAddress", usr.Address);
                        command.Parameters.AddWithValue("@usrPostalCode", usr.PostalCode);
                        command.Parameters.AddWithValue("@usrGender", usr.Gender);
                        command.Parameters.AddWithValue("@usrDescription", usr.Description);
                        command.Parameters.AddWithValue("@usrUserName", usr.UserName);
                        command.Parameters.AddWithValue("@usrEmail", usr.Email);

                        int row = command.ExecuteNonQuery();

                        if (row == 0)
                        {
                            throw new UpdateUserProfileException();
                        }
                    }
                    connection.Close();
                }
            }
            catch (UpdateUserProfileException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected failure");
            }

            return usr;
        }

        public static void UpdateUserPassword(User usr)
        {
            try
            {
                using (MySqlConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    string commandText = "SELECT SecurityToken "
                        + "FROM Users "
                        + "WHERE Userid IN "
                        + "( "
                        + "SELECT UserId "
                        + "FROM "
                        + "( "
                        + "SELECT UserId "
                        + "FROM Users "
                        + "WHERE UserName = @usrUserName "
                        + "OR Email = @usrEmail "
                        + ") t "
                        + ")";
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@usrUserName", usr.UserName);
                        command.Parameters.AddWithValue("@usrEmail", usr.Email);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                usr.SecurityToken = !reader.IsDBNull(reader.GetOrdinal("SecurityToken")) ? reader.GetString("SecurityToken") : usr.SecurityToken;
                            }
                        }
                    }

                    string hash = String.Empty;
                    using (MD5 md5Hash = MD5.Create())
                    {
                        hash = Md5Hash.GetMd5Hash(md5Hash, usr.Password + usr.SecurityToken);
                    }

                    commandText = "UPDATE Users "
                        + "SET Password = @hash "
                        + "WHERE Userid IN "
                        + "( "
                        + "SELECT UserId "
                        + "FROM "
                        + "( "
                        + "SELECT UserId "
                        + "FROM Users "
                        + "WHERE UserName = @usrUserName "
                        + "OR Email = @usrEmail "
                        + ") t "
                        + ")";
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@hash", hash);
                        command.Parameters.AddWithValue("@usrUserName", usr.UserName);
                        command.Parameters.AddWithValue("@usrEmail", usr.Email);

                        int row = command.ExecuteNonQuery();

                        if (row == 0)
                        {
                            throw new UpdateUserPasswordException();
                        }
                    }

                    connection.Close();
                }
            }
            catch (UpdateUserPasswordException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected failure");
            }
        }

        public static User UpdateUserProfilePhoto(User usr)
        {
            try
            {
                using (MySqlConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    string commandText = "UPDATE Users "
                        + "SET ProfilePhotoUrl = @usrProfilePhotoUrl "
                        + "WHERE Userid IN "
                        + "( "
                        + "SELECT UserId "
                        + "FROM "
                        + "( "
                        + "SELECT UserId "
                        + "FROM Users "
                        + "WHERE UserName = @usrUserName "
                        + "OR Email = @usrEmail "
                        + ") t "
                        + ")";
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@usrProfilePhotoUrl", usr.ProfilePhotoUrl);
                        command.Parameters.AddWithValue("@usrUserName", usr.UserName);
                        command.Parameters.AddWithValue("@usrEmail", usr.Email);

                        int row = command.ExecuteNonQuery();

                        if (row == 0)
                        {
                            throw new UpdateUserProfilePhotoException();
                        }
                    }
                    connection.Close();
                }
            }
            catch (UpdateUserProfilePhotoException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected failure");
            }

            return usr;
        }
    }

    /// <summary>
    /// A basic class for a User
    /// </summary>
    public class User
    {
        public User()
        { }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string Gender { get; set; }

        public int IsActive { get; set; }

        public string Password { get; set; }

        public string SecurityToken { get; set; }

        public string Description { get; set; }

        public string ProfilePhotoUrl { get; set; }

        public string DBInstance { get; set; }

        public string DefaultController { get; set; }

        public string DefaultView { get; set; }
    }

    /// <summary>
    /// Md5Hash for user authentication
    /// </summary>
    public class Md5Hash
    {
        private Md5Hash() { }

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int GetRandomNumber(int maxNumber)
        {
            if (maxNumber < 1)
                throw new Exception("The maxNumber value should be greater than 1");
            Random r = new Random();
            return r.Next(1, maxNumber);
        }

        public static string GetRandomString(int length)
        {
            string[] array = new string[54]
            {
                "0","2","3","4","5","6","8","9",
                "a","b","c","d","e","f","g","h","j","k","m","n","p","q","r","s","t","u","v","w","x","y","z",
                "A","B","C","D","E","F","G","H","J","K","L","M","N","P","R","S","T","U","V","W","X","Y","Z"
            };
            StringBuilder sb = new StringBuilder();
            Random r = new Random();
            for (int i = 0; i < length; i++) sb.Append(array[r.Next(53)]);
            return sb.ToString();
        }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
            : base(String.Format("<strong>User</strong> does not exist or <strong>Username & Email</strong> do not match")) { }
    }

    public class UserValidationException : Exception
    {
        public UserValidationException()
            : base(String.Format("Wrong <strong>username</strong> or <strong>password</strong><br /><button type=\"button\" class=\"btn btn - link\" onclick=\"location.href = '../Account/ForgotPassword'\">Forgot password ?</button>")) { }
    }

    public class UserAlreadyExistedException : Exception
    {
        public UserAlreadyExistedException()
            : base(String.Format("<strong>Username</strong> or <strong>email</strong> already exists in the system")) { }
    }

    public class PassCodeException : Exception
    {
        public PassCodeException()
            : base(String.Format("Failed to generate pass code from the system")) { }
    }

    public class AddNewUserException : Exception
    {
        public AddNewUserException()
            : base(String.Format("Failed to add a new user to the system")) { }
    }

    public class PasswordResetException : Exception
    {
        public PasswordResetException()
            : base(String.Format("Failed to reset User Password")) { }
    }

    public class UpdateUserProfileException : Exception
    {
        public UpdateUserProfileException()
            : base(String.Format("Failed to update User Profile")) { }
    }

    public class UpdateUserPasswordException : Exception
    {
        public UpdateUserPasswordException()
            : base(String.Format("Failed to update User Password")) { }
    }

    public class UpdateUserProfilePhotoException : Exception
    {
        public UpdateUserProfilePhotoException()
            : base(String.Format("Failed to update User Profile Photo")) { }
    }
}
