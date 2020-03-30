using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Npce
{
    public class NpceOperationResult
    {
        public NpceOperation Operation { get; set; }

        public bool Success { get; set; }

        public string IdRichiesta { get; set; }

        public string IdOrdine { get; set; }

        public string GuidUtente { get; set; }

        public List<Error> Errors { get; set; }
    }
}

namespace NPCE_WinClient.UI
{
    public class Error
    {
        public string Code { get; set; }

        public string Description { get; set; }
    }
}