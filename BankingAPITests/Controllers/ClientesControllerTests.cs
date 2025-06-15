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
    [TestClass()]
    public class ClientesControllerTests
    {
        private DbContextOptions<BankingAPIContext> CrearOpcionesEnMemoria()
        {
            return new DbContextOptionsBuilder<BankingAPIContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid()) // Aislamiento por test
                .Options;
        }

        [TestMethod()]
        public async Task PostCliente_DeberiaCrearClienteCorrectamente()
        {
            // Arrange
            var opciones = CrearOpcionesEnMemoria();
            using var context = new BankingAPIContext(opciones);
            var controller = new ClientesController(context);

            var cliente = new Cliente
            {
                Nombre = "Maria Perez",
                FechaNacimiento = "1992-10-05",
                Sexo = "Femenino",
                Ingresos = 5000
            };

            // Act
            var resultado = await controller.PostCliente(cliente);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.IsInstanceOfType(resultado.Result, typeof(CreatedAtActionResult));

            var creado = resultado.Result as CreatedAtActionResult;
            Assert.AreEqual("GetCliente", creado.ActionName);

            var clienteCreado = creado.Value as Cliente;
            Assert.IsNotNull(clienteCreado);
            Assert.AreEqual("Maria Perez", clienteCreado.Nombre);
            Assert.AreNotEqual(0, clienteCreado.Id); // Verifica que tenga ID asignado

            // Verificar que se guardó en la base de datos
            var clienteEnDb = await context.Cliente.FindAsync(clienteCreado.Id);
            Assert.IsNotNull(clienteEnDb);
            Assert.AreEqual("Maria Perez", clienteEnDb.Nombre);
        }
    }
}