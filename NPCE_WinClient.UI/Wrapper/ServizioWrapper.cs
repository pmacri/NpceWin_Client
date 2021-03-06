﻿using NPCE_WinClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.Wrapper
{
    public class ServizioWrapper : Wrapper.ModelWrapper<Servizio>
    {
        public ServizioWrapper(Servizio servizio) : base(servizio)
        {
            PagineBollettini = new ObservableCollection<PaginaBollettino>();
        }
        public int Id {
            
            get { return GetValue<int>(); }

            set
            {
                SetValue<int>(value);
            }
        }
        public bool AvvisoRicevimento
        {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }
        public bool FronteRetro
        {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }
        public bool Colore
        {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }
        public bool ArchiviazioneDigitale
        {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }
        public bool Autoconferma
        {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }

        public bool SecondoTentativoRecapito
        {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }
        public string IdRichiesta
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string GuidUtente {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public string IdOrdine
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }
        public int StatoServizioId {
            get { return GetValue<int>(); }

            set
            {
                SetValue<int>(value);
            }
        }

        public int AmbienteId
        {
            get { return GetValue<int>(); }

            set
            {
                SetValue<int>(value);
            }
        }

        public string TipoArchiviazione
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public string TipoArchiviazioneSelected
        {
            get { return GetValue<string>(); }

            set
            {
                SetValue<string>(value);
            }
        }

        public int AnniArchiviazione
        {
            get { return GetValue<int>(); }

            set
            {
                SetValue<int>(value);
            }
        }


        public bool AttestazioneConsegna
        {
            get { return GetValue<bool>(); }

            set
            {
                SetValue<bool>(value);
            }
        }
        public TipoServizio TipoServizio {
            get { return GetValue<TipoServizio>(); }

            set
            {
                SetValue<TipoServizio>(value);
            }
        }
        public StatoServizio StatoServizio
        {
            get { return GetValue<StatoServizio>(); }

            set
            {
                SetValue<StatoServizio>(value);
            }
        }
        public ICollection<PaginaBollettino> PagineBollettini
        {
            get { return GetValue<ICollection<PaginaBollettino>>(); }

            set
            {
                SetValue<ICollection<PaginaBollettino>>(value);
            }
        }
    }
}
