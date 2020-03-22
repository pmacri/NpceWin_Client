using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Wrapper
{
    public class AnagraficaWrapper: ModelWrapper<Anagrafica>
    {
        public AnagraficaWrapper(Anagrafica anagraficaModel):base(anagraficaModel)
        {
        }
        public int Id
        {

            get { return GetValue<int>(); }

            set {
                SetValue<int>(value);
            }
        }
        public string Cognome
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string Nome
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string RagioneSociale {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string ComplementoNominativo
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string DUG
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string Toponimo
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string Esponente
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string NumeroCivico
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string ComplementoIndirizzo
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string Frazione
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string Citta
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string Cap
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string Telefono
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string CasellaPostale
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string Provincia
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string Stato
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public bool IsMittente { get; set; }
    }
}