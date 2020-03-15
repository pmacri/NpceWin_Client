using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.View.Services
{
    public interface IFileService
    {
        SelectFileResult SelectFile();
    }

    public class SelectFileResult
    {
        public string FileName { get; set; }

        public bool Success { get; set; }

        public long FileSize { get; set; }

        public byte[] Content { get; set; }
    }
}
