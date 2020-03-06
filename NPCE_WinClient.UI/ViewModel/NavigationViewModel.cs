using NPCE_WinClient.Model;
using NPCE_WinClient.UI.Data;
using NPCE_WinClient.UI.Event;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NPCE_WinClient.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        
        private IMittenteLookupDataService _serviceLookupDataService;

        private IEventAggregator _eventAggregator;
        public NavigationViewModel(IMittenteLookupDataService serviceLookupDataService, IEventAggregator eventAggregator)
        {
            _serviceLookupDataService = serviceLookupDataService;
            _eventAggregator = eventAggregator;
            Services = new ObservableCollection<LookupItem>();
        }
        public async Task LoadAsync()
        {
            var lookups = await _serviceLookupDataService.GetMittenteLookupAsync();

            Services.Clear();

            foreach (var lookup in lookups)
            {
                Services.Add(lookup);
            }
        }
        public ObservableCollection<LookupItem> Services { get; set; }

        private LookupItem _selectedMittente;
        public LookupItem SelectedMittente
        {
            get { return _selectedMittente; }
            set { 
                _selectedMittente = value;
                OnPropertyChanged();
                if(_selectedMittente != null)
                {
                    _eventAggregator.GetEvent<OpenDetailMittenteViewEvent>().Publish(_selectedMittente.Id);
                }
            }
        }

    }
}
