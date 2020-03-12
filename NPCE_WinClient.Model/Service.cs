using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.Model
{
    public class Service
    {
        public int Id { get; set; }
        public Guid IdRichiesta { get; set; }
        public DateTime CreationTime { get; set; }
        public string State { get; set; }
    }
}
