using NPCE_WinClient.Model;
using NPCE_WinClient.Services.Vol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Documento = NPCE_WinClient.Services.Vol.Documento;

namespace NPCE_WinClient.UI.Npce
{
    public class Vol: NpceOperationBase
    {
        ICameraliService proxy;
        HttpRequestMessageProperty headers;
        InvioRequest invioRequest;
        private ConfermaInvioRequest confermaRequest;

        public Vol(Ambiente ambiente, Visura visura, string idRichiesta) : base(ambiente, visura, idRichiesta)
        {
            Init();
        }

        private void Init()
        {
            var helper = new Helper();
            proxy = helper.GetProxy<ICameraliService>(_ambiente.VolUri, _ambiente.Username, _ambiente.Password);
            headers = GetHttpHeaders(_ambiente);
        }

        public NpceOperationResult Invio()
        {
            SetHeaders();
            invioRequest = new InvioRequest();
            SetDocumento(invioRequest); // e Intestatario
            SetDestinatario(invioRequest);
            SetRichiedente(invioRequest);
            var invioResult = proxy.Invio(invioRequest);
            var errors = new List<Error>();

            foreach (var err in invioResult.Errori)
            {
                errors.Add(new Error
                {
                    Code = err.Codice.ToString(),
                    Description = err.Messaggio.ToString()
                });

            }

            return new NpceOperationResult
            {
                Success = invioResult.Esito == EsitoPostaEvo.OK,
                IdRichiesta = invioResult.IdRichiesta,
                ErrorMessage = invioResult.Esito != EsitoPostaEvo.OK ? invioResult.Errori[0].Messaggio.ToString() : string.Empty,
                Errors = errors,
                IdOrdine= invioResult.IdTicket
            };
        }

        public NpceOperationResult Conferma()
        {
            SetHeaders();
            confermaRequest = new ConfermaInvioRequest();
            confermaRequest.IdRichiesta = _visura.IdRichiesta;

            var confermaResult = proxy.Conferma(confermaRequest);
            var errors = new List<Error>();

            foreach (var err in confermaResult.Errori)
            {
                errors.Add(new Error
                {
                    Code = err.Codice.ToString(),
                    Description = err.Messaggio.ToString()
                });

            }

            return new NpceOperationResult
            {
                Success = confermaResult.Esito == EsitoPostaEvo.OK,
                IdRichiesta = confermaResult.IdTicket,
                ErrorMessage = confermaResult.Esito != EsitoPostaEvo.OK ? confermaResult.Errori[0].Messaggio.ToString() : string.Empty,
                Errors = errors
            };
        }



        private void SetRichiedente(InvioRequest invioRequest)
        {
            invioRequest.Richiedente = new Richiedente
            {
                CAP = _visura.RichiedenteCap,
                Cognome = _visura.RichiedenteCognome,
                Nome = _visura.RichiedenteNome,
                Indirizzo = _visura.RichiedenteIndirizzo,
                Localita = _visura.RichiedenteLocalita,
                Telefono = _visura.RichiedenteTelefono,
                TelefonoPrefisso = _visura.RichiedentePrefissoTelefono
            };
        }

        private void SetDestinatario(InvioRequest invioRequest)
        {
            invioRequest.Destinatario = new Destinatario
            {
                CAP = _visura.DestinatarioCap,
                Email = _visura.DestinatarioEmail,
                Indirizzo = _visura.RichiedenteIndirizzo,
                Localita = _visura.DestinatarioLocalita,
                Nominativo = _visura.DestinatarioNominativo,
                TipoRecapito = MapTipoRecapito(_visura.VisureTipoRecapitoId)

            };
        }

        private TipoRecapito MapTipoRecapito(string visureTipoRecapitoId)
        {
            switch (visureTipoRecapitoId)
            {
                case "E": return TipoRecapito.E;
                case "DL": return TipoRecapito.D;
                case "R": return TipoRecapito.R;
                case "S": return TipoRecapito.S;
                    // TODO Pec
                case "P": return TipoRecapito.D;
                default: return TipoRecapito.E;
            }
        }

        private void SetDocumento(InvioRequest invioRequest)
        {
            Documento documento = new Documento();

            documento.CodiceDocumento = MapCodiceDocumento(_visura.VisureCodiceDocumentoId);
            documento.TipoDocumento = MapTipoDocumento(_visura.VisureTipoDocumentoId);
            documento.Formato = MapFormato(_visura.VisureFormatoDocumentoId);
            // TODO
            documento.Bollo = false;
            documento.Chiusura = string.Empty;
            documento.Intestatario = MapIntestatatario(_visura);
            invioRequest.Documento = documento;
        }

        private Intestatario MapIntestatatario(Visura visura)
        {
            return new Intestatario
            {
                CCIAA = visura.DocumentoIntestatarioCCIAA,
                CodiceFiscale = visura.DocumentoIntestatarioCodiceFiscale,
                Cognome = visura.DocumentoIntestatarioCognome,
                Nome = visura.DocumentoIntestatarioNome,
                NREA = Convert.ToInt32(visura.DocumentoIntestatarioNREA),
                RagioneSociale = visura.DocumentoIntestatarioRagioneSociale
            };
        }

        private Formato MapFormato(string visureFormatoDocumentoId)
        {
            switch (visureFormatoDocumentoId)
            {
                case "pdf": return Formato.PDF;
                case "xml": return Formato.XML;
                default: return Formato.PDF;
            }
        }

        private TipoDocumento MapTipoDocumento(string documentoTipoDocumentoId)
        {
            switch (documentoTipoDocumentoId)
            {
                case "C": return TipoDocumento.CertificatoCamerale;
                case "V": return TipoDocumento.VisuraCamerale;
                default: return TipoDocumento.VisuraCamerale;
            }
        }

        private CodiceDocumento MapCodiceDocumento(string documentoCodiceDocumentoId)
        {
            switch (documentoCodiceDocumentoId)
            {

                case "VISO":
                    return CodiceDocumento.VISO;
                case "VISS":
                    return CodiceDocumento.VISS;
                case "ATTI":
                    return CodiceDocumento.ATTI;
                case "BICM":
                    return CodiceDocumento.BICM;
                case "CART":
                    return CodiceDocumento.CART;
                case "CRIA":
                    return CodiceDocumento.CRIA;
                case "CRIM":
                    return CodiceDocumento.CRIM;
                case "CRIS":
                    return CodiceDocumento.CRIS;
                case "FASC":
                    return CodiceDocumento.FASC;
                case "RIPR":
                    return CodiceDocumento.RIPR;
                case "SCPE":
                    return CodiceDocumento.SCPE;
                case "SCSC":
                    return CodiceDocumento.SCSC;
                case "SCSO":
                    return CodiceDocumento.SCSO;
                case "SOST":
                    return CodiceDocumento.SOST;
                case "TRSF":
                    return CodiceDocumento.TRSF;                
                default:
                    return CodiceDocumento.TRSF;
            }
        }

        private void SetHeaders()
        {
            var fake = new OperationContextScope((IContextChannel)proxy);
            var headers = GetHttpHeaders(_ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;
        }

    }
}
