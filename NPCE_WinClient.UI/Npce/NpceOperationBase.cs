using NPCE_WinClient.Model;
using NPCE_WinClient.Services.Lol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Npce
{
    public class NpceOperationBase
    {
        protected virtual HttpRequestMessageProperty GetHttpHeaders(Ambiente ambiente)
        {
            var property = new HttpRequestMessageProperty();
            property.Headers.Add("customerid", ambiente.customerid);
            property.Headers.Add("smuser", ambiente.smuser);
            property.Headers.Add("costcenter", ambiente.costcenter);
            property.Headers.Add("billingcenter", ambiente.billingcenter);
            property.Headers.Add("idsender", ambiente.idsender);
            property.Headers.Add("contracttype", ambiente.contracttype);
            property.Headers.Add("sendersystem", ambiente.sendersystem);
            property.Headers.Add("contractid", ambiente.contractid);
            property.Headers.Add("customer", ambiente.customer);
            property.Headers.Add("usertype", ambiente.usertype);


            return property;
        }    

        protected virtual NpceOperationResult CreateResult(NpceOperation operation, CEResult ceResult)
        {
            var result = new NpceOperationResult();
            result.Operation = operation;
            result.Success = int.Parse(ceResult.Code) == 0 ? true : false;

            if (!result.Success)
            {
                result.Errors = new List<Error>();
                result.Errors.Add( new Error {  Code= ceResult.Code, Description = ceResult.Description});
            }



            return result;

        }
    }

}
