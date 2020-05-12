using NPCE_WinClient.Model;
using NPCE_WinClient.Services.Col;
using NPCE_WinClient.Services.Mol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Destinatario = NPCE_WinClient.Services.Col.Destinatario;
using Documento = NPCE_WinClient.Services.Col.Documento;
using Intestazione = NPCE_WinClient.Services.Col.Intestazione;
using InvioRequest = NPCE_WinClient.Services.Col.InvioRequest;
using Mittente = NPCE_WinClient.Services.Col.Mittente;
using ProdottoPostaEvo = NPCE_WinClient.Services.Col.ProdottoPostaEvo;

namespace NPCE_WinClient.UI.Npce
{
    public class InvioCol : NpceOperationBase
    {

        IPostaContestService _proxy;
        public InvioCol(Ambiente ambiente, Servizio servizio, string idRichiesta) : base(ambiente, servizio, idRichiesta)
        {

        }


        public NpceOperationResult Execute()
        {
            var helper = new Helper();
            _proxy = helper.GetProxy<IPostaContestService>(_ambiente.ColUri, _ambiente.Username, _ambiente.Password);
            Services.Col.InvioRequest colSubmit = new InvioRequest();
            ProdottoPostaEvo tipoProdotto;
            Enum.TryParse<ProdottoPostaEvo>(_servizio.TipoServizio.Descrizione.ToUpper(), out tipoProdotto);

            colSubmit.Intestazione = new Intestazione { CodiceContratto = _ambiente.ContrattoCOL, Prodotto = tipoProdotto };

            var postaContest = new PostaContest();
            postaContest.AutoConferma = _servizio.Autoconferma;

            SetIntestazione(postaContest);
            SetMittente(postaContest);
            SetDestinatari(postaContest);
            SetDocumenti(postaContest);
            SetOpzioni(postaContest);
           

            var fake = new OperationContextScope((IContextChannel)_proxy);
            var headers = GetHttpHeaders(_ambiente);

            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            colSubmit.PostaContest = postaContest;

            var invioResult = _proxy.Invio(colSubmit);

            return CreateResult(NpceOperation.Invio, invioResult.Esito.ToString() == "OK" ? "0" : "99", invioResult.Esito.ToString(), invioResult.IdRichiesta, null, null);
        }

        private void SetIntestazione(PostaContest postaContest)
        {
        }

        private void SetOpzioni(PostaContest postaContest)
        {
            postaContest.Opzioni = new Services.Col.Opzioni();

            postaContest.Opzioni.Servizio = new Services.Col.OpzioniServizio
            {
                ArchiviazioneDocumenti = false

            };

            postaContest.Opzioni.Stampa = new Services.Col.OpzioniStampa
            {
                FronteRetro = _servizio.FronteRetro,
                TipoColore = _servizio.Colore ? Services.Col.TipoColore.COLORE : Services.Col.TipoColore.BW
            };

            // Dati Opzionali

        }

        private void SetDocumenti(PostaContest postaContest)
        {
            Documento newDocumento;
            var listDocumenti = new List<Documento>();

            foreach (var documento in _servizio.Documenti)
            {
                newDocumento = NewDocumento(documento);
                listDocumenti.Add(newDocumento);
            }

            postaContest.Documenti = listDocumenti.ToArray();
        }

        private Documento NewDocumento(Model.Documento documento)
        {
            return new Documento { MD5 = GetMD5(documento), Contenuto = documento.Content, Estensione = documento.Extension };
        }

        private void SetDestinatari(PostaContest postaContest)
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

            postaContest.Destinatari = listDestinatari.ToArray();

        }

        private Destinatario NewDestinatario(Anagrafica destinatarioServizio)
        {
            var nominativo = destinatarioServizio.RagioneSociale ?? string.Concat(destinatarioServizio.Cognome, destinatarioServizio.Nome);
            return new Destinatario
            {
                Nominativo = nominativo,
                Cap = destinatarioServizio.Cap,
                ComplementoIndirizzo = destinatarioServizio.ComplementoIndirizzo,
                ComplementoNominativo = destinatarioServizio.ComplementoNominativo,
                Indirizzo = string.Concat(destinatarioServizio.DUG, destinatarioServizio.Toponimo, destinatarioServizio.Esponente),
                Comune = destinatarioServizio.Citta,
                Frazione = destinatarioServizio.Frazione,
                Nazione = destinatarioServizio.Stato ?? "Italia",
                Provincia = destinatarioServizio.Provincia
            };
        }

        private void SetMittente(PostaContest postaContest)
        {
            var mittenteServizio = _servizio.Anagrafiche.Single(d => d.IsMittente == true);
            var nominativo = mittenteServizio.RagioneSociale ?? string.Concat(mittenteServizio.Cognome, mittenteServizio.Nome);
            postaContest.Mittente = new Mittente
            {
                Nominativo = nominativo,
                Cap = mittenteServizio.Cap,
                ComplementoIndirizzo = mittenteServizio.ComplementoIndirizzo,
                ComplementoNominativo = mittenteServizio.ComplementoNominativo,
                Indirizzo = string.Concat(mittenteServizio.DUG, mittenteServizio.Toponimo, mittenteServizio.Esponente),
                Comune = mittenteServizio.Citta,
                Frazione = mittenteServizio.Frazione,
                Nazione = mittenteServizio.Stato ?? "Italia",
                Provincia = mittenteServizio.Provincia
            };
        }
    }
}
