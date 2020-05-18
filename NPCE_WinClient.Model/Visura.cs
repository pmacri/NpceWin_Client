using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.Model
{
    public class Visura
    {

        public int Id { get; set; }
        public string RichiedenteNome { get; set; }
        public string IdRichiesta { get; set; }
        public string RichiedenteCognome { get; set; }

        public string RichiedenteIndirizzo { get; set; }

        public string RichiedenteLocalita { get; set; }
        public string RichiedenteCap { get; set; }
        public string RichiedentePrefissoTelefono { get; set; }

        public string RichiedenteTelefono { get; set; }

        // Destinatario
        public string DestinatarioNominativo { get; set; }

        public string DestinatarioLocalita { get; set; }

        public string DestinatarioCap { get; set; }

        public string DestinatarioEmail { get; set; }

        public string DestinatarioIndirizzo { get; set; }

        public VisureTipoRecapito VisureTipoRecapito { get; set; }

        public string VisureTipoRecapitoId { get; set; }

        // Documento
        public string DocumentoIntestatarioNome { get; set; }

        public string DocumentoIntestatarioCognome { get; set; }

        public string DocumentoIntestatarioRagioneSociale { get; set; }

        public string DocumentoIntestatarioCCIAA { get; set; }

        public string DocumentoIntestatarioNREA { get; set; }

        public string DocumentoIntestatarioCodiceFiscale { get; set; }

        public String VisureTipoDocumentoId { get; set; }

        public VisureTipoDocumento DocumentoTipoDocumento { get; set; }

        public String VisureCodiceDocumentoId { get; set; }
        public VisureCodiceDocumento DocumentoCodiceDocumento { get; set; }
        public string VisureFormatoDocumentoId { get; set; }
        public VisureFormatoDocumento DocumentoFormatoDocumento { get; set; }

        public StatoServizio StatoVisura { get; set; }
        public int StatoServizioId { get; set; }
    }
}
