using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjetoAvanadeApi.Domain.Entities;
using ProjetoAvanadeApi.Domain.Services;
using ProjetoAvanadeApi.Infrasctructure.Db;

namespace Test.Domain.Services
{
    [TestClass]
    public class VehicleServicesTests
    {
        private Db CreateContextTests()
        {
            var options = new DbContextOptionsBuilder<Db>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new Db(options);
        }

        [TestMethod]
        public void TestCreateVehicle()
        {
            var context = CreateContextTests();
            var vehicle = new Vehicle
            {
                Nome = "Civic",
                Marca = "Honda",
                Ano = 2020
            };

            var vehicleService = new VehiclesService(context);

            vehicleService.Criar(vehicle);

            var vehicles = vehicleService.Todos(1);
            Assert.AreEqual(1, vehicles.Count);
            Assert.AreEqual("Civic", vehicles.First().Nome);
            Assert.AreEqual("Honda", vehicles.First().Marca);
            Assert.AreEqual(2020, vehicles.First().Ano);
        }

        [TestMethod]
        public void TestGetAllVehicles()
        {
            var context = CreateContextTests();
            var vehicleService = new VehiclesService(context);

            for (int i = 1; i <= 5; i++)
            {
                var vehicle = new Vehicle
                {
                    Nome = $"Vehicle {i}",
                    Marca = "Test Brand",
                    Ano = 2020 + i
                };
                vehicleService.Criar(vehicle);
            }

            var result = vehicleService.Todos(1);

            Assert.AreEqual(5, result.Count);
        }

        [TestMethod]
        public void TestGetVehicleById()
        {
            var context = CreateContextTests();
            var vehicleService = new VehiclesService(context);
            
            var vehicle = new Vehicle
            {
                Nome = "Corolla",
                Marca = "Toyota",
                Ano = 2022
            };
            
            vehicleService.Criar(vehicle);
            var createdVehicle = context.Vehicle.First();

            var result = vehicleService.BuscarPorId(createdVehicle.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual("Corolla", result.Nome);
            Assert.AreEqual("Toyota", result.Marca);
            Assert.AreEqual(2022, result.Ano);
        }

        [TestMethod]
        public void TestUpdateVehicle()
        {
            var context = CreateContextTests();
            var vehicleService = new VehiclesService(context);
            
            var vehicle = new Vehicle
            {
                Nome = "Civic",
                Marca = "Honda",
                Ano = 2020
            };
            
            vehicleService.Criar(vehicle);
            var createdVehicle = context.Vehicle.First();
            
            createdVehicle.Nome = "Civic Updated";
            createdVehicle.Ano = 2023;

            vehicleService.Atualizar(createdVehicle);

            var updatedVehicle = vehicleService.BuscarPorId(createdVehicle.Id);
            Assert.IsNotNull(updatedVehicle);
            Assert.AreEqual("Civic Updated", updatedVehicle.Nome);
            Assert.AreEqual(2023, updatedVehicle.Ano);
        }

        [TestMethod]
        public void TestDeleteVehicle()
        {
            var context = CreateContextTests();
            var vehicleService = new VehiclesService(context);
            
            var vehicle = new Vehicle
            {
                Nome = "To Delete",
                Marca = "Test",
                Ano = 2020
            };
            
            vehicleService.Criar(vehicle);
            var createdVehicle = context.Vehicle.First();

            vehicleService.Deletar(createdVehicle);

            var deletedVehicle = vehicleService.BuscarPorId(createdVehicle.Id);
            Assert.IsNull(deletedVehicle);
        }

        [TestMethod]
        public void TestGetVehiclesWithPagination()
        {
            var context = CreateContextTests();
            var vehicleService = new VehiclesService(context);

            for (int i = 1; i <= 15; i++)
            {
                var vehicle = new Vehicle
                {
                    Nome = $"Vehicle {i}",
                    Marca = "Test Brand",
                    Ano = 2020 + i
                };
                vehicleService.Criar(vehicle);
            }

            var page1 = vehicleService.Todos(1);
            var page2 = vehicleService.Todos(2);

            Assert.AreEqual(5, page2.Count);  // Second page should have 5 items
        }

        [TestMethod]
        public void TestGetVehiclesWithFilters()
        {
            var context = CreateContextTests();
            var vehicleService = new VehiclesService(context);

            var vehicles = new[]
            {
                new Vehicle { Nome = "Civic", Marca = "Honda", Ano = 2020 },
                new Vehicle { Nome = "Corolla", Marca = "Toyota", Ano = 2021 },
                new Vehicle { Nome = "Accord", Marca = "Honda", Ano = 2022 },
                new Vehicle { Nome = "Camry", Marca = "Toyota", Ano = 2020 }
            };

            foreach (var vehicle in vehicles)
            {
                vehicleService.Criar(vehicle);
            }

            // Act - Filter by marca
            var hondaVehicles = vehicleService.Todos(1, null, "Honda", null);
            var toyotaVehicles = vehicleService.Todos(1, null, "Toyota", null);
            var vehicles2020 = vehicleService.Todos(1, null, null, 2020);

            Assert.AreEqual(2, hondaVehicles.Count);
            Assert.AreEqual(2, toyotaVehicles.Count);
            Assert.AreEqual(2, vehicles2020.Count);
        }

        [TestMethod]
        public void TestGetVehicleByIdNotFound()
        {
            var context = CreateContextTests();
            var vehicleService = new VehiclesService(context);

            var result = vehicleService.BuscarPorId(999);

            Assert.IsNull(result);
        }
    }
}
