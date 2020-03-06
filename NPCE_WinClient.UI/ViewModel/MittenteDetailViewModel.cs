using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data;
using NPCE_WinClient.UI.Event;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public class MittenteDetailViewModel : ViewModelBase, IMittenteDetailViewModel
    {
        public MittenteDetailViewModel(INpceDataService dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenDetailMittenteViewEvent>()
                .Subscribe(OnOpenMittenteDetailEvent);
        }

        private async void OnOpenMittenteDetailEvent(long mittenteId)
        {
            await LoadMittenteById(mittenteId);
        }

        private Mittente _mittente;
        private INpceDataService _dataService;
        private IEventAggregator _eventAggregator;

        public Mittente Mittente
        {
            get { return _mittente; }
            set
            {
                _mittente = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadMittenteById(long id)
        {
            Mittente = await _dataService.GetMittenteByIdAsync(id);
        }

    }
}
