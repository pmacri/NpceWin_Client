﻿using NPCE_WinClient.Model;
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

        protected virtual NpceOperationResult CreateResult(NpceOperation operation, string codeCEResult, string descriptionCEResult, 
                                                           string idRichiesta)
        {
            var result = new NpceOperationResult();
            result.Operation = operation;
            result.Success = int.Parse(codeCEResult) == 0 ? true : false;

            result.IdRichiesta = idRichiesta;

            if (!result.Success)
            {
                result.Errors = new List<Error>();
                result.Errors.Add( new Error {  Code= codeCEResult, Description = descriptionCEResult });
            }

            return result;

        }
    }

}
