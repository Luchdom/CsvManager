using System;
using System.Collections.Generic;
using System.Text;

namespace CsvManager.ConsoleApp
{
    //Model
    public class Invoice
    {
        public string Client { get; set; }
        public string ZIPCode { get; set; }
        public string AdressLine1 { get; set; }
        public string AdressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string CompleteAddress => $"{AdressLine1} - {AdressLine2} - {CEP} - {City}/{State}";

        public decimal InvoiceValue { get; set; }
        public int PageNumber { get; set; }
    }
}
