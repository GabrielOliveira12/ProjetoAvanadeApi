using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjetoAvanadeApi.Api.Domain.Services;
using ProjetoAvanadeApi.Domain.Entities;
using ProjetoAvanadeApi.Domain.DTOS;
using ProjetoAvanadeApi.Domain.Services;
using ProjetoAvanadeApi.Infrasctructure.Db;

namespace Test.Domain.Services
{
    [TestClass]
    public class AdminServicesTests
    {
        private Db CreateContextTests()
        {
            var options = new DbContextOptionsBuilder<Db>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new Db(options);
        }

        [TestMethod]
        public void TestCreateAdmin()
        {
            var context = CreateContextTests();
            var adminDto = new AdminDTO
            {
                Email = "admin@example.com",
                Senha = "admin123",
                Perfil = "adm"
            };

            var adminService = new AdminService(context);

            adminService.Criar(adminDto);

            var admins = adminService.Todos(1);
            Assert.AreEqual(1, admins.Count);
            Assert.AreEqual("admin@example.com", admins.First().Email);
        }

        [TestMethod]
        public void TestLoginAdmin_ValidCredentials()
        {
            var context = CreateContextTests();
            var adminService = new AdminService(context);
            
            var adminDto = new AdminDTO
            {
                Email = "admin@test.com",
                Senha = "password123",
                Perfil = "adm"
            };
            
            adminService.Criar(adminDto);

            var loginDto = new LoginDTO
            {
                Email = "admin@test.com",
                Senha = "password123"
            };

            var result = adminService.Login(loginDto);

            Assert.IsNotNull(result);
            Assert.AreEqual("admin@test.com", result.Email);
        }

        [TestMethod]
        public void TestLoginAdmin_InvalidCredentials()
        {
            var context = CreateContextTests();
            var adminService = new AdminService(context);
            
            var loginDto = new LoginDTO
            {
                Email = "admin@test.com",
                Senha = "wrongpassword"
            };

            var result = adminService.Login(loginDto);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestGetAllAdmins()
        {
            var context = CreateContextTests();
            var adminService = new AdminService(context);

            for (int i = 1; i <= 5; i++)
            {
                var adminDto = new AdminDTO
                {
                    Email = $"admin{i}@test.com",
                    Senha = "password123",
                    Perfil = "user"
                };
                adminService.Criar(adminDto);
            }

            var result = adminService.Todos(1);

            Assert.AreEqual(5, result.Count);
        }

        [TestMethod]
        public void TestGetAdminById()
        {
            var context = CreateContextTests();
            var adminService = new AdminService(context);
            
            var adminDto = new AdminDTO
            {
                Email = "admin@test.com",
                Senha = "password123",
                Perfil = "adm"
            };
            
            adminService.Criar(adminDto);
            var createdAdmin = context.Admins.First();

            var result = adminService.BuscarPorId(createdAdmin.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual("admin@test.com", result.Email);
        }

        [TestMethod]
        public void TestUpdateAdmin()
        {
            var context = CreateContextTests();
            var adminService = new AdminService(context);
            
            var adminDto = new AdminDTO
            {
                Email = "admin@test.com",
                Senha = "password123",
                Perfil = "adm"
            };
            
            adminService.Criar(adminDto);
            var admin = context.Admins.First();
            
            admin.Email = "updated@test.com";

            adminService.Atualizar(admin);

            var updatedAdmin = adminService.BuscarPorId(admin.Id);
            Assert.IsNotNull(updatedAdmin);
            Assert.AreEqual("updated@test.com", updatedAdmin.Email);
        }

        [TestMethod]
        public void TestDeleteAdmin()
        {
            var context = CreateContextTests();
            var adminService = new AdminService(context);
            
            var adminDto = new AdminDTO
            {
                Email = "admin@test.com",
                Senha = "password123",
                Perfil = "adm"
            };
            
            adminService.Criar(adminDto);
            var admin = context.Admins.First();

            adminService.Deletar(admin);

            var deletedAdmin = adminService.BuscarPorId(admin.Id);
            Assert.IsNull(deletedAdmin);
        }

        [TestMethod]
        public void TestGetAdminsWithPagination()
        {
            var context = CreateContextTests();
            var adminService = new AdminService(context);

            for (int i = 1; i <= 15; i++)
            {
                var adminDto = new AdminDTO
                {
                    Email = $"admin{i}@test.com",
                    Senha = "password123",
                    Perfil = "user"
                };
                adminService.Criar(adminDto);
            }

            var page1 = adminService.Todos(1);
            var page2 = adminService.Todos(2);

            Assert.AreEqual(5, page2.Count);  // Second page should have 5 items
        }
    }
}