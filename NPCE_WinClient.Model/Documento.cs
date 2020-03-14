using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.Model
{
    public class Documento
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public long Size { get; set; }

        public string Extension { get; set; }

        public byte[] Content { get; set; }

        [Required]
        public string Tag { get; set; }
    }
}
