using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.Npce;

namespace Npce_WinClient.Test
{
    [TestClass]
    public class Visure
    {
        private async Task<Visura> GetVisura(int id)
        {
            Visura visura;
            var visureRepository = new VisureRepository(new NpceDbContext());
            visura = await visureRepository.GetByIdAsync(id);
            return visura;
        }

        [TestMethod]
        public void Invio_Pil()
        {
            Ambiente ambiente = null;
            Visura visura = null;

            ambiente = new Ambiente
            {
                customerid = "nello.citta.npce",
                costcenter = "UNF",
                billingcenter = "IdCdF",
                idsender = "999988",
                sendersystem = "H2H",
                smuser = "nello.citta.npce",
                contracttype = "PosteWeb",
                usertype = "B",
                LolUri = "http://10.10.5.101/LOLGC/LolService.svc",
                VolUri="http://172.21.21.4/NPCE_EntryPoint/WscE.svc",
                Username = "rete\\mic32nv",
                Password = "Passw0rd"
            };

            visura = GetVisura(8).Result;

            visura.VisureTipoRecapito = new VisureTipoRecapito { Id = "D", Descrizione = "Download" };
            
            var operation = new InvioVisuraPIL(visura, ambiente);

            operation.Execute();
            
        }

    }
}
