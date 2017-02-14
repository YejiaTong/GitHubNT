using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExcelReader.Classes.Readers;

namespace ExcelImporter
{
    class Program
    {
        static string DBConnectionString = "server=ttechprod.cw8rzstofewz.us-west-2.rds.amazonaws.com;userid=noah089736;pwd=089736noahTYJ;port=3306;database=WensResearchCN;sslmode=none;";

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Please enter the full file location:");
                string fileLocation = Console.ReadLine();

                Console.WriteLine(".");
                Console.WriteLine(".");
                Console.WriteLine(".");

                RPChineseIndustrialEnterpriseDatabaseReader reader = new RPChineseIndustrialEnterpriseDatabaseReader();
                reader.ConfigureFileLocation(fileLocation);
                reader.ReadExcelXLSFile();

                Console.WriteLine(".");
                Console.WriteLine(".");
                Console.WriteLine(".");
            }
            catch (Exception ex)
            {
                Console.WriteLine("*** *** *** ERROR *** *** ***");
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Press any KEY to exit...");
                Console.ReadLine();
            }
        }
    }
}
