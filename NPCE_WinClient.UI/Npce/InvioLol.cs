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
        LOLServiceSoap _proxy;
        public InvioLol(Ambiente ambiente, Servizio servizio, string idRichiesta) : base(ambiente, servizio, idRichiesta)
        {
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
            if (_servizio.TipoServizio.Descrizione == "Posta1")
            {
                SetPosta1(lolSubmit);
            }

            if (_servizio.PagineBollettini != null && _servizio.PagineBollettini.Count() > 0)
            {
                SetBollettini(lolSubmit);
            }


            var fake = new OperationContextScope((IContextChannel)_proxy);
            var headers = GetHttpHeaders(_ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;
            //OperationContext.Current.Channel.OperationTimeout = TimeSpan.FromMinutes(2);

            var invioResult = _proxy.Invio(_idRichiesta, string.Empty, lolSubmit);

            return CreateResult(NpceOperation.Invio, invioResult.CEResult.Code, invioResult.CEResult.Code, invioResult.IDRichiesta, null, invioResult.GuidUtente);
        }

        private void SetBollettini(LOLSubmit lolSubmit)
        {
            List<reference.PaginaBollettino> pagineBollettiniList = new List<reference.PaginaBollettino>();

            List<reference.PaginaDueBollettini> pagineDueBollettiniList = new List<reference.PaginaDueBollettini>();

            foreach (var paginaBollettino in _servizio.PagineBollettini)
            {
                CreatePaginaBollettino(paginaBollettino, pagineBollettiniList, pagineDueBollettiniList);

            }

            List<reference.PaginaBollettinoBase> finalBollettiniList = new List<reference.PaginaBollettinoBase>();

            finalBollettiniList.AddRange(pagineBollettiniList);
            finalBollettiniList.AddRange(pagineDueBollettiniList);

            lolSubmit.PagineBollettini = finalBollettiniList.ToArray();
        }

        private void CreatePaginaBollettino(Model.PaginaBollettino paginaBollettino, 
                                                                     List<reference.PaginaBollettino> pagineBollettiniList,
                                                                     List<reference.PaginaDueBollettini>  pagineDueBollettiniList)
        {
            if (paginaBollettino.Bollettini.Count==1)
            {
                pagineBollettiniList.Add(CreaPaginaUnBollettino(paginaBollettino));
            }

            else
            {
                pagineDueBollettiniList.Add(CreaPaginaDueBollettini(paginaBollettino));
            }
            
        }

        private reference.PaginaDueBollettini CreaPaginaDueBollettini(Model.PaginaBollettino paginaBollettino)
        {
            var result = new reference.PaginaDueBollettini();

            switch (paginaBollettino.Bollettini.First().Tipo)
            {
                case "896":
                    reference.BollettinoBase bollettino896 = CreaBollettino896(paginaBollettino.Bollettini.First());
                    result.Bollettino = bollettino896;
                    break;
                case "674":
                    reference.BollettinoBase bollettino674 = CreaBollettino674(paginaBollettino.Bollettini.First());
                    result.Bollettino = bollettino674;
                    break;
                case "451":
                    reference.BollettinoBase bollettino451 = CreaBollettino451(paginaBollettino.Bollettini.First());
                    result.Bollettino = bollettino451;
                    break;
                default:
                    break;
            }

            AddBollettino2(paginaBollettino.Bollettini.Skip(1).First(), result);

            return result;
        }

        private void AddBollettino2(Bollettino bollettino, PaginaDueBollettini result)
        {
            switch (bollettino.Tipo)
            {
                case "896":
                    reference.BollettinoBase bollettino896 = CreaBollettino896(bollettino);
                    result.Bollettino2 = bollettino896;
                    break;
                case "674":
                    reference.BollettinoBase bollettino674 = CreaBollettino674(bollettino);
                    result.Bollettino2 = bollettino674;
                    break;
                case "451":
                    reference.BollettinoBase bollettino451 = CreaBollettino451(bollettino);
                    result.Bollettino2 = bollettino451;
                    break;
                default:
                    break;
            }
        }

        private BollettinoBase CreateBollettino2(Model.PaginaBollettino paginaBollettino)
        {
            throw new NotImplementedException();
        }

        private reference.PaginaBollettino CreaPaginaUnBollettino(Model.PaginaBollettino paginaBollettino)
        {
            var result = new reference.PaginaBollettino();

            switch (paginaBollettino.Bollettini.First().Tipo)
            {
                case "896":
                    reference.BollettinoBase bollettino896 = CreaBollettino896(paginaBollettino.Bollettini.First());
                    result.Bollettino = bollettino896;
                    break;
                case "674":
                    reference.BollettinoBase bollettino674 = CreaBollettino674(paginaBollettino.Bollettini.First());
                    result.Bollettino = bollettino674;
                    break;
                case "451":
                    reference.BollettinoBase bollettino451 = CreaBollettino451(paginaBollettino.Bollettini.First());
                    result.Bollettino = bollettino451;
                    break;
                default:
                    break;
            }

            return result ;
        }

        private BollettinoBase CreaBollettino451(Bollettino bollettino)
        {
            return new Bollettino451
            {
                AdditionalInfo = bollettino.AdditionalInfo,
                Causale = "Causale",
                EseguitoDa = new BollettinoEseguitoDa { Nominativo = bollettino.EseguitoDaNominativo, Indirizzo = bollettino.EseguitoDaIndirizzo, CAP = bollettino.EseguitoDaCap, Localita = bollettino.EseguitoDaLocalita },
                ImportoEuro = bollettino.ImportoEuro,
                IBAN = bollettino.IBan,
                NumeroAutorizzazioneStampaInProprio = bollettino.NumeroAutorizzazioneStampaInProprio,
                IntestatoA = bollettino.IntestatoA,
                NumeroContoCorrente = bollettino.NumeroContoCorrente
            };
        }

        private BollettinoBase CreaBollettino674(Bollettino bollettino)
        {
            return new Bollettino674
            {
                AdditionalInfo = bollettino.AdditionalInfo,
                Causale = "Causale",
                EseguitoDa = new BollettinoEseguitoDa { Nominativo = bollettino.EseguitoDaNominativo, Indirizzo = bollettino.EseguitoDaIndirizzo, CAP = bollettino.EseguitoDaCap, Localita = bollettino.EseguitoDaLocalita },
                IBAN = bollettino.IBan,
                NumeroAutorizzazioneStampaInProprio = bollettino.NumeroAutorizzazioneStampaInProprio,
                IntestatoA = bollettino.IntestatoA,
                NumeroContoCorrente = bollettino.NumeroContoCorrente
            };
        }

        private Bollettino896 CreaBollettino896(Bollettino bollettino)
        {
           return new Bollettino896
            {
                AdditionalInfo = bollettino.AdditionalInfo,
                Causale = "Causale",
                CodiceCliente = bollettino.CodiceCliente,
                EseguitoDa = new BollettinoEseguitoDa { Nominativo = bollettino.EseguitoDaNominativo, Indirizzo = bollettino.EseguitoDaIndirizzo, CAP = bollettino.EseguitoDaCap, Localita = bollettino.EseguitoDaLocalita },
                ImportoEuro = bollettino.ImportoEuro,
                IBAN = bollettino.IBan,
                NumeroAutorizzazioneStampaInProprio = bollettino.NumeroAutorizzazioneStampaInProprio,
                IntestatoA = bollettino.IntestatoA,
                NumeroContoCorrente = bollettino.NumeroContoCorrente
            };
        }

        private void SetPosta1(LOLSubmit lolSubmit)
        {
            lolSubmit.DescrizioneLettera = new DescrizioneLettera { TipoLettera = "Posta1" };
        }

        private void SetOpzioni(LOLSubmit lolSubmit)
        {
            var opzioni = new LOLSubmitOpzioni();

            opzioni.OpzionidiStampa = new OpzionidiStampa
            {
                PageSize = OpzionidiStampaPageSize.A4,
                FronteRetro = GetFronteRetroDescription(_servizio.FronteRetro),
                BW = _servizio.Colore ? "false" : "true"
            };

            lolSubmit.Opzioni = opzioni;
        }

        private string GetFronteRetroDescription(bool fronteRetro)
        {
            if (fronteRetro) return "true"; else return "false";
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
