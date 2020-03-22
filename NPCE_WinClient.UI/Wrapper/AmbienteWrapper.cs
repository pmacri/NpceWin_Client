using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Wrapper
{
    public class AmbienteWrapper: ModelWrapper<Ambiente>
    {
        public AmbienteWrapper(Ambiente ambiente): base(ambiente)
        {
        }
        public int Id
        {
            get { return GetValue<int>(); }

            set
            {
                SetValue<int>(value);
            }
        }

        public string Description
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string customerid
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string costcenter
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string billingcenter
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string idsender
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string sendersystem
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string smuser
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string contracttype
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string usertype
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string Contratto {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string Username {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string Password {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string LolUri
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string RolUri
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string ColUri
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string MolUri
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }


        public string contractid {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string customer {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
    }
}
