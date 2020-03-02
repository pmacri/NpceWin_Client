using System.Collections.Generic;
using NPCE_WinClient.Model;

namespace NPCE_WinClient.UI.Data
{
    public interface INpceDataService
    {
        IEnumerable<Service> GetAll();
    }
}