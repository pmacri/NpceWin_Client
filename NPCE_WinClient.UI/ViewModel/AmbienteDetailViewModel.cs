using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.View.Services;
using NPCE_WinClient.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public class AmbienteDetailViewModel : DetailViewModelBase, IAmbienteDetailViewModel 
    {
        private readonly IAmbienteRepository _ambienteRepository;

        public AmbienteDetailViewModel(IAmbienteRepository ambienteRepository, IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService): base(eventAggregator, messageDialogService)
        {
            _ambienteRepository = ambienteRepository;
        }
        public override async Task LoadAsync(int id)
        {
            var ambiente = (id > 0)
               ? await _ambienteRepository.GetByIdAsync(id)
               : CreateNewAmbiente();
            Id = id;

            InitializeAmbiente(ambiente);
        }

        private void InitializeAmbiente(Ambiente ambiente)
        {
            Ambiente = new AmbienteWrapper(ambiente);

            Ambiente.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _ambienteRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Ambiente.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

                if (e.PropertyName== nameof(Ambiente.Description)) SetTitle();
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            // Little trick to trigger validation on a new anagrafica
            if (Ambiente.Id == 0)
            {
                Ambiente.Description = "";
            }

            SetTitle();
        }

        private void SetTitle()
        {
            Title = $"{Ambiente.Description}";
        }

        private AmbienteWrapper _ambiente;

        public AmbienteWrapper Ambiente
        {
            get { return _ambiente; }
            set {
                _ambiente = value;
                OnPropertyChanged();
            }
        }

        private Ambiente CreateNewAmbiente()
        {
            var ambiente = new Ambiente();
            _ambienteRepository.Add(ambiente);
            return ambiente;
        }

        protected override async void OnDeleteExecute()
        {
            var result = MessageDialogService.ShowOKCancelDialog($"Do you really want to cancel the ambiente {Ambiente.Description}",
                                                                  "Question");
            if (result == MessageDialogResult.Cancel)
            {
                return;
            }
            _ambienteRepository.Remove(Ambiente.Model);
            await _ambienteRepository.SaveAsync();
            RaiseDetailDeletedEvent(Ambiente.Id);
        }

        protected override bool OnSaveCanExecute()
        {
            return (_ambiente != null) && (!_ambiente.HasErrors) && (HasChanges);
        }

        protected override async void OnSaveExecute()
        {
            await _ambienteRepository.SaveAsync();
            Id = Ambiente.Id;
            HasChanges = _ambienteRepository.HasChanges();
            RaiseDetailSavedEvent(Ambiente.Id, $"{Ambiente.Description}");
        }
    }
}
