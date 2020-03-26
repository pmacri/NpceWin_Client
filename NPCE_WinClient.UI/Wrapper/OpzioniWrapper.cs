using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Wrapper
{
    public class OpzioniWrapper:ModelWrapper<Opzioni>
    {

        public OpzioniWrapper(Opzioni opzioni): base(opzioni)
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
        public bool AttestazioneConsegna {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }
        public bool FronteRetro {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }
        public bool Colore {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }
    }
}
