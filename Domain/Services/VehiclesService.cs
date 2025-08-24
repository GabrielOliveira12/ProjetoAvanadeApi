using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ProjetoAvanadeApi.Domain.DTOS;
using ProjetoAvanadeApi.Domain.Entities;
using ProjetoAvanadeApi.Infrasctructure.Db;
using ProjetoAvanadeApi.Infrasctructure.Interfaces;


namespace ProjetoAvanadeApi.Domain.Services
{
    public class VehiclesService : IVehicleService
    {
         private readonly Db _context;

        public VehiclesService(Db context)
        {
            _context = context;
        }
         public void Criar(Vehicle vehicles)
        {
            _context.Vehicle.Add(vehicles);
            _context.SaveChanges();
        }

        public void Atualizar(Vehicle vehicles)
        {
            _context.Vehicle.Update(vehicles);
            _context.SaveChanges();
        }

        public List<Vehicle> Todos(int? pagina, string? nome = null, string? marca = null, int? ano = null)
        {
            var query = _context.Vehicle.AsQueryable();

            if (!string.IsNullOrEmpty(nome))
            {
                query = query.Where(v => v.Nome.Contains(nome));
            }

            if (!string.IsNullOrEmpty(marca))
            {
                query = query.Where(v => v.Marca.Contains(marca));
            }

            if (ano.HasValue)
            {
                query = query.Where(v => v.Ano == ano.Value);
            }

            int itensPorPagina = 10;

            if (pagina != null)
                return query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina).ToList();
            
            return query.ToList();  
        }
        public Vehicle? BuscarPorId(int id)
        {
            return _context.Vehicle.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Deletar(Vehicle vehicles)
        {
            _context.Vehicle.Remove(vehicles);
            _context.SaveChanges();
        }
    }
}