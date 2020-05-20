using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.Proxy;
using ComunicazioniElettroniche.Common.Serialization;
using ComunicazioniElettroniche.Visure.BE;
using NPCE_WinClient.Model;
using PosteItaliane.OrderManagement.Schema.SchemaDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Npce
{
    public class ConfermaVisuraPil : NpceOperationBase
    {

        public NpceOperationResult Execute()
        {

            var ce = new ComunicazioniElettroniche.Common.DataContracts.CE();
            ce.Header = GetHeaders(_ambiente);
            ce.Header.GUIDMessage = Guid.NewGuid().ToString();

            ConfermaRequest confermaRequest = new ConfermaRequest
            {
                IdOrdine = _visura.IdOrdine,
                PaymentDate = System.DateTime.Now,
                PaymentDateSpecified = true,
                PaymentType = new PaymentType { PostPayment = true, PostPaymentSpecified = true, TypeId = "6" }
            };

            SetUserInfo(confermaRequest);

            ce.Body = SerializationUtility.SerializeToXmlElement(confermaRequest);

            ConfermaResponse confermaResponse = null;
            using (WsCEClient client = new WsCEClient())
            {
                client.Endpoint.Address = new System.ServiceModel.EndpointAddress(_ambiente.VolUri);
                client.SubmitRequest(ref ce);
                try
                {
                    confermaResponse = SerializationUtility.Deserialize<ConfermaResponse>(ce.Body);
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }

            return CreateResult(NpceOperation.Conferma, confermaResponse.Result.ResType == TResultResType.I ? "0" : "99", "Invio Ok", confermaResponse.IdOrdine, null, null);

        }

        public ConfermaVisuraPil(Visura visura, Ambiente ambiente) : base(ambiente, visura, null)
        {
        }

        private void SetUserInfo(ConfermaRequest result)
        {
            result.UserInfo = new UserInfo
            {
                Canale = _ambiente.sendersystem,
                Cliente = _ambiente.customerid,
                IdCdC = "",
                UserId = _ambiente.smuser,
                IdCliente = _ambiente.customerid
            };
        }


        private ComunicazioniElettroniche.Common.DataContracts.CEHeader GetHeaders(Ambiente ambiente)
        {
            return new ComunicazioniElettroniche.Common.DataContracts.CEHeader
            {
                BillingCenter = ambiente.billingcenter,
                ContractId = ambiente.contractid,
                CostCenter = ambiente.costcenter,
                Customer = ambiente.customer,
                IdCRM = string.Empty,
                SenderSystem = ambiente.sendersystem,
                UserId = ambiente.smuser,
                IDSender = string.Empty,
                UserType = ambiente.usertype
            };
        }
    }
}
