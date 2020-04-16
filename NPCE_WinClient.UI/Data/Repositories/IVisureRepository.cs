using NPCE_WinClient.DataAccess;
using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Data.Repositories
{
    public interface IVisureRepository: IGenericRepository<Visura>
    {
        Task<IEnumerable<VisureTipoDocumento>> GetAllTipiDocumentoAsync();
        Task<VisureTipoDocumento> GetTipoDocumentoByDescriptionAsync(string description);
        Task<IEnumerable<VisureFormatoDocumento>> GetAllFormatiDocumentoAsync();
        Task<VisureFormatoDocumento> GetFormatoDocumentoByDescriptionAsync(string description);
        Task<IEnumerable<VisureCodiceDocumento>> GetAllCodiciDocumentoAsync();
    }
}
