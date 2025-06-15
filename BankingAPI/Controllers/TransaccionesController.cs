using BankingAPI.Data;
using BankingAPI.DTO;
using BankingAPI.DTOs;
using BankingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionesController : ControllerBase
    {
        private readonly BankingAPIContext _context;

        public TransaccionesController(BankingAPIContext context)
        {
            _context = context;
        }


        [HttpGet("transaccion/{id}")]
        public async Task<ActionResult<Transacciones>> GetTransaccion(int id)
        {
            var transaccion = await _context.Transacciones.FindAsync(id);

            if (transaccion == null)
                return NotFound();

            return transaccion;
        }


       


        //Consultar Transacciones por numero de cuenta

        [HttpGet("por-cuenta/{numeroCuenta}")]
        public async Task<ActionResult<IEnumerable<Transacciones>>> GetTransaccionesPorNumeroCuenta(string numeroCuenta)
        {
            var cuenta = await _context.CuentasBancarias
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);

            if (cuenta == null)
                return NotFound($"No se encontró ninguna cuenta con el número: {numeroCuenta}");

            var transacciones = await _context.Transacciones
                .Where(t => t.CuentaId == cuenta.Id)
                .OrderByDescending(t => t.Fecha)
                .ToListAsync();

            if (!transacciones.Any())
                return NotFound("No se encontraron transacciones para esta cuenta.");

            return transacciones;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<Transacciones>> RegistrarTransaccion(RegistrarTransaccionDTO dto)
        {
            // Buscar cuenta por número
            var cuenta = await _context.CuentasBancarias
                .FirstOrDefaultAsync(c => c.NumeroCuenta == dto.NumeroCuenta);

            if (cuenta == null)
                return NotFound("Cuenta bancaria no encontrada.");

            // Validar tipo
            var tipo = dto.Tipo?.Trim().ToLower();
            if (tipo != "deposito" && tipo != "retiro")
                return BadRequest("Tipo de transacción inválido. Usa 'Deposito' o 'Retiro'.");

            // Procesar transacción
            if (tipo == "retiro")
            {
                if (cuenta.Saldo < dto.Monto)
                    return BadRequest("Fondos insuficientes para realizar el retiro.");

                cuenta.Saldo -= dto.Monto;
            }
            else if (tipo == "deposito")
            {
                cuenta.Saldo += dto.Monto;
            }

            // Crear y registrar transacción
            var transaccion = new Transacciones
            {
                Tipo = tipo == "deposito" ? "Deposito" : "Retiro",
                Monto = dto.Monto,
                Fecha = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                SaldoPosterior = cuenta.Saldo,
                CuentaId = cuenta.Id
            };

            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaccion), new { id = transaccion.Id }, transaccion);
        }


        [HttpGet("resumen/{numeroCuenta}")]
        public async Task<ActionResult<ResumenCuentaDTO>> ObtenerResumenPorNumeroCuenta(string numeroCuenta)
        {
            var cuenta = await _context.CuentasBancarias
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);

            if (cuenta == null)
                return NotFound($"No se encontró la cuenta número: {numeroCuenta}");

            var transacciones = await _context.Transacciones
                .Where(t => t.CuentaId == cuenta.Id)
                .OrderBy(t => t.Fecha)
                .Select(t => new ResumenTransaccionDTO
                {
                    Id = t.Id,
                    Tipo = t.Tipo ?? "",
                    Monto = t.Monto,
                    Fecha = t.Fecha ?? "",
                    SaldoPosterior = t.SaldoPosterior
                })
                .ToListAsync();

            var resumen = new ResumenCuentaDTO
            {
                NumeroCuenta = numeroCuenta,
                SaldoFinal = cuenta.Saldo,
                Transacciones = transacciones
            };

            return Ok(resumen);
        }


    }
}
