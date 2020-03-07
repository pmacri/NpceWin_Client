using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data;
using NPCE_WinClient.UI.Event;
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
        public AnagraficaDetailViewModel(INpceDataService dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenDetailAnagraficaViewEvent>()
                .Subscribe(OnOpenAnagraficaDetailEvent);
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        private async void OnSaveExecute()
        {
            await _dataService.SaveAsync(Anagrafica);

            _eventAggregator.GetEvent<AfterAnagraficaSavedEvent>()
                .Publish(new AfterAnagraficaSavedEventArgs { Id = Anagrafica.Id, DisplayMember = $"{Anagrafica.Nome} {Anagrafica.Cognome}" });

        }

        private bool OnSaveCanExecute()
        {
            return true;
        }

        private async void OnOpenAnagraficaDetailEvent(long AnagraficaId)
        {
            await LoadAnagraficaById(AnagraficaId);
        }

        private Anagrafica _Anagrafica;
        private INpceDataService _dataService;
        private IEventAggregator _eventAggregator;


        public Anagrafica Anagrafica
        {
            get { return _Anagrafica; }
            set
            {
                _Anagrafica = value;
                OnPropertyChanged();
            }
        }
        public async Task LoadAnagraficaById(long id)
        {
            Anagrafica = await _dataService.GetAnagraficaByIdAsync(id);
        }


        public ICommand SaveCommand { get; }
    }
}
