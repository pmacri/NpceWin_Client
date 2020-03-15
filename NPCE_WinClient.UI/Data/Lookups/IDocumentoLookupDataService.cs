using NPCE_WinClient.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Data.Lookups
{
    public interface IDocumentoLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetDocumentoLookupAsync();
    }
}