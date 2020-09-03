using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.Frontend.DataContracts;
using ComunicazioniElettroniche.Common.Proxy;
using ComunicazioniElettroniche.Common.Serialization;
using ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL;
using ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitResponse;
using ComunicazioniElettroniche.ROL.Web.BusinessEntities.InvioSubmitROL;
using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Destinatario = ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Destinatario;
using Indirizzo = ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Indirizzo;
using LetteraDestinatario = ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.LetteraDestinatario;
using Nominativo = ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Nominativo;

namespace NPCE_WinClient.UI.Npce
{
    public class LetteraPil : ServizioPil
    {
        public LetteraPil(Model.Servizio lettera, Ambiente ambiente) : base(ambiente, lettera, System.Guid.NewGuid().ToString())
        {
           
        }

        public override NpceOperationResult Conferma()
        {
            throw new NotImplementedException();
        }
        public override NpceOperationResult Invio()
        {
            CE ce = GetCE();

            LetteraSubmit letteraBE = SetLetteraBE();
            letteraBE.IdRichiesta = _idRichiesta;
            LetteraResponse letteraResult = null;

            ce.Body = SerializationUtility.SerializeToXmlElement(letteraBE);

            using (WsCEClient client = new WsCEClient())
            {
                client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_ambiente.LolUri);
                client.SubmitRequest(ref ce);
                try
                {
                    letteraResult = SerializationUtility.Deserialize<LetteraResponse>(ce.Body);
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }

            return CreateResult(NpceOperation.Invio, ce.Result.ResType == TResultResType.I ? "0" : "99", ce.Result.Description?.Substring(0, Math.Min(ce.Result.Description.Length, 500)) ?? "Invio Ok", letteraResult.IdRichiesta, null, null);
        }

        private void SetPosta1(LetteraSubmit lolSubmit)
        {
            if (_servizio.TipoServizio.Descrizione == "Posta1")
            {
                lolSubmit.Tipo = "LOL_PRO";
            }
            else
            {
                lolSubmit.Tipo = "LOL";
            }
        }

        private LetteraSubmit SetLetteraBE()
        {
            LetteraSubmit letteraBE = new LetteraSubmit();

            letteraBE.NumeroDestinatari = _servizio.Anagrafiche.Where(d => d.IsMittente == false).Count();
            SetOpzioni(letteraBE);
            SetMittente(letteraBE);
            SetDestinatari(letteraBE);
            SetDocumenti(letteraBE);
            SetPosta1(letteraBE);

            return letteraBE;
        }

        private void SetOpzioni(LetteraSubmit letteraBE)
        {
            var opzioni = new ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Opzioni();

            opzioni.DataStampa = DateTime.Now;
            
            opzioni.ArchiviazioneDocumenti = _servizio.TipoArchiviazione;

            if (_servizio.AnniArchiviazione > 0)
            {
                opzioni.AnniArchiviazione = _servizio.AnniArchiviazione;
                opzioni.AnniArchiviazioneSpecified = true;
            }

            letteraBE.Opzioni = opzioni;
        }



        private void SetDocumenti(LetteraSubmit lolSubmit)
        {

            LetteraSubmitDocumento newDocumento;
            var listDocumenti = new List<LetteraSubmitDocumento>();

            //foreach (var documento in _servizio.Documenti)
            //{
            newDocumento = NewDocumento();
            listDocumenti.Add(newDocumento);
            //}

            lolSubmit.Documenti = listDocumenti;
        }

        private LetteraSubmitDocumento NewDocumento()
        {
            return new LetteraSubmitDocumento
            {
                FileHash = "AB8EF323B64C85C8DFCCCD4356E4FB9B",
                Uri = @"\\FSSVIL-b451.rete.testposte\ShareFS\InputDocument\ROL_db56a17c-12b2-402a-ad51-9e309f895e79.doc",
                IdPosizione = 1
            };
        }

        private void SetMittente(LetteraSubmit letteraBE)
        {
            var mittenteServizio = _servizio.Anagrafiche.Single(d => d.IsMittente == true);
            var mittente = new ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Mittente();

            var nominativo = new ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Nominativo
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

            letteraBE.Mittente = mittente;
        }

        private void SetDestinatari(LetteraSubmit lolSubmit)
        {

            int count = 0;

            var destinatariServizioList = _servizio.Anagrafiche.Where(d => d.IsMittente == false).ToList();

            var listDestinatari = new List<LetteraDestinatario>();

            foreach (var destinatarioServizio in destinatariServizioList)
            {
                count++;
                LetteraDestinatario newDestinatario = NewDestinatario(destinatarioServizio, lolSubmit);
                listDestinatari.Add(newDestinatario);
            }

            lolSubmit.LetteraDestinatario = listDestinatari.ToArray();

            lolSubmit.NumeroDestinatari = count;

        }

        private LetteraDestinatario NewDestinatario(Anagrafica destinatarioServizio, LetteraSubmit lolSubmit)
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


            // TODO
            destinatario.IdLettera = lolSubmit.IdRichiesta;

            int countDestinatari = (lolSubmit.LetteraDestinatario == null) ? 0 : lolSubmit.LetteraDestinatario.Count();
            destinatario.NumeroDestinatarioCorrente = countDestinatari + 1;

            destinatario.Destinatario = new Destinatario();

            destinatario.Destinatario.Nominativo = nominativo;

            return destinatario;
        }

    }
}