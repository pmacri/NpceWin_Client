using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public interface IServiceDetailViewModel
    {
        Task LoadServiceById(long id);
    }
}