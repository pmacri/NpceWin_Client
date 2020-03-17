using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.Model
{
    public class ServizioDestinatario
    {
            public int ServizioId { get; set; }
            public Servizio Servizio { get; set; }

            public string AnagraficaId { get; set; }
            public Anagrafica Anagrafica { get; set; }
    }
}
