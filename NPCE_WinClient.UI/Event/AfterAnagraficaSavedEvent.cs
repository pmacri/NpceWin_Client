using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Event
{
    public class AfterAnagraficaSavedEvent: PubSubEvent<AfterAnagraficaSavedEventArgs>
    {
    }

    public class AfterAnagraficaSavedEventArgs
    {
        public int Id { get; set; }
        public string DisplayMember { get; set; }

    }
}
