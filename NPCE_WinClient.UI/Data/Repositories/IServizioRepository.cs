using NPCE_WinClient.Model;
using System.Collections;
using System.Collections.Generic;

namespace NPCE_WinClient.UI.Data.Repositories
{
    public interface IServizioRepository: IGenericRepository<Servizio>
    {
        IEnumerable<Anagrafica> GetAllAnagrafiche();
        IEnumerable<TipoServizio> GetAllTipiServizio();
        IEnumerable<Documento> GetAllDocumenti();
        void UpdateTipoServizioAsync(int idServizio, int idTipoServizio);
    }
}