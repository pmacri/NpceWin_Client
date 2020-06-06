using NPCE_WinClient.Model;
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
        public Visura Visura { get; set; }
    }
}
