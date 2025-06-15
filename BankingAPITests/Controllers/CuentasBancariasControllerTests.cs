using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankingAPI.Controllers;
using BankingAPI.Data;
using BankingAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BankingAPI.Controllers.Tests
{
    [TestClass]
    public class CuentasBancariasControllerTests
    {
        private DbContextOptions<BankingAPIContext> CrearOpcionesEnMemoria()
        {
            return new DbContextOptionsBuilder<BankingAPIContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;
        }

        [TestMethod]
        public async Task PostCuenta_DeberiaCrearCuentaCorrectamente()
        {
            // Arrange
            var opciones = CrearOpcionesEnMemoria();
            await using var context = new BankingAPIContext(opciones);
            var controller = new CuentasBancariasController(context);

            var nuevaCuenta = new CuentasBancarias
            {
                NumeroCuenta = "CR0120129837891273897129",
                Saldo = 1000m,
                ClienteId = 1
            };

            // Act
            var resultado = await controller.PostCuenta(nuevaCuenta);

            // Assert
            // 1) Se devuelve CreatedAtActionResult
            Assert.IsInstanceOfType(resultado.Result, typeof(CreatedAtActionResult));
            var created = resultado.Result as CreatedAtActionResult;

            // 2) La acción apuntada es GetCuentaPorNumero
            Assert.AreEqual(nameof(CuentasBancariasController.GetCuentaPorNumero), created.ActionName);

            // 3) El route-value "numeroCuenta" coincide
            Assert.IsTrue(created.RouteValues.ContainsKey("numeroCuenta"));
            Assert.AreEqual("CR0120129837891273897129", created.RouteValues["numeroCuenta"]);

            // 4) El objeto devuelto es la cuenta creada
            var cuentaCreada = created.Value as CuentasBancarias;
            Assert.IsNotNull(cuentaCreada);
            Assert.AreEqual("CR0120129837891273897129", cuentaCreada.NumeroCuenta);
            Assert.AreEqual(1000m, cuentaCreada.Saldo);
            Assert.AreEqual(1, cuentaCreada.ClienteId);

            // 5) Se guardó en la base de datos en memoria
            var enDb = await context.CuentasBancarias.FindAsync(cuentaCreada.Id);
            Assert.IsNotNull(enDb);
            Assert.AreEqual("ABCCR0120129837891273897129123", enDb.NumeroCuenta);
        }

        [TestMethod]
        public async Task PostCuenta_DeberiaDevolverBadRequestSiNumeroDuplicado()
        {
            // Arrange
            var opciones = CrearOpcionesEnMemoria();
            await using var context = new BankingAPIContext(opciones);

            // Insertamos previamente una cuenta con el mismo número
            context.CuentasBancarias.Add(new CuentasBancarias
            {
                NumeroCuenta = "DUP001",
                Saldo = 500m,
                ClienteId = 2
            });
            await context.SaveChangesAsync();

            var controller = new CuentasBancariasController(context);
            var cuentaDuplicada = new CuentasBancarias
            {
                NumeroCuenta = "DUP001",
                Saldo = 800m,
                ClienteId = 3
            };

            // Act
            var resultado = await controller.PostCuenta(cuentaDuplicada);

            // Assert
            Assert.IsInstanceOfType(resultado.Result, typeof(BadRequestObjectResult));
            var bad = resultado.Result as BadRequestObjectResult;
            Assert.IsTrue(bad.Value.ToString().Contains("Ya existe una cuenta con el número"));
        }
    }
}