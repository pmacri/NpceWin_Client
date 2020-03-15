using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Wrapper
{
    public class DocumentoWrapper: ModelWrapper<Documento>
    {
        public DocumentoWrapper(Documento model): base(model)
        {

        }
        public int Id { get; set; }

        public string FileName {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public long Size
        {
            get { return GetValue<long>(); }

            set
            {
                SetValue<long>(value);
            }
        }

        public string Extension {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public byte[] Content {
            get { return GetValue<byte[]>(); }

            set
            {
                SetValue<byte[]>(value);
            }
        }

        public string Tag {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

    }
}
