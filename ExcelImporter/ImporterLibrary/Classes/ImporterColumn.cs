using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImporterLibrary.Classes
{
    public class ImporterColumn
    {
        public string ColumnName { get; set; }
        public int ColumnIndex { get; set; }
        
        public ImporterColumn()
        {
            ColumnName = String.Empty;
            ColumnIndex = 0;
        }

        public ImporterColumn(int index, string name)
        {
            ColumnName = name;
            ColumnIndex = index;
        }
    }
}
