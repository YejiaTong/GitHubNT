using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OfficeOpenXml;
using Excel;

namespace ExcelReader.Classes
{
    public class ReaderBase
    {
        public string FileLocation { get; set; }
        public ExcelPackage XLSXPackage { get; set; }
        public IExcelDataReader XLSPackage { get; set; }

        public ReaderBase()
        {
            FileLocation = String.Empty;
            XLSXPackage = null;
            XLSPackage = null;
        }

        public void ConfigureFileLocation(string fileLocation)
        {
            FileLocation = fileLocation;
        }
    }
}
