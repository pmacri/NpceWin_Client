using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Wrapper
{
    public class PaginaBollettinoWrapper: ModelWrapper<PaginaBollettino>
    {
        public PaginaBollettinoWrapper(PaginaBollettino model):base(model)
        {
            Bollettini = new ObservableCollection<Bollettino>();
        }
        public ICollection<Bollettino> Bollettini { get; set; }
    }

}
