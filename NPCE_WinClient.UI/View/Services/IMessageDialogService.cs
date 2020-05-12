using FriendOrganizer.UI.View.Services;
using MahApps.Metro.Controls.Dialogs;
using System.Threading.Tasks;
using MessageDialogResult = FriendOrganizer.UI.View.Services.MessageDialogResult;

namespace NPCE_WinClient.UI.View.Services
{
    public interface IMessageDialogService
    {
        Task<MessageDialogResult> ShowOkCancelDialogAsync(string text, string title);
        Task ShowInfoDialogAsync(string text);
        Task<ProgressDialogController> ShowProgressAsync(string title, string message);
    }
}