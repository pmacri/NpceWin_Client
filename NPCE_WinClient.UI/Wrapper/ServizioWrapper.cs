using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Wrapper
{
    public class ServizioWrapper : Wrapper.ModelWrapper<Servizio>
    {
        public ServizioWrapper(Servizio servizio) : base(servizio)
        {

        }
        public int Id {
            
            get { return GetValue<int>(); }

            set
            {
                SetValue<int>(value);
            }
        }
        public bool AvvisoRicevimento
        {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }
        public bool FronteRetro
        {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }
        public bool Colore
        {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }
        public bool ArchiviazioneDigitale
        {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }
        public bool Autoconferma
        {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }

        //public ICollection<Anagrafica> Anagrafiche { get; set; }

    }
}
