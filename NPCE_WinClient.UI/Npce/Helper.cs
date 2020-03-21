using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Npce
{
    public class Helper
    {
        public T GetProxy<T>(string endpointAddress, string username, string password)
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();
            myBinding.SendTimeout = TimeSpan.FromMinutes(3);
            myBinding.MaxReceivedMessageSize = 2147483647;
            EndpointAddress myEndpoint = new EndpointAddress(endpointAddress);

            ChannelFactory<T> myChannelFactory = new ChannelFactory<T>(myBinding, myEndpoint);

            myChannelFactory.Credentials.UserName.UserName = username;
            myChannelFactory.Credentials.UserName.Password = password;
            // Create a channel
            T wcfClient = myChannelFactory.CreateChannel();
            
            return wcfClient;
        }
    }
}
