﻿using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.Proxy;
using ComunicazioniElettroniche.Common.Serialization;
using ComunicazioniElettroniche.Visure.BE;
using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CEHeader = ComunicazioniElettroniche.Common.DataContracts.CEHeader;

namespace NPCE_WinClient.UI.Npce
{
    public class InvioVisuraPIL : NpceOperationBase
    {
        private readonly Visura _visura;

        public void Execute()
        {

            var ce = new ComunicazioniElettroniche.Common.DataContracts.CE();
            ce.Header = GetHeaders(_ambiente);
            ce.Header.GUIDMessage = Guid.NewGuid().ToString();

            DocumentiRequest documentiRequest = GetDocumentiRequest(_visura);
            ce.Body = SerializationUtility.SerializeToXmlElement(documentiRequest);

            DocumentiResponse documentiResponse = null;
            using (WsCEClient client = new WsCEClient())
            {
                client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_ambiente.VolUri);
                client.SubmitRequest(ref ce);
                try
                {
                    documentiResponse = SerializationUtility.Deserialize<DocumentiResponse>(ce.Body);
                }
                catch (Exception)
                {

                }
            }
        }

        public InvioVisuraPIL(Visura visura, Ambiente ambiente) : base(ambiente, null, null)
        {
            _visura = visura;
        }

        private ComunicazioniElettroniche.Common.DataContracts.CEHeader GetHeaders(Ambiente ambiente)
        {
            return new CEHeader
            {
                BillingCenter = ambiente.billingcenter,
                CodiceFiscale = ambiente.codicefiscale,
                ContractId = ambiente.contractid,
                ContractType = ambiente.contracttype,
                CostCenter = ambiente.costcenter,
                Customer = ambiente.customer,
                IdCRM = string.Empty,
                SenderSystem = ambiente.sendersystem,
                UserId = ambiente.smuser,
                PartitaIva = ambiente.partitaiva,
                IDSender = string.Empty,
                UserType = ambiente.usertype
            };
        }

        private DocumentiRequest GetDocumentiRequest(Visura visura)
        {
            DocumentiRequest result = new DocumentiRequest();
            result.IdRichiesta = Guid.NewGuid().ToString();

            
            SetDocumentoIntestatario(result);
            SetRecapito(result);
            SetUserInfo(result);
            SetRichiedente(result);
            return result;
        }

        private void SetRichiedente(DocumentiRequest result)
        {
            result.Richiedente = new DocumentiRequestRichiedente
            {
                Nome = _visura.RichiedenteNome,
                Cognome = _visura.RichiedenteCognome,
                Indirizzo = _visura.RichiedenteIndirizzo,
                CAP = _visura.RichiedenteCap,
                Localita = _visura.RichiedenteLocalita,
                TelefonoPrefisso = _visura.RichiedentePrefissoTelefono,
                Telefono = _visura.RichiedenteTelefono
            };
        }

        private void SetUserInfo(DocumentiRequest result)
        {
            result.UserInfo = new UserInfo
            {
                Canale = _ambiente.sendersystem,
                Cliente = _ambiente.customerid,
                IdCdC = "",
                UserId = _ambiente.smuser
            };
        }

        private void SetDocumentoIntestatario(DocumentiRequest result)
        {
            List<DocumentiRequestDocumento> documentiList = new List<DocumentiRequestDocumento>();

            var documento = new DocumentiRequestDocumento();
            documento.Guid = Guid.NewGuid().ToString();
            documento.Tipo = _visura.VisureTipoDocumentoId == "V" ? "VISURACAMERALE" : "CERTIFICATOCAMERALE";
            documento.Codice = _visura.VisureCodiceDocumentoId;
            documento.Formato = _visura.VisureFormatoDocumentoId;
            documento.CameraDiCommercioCodice = _visura.DocumentoIntestatarioCCIAA;
            documento.NumeroCopie = 1;
            documento.Intestatario = new DocumentiRequestDocumentoIntestatario
            {
                CodiceFiscaleDittaIndividuale = _visura.DocumentoIntestatarioCodiceFiscale,
                Cognome = _visura.DocumentoIntestatarioCognome,
                Nome = _visura.DocumentoIntestatarioNome,
                RagioneSociale = _visura.DocumentoIntestatarioRagioneSociale,
                NumeroREA = _visura.DocumentoIntestatarioNREA
            };

            documentiList.Add(documento);

            result.Documento = documentiList.ToArray();

        }

        private void SetRecapito(DocumentiRequest result)
        {
            result.Recapito = new DocumentiRequestRecapito
            {
                CodiceRecapito = _visura.VisureTipoRecapito.Id,
                Email = _visura.DestinatarioEmail,
                Destinatario = new DocumentiRequestRecapitoDestinatario
                {
                    CAP = _visura.DestinatarioCap,
                    Localita = _visura.DestinatarioLocalita,
                    NomeCognome = _visura.DestinatarioNominativo
                }
            };

        }
    }
}