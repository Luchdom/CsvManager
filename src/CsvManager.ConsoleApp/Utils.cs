using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CsvManager.ConsoleApp
{
    public static class Utils
    {
        public static bool IsValidCep(string cep)
        {
            if (string.IsNullOrEmpty(cep))
                return false;

            cep = cep.Replace("-", "").Replace(".", "").Replace(" ", "");

            var regexCep = new Regex(@"^\d{8}$");
            return regexCep.IsMatch(cep) && cep != "00000000";
        }
    }
}
