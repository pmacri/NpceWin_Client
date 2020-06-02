using ComunicazioniElettroniche.Common.DataContracts;
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
        //private readonly Visura _visura;

        public NpceOperationResult Execute(bool autoConferma, bool controllaPrezzo)
        {

            var ce = new ComunicazioniElettroniche.Common.DataContracts.CE();
            ce.Header = GetHeaders(_ambiente);
            ce.Header.GUIDMessage = Guid.NewGuid().ToString();

            DocumentiRequest documentiRequest = GetDocumentiRequest();
            documentiRequest.ControllaPrezzoDiVendita = controllaPrezzo;
            documentiRequest.ControllaPrezzoDiVenditaSpecified = true;
            documentiRequest.Autoconferma = autoConferma;
            documentiRequest.AutoconfermaSpecified = true;

            

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
                catch (Exception ex)
                {
                    throw (ex);
                }
            }

            return CreateResult(NpceOperation.Invio, documentiResponse.Result.ResType == TResultResType.I ? "0" : "99", documentiResponse.Result.Description?.Substring(0, Math.Min(documentiResponse.Result.Description.Length, 500)) ?? "Invio Ok", documentiResponse.IdentificativoRichiesta, documentiResponse?.OrderResponse?.IdOrder, null);

        }

        public InvioVisuraPIL(Visura visura, Ambiente ambiente) : base(ambiente, visura, null)
        {
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

        private DocumentiRequest GetDocumentiRequest()
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
                UserId = _ambiente.smuser,
                IdCliente= _ambiente.customerid
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

            var codiceFiscaleDittaIndividuale = _visura.DocumentoIntestatarioCodiceFiscale?.Length == 19 ? _visura.DocumentoIntestatarioCodiceFiscale : string.Empty;
            var codiceFiscaleImpresa = _visura.DocumentoIntestatarioCodiceFiscale?.Length == 11 ? _visura.DocumentoIntestatarioCodiceFiscale : string.Empty;
            documento.Intestatario = new DocumentiRequestDocumentoIntestatario
            {
                CodiceFiscaleDittaIndividuale = codiceFiscaleDittaIndividuale,
                Cognome = _visura.DocumentoIntestatarioCognome,
                Nome = _visura.DocumentoIntestatarioNome,
                RagioneSociale = _visura.DocumentoIntestatarioRagioneSociale,
                NumeroREA = _visura.DocumentoIntestatarioNREA,
                CodiceFiscaleImpresa= codiceFiscaleImpresa

            };

            documentiList.Add(documento);

            result.Documento = documentiList.ToArray();

        }

        private void SetRecapito(DocumentiRequest result)
        {
            result.Recapito = new DocumentiRequestRecapito
            {
                CodiceRecapito = _visura.VisureTipoRecapitoId,
                Email = _visura.DestinatarioEmail,
                Destinatario = new DocumentiRequestRecapitoDestinatario
                {
                    CAP = _visura.DestinatarioCap,
                    Localita = _visura.DestinatarioLocalita,
                    NomeCognome = _visura.DestinatarioNominativo,
                    Indirizzo = _visura.DestinatarioIndirizzo
                }
            };

        }
    }
}
