﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

namespace NTWebApp.DBAccess
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
    }

    /// <summary>
    /// A basic class for an SiteMap
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
}
