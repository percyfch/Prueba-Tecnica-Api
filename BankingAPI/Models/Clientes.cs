using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BankingAPI.Models
{
    public class Cliente
    {

        public int Id { get; set; }
        public string? Nombre { get; set; }

        public string? FechaNacimiento { get; set; }

        public string? Sexo { get; set; }

        public decimal Ingresos { get; set; }


    }
}
