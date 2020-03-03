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
        private IServiceLookupDataService _serviceLookupDataService;
        private IEventAggregator _eventAggregator;
        private LookupItem selectedService;

        public NavigationViewModel(IServiceLookupDataService serviceLookupDataService, IEventAggregator eventAggregator)
        {
            _serviceLookupDataService = serviceLookupDataService;
            _eventAggregator = eventAggregator;
            Services = new ObservableCollection<LookupItem>();
        }

        public async Task LoadServicesAsync()
        {
            var lookups = await _serviceLookupDataService.GetServiceLookupAsync();

            Services.Clear();

            foreach (var lookup in lookups)
            {
                Services.Add(lookup);
            }
        }

        public ObservableCollection<LookupItem> Services { get; set; }

        private LookupItem _selectedService;

        public LookupItem SelectedService
        {
            get { return _selectedService; }
            set { 
                _selectedService = value;
                OnPropertyChanged();
                if(_selectedService != null)
                {
                    _eventAggregator.GetEvent<OpenDetailServiceViewEvent>().Publish(_selectedService.Id);
                }
            }
        }


    }
}
