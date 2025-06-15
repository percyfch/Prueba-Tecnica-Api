using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BankingAPI.Models
{
    public class Transacciones
    {

        public int Id { get; set; }

        public string? Tipo { get; set; }

        public decimal Monto { get; set; }

        public string? Fecha     { get; set; }

        public decimal SaldoPosterior { get; set; }

        public int CuentaId     { get; set; }

    }
}
