using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

using ImporterLibrary.Classes;
using ImporterLibrary.Classes.Importers;

using OfficeOpenXml;
using Excel;

namespace ExcelReader.Classes.Readers
{
    public class RPChineseIndustrialEnterpriseDatabaseReader : ReaderBase
    {
        public List<ImporterColumn> DefaultColumns { get; set; }
        public RPChineseIndustrialEnterpriseDatabaseImporter Importer { get; set; }

        public RPChineseIndustrialEnterpriseDatabaseReader()
            : base()
        {
            DefaultColumns = new List<ImporterColumn>();
            Importer = new RPChineseIndustrialEnterpriseDatabaseImporter();
        }

        public void ResetDefaultColumns()
        {
            DefaultColumns = new List<ImporterColumn>();
        }

        public void AddToDefaultColumns(int index, string name, bool overwrite)
        {
            try
            {
                if(DefaultColumns == null)
                {
                    throw new Exception("DefaultColumns object is not initialized");
                }

                var obj = DefaultColumns.FirstOrDefault(x => x.ColumnIndex == index);
                if (obj != null)
                {
                    if(overwrite)
                    {
                        obj.ColumnName = name;
                    }
                    else
                    {
                        throw new Exception(String.Format("Item already exists at ColumnIndex: {0}, ColumnName: ", obj.ColumnIndex, obj.ColumnName));
                    }
                }
                else
                {
                    obj = new ImporterColumn(index, name);
                    DefaultColumns.Add(obj);
                }
            }
            catch (Exception ex)
            {
                /* Placeholder */
                throw ex;
            }
        }

        public bool ValidateDefaultColumns()
        {
            try
            {
                if(DefaultColumns == null)
                {
                    throw new Exception("DefaultColumns object is not initialized");
                }

                DefaultColumns = DefaultColumns.OrderBy(x => x.ColumnIndex).ToList();
                int index = 0;
                foreach(var item in DefaultColumns)
                {
                    if(index == 0)
                    {
                        index = item.ColumnIndex;
                    }
                    else if(index == item.ColumnIndex - 1)
                    {
                        index = item.ColumnIndex;
                    }
                    else
                    {
                        return false;
                        throw new Exception(String.Format("DefaultColumns has discontinuous ColumnIndex at {0}", index + 1));
                    }
                }

                return true;
            }
            catch(Exception ex)
            {
                /* Placeholder */
                throw ex;
            }
        }

        public void ReadExcelXLSFile()
        {
            try
            {
                ResetReader();

                if (!File.Exists(FileLocation))
                {
                    throw new Exception(String.Format("Target file at {0} does not exist", FileLocation));
                }

                FileStream stream = File.Open(FileLocation, FileMode.Open, FileAccess.Read);

                ImporterTable table = new ImporterTable();

                using (XLSPackage = ExcelReaderFactory.CreateBinaryReader(stream))
                {
                    /*DataSet result = XLSPackage.AsDataSet();*/
                    int i = 0;
                    while (XLSPackage.Read())
                    {
                        ImporterObject item = new ImporterObject();
                        item["001"] = XLSPackage.GetString(0);
                        item["002"] = XLSPackage.GetString(1);
                        item["003"] = XLSPackage.GetString(2);
                        item["004"] = XLSPackage.GetString(3);
                        item["005"] = XLSPackage.GetString(4);
                        item["006"] = XLSPackage.GetString(5);
                        item["007"] = XLSPackage.GetString(6);
                        item["008"] = XLSPackage.GetString(7);
                        item["009"] = XLSPackage.GetString(8);
                        item["010"] = XLSPackage.GetString(9);
                        table.Objects.Add(item);
                    }
                }

                Importer.Tables.Add(table);
                Importer.ImportRecords();
            }
            catch (Exception ex)
            {
                /* Placeholder */
                throw ex;
            }
        }

        public void ReadExcelXLSXFile()
        {
            try
            {
                ResetReader();

                if(!File.Exists(FileLocation))
                {
                    throw new Exception(String.Format("Target file at {0} does not exist", FileLocation));
                }

                FileInfo existingFile = new FileInfo(FileLocation);
                using (XLSXPackage = new ExcelPackage(existingFile))
                {
                    ExcelWorksheet worksheet = XLSXPackage.Workbook.Worksheets[1];

                    Console.WriteLine("\tCell({0},{1}).Value={2}", 1, 2, worksheet.Cells[1, 2].Value);
                }
            }
            catch (Exception ex)
            {
                /* Placeholder */
                throw ex;
            }
        }

        public void ResetReader()
        {
            try
            {
                if (XLSXPackage != null)
                {
                    XLSXPackage.Dispose();
                }
                if (XLSPackage != null)
                {
                    XLSPackage.Dispose();
                }
                if (Importer != null)
                {
                    Importer = new RPChineseIndustrialEnterpriseDatabaseImporter();
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
