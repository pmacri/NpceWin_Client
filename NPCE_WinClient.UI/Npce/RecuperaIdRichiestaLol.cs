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
        private readonly Servizio _servizio;
        LOLServiceSoap _proxy;
        public RecuperaIdRichiestaLol(Ambiente ambiente, Servizio servizio)
        {
            _ambiente = ambiente;
            _servizio = servizio;
            Execute();            
        }

        private void Execute()
        {
            var helper = new Helper();
            _proxy = helper.GetProxy<LOLServiceSoap>(_ambiente.LolUri);

            var fake = new OperationContextScope((IContextChannel)_proxy);
            var property = new HttpRequestMessageProperty();
            property.Headers.Add("customerid", _ambiente.customerid);
            property.Headers.Add("smuser", _ambiente.smuser);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = property;
            var result = _proxy.RecuperaIdRichiesta();
        }
    }
}
