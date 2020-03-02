using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Data
{
    public class NpceDataService : INpceDataService
    {
        public IEnumerable<Service> GetAll()
        {
            yield return new Service { IdRichiesta = Guid.NewGuid(), CreationTime = DateTime.Now, State = "Prepared" };
            yield return new Service { IdRichiesta = Guid.NewGuid(), CreationTime = DateTime.Now, State = "Prepared" };
            yield return new Service { IdRichiesta = Guid.NewGuid(), CreationTime = DateTime.Now, State = "Prepared" };
            yield return new Service { IdRichiesta = Guid.NewGuid(), CreationTime = DateTime.Now, State = "Prepared" };
        }
    }
}
