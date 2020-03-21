using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.Npce;

namespace Npce_WinClient.Test
{
    [TestClass]
    public class Lol
    {

        [TestMethod]
        public void RecuperaIdRichiesta()
        {
            Ambiente ambiente = null;
            Servizio servizio = null;

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
                Username = "rete\\mic32nv",
                Password = "Passw0rd"
            };


            var operation = new RecuperaIdRichiestaLol(ambiente);

            var idRichiesta = operation.Execute();

        }

        private async Task<Servizio> GetServizio()
        {
            Servizio servizio;
            var servizioRepository = new ServizioRepository(new NpceDbContext());

            servizio = await servizioRepository.GetByIdAsync(9);

            return servizio;
        }

        [TestMethod]

        public void Invio()
        {
            Ambiente ambiente = null;
            Servizio servizio = null;

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
                Username = "rete\\mic32nv",
                Password = "Passw0rd"
            };

            servizio = GetServizio().Result;

            var operation = new RecuperaIdRichiestaLol(ambiente);

            var idRichiesta = operation.Execute();

            var invioOperation = new InvioLol(ambiente, servizio, idRichiesta);

            invioOperation.Execute();
        }
    }
}
