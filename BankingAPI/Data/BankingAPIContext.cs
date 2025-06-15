using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BankingAPI.Models;

namespace BankingAPI.Data
{
    public class BankingAPIContext : DbContext
    {
        public BankingAPIContext (DbContextOptions<BankingAPIContext> options)
            : base(options)
        {
        }

        public DbSet<BankingAPI.Models.Cliente> Cliente { get; set; } = default!;
        public DbSet<BankingAPI.Models.CuentasBancarias> CuentasBancarias { get; set; } = default!;
        public DbSet<BankingAPI.Models.Transacciones> Transacciones { get; set; } = default!;
    }
}
