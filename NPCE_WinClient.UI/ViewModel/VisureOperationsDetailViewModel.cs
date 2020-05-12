using FriendOrganizer.UI.View.Services;
using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.Npce;
using NPCE_WinClient.UI.View.Services;
using NPCE_WinClient.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public class VisureOperationsDetailViewModel : DetailViewModelBase, IDetailViewModel
    {
        private readonly IVisureRepository visureRepository;
        private readonly IAmbienteRepository ambienteRepository;
        private readonly IStatoServizioRepository statoServizioRepository;
        private int idStatoServizioSalvato;

        public VisureOperationsDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IVisureRepository visureRepository,
            IStatoServizioRepository statoServizioRepository,
            IAmbienteRepository ambienteRepository) : base(eventAggregator, messageDialogService)
        {
            this.visureRepository = visureRepository;
            this.ambienteRepository = ambienteRepository;
            this.statoServizioRepository = statoServizioRepository;

            idStatoServizioSalvato = statoServizioRepository.GetByDescription("Salvato").Id;

            Ambienti = new ObservableCollection<AmbienteWrapper>();

            Visure = new ObservableCollection<VisuraWrapper>();

            InvioCommand = new DelegateCommand(OnInvioExecute, OnInvioCanExecute);
        }

        private void OnInvioExecute()
        {
            //Visura.VisureTipoRecapito = new VisureTipoRecapito { Id = "DL", Descrizione = "Download" };

            if (Ambiente.IsPil)
            {
                var operation = new InvioVisuraPIL(Visura.Model, Ambiente.Model);

                operation.Execute(AutoConferma, ControllaPrezzo);
            }            
        }

        private bool OnInvioCanExecute()
        {
            return Visura != null && Visura.StatoServizioId == idStatoServizioSalvato && Ambiente != null;
        }

        protected override void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCanExecute()
        {
            throw new NotImplementedException();
        }

        protected override void OnSaveExecute()
        {
            throw new NotImplementedException();
        }

        public override async Task LoadAsync(int id)
        {
            if (Ambiente == null)
            {
                var ambienti = await ambienteRepository.GetAllAsync();

                foreach (var wrapper in Ambienti)
                {
                    wrapper.PropertyChanged -= Wrapper_PropertyChanged;
                }

                Ambienti.Clear();

                foreach (var ambiente in ambienti)
                {
                    var wrapper = new AmbienteWrapper(ambiente);
                    wrapper.PropertyChanged += Wrapper_PropertyChanged;
                    Ambienti.Add(wrapper);
                }
            }

            Id = id;

            var visure = await visureRepository.GetAllAsync();
            InitializeVisure(visure);
        }

        private void InitializeVisure(IEnumerable<Visura> visure)
        {
            Title = "Visure Operations";

            foreach (var wrapper in Visure)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }

            Visure.Clear();

            foreach (var visura in visure)
            {
                var wrapper = new VisuraWrapper(visura);

                wrapper.PropertyChanged += Wrapper_PropertyChanged;

                Visure.Add(wrapper);
            }
        }

        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = ambienteRepository.HasChanges();
            }
        }

        public ObservableCollection<AmbienteWrapper> Ambienti { get; set; }


        AmbienteWrapper _ambiente;
        public AmbienteWrapper Ambiente
        {
            get { return _ambiente; }
            set
            {
                _ambiente = value;
                OnPropertyChanged();
                ((DelegateCommand)InvioCommand).RaiseCanExecuteChanged();
            }
        }

        VisuraWrapper _visura;
        public VisuraWrapper Visura { get
            {
                return _visura;
            }
            set {
                _visura = value;
                OnPropertyChanged();
                ((DelegateCommand)InvioCommand).RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<VisuraWrapper> Visure { get; set; }

        public DelegateCommand InvioCommand { get; set; }

        public bool AutoConferma { get; set; }

        public bool ControllaPrezzo { get; set; }
    }
}
