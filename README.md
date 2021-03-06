# CsvManager
.NET Standard library to import and export files in csv format.

Utilization sample on CsvManager.ConsoleApp
```csharp
class Program
{
    static void Main(string[] args)
    {
        //How to use
        Console.WriteLine("Reading file...");

        var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..", "files");
        var filePath = Path.Combine(path, "Baseficticia.txt");
        var invoiceWrapper = new InvoiceWrapper();
        List<Invoice> invoices = invoiceWrapper.ImportCsvToModelList(filePath).ToList();

        //Invoice with zero values
        var invoicesWithZeroValues = invoices.Where(x => x.InvoiceValue == 0).ToList();

        //choose properties to export
        var propertiesToExport = new List<string>()
        {
            nameof(Invoice.Client), nameof(Invoice.AdressLine1),
            nameof(Invoice.InvoiceValue), nameof(Invoice.PageNumber)
        };
        //save invoicesWithZeroValues.csv
        invoiceWrapper.ExportListToFile(invoicesWithZeroValues, propertiesToExport, $"{path}/{nameof(invoicesWithZeroValues)}.csv");

    }
}
```
