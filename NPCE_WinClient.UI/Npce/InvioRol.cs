using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;
using NPCE_WinClient.Services.Rol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using reference = NPCE_WinClient.Services;

namespace NPCE_WinClient.UI.Npce
{
    public class InvioRol : NpceOperationBase
    {
        private readonly Ambiente _ambiente;
        private readonly Servizio _servizio;
        private readonly string _idRichiesta;
        ROLServiceSoap _proxy;
        public InvioRol(Ambiente ambiente, Servizio servizio, string idRichiesta)
        {
            _ambiente = ambiente;
            _servizio = servizio;
            _idRichiesta = idRichiesta;
        }

        public NpceOperationResult Execute()
        {
            var helper = new Helper();
            _proxy = helper.GetProxy<ROLServiceSoap>(_ambiente.RolUri, _ambiente.Username, _ambiente.Password);
            ROLSubmit rolSubmit = new ROLSubmit();

            SetMittente(rolSubmit);
            SetDestinatari(rolSubmit);
            SetDocumenti(rolSubmit);
            SetOpzioni(rolSubmit);
            


            var fake = new OperationContextScope((IContextChannel)_proxy);
            var headers = GetHttpHeaders(_ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            var invioResult = _proxy.Invio(_idRichiesta, string.Empty, rolSubmit);

            return CreateResult(NpceOperation.Invio, invioResult.CEResult.Code, invioResult.CEResult.Description, invioResult.IDRichiesta);
        }

        



        private void SetOpzioni(ROLSubmit rolSubmit)
        {
            var opzioni = new ROLSubmitOpzioni();

            opzioni.OpzionidiStampa = new OpzionidiStampa { PageSize = OpzionidiStampaPageSize.A4 };

            rolSubmit.Opzioni = opzioni;
        }

        private void SetDocumenti(ROLSubmit rolSubmit)
        {

            Services.Rol.Documento newDocumento;
            var listDocumenti = new List<reference.Rol.Documento>();

            foreach (var documento in _servizio.Documenti)
            {
                newDocumento = NewDocumento(documento);
                listDocumenti.Add(newDocumento);
            }

            rolSubmit.Documento = listDocumenti.ToArray();
        }

        private reference.Rol.Documento NewDocumento(Model.Documento documento)
        {
            return new reference.Rol.Documento { MD5 = GetMD5(documento), Immagine = documento.Content, TipoDocumento = documento.Extension };
        }

        private string GetMD5(Model.Documento documento)
        {
            using (System.Security.Cryptography.MD5CryptoServiceProvider cryptoService = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                byte[] Ret = cryptoService.ComputeHash(documento.Content);
                return BitConverter.ToString(Ret).Replace("-", "");
            }
        }

        private void SetDestinatari(ROLSubmit rolSubmit)
        {

            int count = 0;

            var destinatariServizioList = _servizio.Anagrafiche.Where(d => d.IsMittente == true).ToList();

            var listDestinatari = new List<Destinatario>();

            foreach (var destinatarioServizio in destinatariServizioList)
            {
                count++;
                Destinatario newDestinatario = NewDestinatario(destinatarioServizio);
                listDestinatari.Add(newDestinatario);
            }

            rolSubmit.Destinatari = listDestinatari.ToArray();

            rolSubmit.NumeroDestinatari = count;

        }

        private Destinatario NewDestinatario(Anagrafica destinatarioServizio)
        {
            var destinatario = new Destinatario();

            var nominativo = new Nominativo
            {
                Nome = destinatarioServizio.Nome,
                Cognome = destinatarioServizio.Cognome,
                Indirizzo = new Indirizzo
                {
                    DUG = destinatarioServizio.DUG,
                    Toponimo = destinatarioServizio.Toponimo,
                    Esponente = destinatarioServizio.Esponente,
                    NumeroCivico = destinatarioServizio.NumeroCivico
                },
                CAP = destinatarioServizio.Cap,
                CasellaPostale = destinatarioServizio.CasellaPostale,
                Citta = destinatarioServizio.Citta,
                ComplementoIndirizzo = destinatarioServizio.ComplementoIndirizzo,
                ComplementoNominativo = destinatarioServizio.ComplementoNominativo,
                Provincia = destinatarioServizio.Provincia,
                Stato = destinatarioServizio.Stato,
                RagioneSociale = destinatarioServizio.RagioneSociale
            };

            destinatario.Nominativo = nominativo;

            return destinatario;
        }

        private void SetMittente(ROLSubmit rolSubmit)
        {

            var mittenteServizio = _servizio.Anagrafiche.Single(d => d.IsMittente == true);
            var mittente = new Mittente();

            var nominativo = new Nominativo
            {
                Nome = mittenteServizio.Nome,
                Cognome = mittenteServizio.Cognome,
                Indirizzo = new Indirizzo
                {
                    DUG = mittenteServizio.DUG,
                    Toponimo = mittenteServizio.Toponimo,
                    Esponente = mittenteServizio.Esponente,
                    NumeroCivico = mittenteServizio.NumeroCivico
                },
                CAP = mittenteServizio.Cap,
                CasellaPostale = mittenteServizio.CasellaPostale,
                Citta = mittenteServizio.Citta,
                ComplementoIndirizzo = mittenteServizio.ComplementoIndirizzo,
                ComplementoNominativo = mittenteServizio.ComplementoNominativo,
                Provincia = mittenteServizio.Provincia,
                Stato = mittenteServizio.Stato,
                RagioneSociale = mittenteServizio.RagioneSociale
            };

            mittente.Nominativo = nominativo;

            rolSubmit.Mittente = mittente;

        }
    }
}
