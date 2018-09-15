using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CsvManager.ConsoleApp
{
    public class InvoiceWrapper : CsvManager<Invoice>
    {
        public override Invoice GetModelFromCsvRow(CsvRow csvRow)
        {
            var invoice = new Invoice
            {
                Client = csvRow[0],
                ZIPCode = csvRow[1],
                AdressLine1 = csvRow[2],
                AdressLine2 = csvRow[3],
                City = csvRow[4],
                State = csvRow[5],
                InvoiceValue = decimal.Parse(csvRow[6], CultureInfo.InvariantCulture),
                PageNumber = Convert.ToInt32(csvRow[7])
            };
            return invoice;
        }

        public override bool IsValidEntry(Invoice model)
        {
            //Remova todos os registros com CEP Inválido (aplicar regras);
            return Utils.IsValidCep(model.CEP);
        }
    }
}
