using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CsvManager
{
    public class CsvWriter
    {
        public string Write<T>(IEnumerable<T> list, IEnumerable<string> propertiesToExport, bool includeHeader = true)
        {
            StringBuilder sb = new StringBuilder();

            Type type = typeof(T);

            PropertyInfo[] properties = type.GetProperties().Where(x=> propertiesToExport.Contains(x.Name)).ToArray();

            if (includeHeader)
            {
                sb.AppendLine(this.CreateCsvHeaderLine(properties));
            }

            foreach (var item in list)
            {
                sb.AppendLine(this.CreateCsvLine(item, properties));
            }

            return sb.ToString();
        }

        public string Write<T>(IEnumerable<T> list, IEnumerable<string> propertiesToExport, string filePath, bool includeHeader = true)
        {
            string csv = this.Write(list, propertiesToExport, includeHeader);

            this.WriteFile(filePath, csv);

            return csv;
        }

        private string CreateCsvHeaderLine(PropertyInfo[] properties)
        {
            List<string> propertyValues = new List<string>();

            foreach (var prop in properties)
            {
                string value = prop.Name;

                var attribute = prop.GetCustomAttribute(typeof(DisplayAttribute));
                if (attribute != null)
                {
                    value = (attribute as DisplayAttribute).Name;
                }

                this.CreateCsvStringItem(propertyValues, value);
            }

            return this.CreateCsvLine(propertyValues);
        }

        private string CreateCsvLine<T>(T item, PropertyInfo[] properties)
        {
            List<string> propertyValues = new List<string>();

            foreach (var prop in properties)
            {
                object value = prop.GetValue(item, null);

                if (prop.PropertyType == typeof(string))
                {
                    this.CreateCsvStringItem(propertyValues, value);
                }
                else if (prop.PropertyType == typeof(string[]))
                {
                    this.CreateCsvStringArrayItem(propertyValues, value);
                }
                else if (prop.PropertyType == typeof(List<string>))
                {
                    this.CreateCsvStringListItem(propertyValues, value);
                }
                else if (prop.PropertyType == typeof(decimal))
                {
                    this.CreateCsvDecimalItem(propertyValues, value);
                }
                else
                {
                    this.CreateCsvItem(propertyValues, value);
                }
            }

            return this.CreateCsvLine(propertyValues);
        }

        private string CreateCsvLine(IList<string> list)
        {
            return string.Join(CsvSettings.DELIMITER.ToString(), list);
        }

        private void CreateCsvItem(List<string> propertyValues, object value)
        {
            if (value != null)
            {
                propertyValues.Add(value.ToString());
            }
            else
            {
                propertyValues.Add(string.Empty);
            }
        }

        private void CreateCsvStringListItem(List<string> propertyValues, object value)
        {
            string formatString = "\"{0}\"";
            if (value != null)
            {
                value = this.CreateCsvLine((List<string>)value);
                propertyValues.Add(string.Format(formatString, this.ProcessStringEscapeSequence(value)));
            }
            else
            {
                propertyValues.Add(string.Empty);
            }
        }

        private void CreateCsvStringArrayItem(List<string> propertyValues, object value)
        {
            string formatString = "\"{0}\"";
            if (value != null)
            {
                value = this.CreateCsvLine(((string[])value).ToList());
                propertyValues.Add(string.Format(formatString, this.ProcessStringEscapeSequence(value)));
            }
            else
            {
                propertyValues.Add(string.Empty);
            }
        }

        private void CreateCsvStringItem(List<string> propertyValues, object value)
        {
            string formatString = "\"{0}\"";
            if (value != null)
            {
                propertyValues.Add(string.Format(formatString, this.ProcessStringEscapeSequence(value)));
            }
            else
            {
                propertyValues.Add(string.Empty);
            }
        }

        private void CreateCsvDecimalItem(List<string> propertyValues, object value)
        {
            string formatString = "{0:0.0}";
            if (value != null)
            {
                propertyValues.Add(string.Format(CultureInfo.InvariantCulture,formatString, value));
            }
            else
            {
                propertyValues.Add(string.Empty);
            }
        }

        private string ProcessStringEscapeSequence(object value)
        {
            return value.ToString().Replace("\"", "\"\"");
        }

        public bool WriteFile(string filePath, string csv)
        {
            bool fileCreated = false;

            if (!string.IsNullOrWhiteSpace(filePath))
            {
                File.WriteAllText(filePath, csv);

                fileCreated = true;
            }

            return fileCreated;
        }
    }
}
