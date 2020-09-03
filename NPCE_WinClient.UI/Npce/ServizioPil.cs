using ComunicazioniElettroniche.Common.DataContracts;
using ComunicazioniElettroniche.Common.Frontend.DataContracts;
using ComunicazioniElettroniche.Common.Proxy;
using ComunicazioniElettroniche.Common.Serialization;
using NPCE_WinClient.Model;
using PosteItaliane.OrderManagement.Schema.SchemaDefinition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Npce
{
    public abstract class ServizioPil : NpceOperationBase
    {
        public ServizioPil(Ambiente ambiente, Servizio servizio, string idRichiesta): base(ambiente,servizio, idRichiesta)
        {

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
                    ce = Conferma(orderResponse, ce, client);
                }

            }

            return CreateResult(NpceOperation.PreConfermaWithAutoconfirm, ce.Result.ResType == TResultResType.I ? "0" : "99", ce.Result.Description?.Substring(0, Math.Min(ce.Result.Description.Length, 500)) ?? "Invio Ok", orderResponse.IdOrder, null, null);
        }
        public CE Conferma(OrderResponse orderResponse, CE ce, WsCEClient client)
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
            ConfirmOrder confirm = new ConfirmOrder
            {
                IdOrder = orderResponse.IdOrder,
                PaymentType = new PaymentType
                {
                    TypeId = op.IdTipoPagamento,
                    PostPayment = true
                }
            };
            ce.Body = SerializationUtility.SerializeToXmlElement(confirm);
            client.SubmitRequest(ref ce);
            return ce;
        }
        public abstract NpceOperationResult Invio();
        public abstract NpceOperationResult Conferma();

        protected CE GetCE()
        {
            var ce = new CE();
            ce.Header = new CEHeader();

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
            ce.Header.GUIDMessage = _servizio.IdRichiesta;
            return ce;
        }


    }
}
