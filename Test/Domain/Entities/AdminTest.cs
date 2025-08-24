using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjetoAvanadeApi.Domain.Entities;

namespace Test.Domain.Entities
{
    public class AdminTest
    {
        [TestMethod]
        public void TestAdminCreation()
        {
            var adm = new Admin();

            adm.Id = 1;
            adm.Email = "admin@example.com";
            adm.Senha = "admin123";
            adm.Perfil = "adm";
            
            Assert.AreEqual(1, adm.Id);
            Assert.AreEqual("admin@example.com", adm.Email);
            Assert.AreEqual("admin123", adm.Senha);
            Assert.AreEqual("adm", adm.Perfil);
        }
    }
}