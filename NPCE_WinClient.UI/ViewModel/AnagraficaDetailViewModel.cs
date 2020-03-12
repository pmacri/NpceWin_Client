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
    public class AnagraficaDetailViewModel : ViewModelBase, IAnagraficaDetailViewModel
    {
        public AnagraficaDetailViewModel(IAnagraficaRepository AnagraficaRepository, IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _AnagraficaRepository = AnagraficaRepository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        private async void OnSaveExecute()
        {
            await _AnagraficaRepository.SaveAsync();
            HasChanges = _AnagraficaRepository.HasChanges();
            _eventAggregator.GetEvent<AfterDetailSavedEvent>()
                .Publish(new AfterDetailSavedEventArgs
                {
                    Id = Anagrafica.Id,
                    DisplayMember = $"{Anagrafica.Nome} {Anagrafica.Cognome}",
                    ViewModelName = nameof(AnagraficaDetailViewModel)
                });
        }

        private AnagraficaWrapper _Anagrafica;
        private IAnagraficaRepository _AnagraficaRepository;
        private IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private bool _hasChanges;
        public AnagraficaWrapper Anagrafica
        {
            get { return _Anagrafica; }
            set
            {
                _Anagrafica = value;
                OnPropertyChanged();
            }
        }
        public async Task LoadAsync(int? id)
        {
            var anagrafica = (id.HasValue)
                ? await _AnagraficaRepository.GetByIdAsync(id.Value)
                : CreateNewAnagrafica();
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
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();


            // Little trick to trigger validation on a new anagrafica
            if (Anagrafica.Id == 0)
            {
                Anagrafica.Cognome = "";
            }
        }

        // E' necessario definirla come proprietà per usarla nel DataBinding della View
        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; set; }
        private Anagrafica CreateNewAnagrafica()
        {
            var anagrafica = new Anagrafica();
            _AnagraficaRepository.Add(anagrafica);
            return anagrafica;
        }
        private async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOKCancelDialog($"Do you really want to cancel the anagrafica {Anagrafica.Nome} {Anagrafica.Cognome}",
                                                                   "Question");
            if (result == MessageDialogResult.Cancel)
            {
                return;
            }
            _AnagraficaRepository.Remove(Anagrafica.Model);
            await _AnagraficaRepository.SaveAsync();
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Publish(
                new AfterDetailDeletedEventArgs
                {
                    Id = Anagrafica.Id,
                    ViewModelName = nameof(AnagraficaDetailViewModel)
                });
        }
        private bool OnSaveCanExecute()
        {
            return (_Anagrafica != null) && (!_Anagrafica.HasErrors) && (HasChanges);
        }

    }
}
