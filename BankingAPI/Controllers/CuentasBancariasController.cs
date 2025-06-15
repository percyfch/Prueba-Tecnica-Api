using BankingAPI.Data;
using BankingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentasBancariasController : ControllerBase
    {
        private readonly BankingAPIContext _context;

        public CuentasBancariasController(BankingAPIContext context)
        {
            _context = context;
        }

        // GET: Traer la lista de cuentas reguis
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuentasBancarias>>> GetCuentas()
        {
            return await _context.CuentasBancarias.ToListAsync();
        }



        [HttpPost]
        public async Task<ActionResult<CuentasBancarias>> PostCuenta(CuentasBancarias cuenta)
        {
            // Validar que el número de cuenta no exista
            var existe = await _context.CuentasBancarias
                .AnyAsync(c => c.NumeroCuenta == cuenta.NumeroCuenta);

            if (existe)
                return BadRequest($"Ya existe una cuenta con el número: {cuenta.NumeroCuenta}");

            _context.CuentasBancarias.Add(cuenta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCuentaPorNumero), new { numeroCuenta = cuenta.NumeroCuenta }, cuenta);
        }

        // PUT: api/CuentasBancarias/5

        [HttpPut("numero/{numeroCuenta}")]
        public async Task<IActionResult> PutCuenta(string numeroCuenta, CuentasBancarias cuentaActualizada)
        {
            // Buscar la cuenta original por su número
            var cuenta = await _context.CuentasBancarias
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);

            if (cuenta == null)
                return NotFound($"No se encontró ninguna cuenta con el número: {numeroCuenta}");

            // Actualizar propiedades deseadas (puedes personalizar esto)
            cuenta.Saldo = cuentaActualizada.Saldo;
            cuenta.ClienteId = cuentaActualizada.ClienteId;

            // Guardar cambios
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error de concurrencia al actualizar la cuenta.");
            }

            return NoContent();
        }




        //Consultar por numero de cuenta
        [HttpGet("numero/{numeroCuenta}")]
        public async Task<ActionResult<CuentasBancarias>> GetCuentaPorNumero(string numeroCuenta)
        {
            var cuenta = await _context.CuentasBancarias
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);

            if (cuenta == null)
                return NotFound($"No se encontró ninguna cuenta con el número: {numeroCuenta}");

            return cuenta;
        }

        //Saldo por numero de cuenta
        [HttpGet("saldo/{numeroCuenta}")]
        public async Task<ActionResult<decimal>> GetSaldoPorNumeroCuenta(string numeroCuenta)
        {
            var cuenta = await _context.CuentasBancarias
                .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);

            if (cuenta == null)
                return NotFound($"No se encontró ninguna cuenta con el número: {numeroCuenta}");

            return Ok(cuenta.Saldo);
        }




    }
}
