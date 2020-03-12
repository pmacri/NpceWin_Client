using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Event
{
    public class OpenDetailViewEvent:PubSubEvent<OpenDetailViewEventargs>
    {
    }

    public class OpenDetailViewEventargs
    {
        public int? Id { get; set; }

        public string ViewModelName { get; set; }
    }
}
