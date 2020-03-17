using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.Event;
using NPCE_WinClient.UI.View.Services;
using NPCE_WinClient.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NPCE_WinClient.UI.ViewModel
{
    public class AnagraficaDetailViewModel : DetailViewModelBase, IAnagraficaDetailViewModel
    {
        private AnagraficaWrapper _anagrafica;
        private IAnagraficaRepository _AnagraficaRepository;
        private readonly IMessageDialogService _messageDialogService;

        public AnagraficaDetailViewModel(IAnagraficaRepository AnagraficaRepository, IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService): base(eventAggregator)
        {
            _AnagraficaRepository = AnagraficaRepository;
            _messageDialogService = messageDialogService;
            
        }
        protected override async void OnSaveExecute()
        {
            await _AnagraficaRepository.SaveAsync();
            Id = Anagrafica.Id;

            HasChanges = _AnagraficaRepository.HasChanges();
            RaiseDetailSavedEvent(Anagrafica.Id, $"{Anagrafica.Nome} {Anagrafica.Cognome}");
        }       
        public AnagraficaWrapper Anagrafica
        {
            get { return _anagrafica; }
            set
            {
                _anagrafica = value;
                OnPropertyChanged();
            }
        }
        public override async Task LoadAsync(int? id)
        {
            var anagrafica = (id.HasValue)
                ? await _AnagraficaRepository.GetByIdAsync(id.Value)
                : CreateNewAnagrafica();
            Id = anagrafica.Id;
            InitializeAnagrafica(anagrafica);
        }

        private void InitializeAnagrafica(Anagrafica anagrafica)
        {
            Anagrafica = new AnagraficaWrapper(anagrafica);
            Anagrafica.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _AnagraficaRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Anagrafica.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }

                if ((e.PropertyName == nameof(Anagrafica.Cognome)) || (e.PropertyName == nameof(Anagrafica.Nome)))
                {
                    SetTitle();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            // Little trick to trigger validation on a new anagrafica
            if (Anagrafica.Id == 0)
            {
                Anagrafica.Cognome = "";
            }

            SetTitle();
        }

        private void SetTitle()
        {
            Title = $"{Anagrafica.Nome} {Anagrafica.Cognome}";
        }

        private Anagrafica CreateNewAnagrafica()
        {
            var anagrafica = new Anagrafica();
            _AnagraficaRepository.Add(anagrafica);
            return anagrafica;
        }
        protected override async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOKCancelDialog($"Do you really want to cancel the anagrafica {Anagrafica.Nome} {Anagrafica.Cognome}",
                                                                   "Question");
            if (result == MessageDialogResult.Cancel)
            {
                return;
            }
            _AnagraficaRepository.Remove(Anagrafica.Model);
            await _AnagraficaRepository.SaveAsync();
            RaiseDetailDeletedEvent(Anagrafica.Id);

        }
        protected override bool OnSaveCanExecute()
        {
            return (_anagrafica != null) && (!_anagrafica.HasErrors) && (HasChanges);
        }

    }
}
