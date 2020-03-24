using FriendOrganizer.UI.View.Services;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.View.Services
{
    public interface IMessageDialogService
    {
        Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title);
        Task ShowInfoDialogAsync(string text);
    }
}