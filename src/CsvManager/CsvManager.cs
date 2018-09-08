using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CsvManager
{
    public abstract class CsvManager<T> where T : class
    {
        public abstract T GetModelFromCsvRow(CsvRow csvRow);

        public abstract bool IsValidEntry(T model);


        public IEnumerable<T> ImportCsvToModelList(string filePath)
        {
            var rows = new List<CsvRow>();
            var importedList = new List<T>();
            using (CsvReader reader = new CsvReader(filePath))
            {
                CsvRow row = new CsvRow();
                reader.ReadRow(row);//Skip header
                while (reader.ReadRow(row))
                {
                    var model = GetModelFromCsvRow(row);
                    if (IsValidEntry(model))
                        importedList.Add(model);
                }
            }
            
            return importedList;
        }

        public void ExportListToFile(IEnumerable<T> modelList, IEnumerable<string> propertiesToExport, string filePath, bool includeHeader = true)
        {
            var csvWriter = new CsvWriter();
            csvWriter.Write(modelList, propertiesToExport, filePath, includeHeader);
        }
    }
}
