using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Wrapper
{
    public class BollettinoWrapper: ModelWrapper<Bollettino>
    {
        public BollettinoWrapper(Bollettino bollettinoModel): base(bollettinoModel)
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

        public string Tipo
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        // Base
        public byte[] Logo { get; set; }

        public string NumeroContoCorrente
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string IntestatoA
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string Causale
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string AdditionalInfo
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string NumeroAutorizzazioneStampaInProprio
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string IBan
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string EseguitoDaNominativo
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string EseguitoDaIndirizzo
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string EseguitoDaCap
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string EseguitoDaLocalita
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        // 896
        public decimal ImportoEuro
        {
            get { return GetValue<decimal>(); }

            set
            {
                SetValue<decimal>(value);
            }
        }

        public string CodiceCliente
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
    }
}
