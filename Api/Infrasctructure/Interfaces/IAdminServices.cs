using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoAvanadeApi.Domain.DTOS;
using ProjetoAvanadeApi.Domain.Entities;

namespace ProjetoAvanadeApi.Infrasctructure.Interfaces
{
    public interface IAdminServices
    {
        void Criar(AdminDTO adminDTO);
        List<Admin> Todos(int? pagina, string? email = null, string? perfil = null);
        Admin? BuscarPorId(int id);
        void Atualizar(Admin admin);
        void Deletar(Admin admin);
        Admin? Login(LoginDTO loginDTO);
    }
}