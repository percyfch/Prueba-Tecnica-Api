using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankingAPI.Controllers;
using BankingAPI.Data;
using BankingAPI.Models;
using BankingAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using BankingAPI.DTO;

namespace BankingAPI.Controllers.Tests
{
    [TestClass]
    public class TransaccionesControllerTests
    {
        private DbContextOptions<BankingAPIContext> CrearOpcionesEnMemoria()
        {
            return new DbContextOptionsBuilder<BankingAPIContext>()
                .UseInMemoryDatabase("TestDb_" + Guid.NewGuid())
                .Options;
        }

        [TestMethod]
        public async Task RegistrarTransaccion_Deposito_DeberiaCrearTransaccionYCambiarSaldo()
        {
            // Arrange
            var opciones = CrearOpcionesEnMemoria();
            await using var context = new BankingAPIContext(opciones);
            // Creamos una cuenta con saldo inicial 100
            var cuenta = new CuentasBancarias { NumeroCuenta = "NUM001", Saldo = 100m, ClienteId = 1 };
            context.CuentasBancarias.Add(cuenta);
            await context.SaveChangesAsync();
            var controller = new TransaccionesController(context);

            var dto = new RegistrarTransaccionDTO
            {
                NumeroCuenta = "NUM001",
                Tipo = "Deposito",
                Monto = 50m
            };

            // Act
            var resultado = await controller.RegistrarTransaccion(dto);

            // Assert
            Assert.IsInstanceOfType(resultado.Result, typeof(CreatedAtActionResult));
            var created = resultado.Result as CreatedAtActionResult;
            Assert.AreEqual(nameof(TransaccionesController.GetTransaccion), created.ActionName);

            var trans = created.Value as Transacciones;
            Assert.IsNotNull(trans);
            Assert.AreEqual("Deposito", trans.Tipo);
            Assert.AreEqual(50m, trans.Monto);
            Assert.AreEqual(150m, trans.SaldoPosterior);
            Assert.AreEqual(cuenta.Id, trans.CuentaId);

            // Verificar que el saldo de la cuenta en BD cambió también
            var enDbCuenta = await context.CuentasBancarias.FirstAsync(c => c.Id == cuenta.Id);
            Assert.AreEqual(150m, enDbCuenta.Saldo);
        }

        [TestMethod]
        public async Task RegistrarTransaccion_RetiroInsuficiente_DeberiaBadRequest()
        {
            // Arrange
            var opciones = CrearOpcionesEnMemoria();
            await using var context = new BankingAPIContext(opciones);
            var cuenta = new CuentasBancarias { NumeroCuenta = "NUM002", Saldo = 30m, ClienteId = 1 };
            context.CuentasBancarias.Add(cuenta);
            await context.SaveChangesAsync();
            var controller = new TransaccionesController(context);

            var dto = new RegistrarTransaccionDTO
            {
                NumeroCuenta = "NUM002",
                Tipo = "Retiro",
                Monto = 50m
            };

            // Act
            var resultado = await controller.RegistrarTransaccion(dto);

            // Assert
            Assert.IsInstanceOfType(resultado.Result, typeof(BadRequestObjectResult));
            var bad = resultado.Result as BadRequestObjectResult;
            StringAssert.Contains(bad.Value.ToString(), "Fondos insuficientes");
        }

        [TestMethod]
        [DataRow("invalid", "Tipo de transacción inválido")]
        [DataRow("", "Tipo de transacción inválido")]
        public async Task RegistrarTransaccion_TipoInvalido_DeberiaBadRequest(string tipo, string mensajeEsperado)
        {
            // Arrange
            var opciones = CrearOpcionesEnMemoria();
            await using var context = new BankingAPIContext(opciones);
            context.CuentasBancarias.Add(new CuentasBancarias { NumeroCuenta = "NUM003", Saldo = 100m, ClienteId = 1 });
            await context.SaveChangesAsync();
            var controller = new TransaccionesController(context);

            var dto = new RegistrarTransaccionDTO
            {
                NumeroCuenta = "NUM003",
                Tipo = tipo,
                Monto = 10m
            };

            // Act
            var resultado = await controller.RegistrarTransaccion(dto);

            // Assert
            Assert.IsInstanceOfType(resultado.Result, typeof(BadRequestObjectResult));
            var bad = resultado.Result as BadRequestObjectResult;
            StringAssert.Contains(bad.Value.ToString(), mensajeEsperado);
        }

        [TestMethod]
        public async Task RegistrarTransaccion_CuentaNoEncontrada_DeberiaNotFound()
        {
            // Arrange
            var opciones = CrearOpcionesEnMemoria();
            await using var context = new BankingAPIContext(opciones);
            var controller = new TransaccionesController(context);

            var dto = new RegistrarTransaccionDTO
            {
                NumeroCuenta = "NO_EXISTE",
                Tipo = "Deposito",
                Monto = 10m
            };

            // Act
            var resultado = await controller.RegistrarTransaccion(dto);

            // Assert
            Assert.IsInstanceOfType(resultado.Result, typeof(NotFoundObjectResult));
            var notFound = resultado.Result as NotFoundObjectResult;
            StringAssert.Contains(notFound.Value.ToString(), "Cuenta bancaria no encontrada");
        }
    }
}