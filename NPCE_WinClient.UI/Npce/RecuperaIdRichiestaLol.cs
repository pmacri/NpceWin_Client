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
    public class RecuperaIdRichiestaLol
    {
        private readonly Ambiente _ambiente;
        LOLServiceSoap _proxy;
        public RecuperaIdRichiestaLol(Ambiente ambiente)
        {
            _ambiente = ambiente;
        }

        public string Execute()
        {
            var helper = new Helper();
            _proxy = helper.GetProxy<LOLServiceSoap>(_ambiente.LolUri, _ambiente.Username, _ambiente.Password);

            

            var fake = new OperationContextScope((IContextChannel)_proxy);
            var property = new HttpRequestMessageProperty();

            property.Headers.Add("customerid", _ambiente.customerid);
            property.Headers.Add("smuser", _ambiente.smuser);
            property.Headers.Add("costcenter", _ambiente.costcenter);
            property.Headers.Add("billingcenter", _ambiente.billingcenter);
            property.Headers.Add("idsender", _ambiente.idsender);
            property.Headers.Add("contracttype", _ambiente.contracttype);
            property.Headers.Add("sendersystem", _ambiente.sendersystem);
            property.Headers.Add("usertype", _ambiente.usertype);



            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = property;

            
            var result = _proxy.RecuperaIdRichiesta();

            return result.IDRichiesta;
        }
    }
}
