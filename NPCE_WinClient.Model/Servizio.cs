using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.Model
{
    public class Servizio
    {
        public Servizio()
        {
            Anagrafiche = new Collection<Anagrafica>();
            Documenti = new Collection<Documento>();
            PagineBollettini= new Collection<PaginaBollettino>();
        }

        public int Id { get; set; }
        public bool AvvisoRicevimento { get; set; }
        public bool ArchiviazioneDigitale { get; set; }
        public bool Autoconferma { get; set; }
        public string IdRichiesta { get; set; }
        public bool AttestazioneConsegna { get; set; }
        public bool FronteRetro { get; set; }
        public bool Colore { get; set; }
        public string GuidUtente { get; set; }
        public string IdOrdine { get; set; }

        // Navigation
        public TipoServizio TipoServizio { get; set; }
        public int? TipoServizioId { get; set; }

       

        public StatoServizio StatoServizio { get; set; }
        public int StatoServizioId { get; set; }

        // Destinatari + Mittente
        public ICollection<Anagrafica> Anagrafiche { get; set; }
        // Documenti
        public ICollection<Documento> Documenti { get; set; }

        public ICollection<PaginaBollettino> PagineBollettini { get; set; }

    }
}
