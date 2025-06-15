using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BankingAPI.Models
{
    public class CuentasBancarias
    {
        public int Id { get; set; }

        public string? NumeroCuenta  { get; set; }

        public  decimal Saldo   { get; set; }

        public int ClienteId    { get; set; }


    }
}
