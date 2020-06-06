using NPCE_WinClient.UI.Wrapper;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Event
{
    public class VisuraSavedEvent : PubSubEvent<VisuraSavedEventArgs>
    {
    }

    public class VisuraSavedEventArgs
    {
        public VisuraWrapper Visura { get; set; }

        public bool FromMain { get; set; }
    }
}
