using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImporterLibrary.Classes
{
    public class ImporterTable
    {
        public string TableName { get; set; }
        public List<ImporterColumn> Columns { get; set; }
        public List<Dictionary<string, object>> Objects { get; set; }

        public ImporterTable()
        {
            TableName = String.Empty;
            Columns = new List<ImporterColumn>();
            Objects = new List<Dictionary<string, object>>();
        }
    }
}
