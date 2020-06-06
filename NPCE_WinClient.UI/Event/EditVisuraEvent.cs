using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Event
{
    public class EditVisuraEvent:PubSubEvent<EditVisuraEventArgs>
    {
    }

    public class EditVisuraEventArgs
    {
        public int IdVisura { get; set; }
    }
}
