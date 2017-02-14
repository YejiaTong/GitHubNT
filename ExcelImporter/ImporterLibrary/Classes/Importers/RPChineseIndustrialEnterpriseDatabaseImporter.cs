using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataManager.DataAccess;

namespace ImporterLibrary.Classes.Importers
{
    public class RPChineseIndustrialEnterpriseDatabaseImporter : ImporterBase
    {
        public List<ImporterTable> Tables { get; set; }

        public RPChineseIndustrialEnterpriseDatabaseImporter()
            : base()
        {
            Tables = new List<ImporterTable>();
        }

        public void ImportRecords()
        {
            try
            {
                string DBConnectionString = "server=ttechprod.cw8rzstofewz.us-west-2.rds.amazonaws.com;userid=noah089736;pwd=089736noahTYJ;port=3306;database=WensResearchCN;sslmode=none;charset=utf8;";

                foreach (var table in Tables)
                {
                    RPChineseIndustrialEnterpriseDatabaseContext.AddNewRecords(table.Objects, DBConnectionString);
                }
            }
            catch (Exception ex)
            {
                /* Placeholder */
                throw ex;
            }
        }
    }
}
