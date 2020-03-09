using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data;
using NPCE_WinClient.UI.Data.Repositories;
using NPCE_WinClient.UI.Event;
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
        public AnagraficaDetailViewModel(INpceRepository npceRepository, IEventAggregator eventAggregator)
        {
            _npceRepository = npceRepository;
            _eventAggregator = eventAggregator;
            
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        private async void OnSaveExecute()
        {
            await _npceRepository.SaveAsync();
            HasChanges = _npceRepository.HasChanges();
            _eventAggregator.GetEvent<AfterAnagraficaSavedEvent>()
                .Publish(new AfterAnagraficaSavedEventArgs { Id = Anagrafica.Id, DisplayMember = $"{Anagrafica.Nome} {Anagrafica.Cognome}" });
        }
        private bool OnSaveCanExecute()
        {
            return (_Anagrafica != null) && (!_Anagrafica.HasErrors) && (HasChanges);
        }

        private AnagraficaWrapper _Anagrafica;
        private INpceRepository _npceRepository;
        private IEventAggregator _eventAggregator;
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
        public async Task LoadById(long id)
        {
            var anagrafica = await _npceRepository.GetByIdAsync(id);
            Anagrafica = new AnagraficaWrapper(anagrafica);
            Anagrafica.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _npceRepository.HasChanges();
                }
                if (e.PropertyName == nameof(Anagrafica.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        // E' necessario definirla come proprietà per usarla nel DataBinding della View

        public bool HasChanges
        {
            get { return _hasChanges; }
            set { 
                if (_hasChanges!= value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }                
            }
        }

        public ICommand SaveCommand { get; }
    }
}
