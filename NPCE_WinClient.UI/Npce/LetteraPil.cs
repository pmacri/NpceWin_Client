using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.Frontend.DataContracts;
using ComunicazioniElettroniche.Common.Proxy;
using ComunicazioniElettroniche.Common.Serialization;
using ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL;
using ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitResponse;
using ComunicazioniElettroniche.LOL.Web.DataContracts;
using ComunicazioniElettroniche.LOL.Web.ServiceContracts;
using NPCE_WinClient.Model;
using PosteItaliane.OrderManagement.Schema.SchemaDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Destinatario = ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Destinatario;
using Indirizzo = ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Indirizzo;
using LetteraDestinatario = ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.LetteraDestinatario;
using Nominativo = ComunicazioniElettroniche.LOL.Web.BusinessEntities.InvioSubmitLOL.Nominativo;

namespace NPCE_WinClient.UI.Npce
{
    public class LetteraPil : NpceOperationBase
    {
        public LetteraPil(Model.Servizio lettera, Ambiente ambiente) : base(ambiente, lettera, null)
        {
        }

        public NpceOperationResult Invio()
        {
            var ce = new CE();
            ce.Header = new CEHeader();
            ce.Header.GUIDMessage = Guid.NewGuid().ToString();
            ce.Header = new CEHeader
            {
                BillingCenter = _ambiente.billingcenter,
                CodiceFiscale = _ambiente.codicefiscale,
                ContractId = _ambiente.contractid,
                ContractType = _ambiente.contracttype,
                CostCenter = _ambiente.costcenter,
                Customer = _ambiente.customer,
                IdCRM = string.Empty,
                SenderSystem = _ambiente.sendersystem,
                UserId = _ambiente.smuser,
                PartitaIva = _ambiente.partitaiva,
                IDSender = string.Empty,
                UserType = _ambiente.usertype
            };

            var idRichiesta = Guid.NewGuid().ToString();
            ce.Header.GUIDMessage = idRichiesta;

            LetteraSubmit letteraBE = SetLetteraBE();

            letteraBE.IdRichiesta = idRichiesta;
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

        public NpceOperationResult PreConferma(string idRichiesta, bool autoconfirm)
        {
            OrderRequest orderRequest = new PosteItaliane.OrderManagement.Schema.SchemaDefinition.OrderRequest();
            OrderResponse orderResponse = null;
            var ce = new CE();
            ce.Header = new CEHeader();
            ce.Header.GUIDMessage = Guid.NewGuid().ToString();
            ce.Header = new CEHeader
            {
                BillingCenter = _ambiente.billingcenter,
                CodiceFiscale = _ambiente.codicefiscale,
                ContractId = _ambiente.contractid,
                ContractType = _ambiente.contracttype,
                CostCenter = _ambiente.costcenter,
                Customer = _ambiente.customer,
                IdCRM = string.Empty,
                SenderSystem = _ambiente.sendersystem,
                UserId = _ambiente.smuser,
                PartitaIva = _ambiente.partitaiva,
                IDSender = string.Empty,
                UserType = _ambiente.usertype
            };

            ce.Header.GUIDMessage = idRichiesta;

            orderRequest.ServiceInstance = new OrderRequestServiceInstance[1];
            orderRequest.ForceOrderCreation = true;

            orderRequest.ServiceInstance[0] = new OrderRequestServiceInstance();
            orderRequest.ServiceInstance[0].GUIDMessage = idRichiesta;


            ce.Body = SerializationUtility.SerializeToXmlElement(orderRequest);

            using (WsCEClient client = new WsCEClient())
            {
                client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_ambiente.LolUri);
                client.SubmitRequest(ref ce);
                try
                {
                    orderResponse = SerializationUtility.Deserialize<OrderResponse>(ce.Body);

                }
                catch (Exception ex)
                {
                    throw (ex);
                }

                var idOrdine = orderResponse.IdOrder;


                if (autoconfirm)
                {
                    // solo postFatturazione
                    ComunicazioniElettroniche.Common.Frontend.DataContracts.OpzionePagamento op = new OpzionePagamento();
                    op.DescrizioneTipoPagamento = string.Empty; // puo' essere solo postfatturazione
                    op.IdTipoPagamento = string.Empty;

                    for (int i = 0; i < orderResponse.PaymentTypes.Length; i++)
                    {
                        if (orderResponse.PaymentTypes[i].PostPayment)
                        {
                            op.DescrizioneTipoPagamento = orderResponse.PaymentTypes[i].TypeDescription;
                            op.IdTipoPagamento = orderResponse.PaymentTypes[i].TypeId;
                            op.PostFatturazione = true;
                            break;
                        }
                    }
                    // Prepare confirm request
                    ConfirmOrder confirm = new ConfirmOrder { IdOrder = idOrdine, PaymentType = new PaymentType { TypeId = op.IdTipoPagamento, PostPayment = true } };
                    ce.Body = SerializationUtility.SerializeToXmlElement(confirm);
                    client.SubmitRequest(ref ce);
                }

            }

            return CreateResult(NpceOperation.PreConfermaWithAutoconfirm, ce.Result.ResType == TResultResType.I ? "0" : "99", ce.Result.Description?.Substring(0, Math.Min(ce.Result.Description.Length, 500)) ?? "Invio Ok", orderResponse.IdOrder, null, null);
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
            SetMittente(letteraBE);
            SetDestinatari(letteraBE);
            SetDocumenti(letteraBE);
            SetPosta1(letteraBE);

            return letteraBE;
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

            destinatario.IdLettera = lolSubmit.IdRichiesta;

            int countDestinatari = (lolSubmit.LetteraDestinatario == null) ? 0 : lolSubmit.LetteraDestinatario.Count();
            destinatario.NumeroDestinatarioCorrente = countDestinatari + 1;

            destinatario.Destinatario = new Destinatario();

            destinatario.Destinatario.Nominativo = nominativo;

            return destinatario;
        }

    }
}