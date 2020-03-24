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
        private IMessageDialogService _messageDialogService;
        private IEnumerable<TipoServizio> _allTipi;

        public ServiceOperationsDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IAmbienteRepository ambienteRepository,
            IServizioRepository servizioRepository) : base(eventAggregator, messageDialogService)
        {
            Title = "Service Operations";

            TipiServizio = new ObservableCollection<TipoServizio>();

            _ambienteRepository = ambienteRepository;
            _servizioRepository = servizioRepository;
            _messageDialogService = messageDialogService;

            Ambienti = new ObservableCollection<AmbienteWrapper>();

            Servizi = new ObservableCollection<ServizioWrapper>();

            InvioCommand = new DelegateCommand(OnInvioExecute);
        }

        private async void OnInvioExecute()
        {
            // Impostare sul servizio il suo TipoServizio
            _servizioRepository.UpdateTipoServizioAsync(Servizio.Id, TipoServizio.Id);

            
            var operation = new RecuperaIdRichiestaLol(Ambiente.Model);

            var idRichiesta = operation.Execute();

            var idServizio = Servizio.Id;

            var servizio =await _servizioRepository.GetByIdAsync(idServizio);

            var invioOperation = new InvioLol(Ambiente.Model, servizio, idRichiesta);

            var result = invioOperation.Execute();

            string message;

            if (result.Success)
            {
                message = $"Operazione {result.Operation.ToString()} completata con successo";
            }
            else
            {
                message = $"Si è verificato il seguente errore:\nCode: {result.Errors[0].Code}\n Description: {result.Errors[0].Description}";
            }

            _messageDialogService.ShowOKCancelDialog(message, "Info");

            Servizio.IdRichiesta = result.IdRichiesta;
            
            OnSaveExecute();
        }

        public ICommand InvioCommand { get; set; }

        public async override Task LoadAsync(int id)
        {

            _allTipi = _servizioRepository.GetAllTipiServizio();

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

            // Tipi servizio
            TipiServizio.Clear();

            foreach (var tipo in _allTipi)
            {
                TipiServizio.Add(tipo);
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

        protected override async void OnSaveExecute()
        {
            await _servizioRepository.SaveAsync();
            await LoadAsync(-1);
        }

        public ObservableCollection<AmbienteWrapper> Ambienti { get; set; }

        private AmbienteWrapper ambienteWrapper;

        private TipoServizio _tipoServizio;
        public AmbienteWrapper Ambiente
        {
            get { return ambienteWrapper; }
            set { 
                ambienteWrapper = value;
                OnPropertyChanged();
            }
        }
        public TipoServizio TipoServizio
        {
            get
            {
                return _tipoServizio;
            }
            set
            {
                _tipoServizio = value;
                if (_tipoServizio != null)
                {
                    if (Servizio.Model.TipoServizio == null)
                    {
                        Servizio.Model.TipoServizio = new TipoServizio();
                        Servizio.Model.TipoServizio = _tipoServizio;
                    }
                    HasChanges = _servizioRepository.HasChanges();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public ObservableCollection<TipoServizio> TipiServizio
        {
            get;
            set;
        }
        public ObservableCollection<ServizioWrapper> Servizi { get; set; }
        public ServizioWrapper Servizio { get; set; }
    }
}
