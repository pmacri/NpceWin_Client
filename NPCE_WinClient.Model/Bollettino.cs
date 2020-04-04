using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.Model
{
    public class Bollettino
    {

        public int Id { get; set; }

        public string Tipo { get; set; }
        // Base
        public byte[] Logo { get; set; }

        public string NumeroContoCorrente { get; set; }

        public string IntestatoA { get; set; }

        public string Causale { get; set; }

        public string AdditionalInfo { get; set; }

        public string NumeroAutorizzazioneStampaInProprio { get; set; }

        public string IBan { get; set; }

        public string EseguitoDaNominativo { get; set; }

        public string EseguitoDaIndirizzo { get; set; }

        public string EseguitoDaCap { get; set; }

        public string EseguitoDaLocalita { get; set; }

        // 896
        public decimal ImportoEuro { get; set; }

        public string CodiceCliente { get; set; }

        public int PaginaBollettinoId { get; set; }

        public PaginaBollettino PaginaBollettino { get; set; }

    }
}
