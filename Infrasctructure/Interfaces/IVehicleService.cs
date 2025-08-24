using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoAvanadeApi.Domain.DTOS;
using ProjetoAvanadeApi.Domain.Entities;

namespace ProjetoAvanadeApi.Infrasctructure.Interfaces
{
    public interface IVehicleService
    {
        void Criar(Vehicle vehicles);
        List<Vehicle> Todos(int? pagina, string? nome = null, string? marca = null, int? ano = null);
        Vehicle? BuscarPorId(int id);
        void Atualizar(Vehicle vehicles);
        void Deletar(Vehicle vehicles);
    }
}