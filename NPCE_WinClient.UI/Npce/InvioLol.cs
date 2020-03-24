using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;
using NPCE_WinClient.Services.Lol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using reference = NPCE_WinClient.Services.Lol;

namespace NPCE_WinClient.UI.Npce
{
    public class InvioLol : NpceOperationBase
    {
        private readonly Ambiente _ambiente;
        private readonly Servizio _servizio;
        private readonly string _idRichiesta;
        private readonly NpceDbContext _context;
        LOLServiceSoap _proxy;
        public InvioLol(Ambiente ambiente, Servizio servizio, string idRichiesta)
        {
            _ambiente = ambiente;
            _servizio = servizio;
            _idRichiesta = idRichiesta;
        }

        public NpceOperationResult Execute()
        {
            var helper = new Helper();
            _proxy = helper.GetProxy<LOLServiceSoap>(_ambiente.LolUri, _ambiente.Username, _ambiente.Password);
            LOLSubmit lolSubmit = new LOLSubmit();

            SetMittente(lolSubmit);
            SetDestinatari(lolSubmit);
            SetDocumenti(lolSubmit);
            SetOpzioni(lolSubmit);
            if (_servizio.TipoServizio.Descrizione=="Posta1")
            {
                SetPosta1(lolSubmit);
            }           


            var fake = new OperationContextScope((IContextChannel)_proxy);
            var headers = GetHttpHeaders(_ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            var invioResult = _proxy.Invio(_idRichiesta, string.Empty, lolSubmit);

            return CreateResult(NpceOperation.Invio, invioResult.CEResult);
        }

        private void SetPosta1(LOLSubmit lolSubmit)
        {
            lolSubmit.DescrizioneLettera = new DescrizioneLettera { TipoLettera = "Posta1" };
        }

        

        private void SetOpzioni(LOLSubmit lolSubmit)
        {
            var opzioni = new LOLSubmitOpzioni();

            opzioni.OpzionidiStampa = new OpzionidiStampa { PageSize = OpzionidiStampaPageSize.A4 };

            lolSubmit.Opzioni = opzioni;
        }

        private void SetDocumenti(LOLSubmit lolSubmit)
        {

            Services.Lol.Documento newDocumento;
            var listDocumenti = new List<reference.Documento>();

            foreach (var documento in _servizio.Documenti)
            {
                newDocumento = NewDocumento(documento);
                listDocumenti.Add(newDocumento);
            }

            lolSubmit.Documento = listDocumenti.ToArray();
        }

        private reference.Documento NewDocumento(Model.Documento documento)
        {
            return new reference.Documento { MD5 = GetMD5(documento), Immagine = documento.Content, TipoDocumento = documento.Extension };
        }

        private string GetMD5(Model.Documento documento)
        {
            using (System.Security.Cryptography.MD5CryptoServiceProvider cryptoService = new System.Security.Cryptography.MD5CryptoServiceProvider())
            {
                byte[] Ret = cryptoService.ComputeHash(documento.Content);
                return BitConverter.ToString(Ret).Replace("-", "");
            }
        }

        private void SetDestinatari(LOLSubmit lolSubmit)
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

            lolSubmit.Destinatari = listDestinatari.ToArray();

            lolSubmit.NumeroDestinatari = count;

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

        private void SetMittente(LOLSubmit lolSubmit)
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

            lolSubmit.Mittente = mittente;

        }
    }
}
