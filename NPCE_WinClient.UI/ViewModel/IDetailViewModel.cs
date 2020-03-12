using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public interface IDetailViewModel
    {
        Task LoadAsync(int? id);
        bool HasChanges { get; }
    }
}