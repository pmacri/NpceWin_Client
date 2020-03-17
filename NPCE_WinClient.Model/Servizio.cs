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
            ServizioDestinatari = new Collection<Anagrafica>();
            Documenti = new Collection<Documento>();
        }

        public int Id { get; set; }
        public bool AvvisoRicevimento { get; set; }
        public bool FronteRetro { get; set; }
        public bool Colore { get; set; }
        public bool ArchiviazioneDigitale { get; set; }
        public bool Autoconferma { get; set; }

        // Navigation
        public TipoServizio TipoServizio { get; set; }
        public int TipoServizioId { get; set; }
        
        // Mittente
        public Anagrafica Mittente { get; set; }
        
        [ForeignKey("Mittente")]
        public int MittenteId { get; set; }

        // Destinatari
        public ICollection<Anagrafica> ServizioDestinatari { get; set; }

        // Documenti
        public ICollection<Documento> Documenti { get; set; }

    }
}
