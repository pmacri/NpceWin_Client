using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public interface IAnagraficaDetailViewModel
    {
        Task LoadAnagraficaById(long id);
    }
}