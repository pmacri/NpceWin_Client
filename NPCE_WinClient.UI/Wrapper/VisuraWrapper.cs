using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Wrapper
{
   public class VisuraWrapper : ModelWrapper<Visura>
    {
        public VisuraWrapper(Visura model): base(model)
        {

        }

        public int Id
        {
            get { return GetValue<int>(); }

            set
            {
                SetValue<int>(value);
            }
        }
        public string RichiedenteNome
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string RichiedenteCognome {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string RichiedenteIndirizzo {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string RichiedenteLocalita {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string RichiedenteCap {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string RichiedentePrefissoTelefono {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string RichiedenteTelefono {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        // Destinatario
        public string DestinatarioNominativo {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string DestinatarioLocalita {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string DestinatarioCap {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string DestinatarioEmail {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public VisureTipoRecapito VisureTipoRecapito {
            get { return GetValue<VisureTipoRecapito>(); }

            set
            {
                SetValue<VisureTipoRecapito>(value);
            }
        }

        // Documento
        public string DocumentoIntestatarioNome {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string DocumentoIntestatarioCognome {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string DocumentoIntestatarioRagioneSociale {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string DocumentoIntestatarioCCIAA {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string DocumentoIntestatarioNREA {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string DocumentoIntestatarioCodiceFiscale {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public VisureTipoDocumento DocumentoTipoDocumento {
            get { return GetValue<VisureTipoDocumento>(); }

            set
            {
                SetValue<VisureTipoDocumento>(value);
            }
        }

        public VisureCodiceDocumento DocumentoCodiceDocumento {
            get { return GetValue<VisureCodiceDocumento>(); }

            set
            {
                SetValue<VisureCodiceDocumento>(value);
            }
        }

        public VisureFormatoDocumento DocumentoFormatoDocumento
        {
            get { return GetValue<VisureFormatoDocumento>(); }

            set
            {
                SetValue<VisureFormatoDocumento>(value);
            }
        }

    }
}
