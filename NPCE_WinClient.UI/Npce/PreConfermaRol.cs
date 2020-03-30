using NPCE_WinClient.Model;
using NPCE_WinClient.Services.Rol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Npce
{
    public class PreConfermaRol : NpceOperationBase
    {
        private string _guidUtente;
        private bool _autoConferma;
        ROLServiceSoap _proxy;
        public PreConfermaRol(Ambiente ambiente, Servizio servizio, string idRichiesta, string guidUtente,
            bool autoConferma) : base(ambiente, servizio, idRichiesta)
        {
            _guidUtente = guidUtente;
            _autoConferma = autoConferma;
        }

        public NpceOperationResult Execute()
        {
            var helper = new Helper();

            _proxy = helper.GetProxy<ROLServiceSoap>(_ambiente.RolUri, _ambiente.Username, _ambiente.Password);
            var listRichieste = new List<Richiesta>();
            listRichieste.Add(new Richiesta() { GuidUtente = _servizio.GuidUtente, IDRichiesta = _idRichiesta });
            PreConfermaRequest request = new PreConfermaRequest { Richieste = listRichieste.ToArray(), autoConferma = _autoConferma };
            var fake = new OperationContextScope((IContextChannel)_proxy);
            var headers = GetHttpHeaders(_ambiente);
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = headers;

            var preConfermaResult = _proxy.PreConferma(request);

            return CreateResult(_autoConferma ? NpceOperation.PreConfermaWithAutoconfirm : NpceOperation.PreConferma, preConfermaResult.PreConfermaResult.CEResult.Code, preConfermaResult.PreConfermaResult.CEResult.Code, _idRichiesta, preConfermaResult.PreConfermaResult.IdOrdine, null);
        }
    }
}
