using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.Npce;
using NPCE_WinClient.UI.View.Services;
using NPCE_WinClient.UI.Wrapper;
using Prism.Commands;
using Prism.Events;

namespace NPCE_WinClient.UI.ViewModel
{
    public class ServiceOperationsDetailViewModel : DetailViewModelBase, IServiceOperationsDetailViewModel
    {
        private IAmbienteRepository _ambienteRepository;
        private IServizioRepository _servizioRepository;
        private AmbienteWrapper ambiente;

        public ServiceOperationsDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IAmbienteRepository ambienteRepository,
            IServizioRepository servizioRepository) : base(eventAggregator, messageDialogService)
        {
            Title = "Service Operations";
            _ambienteRepository = ambienteRepository;
            _servizioRepository = servizioRepository;

            Ambienti = new ObservableCollection<AmbienteWrapper>();

            Servizi = new ObservableCollection<ServizioWrapper>();

            InvioCommand = new DelegateCommand(OnInvioExceute);
        }

        private async void OnInvioExceute()
        {
            var operation = new RecuperaIdRichiestaLol(Ambiente.Model);

            var idRichiesta = operation.Execute();

            var idServizio = Servizio.Id;

            var servizio =await _servizioRepository.GetByIdAsync(idServizio);

            var invioOperation = new InvioLol(Ambiente.Model, servizio, idRichiesta);

            invioOperation.Execute();
        }

        public ICommand InvioCommand { get; set; }

        public async override Task LoadAsync(int id)
        {
            Id = id;
            var ambienti = await _ambienteRepository.GetAllAsync();

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

            var servizi = await _servizioRepository.GetAllAsync();

            foreach (var wrapper in Servizi)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }

            Servizi.Clear();

            foreach (var servizio in servizi)
            {
                var wrapper = new ServizioWrapper(servizio);

                wrapper.PropertyChanged += Wrapper_PropertyChanged;

                Servizi.Add(wrapper);
            }

        }

        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _ambienteRepository.HasChanges();
            }
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

        public ObservableCollection<AmbienteWrapper> Ambienti { get; set; }

        private AmbienteWrapper ambienteWrapper;

        public AmbienteWrapper Ambiente
        {
            get { return ambienteWrapper; }
            set { 
                ambienteWrapper = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ServizioWrapper> Servizi { get; set; }

        public ServizioWrapper Servizio { get; set; }
    }
}
