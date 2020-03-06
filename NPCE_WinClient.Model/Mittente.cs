using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.Model
{
    public class Mittente
    {
        public int Id { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string RagioneSociale { get; set; }
        public string ComplementoNominativo { get; set; }

        public string DUG { get; set; }
        public string Toponimo { get; set; }
        public string Esponente { get; set; }
        public string NumeroCivico { get; set; }
        public string ComplementoIndirizzo { get; set; }
        public string Frazione { get; set; }
        public string Citta { get; set; }
        public string Cap { get; set; }

        public string Telefono { get; set; }

        public string CasellaPostale { get; set; }


        public string Provincia { get; set; }
        public string Stato { get; set; }

    }
}
