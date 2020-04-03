using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.Model
{
    public class Opzioni 
    {
        public int Id { get; set; }
        public bool AttestazioneConsegna { get; set; }
        public bool FronteRetro { get; set; }
        public bool Colore { get; set; }

    }
}
