using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.Proxy;
using ComunicazioniElettroniche.Common.Serialization;
using ComunicazioniElettroniche.PostaEvo.Assembly.External.Serialization;
using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;

namespace NPCE_WinClient.UI.Npce
{
    public class PostaEvoPil : ServizioPil
    {

        public PostaEvoPil(Model.Servizio postaEvo, Ambiente ambiente) : base(ambiente, postaEvo, System.Guid.NewGuid().ToString())
        {

        }
        public override NpceOperationResult Invio()
        {
            CE ce = GetCE();


            PostaEvoSubmit postaEvoBE = SetPostaEvoBE();
            PostaEvoResponse postaEvoResult = null;

            ce.Body = SerializationUtility.SerializeToXmlElement(postaEvoBE);

            using (WsCEClient client = new WsCEClient())
            {
                client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_ambiente.LolUri);
                client.SubmitRequest(ref ce);
                try
                {
                    postaEvoResult = SerializationUtility.Deserialize<PostaEvoResponse>(ce.Body);
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }

            return CreateResult(NpceOperation.Invio, ce.Result.ResType == TResultResType.I ? "0" : "99", ce.Result.Description?.Substring(0, Math.Min(ce.Result.Description.Length, 500)) ?? "Invio Ok", postaEvoResult.IdRichiesta, null, null);
        }

        public override NpceOperationResult Conferma()
        {
            CE ce = GetCE();

            ConfirmService confirm = new ConfirmService
            {
                IdRichiesta = _servizio.IdRichiesta
                 
                 
            };

            ConfirmServiceResponse confirmResult = null;

            ce.Body = SerializationUtility.SerializeToXmlElement(confirm);

            using (WsCEClient client = new WsCEClient())
            {
                client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_ambiente.LolUri);
                client.SubmitRequest(ref ce);
                try
                {
                    confirmResult = SerializationUtility.Deserialize<ConfirmServiceResponse>(ce.Body);
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }

            return CreateResult(NpceOperation.Invio, ce.Result.ResType == TResultResType.I ? "0" : "99", ce.Result.Description?.Substring(0, Math.Min(ce.Result.Description.Length, 500)) ?? "Invio Ok", string.Empty, null, null);
        }

        private PostaEvoSubmit SetPostaEvoBE()
        {
            PostaEvoSubmit result = new PostaEvoSubmit();

            result.IdRichiesta = Guid.NewGuid().ToString();
            result.AutoConferma = _servizio.Autoconferma;

            result.TipoProdotto = _servizio.TipoServizio.Descrizione.ToUpper();
            result.CodiceContratto = (_servizio.TipoServizio.Descrizione == "MOL1" || _servizio.TipoServizio.Descrizione == "MOL4") ? _ambiente.ContrattoMOL : _ambiente.ContrattoMOL;

            SetPostaEvoMittente(result);

            SetPostaEvoDestinatari(result);

            SetPostaEvoDocumenti(result);

            if (_servizio.AttestazioneConsegna)
            {
                SetPostaEvoAR(result);
            }

            SetPostaEvoOpzioni(result);

            return result;
        }

        private void SetPostaEvoDestinatari(PostaEvoSubmit result)
        {

            var destinatariServizioList = _servizio.Anagrafiche.Where(d => d.IsMittente == true).ToList();

            var listDestinatari = new List<SoggettoType>();

            foreach (var destinatarioServizio in destinatariServizioList)
            {
                SoggettoType newDestinatario = NewSoggettoType(destinatarioServizio);


                newDestinatario.Indirizzo = new IndirizzoType
                {
                    Indirizzo = string.Concat(destinatarioServizio.DUG, " ", destinatarioServizio.Toponimo, " ", destinatarioServizio.NumeroCivico, " ", destinatarioServizio.Esponente)
                };

                newDestinatario.Destinazione = new DestinazioneType
                {
                    CAP = destinatarioServizio.Cap,
                    Comune = destinatarioServizio.Citta,
                    Nazione = destinatarioServizio.Stato ?? "ITALIA",
                    Provincia = destinatarioServizio.Provincia,
                    Frazione = destinatarioServizio.Frazione
                };
                listDestinatari.Add(newDestinatario);
            }

            result.Destinatari = listDestinatari.ToArray();
        }

        private void SetPostaEvoAR(PostaEvoSubmit result)
        {
            throw new NotImplementedException();
        }

        private void SetPostaEvoOpzioni(PostaEvoSubmit result)
        {
            result.Opzioni = new PostaEvoSubmitOpzioni
            {
                OpzioniStampa = new PostaEvoSubmitOpzioniOpzioniStampa
                {
                    FronteRetro = false,
                    TipoColore = PostaEvoSubmitOpzioniOpzioniStampaTipoColore.BW
                }
                ,
                OpzioniServizio = new PostaEvoSubmitOpzioniOpzioniServizio
                {
                    ModalitaConsegna = PostaEvoSubmitOpzioniOpzioniServizioModalitaConsegna.S,
                    ModalitaPricing = "NAZ",
                    ArchiviazioneDocumenti = _servizio.TipoArchiviazione,
                    AnniArchiviazione = _servizio.AnniArchiviazione,
                    AnniArchiviazioneSpecified = _servizio.AnniArchiviazione > 0
                }
            };

        }

        private void SetPostaEvoDocumenti(PostaEvoSubmit result)
        {
            var documenti = new List<DocumentoType>();

            documenti.Add(new DocumentoType
            {
                Estensione = ".doc",
                URI = @"\\FSSVIL-b451.rete.testposte\ShareFS\InputDocument\ROL_db56a17c-12b2-402a-ad51-9e309f895e79.doc",
                HashMD5 = "AB8EF323B64C85C8DFCCCD4356E4FB9B"
            });

            result.Documenti = documenti.ToArray();

        }

        private void SetPostaEvoMittente(PostaEvoSubmit result)
        {
            var mittenteServizio = _servizio.Anagrafiche.Single(d => d.IsMittente == true);

            SoggettoType mittente = NewSoggettoType(mittenteServizio);

            mittente.Destinazione = new DestinazioneType();
            mittente.Destinazione.CAP = mittenteServizio.Cap;
            mittente.Destinazione.Comune = mittenteServizio.Citta;
            mittente.Destinazione.Provincia = mittenteServizio.Provincia;
            mittente.Destinazione.Nazione = mittenteServizio.Stato ?? "ITALIA";

            mittente.Indirizzo = NewIndirizzoType(mittenteServizio);

            result.Mittente = mittente;

        }

        private IndirizzoType NewIndirizzoType(Anagrafica mittenteServizio)
        {
            return new IndirizzoType
            {
                Indirizzo = string.Concat(mittenteServizio.DUG, mittenteServizio.Toponimo, " ", mittenteServizio.NumeroCivico, mittenteServizio.Esponente),
                ComplementoIndirizzo = mittenteServizio.ComplementoNominativo

            };

        }

        private SoggettoType NewSoggettoType(Anagrafica mittenteServizio)
        {
            var mittente = new SoggettoType();
            mittente.Nominativo = new NominativoType
            {
                Nominativo = string.Concat(mittenteServizio.Cognome, " ", mittenteServizio.Nome)

            };
            return mittente;
        }
    }
}
