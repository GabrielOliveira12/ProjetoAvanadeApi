using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoAvanadeApi.Domain.DTOS;
using ProjetoAvanadeApi.Domain.Entities;
using ProjetoAvanadeApi.Infrasctructure.Db;
using ProjetoAvanadeApi.Infrasctructure.Interfaces;


namespace ProjetoAvanadeApi.Domain.Services
{
    public class AdminService : IAdminServices
    {
        private readonly Db _context;

        public AdminService(Db context)
        {
            _context = context;
        }

        public void Criar(AdminDTO adminDTO)
        {
            var admin = new Admin
            {
                Email = adminDTO.Email,
                Senha = adminDTO.Senha,
                Perfil = adminDTO.Perfil
            };
            _context.Admins.Add(admin);
            _context.SaveChanges();
        }

        public List<Admin> Todos(int? pagina, string? email = null, string? perfil = null)
        {
            var query = _context.Admins.AsQueryable();

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(a => a.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(perfil))
            {
                query = query.Where(a => a.Perfil.Contains(perfil));
            }

            int itensPorPagina = 10;

            if (pagina != null)
                return query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina).ToList();

            return query.ToList();
        }

        public Admin? BuscarPorId(int id)
        {
            return _context.Admins.Where(a => a.Id == id).FirstOrDefault();
        }
        public void Atualizar(Admin admin)
        {
            _context.Admins.Update(admin);
            _context.SaveChanges();
        }

    
        public void Deletar(Admin admin)
        {
            _context.Admins.Remove(admin);
            _context.SaveChanges();
        }

        public Admin? Login(LoginDTO loginDTO)
        {
            var adm = _context.Admins.Where(a => a.Email == loginDTO.Email && a.Senha == loginDTO.Senha).FirstOrDefault();
            return adm;
        }
    }
}