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

            DeleteCommand = new DelegateCommand(OnDeleteExecute);
        }

        private async void OnSaveExecute()
        {
            await _npceRepository.SaveAsync();
            HasChanges = _npceRepository.HasChanges();
            _eventAggregator.GetEvent<AfterAnagraficaSavedEvent>()
                .Publish(new AfterAnagraficaSavedEventArgs { Id = Anagrafica.Id, DisplayMember = $"{Anagrafica.Nome} {Anagrafica.Cognome}" });
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
        public async Task LoadById(long? id)
        {
            var anagrafica = (id.HasValue)
                ? await _npceRepository.GetByIdAsync(id.Value)
                : CreateNewAnagrafica();
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


            // Little trick to trigger validation on a new anagrafica
            if(Anagrafica.Id==0)
            {
                Anagrafica.Cognome = "";
            }
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
        public ICommand DeleteCommand { get; set; }
        private Anagrafica CreateNewAnagrafica()
        {
            var anagrafica = new Anagrafica();
            _npceRepository.Add(anagrafica);
            return anagrafica;
        }
        private async void OnDeleteExecute()
        {
            _npceRepository.Remove(Anagrafica.Model);
            await _npceRepository.SaveAsync();
            _eventAggregator.GetEvent<AfterAnagraficaDeletedEvent>().Publish(Anagrafica.Id);
        }
        private bool OnSaveCanExecute()
        {
            return (_Anagrafica != null) && (!_Anagrafica.HasErrors) && (HasChanges);
        }

    }
}
