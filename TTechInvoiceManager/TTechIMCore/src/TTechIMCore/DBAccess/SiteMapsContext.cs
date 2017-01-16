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
    public class SiteMapsContext : DBWorker
    {
        public static IEnumerable<SiteMap> LoadAllSiteMaps()
        {
            List<SiteMap> ret = new List<SiteMap>();
            try
            {
                using (MySqlConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    string commandText = "SELECT SiteMapId, SiteMapName, SiteMapController, "
                        + "SiteMapView, Description "
                        + "FROM SiteMap "
                        + "ORDER BY SiteMapController, SiteMapView";
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SiteMap item = new SiteMap();
                                item.SiteMapId = !reader.IsDBNull(reader.GetOrdinal("SiteMapId")) ? reader.GetInt32("SiteMapId") : 0;
                                item.SiteMapName = !reader.IsDBNull(reader.GetOrdinal("SiteMapName")) ? reader.GetString("SiteMapName") : String.Empty;
                                item.SiteMapController = !reader.IsDBNull(reader.GetOrdinal("SiteMapController")) ? reader.GetString("SiteMapController") : String.Empty;
                                item.SiteMapView = !reader.IsDBNull(reader.GetOrdinal("SiteMapView")) ? reader.GetString("SiteMapView") : String.Empty;
                                item.Description = !reader.IsDBNull(reader.GetOrdinal("Description")) ? reader.GetString("Description") : String.Empty;

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

        public static bool UpdateUserDefaultView(User usr, SiteMap siteMap)
        {
            List<SiteMap> ret = new List<SiteMap>();
            try
            {
                using (MySqlConnection connection = database.CreateConnection())
                {
                    connection.Open();
                    string commandText = "UPDATE Users "
                        + "SET DefaultView = @siteMapId "
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
                        + ")"; ;
                    using (MySqlCommand command = database.CreateCommand(commandText, connection))
                    {
                        command.Parameters.AddWithValue("@siteMapId", siteMap.SiteMapId);
                        command.Parameters.AddWithValue("@usrUserName", usr.UserName);
                        command.Parameters.AddWithValue("@usrEmail", usr.Email);

                        int row = command.ExecuteNonQuery();

                        if (row == 0)
                        {
                            throw new UpdateUserDefaultViewException();
                        }
                    }
                    connection.Close();
                }
            }
            catch (UpdateUserDefaultViewException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected failure");
            }

            return true;
        }
    }

    /// <summary>
    /// A basic class for Site Map
    /// </summary>
    public class SiteMap
    {
        public SiteMap()
        { }

        public int SiteMapId { get; set; }

        public string SiteMapName { get; set; }

        public string SiteMapController { get; set; }

        public string SiteMapView { get; set; }

        public string Description { get; set; }
    }

    public class UpdateUserDefaultViewException : Exception
    {
        public UpdateUserDefaultViewException()
            : base(String.Format("Failed to update User default front page")) { }
    }
}
