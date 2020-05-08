using NPCE_WinClient.Model;
using NPCE_WinClient.Services.Mol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Documento = NPCE_WinClient.Services.Mol.Documento;

namespace NPCE_WinClient.UI.Npce
{
    public class InvioMol : NpceOperationBase
    {

        IRaccomandataMarketService _proxy;
        public InvioMol(Ambiente ambiente, Servizio servizio, string idRichiesta) : base(ambiente, servizio, idRichiesta)
        {

        }


        public NpceOperationResult Execute()
        {
            var helper = new Helper();
            _proxy = helper.GetProxy<IRaccomandataMarketService>(_ambiente.MolUri, _ambiente.Username, _ambiente.Password);
            InvioRequest molSubmit = new InvioRequest();

            molSubmit.Intestazione = new Intestazione { CodiceContratto = "00000000040000018417", Prodotto = ProdottoPostaEvo.MOL1 };

            var marketOnLine = new MarketOnline();

            SetIntestazione(marketOnLine);
            SetMittente(marketOnLine);
            SetDestinatari(marketOnLine);
            SetDocumenti(marketOnLine);
            SetOpzioni(marketOnLine);
            if (_servizio.Anagrafiche.Any(d => d.IsDestinatarioAR))
            {
                SetDestinatariAr(marketOnLine);
            }

            var fake = new OperationContextScope((IContextChannel)_proxy);
            var headers = GetHttpHeaders(_ambiente);

            headers.Headers["smuser"] = "H2HSTG18";
            headers.Headers["customerid"] = "3909990439";

            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            molSubmit.MarketOnline = marketOnLine;

            var invioResult = _proxy.Invio(molSubmit);

            return CreateResult(NpceOperation.Invio, invioResult.Esito.ToString() == "OK" ? "0" : "99", invioResult.Esito.ToString(), invioResult.IdRichiesta, null, null);
        }

        private void SetIntestazione(MarketOnline marketOnLine)
        {
            
        }

        private void SetDestinatariAr(MarketOnline marketOnLine)
        {
            foreach (var destAr in _servizio.Anagrafiche.Where(d => d.IsDestinatarioAR))
            {
                marketOnLine.DestinatarioAR = NewDestinatarioAR(destAr);
            }

            
        }

        private void SetOpzioni(MarketOnline marketOnLine)
        {
            marketOnLine.Opzioni = new Services.Mol.Opzioni();

            marketOnLine.Opzioni.Servizio = new OpzioniServizio
            {
                ArchiviazioneDocumenti = false,
                AttestazioneConsegna = _servizio.AvvisoRicevimento,
                SecondoTentativoRecapito = false

            };

            marketOnLine.Opzioni.Stampa = new OpzioniStampa
            {
                FronteRetro = _servizio.FronteRetro,
                TipoColore = _servizio.Colore ? TipoColore.COLORE : TipoColore.BW
            };

            // Dati Opzionali

        }

        private void SetDocumenti(MarketOnline marketOnLine)
        {
            Documento newDocumento;
            var listDocumenti = new List<Documento>();

            foreach (var documento in _servizio.Documenti)
            {
                newDocumento = NewDocumento(documento);
                listDocumenti.Add(newDocumento);
            }

            marketOnLine.Documenti = listDocumenti.ToArray();
        }

        private Documento NewDocumento(Model.Documento documento)
        {
            return new Documento {  MD5 = GetMD5(documento),  Contenuto = documento.Content,  Estensione = documento.Extension };
        }

        private void SetDestinatari(MarketOnline marketOnLine)
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

            marketOnLine.Destinatari = listDestinatari.ToArray();

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

        private void SetMittente(MarketOnline marketOnLine)
        {
            var mittenteServizio = _servizio.Anagrafiche.Single(d => d.IsMittente == true);
            var nominativo = mittenteServizio.RagioneSociale ?? string.Concat(mittenteServizio.Cognome, mittenteServizio.Nome);
            marketOnLine.Mittente = new Mittente
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

        private DestinatarioAR NewDestinatarioAR(Anagrafica destAr)
        {
            var nominativo = destAr.RagioneSociale ?? string.Concat(destAr.Cognome, destAr.Nome);
            return new DestinatarioAR
            {
                Nominativo = nominativo,
                Cap = destAr.Cap,
                ComplementoIndirizzo = destAr.ComplementoIndirizzo,
                ComplementoNominativo = destAr.ComplementoNominativo,
                Indirizzo = string.Concat(destAr.DUG, destAr.Toponimo, destAr.Esponente),
                Comune = destAr.Citta,
                Frazione = destAr.Frazione,
                Nazione = destAr.Stato,
                Provincia = destAr.Provincia
            };
        }
    }
}
