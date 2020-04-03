using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.Model
{
    public class PaginaBollettino
    {
        public PaginaBollettino()
        {
            Bollettini = new ObservableCollection<Bollettino>();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public ICollection<Bollettino> Bollettini { get; set; }
    }
}
