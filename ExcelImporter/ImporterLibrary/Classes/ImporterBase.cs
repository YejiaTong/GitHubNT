using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImporterLibrary.Classes
{
    public class ImporterBase
    {
        public string ImporterName { get; set; }
        public string Description { get; set; }

        public ImporterBase()
        {
            ImporterName = String.Empty;
            Description = String.Empty;
        }
    }
}
