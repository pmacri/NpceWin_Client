using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.Model
{
    public class Anagrafica
    {

        public Anagrafica()
        {
            ServizioDestinatari = new Collection<Servizio>();
        }
        public int Id { get; set; }

        [Required]
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
        public bool IsMittente { get; set; }
        public bool IsDefaultMittente { get; set; }
        // Servizi
        public ICollection<Servizio> ServizioDestinatari { get; set; }
    }
}
