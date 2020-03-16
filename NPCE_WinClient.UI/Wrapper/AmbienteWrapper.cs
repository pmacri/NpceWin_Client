using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Wrapper
{
    public class AmbienteWrapper: ModelWrapper<Ambiente>
    {
        public AmbienteWrapper(Ambiente ambiente): base(ambiente)
        {
        }
        public int Id { get; set; }

        public string Description { get; set; }
        public string customerid { get; set; }
        public string costcenter { get; set; }
        public string billingcenter { get; set; }
        public string idsender { get; set; }
        public string sendersystem { get; set; }
        public string smuser { get; set; }
        public string contracttype { get; set; }
        public string usertype { get; set; }
    }
}
