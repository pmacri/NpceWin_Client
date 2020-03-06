using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public interface IMittenteDetailViewModel
    {
        Task LoadMittenteById(long id);
    }
}