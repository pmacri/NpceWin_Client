using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.Frontend.DataContracts;
using ComunicazioniElettroniche.Common.Proxy;
using ComunicazioniElettroniche.Common.Serialization;
using ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitResponse;
using ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitROL;
using ComunicazioniElettroniche.Visure.ReportServerReference;
using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Destinatario = ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitROL.Destinatario;
using Indirizzo = ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitROL.Indirizzo;
using LetteraDestinatario = ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitROL.LetteraDestinatario;
using Nominativo = ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitROL.Nominativo;

namespace NPCE_WinClient.UI.Npce
{
    public class RaccomandataPil : ServizioPil
    {
        public RaccomandataPil(Model.Servizio Raccomandata, Ambiente ambiente) : base(ambiente, Raccomandata, System.Guid.NewGuid().ToString())
        {

        }

        public override NpceOperationResult Conferma()
        {
            throw new NotImplementedException();
        }

        public override NpceOperationResult Invio()
        {
            CE ce = GetCE();

            RaccomandataSubmit RaccomandataBE = SetRaccomandataBE();
            RaccomandataBE.IdRichiesta = _idRichiesta;
            RaccomandataResponse RaccomandataResult = null;

            ce.Body = SerializationUtility.SerializeToXmlElement(RaccomandataBE);

            using (WsCEClient client = new WsCEClient())
            {
                client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_ambiente.RolUri);
                client.SubmitRequest(ref ce);
                try
                {
                    RaccomandataResult = SerializationUtility.Deserialize<RaccomandataResponse>(ce.Body);

                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }

            return CreateResult(NpceOperation.Invio, ce.Result.ResType == TResultResType.I ? "0" : "99", ce.Result.Description?.Substring(0, Math.Min(ce.Result.Description.Length, 500)) ?? "Invio Ok", RaccomandataResult.IdRichiesta, null, null);
        }


        private RaccomandataSubmit SetRaccomandataBE()
        {
            RaccomandataSubmit RaccomandataBE = new RaccomandataSubmit();

            RaccomandataBE.NumeroDestinatari = _servizio.Anagrafiche.Where(d => d.IsMittente == false).Count();
            SetMittente(RaccomandataBE);
            SetDestinatari(RaccomandataBE);
            SetDocumenti(RaccomandataBE);
            SetOpzioni(RaccomandataBE);
            if (_servizio.AvvisoRicevimento)
            {
                SetAR(RaccomandataBE);
            }

            return RaccomandataBE;
        }

        private void SetAR(RaccomandataSubmit raccomandataBE)
        {
            var ricevuta = new RaccomandataSubmitRicevuta();
            raccomandataBE.Ricevuta = ricevuta;

            var arServizio = _servizio.Anagrafiche.Single(d => d.IsDestinatarioAR == true);

            ricevuta.Nominativo = new RaccomandataSubmitRicevutaNominativo()
            {
                Nome = arServizio.Nome,
                Cognome = arServizio.Cognome,
                Indirizzo = new RaccomandataSubmitRicevutaNominativoIndirizzo
                {
                    DUG = arServizio.DUG,
                    Toponimo = arServizio.Toponimo,
                    Esponente = arServizio.Esponente,
                    NumeroCivico = arServizio.NumeroCivico
                },
                CAP = arServizio.Cap,
                CasellaPostale = arServizio.CasellaPostale,
                Citta = arServizio.Citta,
                ComplementoIndirizzo = arServizio.ComplementoIndirizzo,
                ComplementoNominativo = arServizio.ComplementoNominativo,
                Provincia = arServizio.Provincia,
                Stato = arServizio.Stato,
                RagioneSociale = arServizio.RagioneSociale
            };


            if (arServizio.Telefono != null)
                ricevuta.Nominativo.Telefono = arServizio.Telefono;

            ricevuta.Nominativo.UfficioPostale = ricevuta.Nominativo.TipoIndirizzo = arServizio.CasellaPostale;
        }

        private void SetDocumenti(RaccomandataSubmit rolSubmit)
        {

            RaccomandataSubmitDocumento newDocumento;
            var listDocumenti = new List<RaccomandataSubmitDocumento>();

            //foreach (var documento in _servizio.Documenti)
            //{
            newDocumento = NewDocumento();
            listDocumenti.Add(newDocumento);
            //}

            rolSubmit.Documenti = listDocumenti;
        }

        private RaccomandataSubmitDocumento NewDocumento()
        {
            return new RaccomandataSubmitDocumento
            {
                FileHash = "AB8EF323B64C85C8DFCCCD4356E4FB9B",
                Uri = @"\\FSSVIL-b451.rete.testposte\ShareFS\InputDocument\ROL_db56a17c-12b2-402a-ad51-9e309f895e79.doc",
                IdPosizione = 1
            };
        }

        private void SetMittente(RaccomandataSubmit RaccomandataBE)
        {
            var mittenteServizio = _servizio.Anagrafiche.Single(d => d.IsMittente == true);
            var mittente = new ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitROL.Mittente();

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

            RaccomandataBE.Mittente = mittente;
        }

        private void SetDestinatari(RaccomandataSubmit rolSubmit)
        {

            int count = 0;

            var destinatariServizioList = _servizio.Anagrafiche.Where(d => d.IsMittente == false).ToList();

            var listDestinatari = new List<ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitROL.LetteraDestinatario>();

            foreach (var destinatarioServizio in destinatariServizioList)
            {
                count++;
                ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitROL.LetteraDestinatario newDestinatario = NewDestinatario(destinatarioServizio, rolSubmit);
                listDestinatari.Add(newDestinatario);
            }

            rolSubmit.RaccomandataDestinatario = listDestinatari.ToArray();

            rolSubmit.NumeroDestinatari = count;

        }

        private void SetOpzioni(RaccomandataSubmit rolSubmit)
        {
            var opzioni = new ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitROL.Opzioni();

            opzioni.DataStampa = DateTime.Now;

            ModalitaArchiviazione tipoArchiviazione;

            Enum.TryParse<ModalitaArchiviazione>(_servizio.TipoArchiviazione, out tipoArchiviazione);

            opzioni.ArchiviazioneDocumenti = tipoArchiviazione;

            if (_servizio.AnniArchiviazione > 0)
            {
                opzioni.AnniArchiviazione = _servizio.AnniArchiviazione;
                opzioni.AnniArchiviazioneSpecified = true;

                

            }

            rolSubmit.Opzioni = opzioni;
        }

        private LetteraDestinatario NewDestinatario(Anagrafica destinatarioServizio, RaccomandataSubmit rolSubmit)
        {
            var destinatario = new LetteraDestinatario();

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


            int countDestinatari = (rolSubmit.RaccomandataDestinatario == null) ? 0 : rolSubmit.RaccomandataDestinatario.Count();
            destinatario.NumeroDestinatarioCorrente = countDestinatari + 1;
            destinatario.IdRaccomandata = string.Empty;

            destinatario.Destinatario = new Destinatario();

            destinatario.Destinatario.Nominativo = nominativo;

            return destinatario;
        }




    }
}