using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NPCE_WinClient.UI.View.Services
{
    public class FileService : IFileService
    {
        public SelectFileResult SelectFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return new SelectFileResult { Success = false };

            }
            else
            {
                //Get the path of specified file
                string fileName = openFileDialog.SafeFileName;

                //Read the contents of the file into a stream

                var fileStream = openFileDialog.OpenFile();
                
                var size = fileStream.Length;

                byte[] fileBytes;

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        reader.BaseStream.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                }

                var result = new SelectFileResult
                {
                    Content = fileBytes,
                    FileName = fileName,
                    FileSize = size,
                    Success = true
                };

                return result;
            }
        }

        public byte[] ReadToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }



}
