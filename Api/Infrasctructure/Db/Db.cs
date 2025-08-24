using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;
using ProjetoAvanadeApi.Domain.Entities;

namespace ProjetoAvanadeApi.Infrasctructure.Db
{
    public class Db : DbContext
    {
       private readonly IConfiguration? _configuration;
        
        public Db(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Db(DbContextOptions<Db> options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; } = default!;
        public DbSet<Vehicle> Vehicle { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().ToTable("Vehicles");
            
            modelBuilder.Entity<Admin>().HasData(
                new Admin {
                    Id = 1,Email = "useradm@teste.com", Senha = "123456", Perfil = "Adm"
                }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_configuration != null)
            {
                var stringDeConexao = _configuration.GetConnectionString("mysql")?.ToString();
                if (!string.IsNullOrEmpty(stringDeConexao))
                {
                    optionsBuilder.UseMySql(stringDeConexao, ServerVersion.AutoDetect(stringDeConexao));
                }
            }
        }
    }
}