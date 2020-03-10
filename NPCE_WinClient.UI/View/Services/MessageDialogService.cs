using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NPCE_WinClient.UI.View.Services
{
    public class MessageDialogService : IMessageDialogService
    {
        public MessageDialogResult ShowOKCancelDialog(string text, string caption)
        {
            var result = MessageBox.Show(text, caption, MessageBoxButton.OKCancel);

            return result == MessageBoxResult.OK
                ? MessageDialogResult.OK
                : MessageDialogResult.Cancel;
        }
    }
    public enum MessageDialogResult
    {
        OK,
        Cancel
    }
}
